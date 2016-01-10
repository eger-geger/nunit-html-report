using System;
using System.IO;
using Newtonsoft.Json;

namespace NUnitReporter.EventReport
{
    /// <summary>
    ///     Stores and reads action reports on disk. 
    ///     Beware of <see cref="IOException"/> which may be thrown when writing or reading report failed.
    /// </summary>
    public class DiskStorage : IEventStorage
    {
        private readonly JsonSerializer _serializer;
        private readonly Lazy<DirectoryInfo> _workingDirectory;

        /// <summary>
        /// Initializes new instance of disk storage with given folder path
        /// </summary>
        /// <param name="workingDirectoryPath">Folder where reports will be stored</param>
        /// <exception cref="ArgumentNullException">Thrown when provided path is null or empty</exception>
        public DiskStorage(string workingDirectoryPath)
        {
            if (string.IsNullOrEmpty(workingDirectoryPath))
            {
                throw new ArgumentNullException(nameof(workingDirectoryPath));
            }

            _workingDirectory = new Lazy<DirectoryInfo>(() => CreateDirectoryIfNotExist(workingDirectoryPath));

            _serializer = JsonSerializer.CreateDefault(new JsonSerializerSettings
            {
               TypeNameHandling = TypeNameHandling.Objects,
               ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            });
        }

        /// <summary>
        /// Save report with given identifier on disk
        /// </summary>
        /// <param name="id">NUnit test unique identifier</param>
        /// <param name="report">Instance of action report</param>
        public void Save(string id, IEventReport report)
        {
            var outputFile = GetReportFile(id);

            if (outputFile.Exists)
            {
                throw new IOException($"Cannot save report to disk because file already exist: {outputFile.FullName}");
            }

            using (var writer = outputFile.CreateText())
            {
                _serializer.Serialize(writer, report);
            }
        }

        /// <summary>
        ///     Read action report from disk
        /// </summary>
        /// <param name="id">NUnit test unique identifier</param>
        /// <returns>Instance of action report associated with provided identifier</returns>
        public IEventReport Get(string id)
        {
            var reportFile = GetReportFile(id);

            if (!reportFile.Exists)
            {
                throw new ArgumentException($"File does not exist: {reportFile.FullName}", nameof(id));
            }

            using (var reader = new JsonTextReader(new StreamReader(reportFile.OpenRead())))
            {
                return _serializer.Deserialize<IEventReport>(reader);
            }
        }

        /// <summary>
        ///     Check if report associated with provided identifier exist
        /// </summary>
        /// <param name="id">NUnit test unique identifier</param>
        /// <returns>
        ///     <value>TRUE</value>
        ///     if test report exist, otherwise -
        ///     <value>FALSE</value>
        /// </returns>
        public bool Exist(string id)
        {
            return GetReportFile(id).Exists;
        }

        private FileInfo GetReportFile(string id)
        {
            return new FileInfo(Path.Combine(_workingDirectory.Value.FullName, $"{id}.json"));
        }

        private static DirectoryInfo CreateDirectoryIfNotExist(string path)
        {
            var directoryInfo = new DirectoryInfo(path);

            if (!directoryInfo.Exists)
            {
                try
                {
                    directoryInfo.Create();
                }
                catch (Exception ex)
                {
                    throw new ArgumentException($"Failed to create working folder: {path}", nameof(path), ex);
                }
            }

            return directoryInfo;
        }
    }
}