using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnitReporter.EventReport;

namespace NUnitReporterTests
{
    public class DummyTest
    {
        [Test]
        public void GenerateTestReport()
        {
            var report = new DefaultEventReport();
            
            report.RecordEvent("Open Chrome browser");    
            report.RecordEvent("Open login page");

            var signInId = report.RecordActivityStarted("SingIn", "admin", "admin-password");

            var enterUserNameId = report.RecordActivityStarted("enter username", "admin");
            report.RecordEvent("findElement", "By.CssSelector", "#username");
            report.RecordEvent("sendKeys", "admin");
            report.RecordActivityFinished(enterUserNameId);

            var enterPasswordId = report.RecordActivityStarted("enter password", "admin-password");
            report.RecordEvent("findElement", "By.CssSelector", "#password");
            report.RecordEvent("sendKeys", "admin-password");
            report.RecordActivityFinished(enterPasswordId);

            report.RecordActivityFinished(signInId);

            new DiskStorage(TestContext.CurrentContext.TestDirectory).Save("test_report", report);
        }

    }
}
