# WebApiCSVParser
## почти готово!
### загляните через 2 часа 

## Доступное Api
- пост-метод : принимает файл . возвращает 201 или 422 + ошибка
- гет-метод : отправляет список из 10 последних значений из таблицы Value
- coming soon

## Особенности
### В данном Api нет графического интерфейса.
### Для просмотра и использования, пользуйтесь swagger-ом

## Настройка перед работой
### Скорее всего потребуется ручками изменить конфигурацию файла appsettings.json в строке ConnectionString под своё окружение
### Возможно потребуется самим сделать в .net cli миграцию dotnet ef migrations add init --context models/appdbcontext --project webapicsvparser.csproj --startup-project webapicsvparser.csproj

## Используемые технологии
- .NET 9
- EF Core
- PostgreSQL
- Swagger
