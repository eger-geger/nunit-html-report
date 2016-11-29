using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CommandLine;

[assembly: AssemblyTitle("NUnitReporter")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("NUnitReporter")]
[assembly: AssemblyCopyright("Copyright ©  2015")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: ComVisible(false)]

[assembly: Guid("4c567c1d-1959-487d-b42e-f745b235ede1")]

[assembly: InternalsVisibleTo("NUnitReporterTests")]
[assembly: AssemblyVersion("1.2.0.0")]
[assembly: AssemblyFileVersion("1.2.0.0")]
[assembly: AssemblyUsage(
    "nure.exe TestResult.xml",
    "nure.exe TestResult.xml -o path/to/generated/report",
    "nure.exe TestResult.xml -a path/to/screenshots -o path/to/generated/report"
    )]
