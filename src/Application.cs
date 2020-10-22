using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using VkNet;
using VkNet.Exception;
using VkNet.Model.RequestParams;
using VkNet.Utils;
using static System.Text.RegularExpressions.Regex;
using static System.TimeSpan;
using static OpenQA.Selenium.By;
using static Vk;


internal static class Application
{
    internal const string UsageGuide = "Invalid arguments. Usage:\n" +
                                       "  With authorization data: dotnet vkm [login] [password] [audio]\n" +
                                       "  With cache: dotnet vkm [audio]";

    private static async Task Main(string[] args)
    {
        var lemma = args
            .LastOrDefault()
            ?.ToLowerInvariant() ?? throw new ArgumentException(UsageGuide);

        var downloader = new HttpClient();

        var audios = args.SkipLast(1)
            .ToImmutableList()
            .LoginForVkApi()
            .Audio
            .Get(new AudioGetParams {Count = 6000})
            .Where(audio => audio.Title.ToLowerInvariant().Contains(lemma))
            .Select(audio => (Filename: $"{audio.Title}.mp3", Url: audio.Url.RestoreMp3Url()));

        foreach (var (filename, url) in audios)
        {
            $"Downloading {filename}".Println(ConsoleColor.DarkBlue);
            var bytes = await downloader.GetByteArrayAsync(url);
            await File.WriteAllBytesAsync(filename, bytes);
        }
    }

    private static Uri RestoreMp3Url(this Uri url) => new Uri(Replace(
        url.ToString(),
        @"/[a-zA-Z\d]{6,}(/.*?[a-zA-Z\d]+?)/index.m3u8()",
        @"$1$2.mp3"
    ));
}