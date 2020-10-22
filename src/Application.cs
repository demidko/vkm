using System;
using System.IO;
using System.Threading;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using VkNet.Exception;
using static System.TimeSpan;
using static OpenQA.Selenium.By;
using static VkAuthorization;


internal static class Application
{
    private static void Main(string[] args)
    {
        var api = LoginForVkApi(args)
    }
}