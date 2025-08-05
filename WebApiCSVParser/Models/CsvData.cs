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
        //public void Add(string str)
        //{
        //    timeScales.Add(str);
        //}
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
                         Date                 ;          ExecutionTime      ;                Value
          <Время начала ГГГГ-ММ-ДДTчч-мм-сс.ммммZ>;<Время выполнения в секундах>;<Показатель в виде числа с плавающей 
 */
        //string sqlFormattedDate = now.ToString("yyyy-MM-dd HH:mm:ss.mmmmZ");
        //DateTime.TryParseExact(dateString, "yyyy-MM-dd HH:mm:ss.mmmmZ", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate)

        /*
            Необходимо выполнить валидацию:º
Æ дата не может быть позже текущей и раньше 01.01.2000

Æ время выполнения не может быть меньше 0

Æ значение показателя не может быть меньше 0

Æ количество строк не может быть меньше 1 и больше 10 000

Æ значения должны соответствовать своим типам, отсутствие одного
 из значений в записи недопустимо.

Если какое-либо условие не выполнено, нужно считать файл невалидным, откатить изменения и вернуть пользователю соответствующую ошибку.

         */
    }
}
