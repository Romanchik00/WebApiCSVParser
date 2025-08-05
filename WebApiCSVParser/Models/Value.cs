using WebApiCSVParser.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiCSVParser.Models
{
    [Table("values")]
    public class Value
    {
        [Column("id"),Key]
        public int Id { get; set; } // Автоинкрементный Primary Key

        [Column("file_name"),Required]
        public string FileName { get; set; }
        [Column("date")]
        public DateTime Date { get; set; }
        [Column("execution_time")]
        public double ExecutionTime { get; set; }
        [Column("value")]
        public double Value_ {  get; set; }

    }
}
