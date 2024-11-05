using System.IO;
using System.Text.RegularExpressions;

namespace FunctionApp2.Services
{
    public class FileService
    {
        public string SaveFile(Stream fileStream, string fileName, string directoryPath)
        {
            Directory.CreateDirectory(directoryPath);
            var filePath = Path.Combine(directoryPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                fileStream.CopyTo(stream);
            }

            return filePath;
        }

        public string RenameFile(string filePath, string newFileName)
        {
            string directory = Path.GetDirectoryName(filePath);
            string extension = Path.GetExtension(filePath);

            if (!newFileName.EndsWith(extension))
            {
                newFileName += extension;
            }

            string newFilePath = Path.Combine(directory, newFileName);

            int count = 1;
            while (File.Exists(newFilePath))
            {
                newFileName = Path.GetFileNameWithoutExtension(newFileName) + $"({count++})" + extension;
                newFilePath = Path.Combine(directory, newFileName);
            }

            File.Move(filePath, newFilePath); 

            return newFilePath; 
        }


    }
}
