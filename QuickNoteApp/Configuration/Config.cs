using System;
using System.IO;

namespace QuickNoteApp.Configuration;

public class Config
{
    public string FilePath { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "quicknote.txt");
    public FontConfig Font { get; set; } = new();
}
