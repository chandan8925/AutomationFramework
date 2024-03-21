using OpenQA.Selenium;
using BenefitsDashboardUITests.Helper;

namespace BenefitsDashboardUITests.Pages
{
    public class DeleteModal
    {
        private IWebDriver _driver;
        private HelperClass _helper;

        public DeleteModal(IWebDriver driver)
        {
            _driver = driver;
            _helper = new HelperClass(driver);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(7);
        }

        #region Elements
        private static By _deleteButton = By.XPath("//button[@id='deleteEmployee']");
        private static By _cancelButton = By.XPath("//h5[text()='Delete Employee']/../..//button[@class='btn btn-secondary']");
        private static By _closeIcon = By.XPath("//h5[text()='Delete Employee']/../..//button[@class='close']");
        private static By _modal = By.XPath("//div[@class='modal-content']//h5[text()='Delete Employee']/..//..");
        private static By _modalContentText = By.XPath("//input[@id='deleteId']/..//div");

        #endregion Elements

        #region Methods

        /// <summary>
        /// Get modal content text
        /// </summary>
        /// <returns></returns>
        public string GetModalContentText()
        {
            return _driver.FindElement(_modalContentText).Text;
        }

        /// <summary>
        /// Click delete button
        /// </summary>
        public void ClickOnDeleteButton()
        {
            _helper.WaitForElementClickable(_deleteButton);
            _driver.FindElement(_deleteButton).Click();
        }

        /// <summary>
        /// Close the modal using close icon or cancel button
        /// </summary>
        /// <param name="useCancelButton"></param>
        public void CancelDeleteModal(bool useCancelButton = true)
        {
            if (useCancelButton)
            {
                _driver.FindElement(_cancelButton).Click();
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
        public bool IsModalDisplayed()
        {
            return _driver.FindElement(_modal).Displayed;
        }
        #endregion Methods
    }
}