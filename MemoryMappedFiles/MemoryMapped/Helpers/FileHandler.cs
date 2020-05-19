using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MemoryMapped.Helpers
{
    static public class FileHandler
    {
        public const string FilesDirectory = "files";
        static public bool Validate(string fileName)
            => Directory.Exists(FilesDirectory) && File.Exists(Path.Combine(FilesDirectory, fileName));

        static public void ValidateAndCreate(string fileName)
        {
            if (Validate(fileName))
                Directory.Delete(FilesDirectory, true);

            if (!Directory.Exists(FilesDirectory))
                Directory.CreateDirectory(FilesDirectory);

            File.Create(Path.Combine(FilesDirectory, fileName)).Close();
        }
    }
}
