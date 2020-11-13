using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;
using static System.IO.Directory;
using static System.IO.File;
using static Vk;
using CommandLine;

await Parser.Default.ParseArguments<Options>(args).WithParsedAsync(async options =>
{
    using var api = options.Login switch
    {
        "" => LoginToVkApiWithCache(),
        _ => LoginToVkApi(options.Login, options.Password)
    };

    var directory = options.Directory switch
    {
        "" => api.UserLink(),
        _ => options.Directory
    };

    if (!Directory.Exists(directory)) CreateDirectory(directory);

    Func<Audio, bool> filter = options.Title switch
    {
        "" => always => true,
        _ => x => x.Title.ToUpper().Contains(options.Title.ToUpper())
    };

    var audios = api.Audio
        .Get(new AudioGetParams {Count = 6000})
        .Where(filter)
        .Select(x => (
            Filename: $"{directory}/{x.Artist} - {x.Title}.mp3",
            Url: Regex.Replace(
                x.Url.ToString(),
                @"/[a-zA-Z\d]{6,}(/.*?[a-zA-Z\d]+?)/index.m3u8()",
                @"$1$2.mp3"
            )))
        .Where(x => !File.Exists(x.Filename));

    using var http = new HttpClient();

    foreach (var (filename, url) in audios)
        try
        {
            $"Downloading {filename}...".Log();
            WriteAllBytes(filename, await http.GetByteArrayAsync(url));
        }
        catch (Exception e)
        {
            e.LogError();
        }
});