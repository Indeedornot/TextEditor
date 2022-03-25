using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using TextEditor.Events;
using TextEditor.Models;

namespace TextEditor.ViewModels;

public partial class SidebarViewModel : ObservableRecipient
{
    public SidebarViewModel()
    {
        Messenger.Register<FilesChosenEvent>(this, (r, m) => ReceiveFiles(m.Value));
        Messenger.Register<FolderChosenEvent>(this, (r, m) => ReceiveFolder(m.Value));
        Messenger.Register<SingleFileChosenEvent>(this, (r, m) => ReceiveFile(m.Value));
    }

    [ObservableProperty] private string _folderName = string.Empty;
    [ObservableProperty] private ObservableCollection<Item> _files = new();

    private Item _selectedItem = new();

    private Item SelectedItem
    {
        get => _selectedItem;
        set
        {
            if (value is FileItem temp)
            {
                FileChosen(temp);
            }

            SetProperty(ref _selectedItem, value);
        }
    }

    private void ReceiveFiles(List<FileItem> fileItems)
    {
        foreach (var item in fileItems.Where(item => !Files.Contains(item)))
        {
            Files.Add(item);
        }
    }

    private void ReceiveFile(FileItem fileItem)
    {
        if (!Files.Contains(fileItem))
            Files.Add(fileItem);
    }

    private void ReceiveFolder(DirectoryItem dirItem)
    {
        var types = Messenger.Send<SupportedTypesMessage>().Response;
        FolderName = dirItem.Name;
        foreach (var fileItem in
                        dirItem.Items.Where
                            (item => item is FileItem
                                 && types.Contains(Path.GetExtension(item.Path)
                                 )))
        {
            ReceiveFile((FileItem)fileItem);
        }
    }

    private void FileChosen(FileItem chosenFile) => Messenger.Send(new SingleFileChosenEvent(chosenFile));

    [ICommand]
    private void SelectedItemChanged(object selectedItem)
    {

        if (selectedItem is not Item temp) return;
        SelectedItem = temp;
    }
    //TreeView
}