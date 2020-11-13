using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommandLine;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;
using static System.ConsoleColor;
using static System.IO.Directory;
using static System.IO.File;
using static Vk;


await Parser.Default.ParseArguments<Options>(args).WithParsedAsync(async options =>
{
    
    using var api = options.Login switch
    {
        "" => LoginToVkApi(),
        _ => LoginToVkApi(options.Login, options.Password)
    };

    Func<Audio, bool> filter = options.Title switch
    {
        "" => _ => true,
        _ => x => x.Title.ToUpperInvariant().Contains(options.Title)
    };

    using var http = new HttpClient();

    var audios = api.Audio
        .Get(new AudioGetParams {Count = 6000})
        .Where(filter)
        .Select(x => (Title: $"{x.Artist} - {x.Title}", Url: Regex.Replace(
            x.Url.ToString(),
            @"/[a-zA-Z\d]{6,}(/.*?[a-zA-Z\d]+?)/index.m3u8()",
            @"$1$2.mp3"
        )));

    if (!Directory.Exists(options.Path))
    {
        CreateDirectory(options.Path);
    }

    foreach (var (title, url) in audios)
    {
        $"Downloading {title}...".Println(DarkBlue);
        WriteAllBytes($"{options.Path}/{title}.mp3", await http.GetByteArrayAsync(url));
    }
});