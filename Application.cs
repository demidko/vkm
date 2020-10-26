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
using static Vk;

var vk = args.Length switch
{
    1 => LoginToVkApi(),
    3 => LoginToVkApi(args[0], args[1]),
    _ => throw new ArgumentException(
        "Invalid arguments. Usage:\n" +
        "  With authorization data: dotnet vkm [login] [password] [audio]\n" +
        "  With cache: dotnet vkm [audio]"
    )
};

var lemma = args.Last().ToUpperInvariant();

var audios = vk.Audio.Get(new AudioGetParams { Count = 6000 })
    .Where(x => x.Title.ToUpperInvariant().Contains(lemma))
    .Select(x => (x.Title, Url: Regex.Replace(
        x.Url.ToString(),
        @"/[a-zA-Z\d]{6,}(/.*?[a-zA-Z\d]+?)/index.m3u8()",
        @"$1$2.mp3"
    )));

using var http = new HttpClient();

foreach (var (title, url) in audios)
{
    $"Downloading {title}...".Println(DarkBlue);
    await WriteAllBytesAsync($"{title}.mp3", await http.GetByteArrayAsync(url));
}