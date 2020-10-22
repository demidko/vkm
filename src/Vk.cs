using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using VkNet;
using VkNet.AudioBypassService.Extensions;
using VkNet.Model;
using static System.IO.File;
using static System.ConsoleColor;
using static Application;
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
    /// Метод входит под именем и паролем пользователя
    /// </summary>
    /// <param name="args">логин + пароль или пустой список</param>
    /// <returns>VK Api</returns>
    internal static VkApi LoginForVkApi(this IReadOnlyList<string> args)
    {
        // Включаем доступ к своим сообщениям и комментариям
        var api = new VkApi(new ServiceCollection().AddAudioBypass());
        var (login, password) = LoadAuthorizationData(args);
        api.Authorize(new ApiAuthParams
        {
            // Используем идентификатор который откопали где-то в интернете
            ApplicationId = 1980660,
            Login = login,
            Password = password,
            Settings = All
        });
        $"Login as vk.com/id{api.UserId}".Println(DarkBlue);
        return api;
    }

    /// <summary>
    /// Метод загружает пару (логин, пароль) с помощью аргументов
    /// </summary>
    private static (string Login, string Password) LoadAuthorizationData(IReadOnlyList<string> args) =>
        args.Count switch
        {
            0 => LoadAuthorizationDataFromCache(),
            2 => LoadAuthorizationDataFromInput(args),
            _ => throw new ArgumentException(Guide)
        };


    /// <summary>
    /// Метод извлекает пару (логин, пароль) из аргументов
    /// </summary>
    private static (string Login, string Password) LoadAuthorizationDataFromInput(IReadOnlyList<string> args)
    {
        WriteAllLines(CachePath, args);
        return (args[0], args[1]);
    }

    /// <summary>
    /// Метод извлекает пару (логин, пароль) из кеша
    /// </summary>
    private static (string Login, string Password) LoadAuthorizationDataFromCache()
    {
        "Reading login and password from cache...".Println(DarkBlue);
        if (!Exists(CachePath))
            throw new FileNotFoundException(
                $"Authorization cache wasn't found. Please restart application with login and password"
            );
        var lines = ReadAllLines(CachePath);
        return lines.Length == 2
            ? (lines[0], lines[1])
            : throw new IOException(
                $"Invalid authorization cache. Please restart application with login and password"
            );
    }
}