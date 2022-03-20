using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using TextEditor.Models;
using TextEditor.ViewModels.Events;

namespace TextEditor.ViewModels;

public partial class SidebarViewModel : ObservableRecipient
{
    public SidebarViewModel()
    {
        Messenger.Register<FilesChosenEvent>(this, (r, m) => ReceiveFiles(m.Value));
        Messenger.Register<FolderChosenEvent>(this, (r, m) => ReceiveFolder(m.Value));
        Messenger.Register<SingleFileChosenEvent>(this, (r, m) => ReceiveFile(m.Value));
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
        FolderName = dirItem.Name;
        foreach (var fileItem in dirItem.Items.Where(item => item is FileItem && item.Path.EndsWith(".txt")))
        {
            ReceiveFile((FileItem)fileItem);
        }
    }

    [ObservableProperty] private string _folderName = string.Empty;
    [ObservableProperty] private ObservableCollection<Item> _files = new();

    #region TreeViewLogic

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

    private void FileChosen(FileItem chosenFile) => Messenger.Send(new SingleFileChosenEvent(chosenFile));

    [ICommand]
    private void SelectedItemChanged(object selectedItem)
    {

        if (selectedItem is not Item temp) return;
        SelectedItem = temp;
    }
    //TreeView

    #endregion
}