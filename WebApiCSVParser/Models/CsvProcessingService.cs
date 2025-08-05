using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApiCSVParser.Models
{
    public class CsvProcessingService
    {
        private readonly AppDbContext _dbContext;

        public CsvProcessingService(AppDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext)); // !!! DangerZone !!!
        }

        private IResult CsvTryParse(IFormFile CSV, out CsvData? data)
        {
            var newData = new CsvData();
            bool Error = false;
            string inval = "";
            int indx = 0;
            string? errmess = "";

            using (Stream stream = CSV.OpenReadStream())
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    string? line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (!newData.TryAdd(line, out errmess))
                        {
                            Error = true;
                            inval = line;
                            break;
                        }
                        indx++;
                    }
                    if (line == null && indx == 0)
                    {
                        data = null;
                        return Results.NoContent();
                    }
                }
            }

            if (Error)
            {
                string ErrorMessage = $"File: {CSV.FileName}  _"
                                    + $"Line: [{indx}] {inval}  _"
                                    + $"Error: {errmess}";
                data = null;
                return Results.UnprocessableEntity(ErrorMessage);
            }
            else
            {


                data = newData;
                return Results.Ok();
                //return Results.Created();
            }
        }

        public async Task<IResult> ProcessCsvFile(IFormFile file) // Возвращает Task, а не IActionResult
        {
            // ... (валидация файла, парсинг CSV в List<Value>) ...


            var fileName = file.FileName;
            CsvData? parsedValues;
            IResult answer;
            try
            {
                answer = CsvTryParse(file, out parsedValues);
            }
            catch (ArgumentOutOfRangeException AORE)
            {
                return Results.UnprocessableEntity($"File: {fileName} is overage [{AORE}]");
            }

            if (answer.Equals(Results.UnprocessableEntity()) || answer.Equals(Results.NoContent()))
            {
                return answer;
            }

            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    // 1. Удаление старых данных Value для этого файла
                    var existingValues = await _dbContext.Values.Where(v => v.FileName == fileName).ToListAsync();
                    _dbContext.Values.RemoveRange(existingValues);
                    await _dbContext.SaveChangesAsync();

                    // 2. Добавление новых данных Value
                    var newValuesForDb = parsedValues.Get().Select(v => new Value
                    {
                        FileName = fileName,
                        Date = DateTime.SpecifyKind(v.Date, DateTimeKind.Utc),
                        ExecutionTime = v.ExecutionTime,
                        Value_ = v.Value
                    }).ToList();
                    _dbContext.Values.AddRange(newValuesForDb);
                    await _dbContext.SaveChangesAsync();

                    // 3. Расчет и обновление/вставка в Results
                    var calculatedResult = CalculateResultData(newValuesForDb, fileName);

                    var existingResult = await _dbContext.Results.FirstOrDefaultAsync(r => r.FileName == fileName);
                    if (existingResult != null)
                    {
                        // Обновить существующий Result
                        existingResult.SecDeltaDate = calculatedResult.SecDeltaDate;
                        existingResult.MinDate = calculatedResult.MinDate;
                        existingResult.AvgExecutionTime = calculatedResult.AvgExecutionTime;
                        existingResult.AvgValue = calculatedResult.AvgValue;
                        existingResult.MedianValue = calculatedResult.MedianValue;
                        existingResult.MinValue = calculatedResult.MinValue;
                        existingResult.MaxValue = calculatedResult.MaxValue;
                    }
                    else
                    {
                        // Добавить новый Result
                        _dbContext.Results.Add(calculatedResult);
                    }
                    await _dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    //throw; // Re-throw исключение, чтобы контроллер мог его обработать
                    return Results.InternalServerError(ex.ToString());
                }
                return Results.Created();
            }
        }

        private Result CalculateResultData(List<Value> values, string fileName)
        {
            if (values == null || values.Count == 0)
            {
                // Обработка случая пустого списка, если валидация не отсекла
                throw new InvalidOperationException("No values to calculate results.");
            }

            var sortedValuesByDate = values.OrderBy(v => v.Date).ToList();
            var sortedValuesByValue = values.OrderBy(v => v.Value_).ToList();

            var minDate = sortedValuesByDate.First().Date;
            var maxDate = sortedValuesByDate.Last().Date;
            var secDeltaDate = (int)(maxDate - minDate).TotalSeconds;

            var avgExecutionTime = values.Average(v => v.ExecutionTime);
            var avgValue = values.Average(v => v.Value_);

            double medianValue;
            if (sortedValuesByValue.Count % 2 == 0)
            {
                // Четное количество элементов
                var midIndex1 = sortedValuesByValue.Count / 2 - 1;
                var midIndex2 = sortedValuesByValue.Count / 2;
                medianValue = (sortedValuesByValue[midIndex1].Value_ + sortedValuesByValue[midIndex2].Value_) / 2.0;
            }
            else
            {
                // Нечетное количество элементов
                var midIndex = sortedValuesByValue.Count / 2;
                medianValue = sortedValuesByValue[midIndex].Value_;
            }

            var minValue = sortedValuesByValue.First().Value_;
            var maxValue = sortedValuesByValue.Last().Value_;

            return new Result
            {
                FileName = fileName,
                SecDeltaDate = secDeltaDate,
                MinDate = minDate,
                AvgExecutionTime = avgExecutionTime,
                AvgValue = avgValue,
                MedianValue = medianValue,
                MinValue = minValue,
                MaxValue = maxValue
            };
        }
        public IEnumerable<string> LastValues(string filename)
        {
            IEnumerable<string> vals = (
                from el in _dbContext.Values 
                where el.FileName == filename 
                orderby el.Date descending
                select el.Value_.ToString())
                .AsEnumerable().Take(10);
            return vals;     
        }
        /*
        Id { get; set; }

        FileName { get; set; }

        SecDeltaDate {  get; set; }

        MinDate {  get; set; }

        AvgExecutionTime {  get; set; }

        AvgValue {  get; set; }

        MedianValue {  get; set; }
 
        MinValue {  get; set; }

        MaxValue {  get; set; }
         */
        public IEnumerable<string> ResultByName(string name)
        {
            var res = from el in _dbContext.Results 
                      where el.FileName == name 
                      select $"{el.Id};{el.FileName};{el.SecDeltaDate};{el.MinDate};{el.AvgExecutionTime};{el.AvgValue};{el.MedianValue};{el.MinValue};{el.MaxValue}";
            return res;
        }
    }
}