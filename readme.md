## vkm

Кроссплатформенная нативная утилита загружает вашу музыку из ВК. Для сборки потребуется [.NET SDK 5](https://dot.net),
для runtime зависимостей нет.

### Как собирать?

В директории репозитория выполните команду:

```
dotnet publish -c Release -r RID -p:PublishSingleFile=true -p:PublishTrimmed=true
```

Где вместо `RID` должен стоять идентификатор системы: `linux-x64`, `linux-arm`, ` osx-x64`, `win-x64` или `win-x86`
(список остальных можно посмотреть в [каталоге](https://docs.microsoft.com/en-us/dotnet/core/rid-catalog)).  
Пример:

```
dotnet publish -c Release -r osx-x64 -p:PublishSingleFile=true -p:PublishTrimmed=true
```

Смотрите также: [примеры сборки](https://github.com/dotnet/designs/blob/main/accepted/2020/single-file/design.md).

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

### TODO

* Починить закачку аудио по ссылкам вида `https://psv4.vkuseraudio.net/*`:  скормить hls ffmpeg'у


