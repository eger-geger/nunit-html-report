using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace NUnitReporter.Attachments
{
    public class ImageAttachmentProvider : IAttachmentProvider
    {
        private const String ImageElement = "image";
        private const String ImageListElement = "images";

        private static readonly String[] ImageFileSearchPatterns = {"*.jpg", "*.jpeg", "*.png"};

        private readonly String _imageFolderAbsolutePath;
        private readonly String _outputFolderAsolutePath;

        public ImageAttachmentProvider(string imageFolderPath, string outputFolderPath)
        {
            Validate.FolderExist(imageFolderPath, "imageFolderPath");

            if (String.IsNullOrEmpty(outputFolderPath))
            {
                throw new ArgumentException("Folder path is null or empty", "outputFolderPath");
            }

            _imageFolderAbsolutePath = Path.GetFullPath(imageFolderPath);
            _outputFolderAsolutePath = Path.GetFullPath(outputFolderPath);
        }

        public IEnumerable<String> Images
        {
            get
            {
                return ImageFileSearchPatterns
                    .SelectMany(pattern => Directory.GetFiles(_imageFolderAbsolutePath, pattern))
                    .Select(Path.GetFullPath);
            }
        }

        public bool HasAttachments
        {
            get { return Images.Any(); }
        }

        public string AttachmentElementName
        {
            get { return ImageElement; }
        }

        public string AttachmentListElementName
        {
            get { return ImageListElement; }
        }

        public IEnumerable<string> GetTestCaseAttachments(string testCaseId)
        {
            var pathSeparator = Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture);

            var outputFolderUri = new Uri(
                _outputFolderAsolutePath.EndsWith(pathSeparator) 
                    ? _outputFolderAsolutePath
                    : _outputFolderAsolutePath + pathSeparator, 
                    UriKind.Absolute);

            return Images
                .Where(path => (Path.GetFileNameWithoutExtension(path) ?? String.Empty).Contains(testCaseId))
                .Select(path => new Uri(path, UriKind.Absolute))
                .Select(outputFolderUri.MakeRelativeUri)
                .Select(uri => uri.ToString())
                .Select(Uri.UnescapeDataString);
        }
    }
}