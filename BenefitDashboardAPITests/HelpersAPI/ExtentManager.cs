using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Config;

namespace BenefitDashboardAPITests.HelpersAPI
{
    public class ExtentManager
    {
        public static readonly Lazy<ExtentReports> _lazy = new Lazy<ExtentReports>(() => new ExtentReports());

        public static ExtentReports Instance { get { return _lazy.Value; } }

        static ExtentManager()
        {
            var fileName = "Test API Execution Reports";
            var dir = AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin\\Debug", "");
            ExtentSparkReporter htmlReporter = new ExtentSparkReporter(dir + "\\Test_Execution_APIReports" + "\\APIAutomation_Report" + ".html");

            htmlReporter.Config.DocumentTitle = fileName;
            htmlReporter.Config.Theme = Theme.Standard;
            htmlReporter.Config.Encoding = "utf-8";
            htmlReporter.Config.ReportName = fileName;
            htmlReporter.Config.TimelineEnabled = true;
            Instance.AttachReporter(htmlReporter);
        }

        public ExtentManager()
        {
        }
    }
}