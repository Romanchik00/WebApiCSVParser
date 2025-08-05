namespace WebApiCSVParser.Models
{
    public class CsvData
    {
        List<CsvTimeScale> timeScales;
        int currcap = 0;
        public CsvData()
        { 
            timeScales = new();
            timeScales.Capacity = 10000;
        }
        public List<CsvTimeScale> Get()
        {
            return timeScales;
        }
        public bool TryAdd(string str, out string? errmess) 
        {
            CsvTimeScale TS;

            if(currcap >= 10000)
            {
                throw new ArgumentOutOfRangeException("Over 10 000 range");
            }

            try
            {
                TS = new CsvTimeScale(str);
            }
            catch(ArgumentException AE)
            {
                errmess = AE.Message + $": {AE.ParamName}";
                return false;
            }

            if(TS.Date < new DateTime(2000, 1, 1))
            {
                errmess = "Invalid data: uncorrect date";
                return false;
            }
            
            if(TS.ExecutionTime < 0)
            {
                errmess = "Invalid data: uncorrect execution time";
                return false;
            }

            timeScales.Add(TS);
            ++currcap;
            errmess = null;
            return true;
        }

        /*
            

Æ количество строк не может быть меньше 1 и больше 10 000

            public class CsvProcessingService
{
    private readonly AppDbContext _dbContext;

    public CsvProcessingService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IActionResult> ProcessCsvFile(IFormFile file)
    {
        // ... (валидация файла, парсинг CSV в List<Value>) ...
        var fileName = file.FileName;
        var parsedValues = new List<Value>(); // Ваши распарсенные данные

        // Логика валидации и заполнения List<Value>
        // ...

        using (var transaction = await _dbContext.Database.BeginTransactionAsync())
        {
            try
            {
                // 1. Удаление старых данных Value для этого файла
                var existingValues = await _dbContext.Values.Where(v => v.FileName == fileName).ToListAsync();
                _dbContext.Values.RemoveRange(existingValues);
                await _dbContext.SaveChangesAsync();

                // 2. Добавление новых данных Value
                var newValuesForDb = parsedValues.Select(v => new Value
                {
                    FileName = fileName,
                    Date = v.Date,
                    ExecutionTime = v.ExecutionTime,
                    Value_ = v.Value_ // если переименовали
                }).ToList();
                _dbContext.Values.AddRange(newValuesForDb);
                await _dbContext.SaveChangesAsync();

                // 3. Расчет и обновление/вставка в Results
                var calculatedResult = CalculateResultData(parsedValues, fileName);

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
                return Ok($"File '{fileName}' processed successfully.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                // Логирование ошибки
                return BadRequest($"Error processing file '{fileName}': {ex.Message}");
            }
        }
    }

    private Result CalculateResultData(List<Value> values, string fileName)
    {
        if (values == null || !values.Any())
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
}

         */
    }
}
