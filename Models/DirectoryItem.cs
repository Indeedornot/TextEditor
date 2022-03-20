using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TextEditor.Models;

public class DirectoryItem : Item
{
    public List<Item> Items { get; set; }

    public DirectoryItem()
    { }

    public DirectoryItem(string path)
    {
        Name = System.IO.Path.GetDirectoryName(path) ?? string.Empty;
        Path = path;
        Items = GetItems(Path);
    }

    public static List<Item> GetItems(string path)
    {
        var dirInfo = new DirectoryInfo(path);

        var items = new List<Item>();

        items.AddRange(dirInfo.GetDirectories()
            .Select(directory => new DirectoryItem
            {
                Name = directory.Name,
                Path = directory.FullName,
                Items = GetItems(directory.FullName)
            }).Cast<Item>().ToList());

        items.AddRange(dirInfo.GetFiles()
            .Select(file => new FileItem
            {
                Name = file.Name,
                Path = file.FullName
            }));

        return items;
    }
}
