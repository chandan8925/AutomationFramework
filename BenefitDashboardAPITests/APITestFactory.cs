using AventStack.ExtentReports;
using BenefitDashboardAPITests.HelpersAPI;
using BenefitsDashboardAPITests.APICalls;
using Microsoft.Extensions.Configuration;
using NUnit.Framework.Interfaces;

public class APITestFactory
{
    private IConfiguration _config;
    public string BaseUrl;
    public string Token;
    public EmployeeAPICalls EmployeeAPICalls;
    public ExtentReports report = new ExtentReports();
    public ExtentTest test;
    public ExtentManager extentManager = new ExtentManager();
    public HelperMethods HelperMethods ;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
       ExtentTestManager.CreateParentTest(GetType().Name);
    }

    [SetUp]
    public void Setup()
    {
        ExtentTestManager.CreateTest(TestContext.CurrentContext.Test.Name);
        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.test.json");

        _config = builder.Build();
        BaseUrl = _config["BaseUrl"]!;
        Token = _config["Token"]!;
        EmployeeAPICalls = new EmployeeAPICalls(BaseUrl, Token);
        HelperMethods = new HelperMethods();


    }

    [TearDown]
    public void AfterTest()
    {
        var status = TestContext.CurrentContext.Result.Outcome.Status;
        var stacktrace = string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace)
                ? ""
                : string.Format("<pre>{0}</pre>", TestContext.CurrentContext.Result.StackTrace);
        Status logstatus;
        var errorMessage = TestContext.CurrentContext.Result.Message;
        switch (status)
        {
            case TestStatus.Failed:
                logstatus = Status.Fail;
                break;

            case TestStatus.Inconclusive:
                logstatus = Status.Warning;
                break;

            case TestStatus.Skipped:
                logstatus = Status.Skip;
                break;

            default:
                logstatus = Status.Pass;
                break;
        }

        ExtentTestManager.GetTest().Log(logstatus, "Test ended with " + logstatus + stacktrace);
        ExtentManager.Instance.Flush();
    }
}
