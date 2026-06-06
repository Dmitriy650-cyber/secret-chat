# SecretChat

.NET MAUI приложение для общения онлайн.

## Описание

Мобильное приложение для Android и iOS, позволяющее обмениваться сообщениями в режиме реального времени. 
Вы можете искать контакты, добавлять их, создавать и удалять чаты, а также изменять свой профиль и просматривать профили других пользователей.

## Особенности

- Реализация UI на XAML
- Интеграция с SignalR для реальных обновлений
- Entity Framework Core для ORM
- JWT авторизация

![image alt](https://github.com/Dmitriy650-cyber/secret-chat/blob/master/image.jpg?raw=true)

## Требования

- .NET 10.0
- SQL Server 2022+
- Visual Studio 2022/2026

## Установка и запуск

Для запуска приложения выполните следующую последовательность действий:

1. Клонируйте репозиторий
2. В настройках запуска проекта выберите **Api**
3. В файле `SecretChat.Api > appsettings.json > ConnectionStrings > Default` измените адрес на вашу строку подключения
4. В настройках запуска проекта создайте туннель разработчика (публичный, общедоступный)
5. Измените `SecretChat.Api > Properties > launchSettings.json > https.launchBrowser` на `true`
6. Запустите **Api**, выбрав ваш туннель разработчика
7. В открывшейся странице браузера скопируйте url адрес до `/Swagger`
8. Закройте **Api** и измените `SecretChat.Api > Properties > launchSettings.json > https.launchBrowser` на `false`
9. Скопированный путь вставьте в `SecretChat.Shared > AppConstants > ApiBaseUrl` вместо моего
10. Этот же путь вставьте в `SecretChat.Api > appsettings.json > Domain` вместо моего
12. Запустите **Api** в режиме без отладки
13. Запустите **Mobile** на эмуляторе или физическом устройстве

## Участники

- [Dmitriy Babkov](https://github.com/Dmitriy650-cyber) — разработчик

## Лицензия

MIT License
