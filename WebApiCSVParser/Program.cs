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
