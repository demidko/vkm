using System;
using static System.Console;
using static System.ConsoleColor;

internal static class Terminal
{
    internal static void Print<T>(this T obj, ConsoleColor color = Black)
    {
        var previousColor = ForegroundColor;
        ForegroundColor = color;
        Write(obj);
        ForegroundColor = previousColor;
    }

    internal static void Println<T>(this T obj, ConsoleColor color = Black)
    {
        obj.Print(color);
        WriteLine();
    }

    internal static string Input(string message, ConsoleColor color = Black)
    {
        message.Print(color);
        return ReadLine()!;
    }
}