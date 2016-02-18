using System;
using System.IO;
using NUnit.Framework;
using NUnitReporter.EventReport;

namespace NUnitReporterTests.EventReport
{
    public class DiskStorageTests
    {
        private static readonly string WorkingDirectory = Path.Combine(TestContext.CurrentContext.TestDirectory,
            "disk_storage_tests");

        private readonly IEventReport _report = new DefaultEventReport();

        private readonly DiskStorage _storage = new DiskStorage(WorkingDirectory);

        [OneTimeSetUp]
        public void SetUpReport()
        {
            var actionGuid = _report.RecordActivityStarted("take a rest", "couch");
            _report.RecordEvent("went home", "car", "legs");
            _report.RecordScreenshot("sleeping_beauty.png");
            _report.RecordActivityFinished(actionGuid);
            _report.RecordError(new InvalidOperationException("It's Monday noon. Get up and work!"));
        }

        [SetUp]
        public void CreateWorkingDirectory()
        {
            Directory.CreateDirectory(WorkingDirectory);
        }

        [TearDown]
        public void RemoveWorkingDirectory()
        {
            Directory.Delete(WorkingDirectory, true);
        }

        [Test]
        public void ShouldStoreActionReportOnDisk()
        {
            var testName = "should_store_report_on_disk";

            _storage.Save(testName, _report);

            Assert.That(File.Exists(Path.Combine(WorkingDirectory, $"{testName}.json")),
                "Failed to write report to disk");

            Assert.That(_storage.Exist(testName), 
                "Failed to find report on disk after it was written");

            Assert.That(_storage.Get(testName), Is.EqualTo(_report), 
                "Failed to read report from disk");
        }

        [Test]
        public void ShouldCreateFolderWhenItDoesNotExist()
        {
            RemoveWorkingDirectory();

            _storage.Save("should_create_folder", _report);

            Assert.That(Directory.Exists(WorkingDirectory), "Working directory was not created");

            Assert.That(File.Exists(Path.Combine(WorkingDirectory, "should_create_folder.json")),
                "The report was not written to disk");
        }
    }
}