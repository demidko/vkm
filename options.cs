using System.Diagnostics.CodeAnalysis;
using CommandLine;
using static System.String;

internal class Options
{
    [Option('t', "title", Required = false,
        HelpText = "The name of your track. If the parameter is missing, all your music will be loaded.")]
    public string? Title { set; get; }
    
    [Option('l', "login", Required = false,
        HelpText =
            "Your phone number or email to enter VK. If the parameter is missing, the previous login will be used.")]
    public string? Login { set; get; }
    
    [Option('p', "password", Required = false,
        HelpText =
            "The VK password is not transferred anywhere from your computer. If the parameter is missing, the previous password will be used.")]
    public string? Password { set; get; }
    
    [Option('d', "directory", Required = false,
        HelpText =
            "Music download directory. If the directory does not exist, it will be created. The default is ./vk_music")]
    public string? Directory { set; get; }
}