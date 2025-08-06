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
    }
}
