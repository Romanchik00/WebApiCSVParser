using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text;
using WebApiCSVParser.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration["AppSettings:ConnectionString"]));


//builder.Services.AddAntiforgery();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

//app.UseAntiforgery();

app.MapControllers();

app.MapGet("/", () => "Hello");
//app.MapPost("/", () =>
// /api/...

app.MapPost("/api/test", () => "Запрос получен");


//app.MapPost("/api/val", async (HttpContext context) => { --!!!-ПЛОХО-!!!--
//    var file = context.Request.Form.Files["CSV"]; // "CSV" - это имя параметра в Swagger

//    if (file == null)
//    {
//        return Results.BadRequest("Файл не был отправлен.");
//    }

//    // ... обработка файла ...

//    return Results.Ok("Запрос получен (альтернативный способ)");
//});

app.MapPost("/api/str", (string s) => $"hello {s}");

app.MapPost("/api/val", (IFormFile CSV) =>
{
    var data = new CsvData();
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
                if (!data.TryAdd(line, out errmess))
                {
                    Error = true;
                    inval = line;
                    break;
                }
                indx++;
            }
        }
    }

    if (Error)
    {
        data = null;
        string ErrorMessage = $"File: {CSV.FileName}  _"
                            + $"Line: [{indx}] {inval}  _"
                            + $"Error: {errmess}";
        return Results.UnprocessableEntity(ErrorMessage);
    }
    else
    {

        //---------------------------------------------------------------------------------------------------------
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
        //---------------------------------------------------------------------------------------------------------

        return Results.Ok();
        //return Results.Created();
    }
}).DisableAntiforgery();

//app.MapGet("/antiforgery/token", (IAntiforgery antiforgery, HttpContext context) =>
//{
//    var tokens = antiforgery.GetAndStoreTokens(context);
//    context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken!, new CookieOptions { HttpOnly = false });
//    return Results.Ok();
//});

app.Run();
