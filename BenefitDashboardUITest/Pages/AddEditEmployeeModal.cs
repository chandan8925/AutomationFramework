using OpenQA.Selenium;
using BenefitsDashboardUITests.Helper;

namespace BenefitsDashboardUITests.Pages
{
    public class AddEditEmployeeModal
    {
        private IWebDriver _driver;
        private HelperClass _helper;

        public AddEditEmployeeModal(IWebDriver driver)
        {
            _driver = driver;
            _helper = new HelperClass(driver);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(7);
        }

        #region Elements

        private static By _firstName = By.XPath("//input[@id='firstName']");
        private static By _lastName = By.XPath("//input[@id='lastName']");
        private static By _dependent = By.XPath("//input[@id='dependants']");
        private static By _addButton = By.XPath("//button[@id='addEmployee']");
        private static By _updateButton = By.XPath("//button[@id='updateEmployee']");
        private static By _cancel = By.XPath("//h5[text()='Add Employee']/../..//button[@class='btn btn-secondary']");
        private static By _closeIcon = By.XPath("//h5[text()='Add Employee']/../..//button[@class='close']");
        private static By _modal = By.XPath("//div[@class='modal-content']//h5[text()='Add Employee']/..//..");

        #endregion Elements

        #region Methods

        /// <summary>
        /// Add Or Edit Employee Details
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="dependent"></param>
        /// <param name="isEdit"></param>
        public void AddOrEditEmployee(string firstName = null, string lastName = null, int dependent = 0, bool isEdit = false)
        {
            if (!string.IsNullOrEmpty(firstName))
            {
                _driver.FindElement(_firstName).Clear();
                _driver.FindElement(_firstName).SendKeys(firstName);
            }
            if (!string.IsNullOrEmpty(lastName))
            {
                _driver.FindElement(_lastName).Clear();
                _driver.FindElement(_lastName).SendKeys(lastName);
            }

            if (!string.IsNullOrEmpty(dependent.ToString()))
            {
                _driver.FindElement(_dependent).Clear();
                _driver.FindElement(_dependent).SendKeys(dependent.ToString());
            }

            if (isEdit)
            {
                _driver.FindElement(_updateButton).Click();

            }
            else
            {
                _driver.FindElement(_addButton).Click();
            }
        }

        /// <summary>
        /// Close the modal using close icon or cancel button
        /// </summary>
        /// <param name="useCancelButton"></param>
        public void CancelEmployeeModal(bool useCancelButton = true)
        {
            if (useCancelButton)
            {
                _driver.FindElement(_cancel).Click();
            }
            else
            {
                _driver.FindElement(_closeIcon).Click();
            }
        }

        /// <summary>
        /// Validate if the modal is displayed or not
        /// </summary>
        /// <returns></returns>
        public bool IsModalDisplayed(bool isModalDisplayed = true)
        {
            if (!isModalDisplayed)
            {
                _helper.WaitForInvisibilityOfElement(_modal);
            }

            return _driver.FindElement(_modal).Displayed;
        }
        #endregion Methods
    }
}