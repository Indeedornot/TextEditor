using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TextEditor.Helpers;

public static class WritableItems
{
    public static bool IsDirectoryWritable(string dirPath, bool throwIfFails = false)
    {
        try
        {
            using (FileStream fs = File.Create(
                       Path.Combine(
                           dirPath,
                           Path.GetRandomFileName()
                       ),
                       1,
                       FileOptions.DeleteOnClose)
                  )
            { }
            return true;
        }
        catch
        {
            if (throwIfFails)
                throw;
            else
                return false;
        }
    }

    public static bool IsFileWritable(string filePath, bool throwIfFails = false)
    {
        try
        {
            using var fs = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
        }
        catch (IOException)
        {
            if (throwIfFails) throw;
            return false;
        }
        return true;
    }

    public static bool SaveFile(string path, string content)
    {
        if (string.IsNullOrEmpty(path)) return false;
        if (File.Exists(path) && IsFileWritable(path))
        {
            File.WriteAllText(path, content);
        }
        else if (Directory.Exists(path) && IsDirectoryWritable(path))
        {
            File.WriteAllText(path + DateTime.Now.ToFileTime() + ".txt", content);
        }

        return true;
    }

    public static string[] GetFileContentArray(string path)
    {
        if (string.IsNullOrEmpty(path) || IsFileLocked(new FileInfo(path))) return Array.Empty<string>();

        return File.ReadAllLines(path);
    }

    public static List<string> GetFileContentList(string path)
    {
        return GetFileContentArray(path).ToList();
    }

    public static string GetFileContnetString(string path)
    {
        return string.Join("\n", GetFileContentArray(path));
    }

    public static bool IsFileLocked(FileInfo file)
    {
        try
        {
            using var stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            stream.Close();
        }
        catch (IOException)
        {
            //the file is unavailable because it is:
            //still being written to
            //or being processed by another thread
            //or does not exist (has already been processed)
            return true;
        }

        //file is not locked
        return false;
    }
}