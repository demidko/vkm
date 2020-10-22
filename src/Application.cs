using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using VkNet;
using VkNet.Exception;
using VkNet.Model.RequestParams;
using static System.TimeSpan;
using static OpenQA.Selenium.By;
using static Vk;


internal static class Application
{
    private const string UrlPattern = @"/[a-zA-Z\d]{6,}(/.*?[a-zA-Z\d]+?)/index.m3u8()";

    private const string ReplaceUrlPattern = @"\1\2.mp3";

    internal const string UsageGuide = "Invalid arguments. Usage:\n" +
                                       "  With authorization data: dotnet vkm [login] [password] [audio]\n" +
                                       "  With cache: dotnet vkm [audio]";

    private static void Main(string[] args)
    {
        var lemma = args
            .LastOrDefault()
            ?.ToLowerInvariant() ?? throw new ArgumentException(UsageGuide);
        var audio = args
            .SkipLast(1)
            .ToImmutableList()
            .LoginForVkApi()
            .Audio
            .Get(new AudioGetParams {Count = 6000})
            .Where(x => x.Title.ToLowerInvariant().Contains(lemma))
            .Select(x => Regex.Replace(x.Url.ToString(), UrlPattern, ReplaceUrlPattern));

        foreach (var i in audio)
        {
            Console.WriteLine(i);
        }
    }
}