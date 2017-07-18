using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRealm.Utilities.Common.Helpers
{
    public sealed class FsHelper
    {
        public static string BaseOperationPath
        {
            get
            {
                return Path.Combine(Path.GetTempPath(), "MyRealm");
            }
        }

        public static void CreateDirectoryIfNotExists(string fullPath)
        {
            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);
        }

        public static void CreateFileIfNotExists(string directoryPath, string fileName, byte[] fileBytes)
        {
            CreateDirectoryIfNotExists(directoryPath);

            string fileFullPath = Path.Combine(directoryPath, fileName);

            if (File.Exists(fileFullPath))
                return;

            using (var writer = new FileStream(fileFullPath, FileMode.Create))
            {
                writer.Write(fileBytes, 0, fileBytes.Length);
            }

        }

        public static void CreateFileIfNotExists(string directoryPath, string fileName, string fileContents)
        {
            CreateDirectoryIfNotExists(directoryPath);

            string fileFullPath = Path.Combine(directoryPath, fileName);

            if (File.Exists(fileFullPath))
                return;

            using (var writer = new StreamWriter(fileFullPath, false))
            {
                writer.Write(fileContents);
            }
        }

        public static void DeleteFileIfExists(string fullPath)
        {
            if (!File.Exists(fullPath))
                return;

            File.Delete(fullPath);
        }

        public static string GetFileContents(string fileFullPath)
        {
            using (var reader = new StreamReader(fileFullPath))
            {
                return reader.ReadToEnd();
            }
        }


        public static byte[] GetFileContentsAsBytes(string fileFullPath)
        {
            return File.ReadAllBytes(fileFullPath);
        }
    }
}
