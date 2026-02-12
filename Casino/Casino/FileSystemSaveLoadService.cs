using System;
using System.IO;

namespace Casino.Services
{
    public class FileSystemSaveLoadService : ISaveLoadService<string>
    {
        private readonly string _path;

        public FileSystemSaveLoadService(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Path cannot be null or empty.");
            }

            _path = path;
        }

        public void SaveData(string data, string identifier)
        {
            EnsureDirectoryExists();

            string filePath = GetFilePath(identifier);
            File.WriteAllText(filePath, data);
        }

        public string LoadData(string identifier)
        {
            EnsureDirectoryExists();

            string filePath = GetFilePath(identifier);

            if (!File.Exists(filePath))
            {
                return null;
            }

            return File.ReadAllText(filePath);
        }

        private void EnsureDirectoryExists()
        {
            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }
        }

        private string GetFilePath(string identifier)
        {
            return Path.Combine(_path, $"{identifier}.txt");
        }
    }
}
