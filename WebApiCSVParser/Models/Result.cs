using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiCSVParser.Models
{
    [Table("Results")]
    public class Result
    {
        [Column("id"), Key]
        public int Id { get; set; } // Автоинкрементный Primary Key

        [Column("file_name"),Required]
        public string FileName { get; set; }
        [Column("sec_delta_date")]
        public int SecDeltaDate {  get; set; } // (максимальное Date – минимальное Date)
        [Column("min_date")]
        public DateTime MinDate {  get; set; }
        [Column("avg_exect_time")]
        public double AvgExecutionTime {  get; set; }
        [Column("avg_value")]
        public double AvgValue {  get; set; }
        [Column("median_value")]
        public double MedianValue {  get; set; }
        [Column("min_value")]
        public double MinValue {  get; set; }
        [Column("max_value")]
        public double MaxValue {  get; set; }
    }

    //public class ResultDbContext : DbContext
    //{
    //    public ResultDbContext(DbContextOptions<ResultDbContext> options) : base(options) { }

    //    public DbSet<Result> Results => Set<Result>();

    //    protected override void OnModelCreating(ModelBuilder modelBuilder)
    //    {
    //        // Создаем уникальный индекс на поле FileName
    //        modelBuilder.Entity<Result>()
    //            .HasIndex(v => v.FileName)
    //            .IsUnique();
    //    }
    //}
}
