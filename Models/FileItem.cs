namespace TextEditor.Models;

public class FileItem : Item
{
    public FileItem(string name, string path)
    {
        Name = name;
        Path = path;
    }

    public FileItem(string path)
    {
        Name = System.IO.Path.GetFileName(path);
        Path = path;
    }

    public FileItem()
    { }
}