using System;
using System.IO;

namespace NUnitReporter
{
    public static class Validate
    {
        public static void FilePath(String filePath, String nullOrEmptyMessage = null)
        {
            if (String.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException(nullOrEmptyMessage ?? "File path is null or empty");
            }

            if (!File.Exists(filePath))
            {
                throw new ArgumentException(String.Format("File [{0}] does not exist", filePath));
            }
        }

        public static void FolderPath(String folderPath, String nullOrEmptyMessage = null)
        {
            if (String.IsNullOrEmpty(folderPath))
            {
                throw new ArgumentException(nullOrEmptyMessage ?? "Folder path is null or empty");
            }

            if (!Directory.Exists(folderPath))
            {
                throw new ArgumentException(String.Format("Folder [{0}] does not exist", folderPath));
            }
        }
    }
}