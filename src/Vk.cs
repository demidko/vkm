using System.IO;
using Microsoft.Extensions.DependencyInjection;
using VkNet;
using VkNet.AudioBypassService.Extensions;
using VkNet.Model;
using static System.IO.File;
using static System.ConsoleColor;
using static VkNet.Enums.Filters.Settings;

/// <summary>
/// Модуль отвечает за авторизацию пользователя в ВК
/// </summary>
internal static class Vk
{
    /// <summary>
    /// Кеш содержит две строки (1 логин, 2 пароль)
    /// </summary>
    private const string CachePath = ".authorization";

    /// <summary>
    /// Метод входит в VK API под именем и паролем пользователя
    /// </summary>
    /// <param name="user">Пользователь</param>
    /// <returns>VK Api</returns>
    internal static VkApi LoginToVkApi(this (string Login, string Password) user)
    {
        // Включаем доступ к своим сообщениям и комментариям
        var api = new VkApi(new ServiceCollection().AddAudioBypass());
        api.Authorize(new ApiAuthParams
        {
            // Используем идентификатор который откопали где-то в интернете
            ApplicationId = 1980660,
            Login = user.Login,
            Password = user.Password,
            Settings = All
        });
        $"Login as vk.com/id{api.UserId}".Println(DarkBlue);
        WriteAllLines(CachePath, new[] {user.Login, user.Password});
        return api;
    }


    /// <summary>
    /// Метод получает VK API под именем и паролем пользователя из кеша
    /// </summary>
    /// <returns>VK Api</returns>
    internal static VkApi LoginToVkApi()
    {
        "Reading login and password from cache...".Println(DarkBlue);
        if (!Exists(CachePath))
            throw new FileNotFoundException(
                $"Authorization cache wasn't found. Please restart application with login and password"
            );
        var lines = ReadAllLines(CachePath);
        var (login, password) = lines.Length == 2
            ? (lines[0], lines[1])
            : throw new IOException(
                $"Invalid authorization cache. Please restart application with login and password"
            );
        return (login, password).LoginToVkApi();
    }
}