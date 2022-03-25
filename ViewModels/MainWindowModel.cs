using System;
using System.IO;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

using TextEditor.Events;

namespace TextEditor.ViewModels
{
    public class MainWindowModel : ObservableRecipient
    {
        //request currently possible extensions
        //request temporary save path
        public MainWindowModel()
        {
            Messenger.Register<MainWindowModel, SupportedTypesMessage>(this, (r, m) => m.Reply(r._supportedExtensions));
            Messenger.Register<MainWindowModel, TemporarySaveFolderMessage>(this, (r, m) => m.Reply(r._tempSaveDirectory));
        }

        private string[] _supportedExtensions = new[] { ".txt" };

        private readonly string _tempSaveDirectory =
            Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.FullName //TextEditor Path
            + @"\TemporaryFiles\";
    }
}