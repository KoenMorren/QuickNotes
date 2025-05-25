using System;
using System.IO;

namespace QuickNoteApp.Configuration;

public class Config
{
    public string FilePath { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "quicknote.txt");
    public string Shortcut { get; set; } = "ALT+E";
    public bool DarkMode { get; set; } = false;
    public ResolutionConfig Resolution { get; set; } = new();
    public FontConfig Font { get; set; } = new();
}