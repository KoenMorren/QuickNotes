namespace QuickNoteApp.Configuration;

public class FontConfig
{
    public string FontName { get; set; } = "Cascadia Code";
    public float FontSize { get; set; } = 14f;
    public bool EnableLigatures { get; set; } = false; // TODO does not work
}