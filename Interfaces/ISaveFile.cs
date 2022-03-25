namespace TextEditor
{
    public interface ISaveFile
    {
        public bool SaveFile(string path, string content, string[] supportedTypes);
    }
}