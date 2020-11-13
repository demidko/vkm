## vkm

Кроссплатформенная утилита загружает вашу музыку из ВК. Для сборки проекта потребуется [.NET SDK 5](https://dot.net), однако для работы приложения зависимости не нужны.

### Как собрать нативное self-executable приложение?
В директории репозитория выполнить команду:
```
dotnet publish -c Release -r RID -p:PublishSingleFile=true -p:PublishTrimmed=true
```
Где вместо `RID` должен стоять идентификатор вашей системы: `linux-x64`, `linux-arm`, `linux-x64`, ` osx-x64` `win-x64`
или `win-x86`.  
Список всех остальных идентификаторов можно посмотреть в [каталоге](https://docs.microsoft.com/en-us/dotnet/core/rid-catalog).  
Пример:
```
dotnet publish -c Release -r linux-x64 -p:PublishSingleFile=true -p:PublishTrimmed=true
```
После сборки приложение можно будет запускать из командной строки без каких либо runtime-зависимостей.


### Как запускать проект напрямую из исходного кода?

В директории репозитория передать после  команды `dotnet run --` параметры приложения:

```
dotnet run -- --help
```

##### Благодарность хабраюзеру [@SuperHackerVk](https://habr.com/ru/users/superhackervk) за [способ получения mp3 ссылки](https://habr.com/ru/post/519302/)


