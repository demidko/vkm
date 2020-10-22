using System;
using System.Collections.Immutable;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VkNet.Model.RequestParams;
using static System.ConsoleColor;
using static System.IO.File;

internal static class Application
{
    internal const string Guide =
        "Invalid arguments. Usage:\n" +
        "  With authorization data: dotnet vkm [login] [password] [audio]\n" +
        "  With cache: dotnet vkm [audio]";

    private static async Task Main(string[] args)
    {
        var lemma = args.LastOrDefault()?.ToUpperInvariant()
                    ?? throw new ArgumentException(Guide);

        var audios = args.SkipLast(1).ToImmutableList()
            .LoginForVkApi()
            .Audio.Get(new AudioGetParams {Count = 6000})
            .Where(x => x.Title.ToUpperInvariant().Contains(lemma))
            .Select(x => (x.Title, Url: x.Url.RestoreMp3()));

        using var http = new HttpClient();

        foreach (var (title, url) in audios)
        {
            $"Downloading {title}".Println(DarkBlue);
            await WriteAllBytesAsync($"{title}.mp3", await http.GetByteArrayAsync(url));
        }
    }

    private static string RestoreMp3(this Uri url) => Regex.Replace(
        url.ToString(),
        @"/[a-zA-Z\d]{6,}(/.*?[a-zA-Z\d]+?)/index.m3u8()",
        @"$1$2.mp3"
    );
}