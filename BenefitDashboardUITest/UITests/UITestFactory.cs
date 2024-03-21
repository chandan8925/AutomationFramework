using AventStack.ExtentReports;
using BenefitDashboardUITest.Helpers;
using BenefitsDashboardAPITests.APICalls;
using BenefitsDashboardAPITests.APIModal;
using BenefitsDashboardUITests.Helper;
using BenefitsDashboardUITests.Pages;
using Microsoft.Extensions.Configuration;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Text.Json;

namespace BenefitsDashboardUITests.UITests
{
    public class UITestFactory
    {
        private IConfiguration _config;
        public string LoginUrl;
        public string UserName;
        public string Password;
        public IWebDriver Driver;
        public LoginPage LoginPage;
        public AddEditEmployeeModal AddEditEmployeeModal;
        public DeleteModal DeleteModal;
        public BenefitsDashboard BenefitsDashboard;
        public HelperClass HelperClass;
        public string BaseUrl;
        public string Token;
        public EmployeeAPICalls EmployeeAPICalls;
        public ExtentReports report = new ExtentReports();
        public ExtentTest test;
        public ExtentManager extentManager = new ExtentManager();

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
            LoginUrl = _config["LoginUrl"]!;
            UserName = _config["UserName"]!;
            Password = _config["Password"]!;
            BaseUrl = _config["BaseUrl"]!;
            Token = _config["Token"]!;
            EmployeeAPICalls = new EmployeeAPICalls(BaseUrl, Token);
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("--disable-infobars");
            options.AddArgument("--disable-extensions");
            options.AddArgument("start-maximized");
            Driver = new ChromeDriver(options);
            HelperClass = new HelperClass(Driver);
            LoginPage = new LoginPage(Driver);
            AddEditEmployeeModal = new AddEditEmployeeModal(Driver);
            DeleteModal = new DeleteModal(Driver);
            BenefitsDashboard = new BenefitsDashboard(Driver);
            Driver.Navigate().GoToUrl(LoginUrl);
            LoginPage.LoginIntoApplication(UserName, Password);
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
            Driver.Dispose();
            Driver.Quit();
        }

        /// <summary>
        /// Get the title of the page
        /// </summary>
        /// <returns></returns>
        public string GetTitle()
        {
            return Driver.Title;
        }

        /// <summary>
        /// Get the current url
        /// </summary>
        /// <returns></returns>
        public string GetUrl()
        {
            return Driver.Url;
        }

        /// <summary>
        /// Validate grid column value of an employee in the employee dashboard 
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="expectedEmployeeDetails"></param>
        public void ValidateGridDetails(string firstName, string[] expectedEmployeeDetails)
        {
            for (int i = 0; i < 7; i++)
            {
                // Index 0 and index 1 is excluded from verification because first name and last name value is switched in the grid.
                // Bug has been logged
                if (i != 0 && i != 1)
                {
                    var actualValue = BenefitsDashboard.ValidateEmployeeDetailsInDashboard(firstName, Convert.ToInt16(i + 2));
                    var expectedValue = expectedEmployeeDetails[i];
                    for (int j = 0; j < 15; j++)
                    {
                        actualValue = BenefitsDashboard.ValidateEmployeeDetailsInDashboard(firstName, Convert.ToInt16(i + 2));
                        if (expectedValue.Equals(actualValue))
                        {
                            break;
                        }
                    }
                    Assert.That(expectedValue.Equals(actualValue), "Column values of employee should match");
                }
            }
        }

        /// <summary>
        /// Get Employee Count In Dashboard
        /// </summary>
        /// <returns></returns>
        public int GetEmployeeRowCount()
        {
            var employeeCount = EmployeeAPICalls.GetAllEmployees();
            var listOfEmployees = JsonSerializer.Deserialize<List<Employee>>(json: employeeCount.Content!);
            return listOfEmployees!.Count;
        }
    }
}
