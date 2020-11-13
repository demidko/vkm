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


Parser.Default.ParseArguments<Options>(args).WithParsed(async args =>
{
    var api = args.Login switch
    {
        "" => LoginToVkApi(),
        _ => LoginToVkApi(args.Login, args.Password);
    };

    if (!Directory.Exists(args.Path))
    {
        CreateDirectory(args.Path);
    }

    Func<Audio, bool> filter = args.Title switch
    {
        "" => _ => true,
        _ => x => x.Title.ToUpperInvariant().Contains(args.Title)
    };

    var audios = api.Audio
        .Get(new AudioGetParams {Count = 6000})
        .Where(filter)
        .Select(x => (x.Title, Url: Regex.Replace(
            x.Url.ToString(),
            @"/[a-zA-Z\d]{6,}(/.*?[a-zA-Z\d]+?)/index.m3u8()",
            @"$1$2.mp3"
        )));

    Func<string, Task<byte[]>> download = new HttpClient().GetByteArrayAsync;

    foreach (var (title, url) in audios)
    {
        $"Downloading {title}...".Println(DarkBlue);

        WriteAllBytes($"{title}.mp3", await download(url));
    }
});