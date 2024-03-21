using OpenQA.Selenium;
using BenefitsDashboardUITests.Helper;

namespace BenefitsDashboardUITests.Pages
{
    public class BenefitsDashboard
    {
        private IWebDriver _driver;
        private HelperClass _helper;
        public BenefitsDashboard(IWebDriver driver)
        {
            _driver = driver;
            _helper = new HelperClass(_driver);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        #region Elements

        private static By _addEmployee = By.XPath("//button[@id='add']");
        private static string _gridColumnValue = "//tr//td[contains(text(),'{0}')]/ancestor::tr//td[{1}]";
        private static By _employeeTable = By.XPath("//table[@id=\"employeesTable\"]");
        private static string _editIcon = "//tr//td[contains(text(),'{0}')]/..//i[@class='fas fa-edit']";
        private static string _deleteIcon = "//tr//td[contains(text(),'{0}')]/..//i[@class='fas fa-times']";
        private static string _employeeInfoRow = "//tr//td[contains(text(),'{0}')]/..";

        #endregion Elements

        #region Methods

        /// <summary>
        /// Click On Add employee Button
        /// </summary>
        public void ClickAddEmployeeButton()
        {

            _driver.FindElement(_addEmployee).Click();
        }

        /// <summary>
        /// Validate Grid Details
        /// </summary>
        /// <param name="id"></param>
        /// <param name="firstName"></param>
        /// <returns></returns>
        public string ValidateEmployeeDetailsInDashboard(string firstName, int index)
        {
            By gridColumnValue = By.XPath(string.Format(_gridColumnValue, firstName, index));
            return _driver.FindElement(gridColumnValue).Text;
        }

        /// <summary>
        /// Validate employee is displayed in the grid
        /// </summary>
        /// <param name="firstName"></param>
        /// <returns></returns>
        public bool ValidateEmployeeDetailsIsDisplayedInDashBoard(string firstName)
        {
            By employeeRowValue = By.XPath(string.Format(_employeeInfoRow, firstName));
            if (_driver.FindElements(employeeRowValue).Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Click Delete Button
        /// </summary>
        /// <param name="firstName"></param>
        /// <returns></returns>
        public void ClickOnDeleteIconButton(string firstName)
        {
            By deleteIconButton = By.XPath(string.Format(_deleteIcon, firstName));
            _driver.FindElement(deleteIconButton).Click();
        }

        /// <summary>
        /// Click Edit Button
        /// </summary>
        /// <param name="firstName"></param>
        /// <returns></returns>
        public void ClickOnEditIconButton(string firstName)
        {
            By editIconButton = By.XPath(string.Format(_editIcon, firstName));
            _driver.FindElement(editIconButton).Click();
        }

        /// <summary>
        /// Validate Employee Dashboard details is displayed
        /// </summary>
        /// <returns></returns>
        public bool IsEmployeeDashboardDisplayed()
        {
            _helper.WaitForElementToBeVisible(_employeeTable);
            return _driver.FindElement(_employeeTable).Displayed;
        }
        #endregion Methods
    }
}