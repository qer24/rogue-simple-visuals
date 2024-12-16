using System;
using UnityEngine;

public static class ColorConverter
{
    /// <summary>
    /// Converts a System.ConsoleColor to UnityEngine.Color
    /// </summary>
    /// <param name="consoleColor">The ConsoleColor to convert</param>
    /// <returns>Equivalent UnityEngine.Color</returns>
    public static Color ToUnityColor(ConsoleColor consoleColor)
    {
        return consoleColor switch
        {
            ConsoleColor.Black       => Color.black,
            ConsoleColor.Blue        => Color.blue,
            ConsoleColor.Cyan        => Color.cyan,
            ConsoleColor.DarkBlue    => new Color(0f, 0f, 0.5f),
            ConsoleColor.DarkCyan    => new Color(0f, 0.5f, 0.5f),
            ConsoleColor.DarkGray    => Color.gray,
            ConsoleColor.DarkGreen   => new Color(0f, 0.5f, 0f),
            ConsoleColor.DarkMagenta => new Color(0.5f, 0f, 0.5f),
            ConsoleColor.DarkRed     => new Color(0.5f, 0f, 0f),
            ConsoleColor.DarkYellow  => new Color(0.5f, 0.5f, 0f),
            ConsoleColor.Gray        => Color.gray,
            ConsoleColor.Green       => Color.green,
            ConsoleColor.Magenta     => Color.magenta,
            ConsoleColor.Red         => Color.red,
            ConsoleColor.White       => Color.white,
            ConsoleColor.Yellow      => Color.yellow,
            _                        => Color.white // Default case
        };
    }

    /// <summary>
    /// Converts a System.ConsoleColor to UnityEngine.Color with optional alpha
    /// </summary>
    /// <param name="consoleColor">The ConsoleColor to convert</param>
    /// <param name="alpha">Optional alpha value (0-1)</param>
    /// <returns>Equivalent UnityEngine.Color with specified alpha</returns>
    public static Color ToUnityColor(ConsoleColor consoleColor, float alpha)
    {
        Color baseColor = ToUnityColor(consoleColor);
        return new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
    }
}
