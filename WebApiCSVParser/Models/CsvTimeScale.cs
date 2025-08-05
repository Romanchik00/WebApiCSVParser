using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApiCSVParser.Models
{
    public class CsvTimeScale
    {
        public DateTime Date { get; set; }
        public double ExecutionTime { get; set; }
        public double Value { get; set; }
        public CsvTimeScale(DateTime date, double exctime, double val) 
        {
            Date = date;
            ExecutionTime = exctime;
            Value = val;
        }
        public CsvTimeScale(string str)
        {
            bool flag = true;
            string? param = null;
            var args = str.Split(";");
            if (args.Length < 3) 
            {
                throw new ArgumentException("Invalid data: uncorrect csv time scale line",str);
            }
            if (DateTime.TryParseExact(args[0], "yyyy-MM-ddTHH:mm:ss.ffffZ", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                Date = date;
            }
            else 
            {
                flag = false;
                param = nameof(Date);
            }
            if(double.TryParse(args[1],out double res))
            {
                ExecutionTime = res;
            }
            else 
            {
                flag = false;
                param = nameof(ExecutionTime);
            }
            if (double.TryParse(args[2], NumberStyles.Any, CultureInfo.InvariantCulture,out double rst))
            {
                Value = rst;
            }
            else 
            {
                flag = false; 
                param = nameof(Value);
            }
            if (!flag) 
            {
                throw new ArgumentException("Invalid or null argument",param);
            }
        }
        public static implicit operator CsvTimeScale(string str) => new CsvTimeScale(str);
    }
}
