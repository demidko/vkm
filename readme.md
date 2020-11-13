## vkm

Кроссплатформенная нативная утилита загружает вашу музыку из ВК. Для сборки потребуется [.NET SDK 5](https://dot.net),
для runtime зависимостей нет.

### Как собирать?

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

### Как запускать?

Чтобы увидеть параметры утилиты наберите:

```
./vkm --help
```

### Как запускать напрямую из исходного кода?

В директории репозитория выполните команду `dotnet run`, передав ей после `--` параметры приложения, например:

```
dotnet run -- --help
```

##### Благодарность хабраюзеру [@SuperHackerVk](https://habr.com/ru/users/superhackervk) за [способ получения mp3 ссылки](https://habr.com/ru/post/519302/)


