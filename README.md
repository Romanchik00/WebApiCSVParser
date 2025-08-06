# WebApiCSVParser
## Открытое Api с поддержкой swagger для обработки файлов в формате csv , а так же чтения/записи данных в базу данных

## Доступное Api
- пост-метод (/api/Data/upload): принимает файл . возвращает статус 201 в случае успеха или 422 + ошибка в случае провала
- гет-метод (/api/Data/values): отправляет список из 10 последних значений из таблицы Value
- гет-метод (/api/Data/result): НЕДОСТУПЕН . не успел разработать.

## Особенности
### В данном Api нет графического интерфейса.
### Для просмотра и использования, пользуйтесь swagger-ом (http(s):localhost:PORT/swagger) в режиме дебагера

## Настройка перед работой
### Скорее всего потребуется ручками изменить конфигурацию файла appsettings.json в строке ConnectionString под своё окружение
### Возможно потребуется самим сделать в .net cli миграцию dotnet ef migrations add init --context models/appdbcontext --project webapicsvparser.csproj --startup-project webapicsvparser.csproj

## Используемые технологии
- .NET 9
- EF Core
- PostgreSQL
- Swagger
