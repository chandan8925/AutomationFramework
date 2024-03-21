using OpenQA.Selenium;
using BenefitsDashboardUITests.Helper;

namespace BenefitsDashboardUITests.Pages
{
    public class LoginPage
    {
        private IWebDriver _driver;
        private HelperClass _helper;

        public LoginPage(IWebDriver driver)
        {
            _driver = driver;
            _helper = new HelperClass(driver);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(7);
        }

        #region Elements

        private static By _userName = By.XPath("//input[@id='Username']");
        private static By _password = By.XPath("//input[@id='Password']");
        private static By _loginButton = By.XPath("//button[@type='submit']");

        #endregion Elements

        #region Methods

        /// <summary>
        /// Login into application
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public void LoginIntoApplication(string userName, string password)
        {
            _driver.FindElement(_userName).SendKeys(userName);
            _driver.FindElement(_password).SendKeys(password);
            _driver.FindElement(_loginButton).Click();
        }

        #endregion Methods
    }
}