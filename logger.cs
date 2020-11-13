using System;
using static System.Console;
using static System.ConsoleColor;
using static System.String;

internal static class Logger
{
    internal static void Log<T>(this T obj, string withTag = "", ConsoleColor withColor = DarkBlue)
    {
        var previousColor = ForegroundColor;
        ForegroundColor = withColor;
        if (withTag is not "") Write($"{withTag} -- ");
        WriteLine(obj);
        ForegroundColor = previousColor;
    }

    internal static void LogError<T>(this T obj) => obj.Log(withColor: DarkRed);
    internal static void LogWarn<T>(this T obj) => obj.Log(withColor: DarkYellow);
}