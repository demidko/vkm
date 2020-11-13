using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using CommandLine;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;
using static System.IO.Directory;
using static System.IO.File;
using static Vk;
using System.Linq;


await Parser.Default.ParseArguments<Options>(args).WithParsedAsync(async options =>
{
    using var api = options.Login switch
    {
        null or {Length: 0} => LoginToVkApiWithCache(),
        _ => LoginToVkApi(options.Login, options.Password)
    };

    Func<Audio, bool> filter = options.Title switch
    {
        null or {Length: 0} => _ => true,
        _ => x => x.Title.ToUpperInvariant().Contains(options.Title)
    };

    var audios = api.Audio
        .Get(new AudioGetParams {Count = 6000})
        .Where(filter)
        .Select(x => (Title: $"{x.Artist} - {x.Title}", Url: Regex.Replace(
            x.Url.ToString(),
            @"/[a-zA-Z\d]{6,}(/.*?[a-zA-Z\d]+?)/index.m3u8()",
            @"$1$2.mp3"
        )));

    if (!Directory.Exists(options.Path)) CreateDirectory(options.Path);

    using var http = new HttpClient();

    foreach (var (title, url) in audios)
    {
        $"Downloading {title}...".Log();
        WriteAllBytes($"{options.Path}/{title}.mp3", await http.GetByteArrayAsync(url));
    }
});