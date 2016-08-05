# Nure (NUnit Report)

HTML report generator for NUnit3. Basicaly it takes nunit 3 TestResult.xlm file as input and produces human readable TestResult.html as an ouput. Following command will do that:

```
> path\to\nure.exe path\to\TestResult.xml -o path\to\output\folder
```
Report generator is available at Nuget as [Nure](https://www.nuget.org/packages/nure/). It is being insalled as solution level package.

## Screenhots

In addition to infomration contained in NUnit test result file report can include images and event log. Images can be anything with "\*.jpeg", "\*.jpg" or "\*.png" extensions. Files are being associated with tests based on test ID (*NUnit.Framework.TestContext.CurrentContext.Test.ID*): if file name contains test ID in st's name it belongs to given test. In order to generate report with images run followig command:

```
> path\to\nure.exe path\to\TestResult.xml -o path\to\output\folder -a path\to\attachments\folder
```

## Event Log

Event log are hierarchical list of events (actions). "Hierarchical" means that top-level events can have children which may have children on they own. In order to appear in report events need to be captured to *NUnitReporter.EventReport.IEventReport* and save with *NUnitReporter.EventReport.IEventStorage*:

```C#
//creating disk based storage using current nunit working directory
IEventStorage eventStorage = new DiskStorage(TestContext.CurrentContext.WorkDirectory);

IEventReportFactory reportFactory = new EventReportFactory();

//for every NUnit3 test factory will return separate report instance
IEventReport report = reportFactory.CurrentTestReport;

//id is reqired in order to finalize a complex action later on
String actionId = report.RecordActivityStarted("Search with Goole", "NUnit");
report.RecordEvent("Navigate to URL", "http://www.google.com");
report.RecordEvent("Enter search phrase", "NUnit");
report.RecordEvent("Click search button");
report.RecordActivityFinished(actionId);

//save report on disk
eventStorage.Save(TestContext.CurrentContext.Test.ID, report);
```

Event log is associated with a particular test based on test ID. In order to generate report containing event log use *-a* option to set path to folder used to save reports during test. If you want to have both images and event log be part of the same report place them into the same folder.

You can start capturing event by installing [Nure.EventReport](https://www.nuget.org/packages/Nure.EventReport/) Nuget package.

## Proxy Factory

While adding report entry by hand is relatively easy using it at scale can become cumbersome. *NUnitReporter.EventReport.ProxyFactory.ReportingProxyFactory* solves this by creating self-reporting proxies. It uses (Castle Dymamic Proxy)[http://www.castleproject.org/projects/dynamicproxy/] under the hood to create dynamic proxies. Dues to it's limatations mehods and properties need to be virtual in order to be recorded. Usage example:

```C#
IEventReportFactory reportFactory = new EventReportFactory();

var proxyFactory = new ReportingProxyFactory(reportFactory);

var reportingProxy = proxyFactory.Create<SomeObject>(constructorArg1, constructorArg2);
reportingProxy.DoStuff();
reportingProxy.DoOtherStuff();
```

Proxy object produced by *ReportingProxyFactory* will record every method and property (setting only) usage to event report. The way specific methd appears in the report can be customized with *NUnitReporter.EventReport.ProxyFactory.EventReportAttribute* placed on property or method. Currently it allows ignoring specific method or property.

In order to use in your project install [Nure.EventReport.ProxyFactory](https://www.nuget.org/packages/Nure.EventReport.ProxyFactory/) Nuget package. It requires [Nure.EventReport](https://www.nuget.org/packages/Nure.EventReport/) to be installed as well.
