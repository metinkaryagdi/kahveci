using System;
using System.IO;

public class FileLogger
{
    private readonly string _filePath;

    public FileLogger(string filePath)
    {
        _filePath = filePath;
    }

    public void Log(string message)
    {
        // Log mesajını dosyaya yaz
        using (var writer = new StreamWriter(_filePath, append: true))
        {
            writer.WriteLine($"{DateTime.Now}: {message}");
        }
    }
}
