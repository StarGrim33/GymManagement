# GymManagement

GymManagement - это приложение для управления тренажерным залом.

### Чистая архитектура

Проект следует принципам чистой архитектуры, что обеспечивает разделение ответственности и независимость от фреймворков. Основные слои включают:

- **Domain**: Содержит бизнес-логику и сущности.
- **Application**: Содержит интерфейсы и сервисы для использования в бизнес-логике.
- **Infrastructure**: Содержит реализации интерфейсов, взаимодействие с базой данных и внешними сервисами.
- **API**: Веб-интерфейс для взаимодействия с клиентами.

### CQRS и MediatR

Проект использует паттерн CQRS (Command Query Responsibility Segregation) для разделения операций чтения и записи. MediatR используется для обработки команд и запросов, что упрощает управление зависимостями и улучшает тестируемость.

### База данных PostgreSQL

Для хранения данных используется база данных PostgreSQL. Взаимодействие с базой данных осуществляется с помощью Entity Framework Core и Dapper.

### Keycloak

Внедрена система авторизации и аутентификации. 

### Outbox Pattern

Для обеспечения надежной доставки сообщений используется паттерн Outbox. Сообщения сохраняются в таблице `outbox_messages` и обрабатываются асинхронно.

## Требования

- .NET 9
- PostgreSQL
- Visual Studio 2022

## Установка

1. Клонируйте репозиторий:
    git clone https://github.com/yourusername/GymManagement.git
2. Перейдите в директорию проекта:
    cd GymManagement
3. Настройте строку подключения к базе данных в файле `appsettings.json`:
4. Выполните миграции для создания необходимых таблиц в базе данных:
    dotnet ef database update

## Запуск

1. Откройте проект в Visual Studio 2022.
2. Убедитесь, что проект `GymManagement.Api` установлен как стартовый.
3. Нажмите `F5` для запуска приложения.

## Использование

### Обработка сообщений Outbox

Класс `ProcessOutboxMessages` отвечает за обработку сообщений из таблицы `outbox_messages`. Он использует Dapper для взаимодействия с базой данных и MediatR для публикации событий домена.
