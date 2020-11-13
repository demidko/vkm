## vkm

Кроссплатформенная утилита загружает вашу музыку из ВК. Для сборки проекта понадобится
установить [.NET SDK 5](https://dot.net), однако для работы приложения фреймворк не требуется.

### Как собрать нативное self-executable приложение?

В директории репозитория выполнить команду:

```
dotnet publish -c Release -r RID -p:PublishSingleFile=true -p:PublishTrimmed=true
```

Где вместо `RID` должен стоять идентификатор вашей системы: `linux-x64`, `linux-arm`, `linux-x64`, ` osx-x64` `win-x64`
или `win-x86`.  
Список всех остальных идентификаторов можно посмотреть в [каталоге](https://docs.microsoft.com/en-us/dotnet/core/rid-catalog).  
Например:
```
dotnet publish -c Release -r linux-x64 -p:PublishSingleFile=true -p:PublishTrimmed=true
```

После сборки приложение можно будет запускать из командной строки без каких либо runtime-зависимостей.

### Как запускать готовое приложение?

### Как запускать проект напрямую из исходного кода?

В директории репозитория выполнить команду `dotnet run`, например`:

```
dotnet run -- --help
```

##### Благодарность хабраюзеру [@SuperHackerVk](https://habr.com/ru/users/superhackervk) за [способ получения mp3 ссылки](https://habr.com/ru/post/519302/)


