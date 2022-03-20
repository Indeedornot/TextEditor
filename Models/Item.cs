namespace TextEditor.Models;

public class Item
{
    public string Name { get; set; }
    public string Path { get; set; }

    //File can have path and name, or only name, but always a name
    public bool IsValid => !string.IsNullOrEmpty(Name);
    public bool HasPath => !string.IsNullOrEmpty(Path);
}