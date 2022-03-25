using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using TextEditor.Events;
using TextEditor.Models;

using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace TextEditor.ViewModels;

public partial class ToolbarViewModel : ObservableRecipient
{
    private List<FileItem> _fileItems = new();

    public List<FileItem> FileItems
    {
        get => _fileItems;
        set
        {
            Messenger.Send(new FilesChosenEvent(value));

            SetProperty(ref _fileItems, value);
        }
    }

    #region Commands

    [ICommand]
    private void New()
    {
        Messenger.Send(new SingleFileChosenEvent(
            new FileItem(DateTime.Now.ToFileTime().ToString(), null)
            ));
    }

    [ICommand]
    private void OpenFiles()
    {
        OpenFileDialog openFileDialog = new()
        {
            Multiselect = true,
            AddExtension = true,
            CheckFileExists = true,
            CheckPathExists = true,
            Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            var fileNames = openFileDialog.FileNames.ToList();
            var temp = fileNames.Select(t => new FileItem(t)).ToList();

            FileItems = temp;
        }
    }

    [ICommand]
    private void OpenFolder()
    {
        var dialog = new System.Windows.Forms.FolderBrowserDialog();
        System.Windows.Forms.DialogResult result = dialog.ShowDialog();
        if (result == System.Windows.Forms.DialogResult.OK)
        {
            Messenger.Send(new FolderChosenEvent(new DirectoryItem(dialog.SelectedPath)));
        }
    }

    [ICommand]
    private void Save()
    {
        var types = Messenger.Send<SupportedTypesMessage>().Response;
        var temporarySavePath = Messenger.Send<TemporarySaveFolderMessage>().Response;
        Messenger.Send(new SaveContentEvent((temporarySavePath, types)));
    }

    [ICommand]
    private void Cut()
    { }

    [ICommand]
    private void Copy()
    { }

    [ICommand]
    private void Paste()
    { }

    #endregion Commands
}