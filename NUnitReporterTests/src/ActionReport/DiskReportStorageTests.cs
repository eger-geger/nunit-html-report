using System;
using System.IO;
using NUnit.Framework;
using NUnitReporter.ActionReport;

namespace NUnitReporterTests.ActionReport
{
    public class DiskReportStorageTests
    {
        private static readonly string WorkingDirectory = Path.Combine(TestContext.CurrentContext.TestDirectory,
            "disk_storage_tests");

        private readonly IActionReport _report = new DefaultActionReport();

        private readonly DiskReportStorage _storage = new DiskReportStorage(WorkingDirectory);

        [OneTimeSetUp]
        public void SetUpReport()
        {
            var actionGuid = _report.ActionStarted("take a rest", "couch");
            _report.ActionTaken("went home", "car", "legs");
            _report.ImageTaken("sleeping_beauty.png");
            _report.ActionFinished(actionGuid);
            _report.ErrorThrown(new InvalidOperationException("It's Monday noon. Get up and work!"));
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