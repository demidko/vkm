using System;
using System.Collections.Immutable;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VkNet.Model.Attachments;
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
            .Select(x => (x.Title, Url: x.GetMp3Url()));

        using var http = new HttpClient();

        foreach (var (title, url) in audios)
        {
            $"Downloading {title}".Println(DarkBlue);
            await WriteAllBytesAsync($"{title}.mp3", await http.GetByteArrayAsync(url));
        }
    }

    private static string GetMp3Url(this Audio audio) => Regex.Replace(
        audio.Url.ToString(),
        @"/[a-zA-Z\d]{6,}(/.*?[a-zA-Z\d]+?)/index.m3u8()",
        @"$1$2.mp3"
    );
}