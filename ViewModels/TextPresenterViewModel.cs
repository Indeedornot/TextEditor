using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

using TextEditor.Events;
using TextEditor.Helpers;
using TextEditor.Models;

namespace TextEditor.ViewModels;

public partial class TextPresenterViewModel : ObservableRecipient, ISaveFile
{
    public TextPresenterViewModel()
    {
        Messenger.Register<SingleFileChosenEvent>(this, (r, m) => ReceiveFile(m.Value));
        Messenger.Register<SaveContentEvent>(this, (r, m) =>
        {
            var path = ChosenFileItem.HasPath ? ChosenFileItem.Path : m.Value.Item1;
            SaveFile(path, TextBoxContent, m.Value.Item2);
        });
    }

    [ObservableProperty]
    private List<string> _fileContent = new();

    [ObservableProperty]
    private bool _isReadOnly;

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
                TextBoxContent = WritableItems.GetFileContnetString(value.Path);
            }
            else
            {
                IsReadOnly = false;
                if (HasTextBoxContnet)
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

            TextBoxLineCountFormatted = FormatLineCount(value.Split("\n").Length);

            if (!ChosenFileItem.IsValid)
            {
                Messenger.Send(new SingleFileChosenEvent(
                    new FileItem(DateTime.Now.ToFileTime().ToString(), null)
                ));
            }
            SetProperty(ref _textBoxContent, value);
        }
    }

    private bool HasTextBoxContnet => !string.IsNullOrEmpty(TextBoxContent);

    private string _textBoxLineCountFormatted = string.Empty;

    public string TextBoxLineCountFormatted
    {
        get { return _textBoxLineCountFormatted; }
        private set
        {
            SetProperty(ref _textBoxLineCountFormatted, value);
        }
    }

    private static string FormatLineCount(int lineCount)
    {
        return string.Join("\n", Enumerable.Range(0, lineCount));
    }

    private void ReceiveFile(FileItem item)
    {
        if (item != ChosenFileItem)
            ChosenFileItem = item;
    }

    public bool SaveFile(string path, string content, string[] supportedTypes)
    {
        return supportedTypes.Contains(Path.GetExtension(path)) && WritableItems.SaveFile(path, content);
    }
}