using System;
using static System.Console;
using static System.ConsoleColor;

internal static class Logger
{
    internal static void Log<T>(this T obj, ConsoleColor withColor = DarkBlue)
    {
        var previousColor = ForegroundColor;
        ForegroundColor = withColor;
        WriteLine(obj);
        ForegroundColor = previousColor;
    }
}