using WebApiCSVParser.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiCSVParser.Models
{
    [Table("Values")]
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

    //public class ValueDbContext : DbContext
    //{
    //    public ValueDbContext(DbContextOptions<ValueDbContext> options) : base(options) { }

    //    public DbSet<Value> Values => Set<Value>();

    //    protected override void OnModelCreating(ModelBuilder modelBuilder)
    //    {
    //        // Создаем уникальный индекс на поле FileName
    //        modelBuilder.Entity<Value>()
    //            .HasIndex(v => v.FileName)
    //            .IsUnique();
    //    }
    //}

// Пример использования:
//using (var context = new MyDbContext(...))
//{
//    // Проверяем, существует ли запись с таким же FileName
//    var existingValue = context.Values.FirstOrDefault(v => v.FileName == "existing_file.csv");

//    if (existingValue != null)
//    {
//        // Обновляем существующую запись
//        existingValue.Data = "Новые данные";
//    }
//    else
//{
//    // Создаем новую запись
//    var newValue = new Value { FileName = "existing_file.csv", Data = "Новые данные" };
//    context.Values.Add(newValue);
//}

//try
//{
//    context.SaveChanges();
//}
//catch (DbUpdateException ex)
//{
//    // Обрабатываем исключение, если FileName уже существует
//    Console.WriteLine($"Ошибка: FileName '{newValue.FileName}' уже существует в базе данных.");
//    // Логируем ошибку, возвращаем сообщение об ошибке клиенту и т.д.
//}
//}
    /*
        Пояснения:

[Key]: Атрибут указывает, что Id является первичным ключом.
[Required]: Атрибут указывает, что FileName не может быть null или пустым.
HasIndex(v => v.FileName).IsUnique(): В методе OnModelCreating мы создаем уникальный индекс на поле FileName. Это гарантирует, что в таблице Values не будет двух записей с одинаковым FileName. Это очень важно, так как просто объявить поле FileName строкой недостаточно, чтобы обеспечить уникальность на уровне базы данных.
Обработка DbUpdateException: При вызове SaveChanges() необходимо обработать исключение DbUpdateException. Это исключение будет сгенерировано, если вы попытаетесь добавить запись с FileName, который уже существует в базе данных. Обработка исключения позволяет вам корректно обработать эту ситуацию (например, залогировать ошибку, вернуть сообщение об ошибке клиенту, предложить пользователю ввести другое имя файла и т.д.).
FirstOrDefault(): Мы используем FirstOrDefault() для поиска существующей записи с заданным FileName. Это возвращает либо запись, если она существует, либо null, если запись не найдена.
     */

    //------------------------------------------------------------------------------------------------
}
