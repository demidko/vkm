using System.IO;
using Microsoft.Extensions.DependencyInjection;
using VkNet;
using VkNet.AudioBypassService.Extensions;
using VkNet.Model;
using static System.IO.File;
using static VkNet.Enums.Filters.Settings;

internal static class Vk
{
    private const string Cache = ".authorization";

    internal static VkApi LoginToVkApi(string withLogin, string andPassword)
    {
        // Включаем доступ к своим сообщениям, комментариям и музыке
        var api = new VkApi(new ServiceCollection().AddAudioBypass());
        api.Authorize(new ApiAuthParams
        {
            // Используем идентификатор который откопали где-то в интернете
            ApplicationId = 1980660,
            Login = withLogin,
            Password = andPassword,
            Settings = All
        });
        $"Login as vk.com/{api.Account.GetProfileInfo().ScreenName ?? $"id{api.UserId}"}".Log();
        WriteAllLines(Cache, new[] {withLogin, andPassword});
        return api;
    }

    internal static VkApi LoginToVkApiWithCache()
    {
        "Reading login and password from cache...".Log();
        if (!Exists(Cache))
            throw new FileNotFoundException(
                $"Authorization cache wasn't found. Please restart application with login and password"
            );
        return ReadAllLines(Cache) switch
        {
            {Length: 2} lines => LoginToVkApi(withLogin: lines[0], andPassword: lines[1]),
            _ => throw new IOException(
                $"Invalid authorization cache. Please restart application with login and password"
            )
        };
    }
}