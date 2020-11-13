using CommandLine;

internal struct Options
{
    [Option('t', "title", Required = false,
        HelpText = "The name of your track. If the parameter is missing, all your music will be loaded.")]
    public string Title { init; get; }

    [Option('l', "title", Required = false,
        HelpText =
            "Your phone number or email to enter VK. If the parameter is missing, the previous login will be used.")]
    public string Login { init; get; }

    [Option('l', "title", Required = false,
        HelpText =
            "The VK password is not transferred anywhere from your computer. If the parameter is missing, the previous password will be used.")]
    public string Password { init; get; }

    [Option('p', "path", Required = false,
        HelpText =
            "Music download directory. If the directory does not exist, it will be created. The default is the current directory")]
    public string Path { init; get; }
}