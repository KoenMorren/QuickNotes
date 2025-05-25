using System;
using System.Windows.Input;

namespace QuickNoteApp.Shortcut;

public static class ShortcutParser
{
    public static (ModifierKeys modifiers, Key key) Parse(string hotkey)
    {
        var parts = hotkey.Split('+', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        ModifierKeys modifiers = ModifierKeys.None;
        Key? key = null;

        foreach (var part in parts)
        {
            switch (part.ToUpperInvariant())
            {
                case "CTRL":
                    modifiers |= ModifierKeys.Control;
                    break;
                case "ALT":
                    modifiers |= ModifierKeys.Alt;
                    break;
                case "SHIFT":
                    modifiers |= ModifierKeys.Shift;
                    break;
                case "WIN":
                case "WINDOWS":
                    modifiers |= ModifierKeys.Windows;
                    break;
                default:
                    key = MapToKey(part);
                    break;
            }
        }

        if (key == null)
            throw new FormatException($"Invalid key in hotkey string: {hotkey}");

        return (modifiers, key.Value);
    }

    private static Key MapToKey(string keyPart)
    {
        return keyPart switch
        {
            "`" => Key.Oem3, // This is the key for backtick/tilde on US keyboards
            "~" => Key.Oem3,
            _ => Enum.TryParse<Key>(keyPart, true, out var parsedKey)
                ? parsedKey
                : throw new ArgumentException($"Unknown key: {keyPart}")
        };
    }
}