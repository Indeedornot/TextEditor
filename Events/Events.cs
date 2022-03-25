using System.Collections.Generic;

using CommunityToolkit.Mvvm.Messaging.Messages;

using TextEditor.Models;

namespace TextEditor.Events;

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

/// <summary>
/// Returns tuple string - temporarysavepath, string[] - supported types 
/// </summary>
public sealed class SaveContentEvent : ValueChangedMessage<(string, string[])>
{
    public SaveContentEvent((string, string[]) value) : base(value)
    {
    }
}

public class TemporarySaveFolderMessage : RequestMessage<string>

{
}

public class SupportedTypesMessage : RequestMessage<string[]>

{
}

