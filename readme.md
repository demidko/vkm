## vkm

Кроссплатформенная утилита загружает вашу музыку из ВК. Для сборки потребуется [.NET SDK 5](https://dot.net).

### Как собрать нативное self-executable приложение без runtime-зависимости от фреймворка?

В директории репозитория выполнить команду:

```
dotnet publish -c Release -r RID -p:PublishSingleFile=true -p:PublishTrimmed=true
```

Где вместо `RID` должен стоять идентификатор системы: `linux-x64`, `linux-arm`, `linux-x64`, ` osx-x64`, `win-x64`
или `win-x86` (список остальных можно посмотреть в [каталоге](https://docs.microsoft.com/en-us/dotnet/core/rid-catalog))
.

Пример:

```
dotnet publish -c Release -r linux-x64 -p:PublishSingleFile=true -p:PublishTrimmed=true
```

После этого можно посмотреть параметры утилиты:

```
./vkm --help
```

### Также существует возможность запускать утилиту напрямую из исходного кода

В директории репозитория после команды `dotnet run --` передать параметры приложения, например:

```
dotnet run -- --help
```

##### Благодарность хабраюзеру [@SuperHackerVk](https://habr.com/ru/users/superhackervk) за [способ получения mp3 ссылки](https://habr.com/ru/post/519302/)


