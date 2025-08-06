# WebApiCSVParser
## Открытое Api с поддержкой swagger для обработки файлов в формате csv , а так же чтения/записи данных в базу данных

## Доступное Api
- пост-метод (/api/Data/upload): принимает файл . возвращает статус 201 в случае успеха или 422 + ошибка в случае провала
- гет-метод (/api/Data/values): отправляет список из 10 последних значений из таблицы Value
- гет-метод (/api/Data/result): Метод возвращает фильтрованный или пустой список строк из таблицы Results. В качестве фильтров используются: имя файла, диапозоны для времени старта первой операции, среднего показателя и среднего времени выполнения.

## Особенности
### В данном Api нет графического интерфейса.
### Для просмотра и использования, пользуйтесь swagger-ом (http(s):localhost:PORT/swagger) в режиме дебагера

## Настройка перед работой
### Скорее всего потребуется изменить конфигурацию : файла appsettings.json в строке ConnectionString под своё окружение
### Возможно потребуется сделать в .net cli миграцию : dotnet ef migrations add init --context models/appdbcontext --project webapicsvparser.csproj --startup-project webapicsvparser.csproj
### Вероятно потребуется установить через NuGet некоторые из перечисленых пакетов:
- Microsoft.AspNetCore.OpenApi
- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.Design
- Microsoft.EntityFrameworkCore.Tools
- Npgsql
- Npgsql.EntityFrameworkCore.PostgreSQL
- Npgsql.Json.NET
- Swashbuckle.AspNetCore.Swagger
- Swashbuckle.AspNetCore.SwaggerGen
- Swashbuckle.AspNetCore.SwaggerUI

## Используемые технологии
- .NET 9
- EF Core
- PostgreSQL
- Swagger
