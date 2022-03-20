using System.Collections.Generic;

using CommunityToolkit.Mvvm.Messaging.Messages;

using TextEditor.Models;

namespace TextEditor.ViewModels.Events;

public sealed class FilesChosenEvent : ValueChangedMessage<List<FileItem>>
{
    public FilesChosenEvent(List<FileItem> value) : base(value)
    {
    }
}

public sealed class SingleFileChosenEvent : ValueChangedMessage<FileItem>
{
    public SingleFileChosenEvent(FileItem value) : base(value)
    {
    }
}

public sealed class FolderChosenEvent : ValueChangedMessage<DirectoryItem>
{
    public FolderChosenEvent(DirectoryItem value) : base(value) { }
}

public sealed class SaveContentEvent
{
}

