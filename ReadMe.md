## vkm
Кроссплатформенная утилита загружает вашу музыку из ВК.
### Как запускать проект напрямую из исходного кода?
1. Установить [.NET](https://dot.net)
2. В директории репозитория выполнить команду:  
`dotnet run [login] [password] [your-vk-saved-audio-name]`  
После этого можно запускать используя кеш предыдущей авторизации:  
`dotnet run [your-vk-saved-audio-name]`  

Пример скачивания музыки в первый раз:  
`dotnet run ivan@mail.ru 123456 'harpens kraft'`  

Пример последующих скачиваний:  
`dotnet run harpens`

##### Благодарность хабраюзеру [@SuperHackerVk](https://habr.com/ru/users/superhackervk) за [способ получения mp3 ссылки](https://habr.com/ru/post/519302/)


