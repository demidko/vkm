using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using VkNet;
using VkNet.Abstractions.Core;
using VkNet.AudioBypassService.Extensions;
using VkNet.Model;
using VkNet.Utils.AntiCaptcha;
using static System.Console;
using static System.IO.File;
using static VkNet.Enums.Filters.Settings;

internal static class Vk
{
    private const string Cache = ".authorization";

    internal static string UserLink(this VkApi api) =>
        $"vk.com/{api.Account.GetProfileInfo().ScreenName ?? $"id{api.UserId}"}";

    internal static VkApi LoginToVkApi(string withLogin, string andPassword)
    {
        // Включаем доступ к своим сообщениям, комментариям и музыке
        var hooks = new ServiceCollection()
            .AddAudioBypass()
            .AddSingleton<PrimitiveCaptchaSolver>();
        var api = new VkApi(hooks);
        api.Authorize(new ApiAuthParams
        {
            // Используем идентификатор который откопали где-то в интернете
            ApplicationId = 1980660,
            Login = withLogin,
            Password = andPassword,
            Settings = All
        });
        $"Login as {api.UserLink()}".Log();
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

    private class PrimitiveCaptchaSolver : ICaptchaSolver
    {
        public string Solve(string url)
        {
            $"Please enter captcha: {url}".LogWarn();
            return ReadLine()!;
        }

        public void CaptchaIsFalse()
        {
            "Invalid captcha!".LogError();
        }
    }
}