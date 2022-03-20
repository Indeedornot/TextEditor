using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

using TextEditor.Models;
using TextEditor.ViewModels.Events;

namespace TextEditor.ViewModels;

public partial class TextPresenterViewModel : ObservableRecipient
{
    public TextPresenterViewModel()
    {
        Messenger.Register<SingleFileChosenEvent>(this, (r, m) => ReceiveFile(m.Value));
        Messenger.Register<SaveContentEvent>(this, (r, m) =>
        {
            if (string.IsNullOrEmpty(TextBoxContent)) return;
            SaveFile(ChosenFileItem.Path);
        });
    }

    [ObservableProperty]
    private List<string> _fileContent = new();

    [ObservableProperty]
    private bool _isReadOnly;

    private readonly string _tempSaveDirectory =
        Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.FullName //TextEditor Path
        + @"\TemporaryFiles\";

    private FileItem _chosenFileItem = new();

    private FileItem ChosenFileItem
    {
        get => _chosenFileItem;
        set
        {
            SetProperty(ref _chosenFileItem, value);

            //Get ReadOnly and Content
            if (value.HasPath)
            {
                var fileInfo = new FileInfo(value.Path);
                IsReadOnly = fileInfo.IsReadOnly;
                GetFileContent(value.Path);
            }
            else
            {
                IsReadOnly = false;
                if (!string.IsNullOrEmpty(TextBoxContent))
                    TextBoxContent = string.Empty;
            }
        }
    }

    private string _textBoxContent = string.Empty;

    public string TextBoxContent
    {
        get => _textBoxContent;
        set
        {
            //If no file chosen
            if (!ChosenFileItem.IsValid)
            {
                Messenger.Send(new SingleFileChosenEvent(
                    new FileItem(DateTime.Now.ToFileTime().ToString(), null)
                ));
            }
            SetProperty(ref _textBoxContent, value);
        }
    }

    private void ReceiveFile(FileItem item)
    {
        if (item != ChosenFileItem)
            ChosenFileItem = item;
    }

    private void GetFileContent(string path)
    {
        if (string.IsNullOrEmpty(path) || !File.Exists(path)) TextBoxContent = string.Empty;

        var temp = File.ReadAllLines(path);
        FileContent = temp.ToList();
        TextBoxContent = string.Join(" ", FileContent);
    }

    private void SaveFile(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            File.WriteAllText(_tempSaveDirectory + DateTime.Now.ToFileTime() + ".txt", TextBoxContent);
        }
        else
        {
            File.WriteAllText(path, TextBoxContent);
        }
    }

    //[ObservableProperty]
    //private bool _isReadOnly = false;

    //async Task RunInBackground(TimeSpan timeSpan, Action action)
    //{
    //    var periodicTimer = new PeriodicTimer(timeSpan);
    //    while (await periodicTimer.WaitForNextTickAsync())
    //    {
    //        action();
    //    }
    //}

    //_ = RunInBackground(TimeSpan.FromSeconds(1), () => Console.WriteLine("Printing"));
}