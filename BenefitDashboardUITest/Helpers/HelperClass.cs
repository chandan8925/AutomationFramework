using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Text;

namespace BenefitsDashboardUITests.Helper
{
    public class HelperClass
    {
        private IWebDriver _driver;
        public static Random Random = new Random();
        private WebDriverWait _wait;

        /// <summary>
        /// Helper Class
        /// </summary>
        /// <param name="driver"></param>
        public HelperClass(IWebDriver driver)
        {
            _driver = driver;
        }

        public IWebElement WaitForElementToBeVisible(By by, int timeOutValue = 7)
        {
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeOutValue));
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(by));
            return _driver.FindElement(by);
        }

        public IWebElement WaitForElementExists(By by, int timeOutValue = 7)
        {
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeOutValue));
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(by));
            return _driver.FindElement(by);
        }

        public IWebElement WaitForElementClickable(By by, int timeOutValue = 7)
        {
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeOutValue));
            IWebElement element = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(by));
            return element;
        }

        public bool WaitForInvisibilityOfElement(By by, int timeOutValue = 7)
        {
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeOutValue));
            return _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(by));
        }

        public bool WaitForPageUrlContains(string txt, int timeOutValue = 7)
        {
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeOutValue));
            return _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlContains(txt));
        }

        /// <summary>
        /// Generate Random String with desired length
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var stringBuilder = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                stringBuilder.Append(chars[Random.Next(chars.Length)]);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Generate depenedent ranging from 0 to 32
        /// </summary>
        /// <returns></returns>
        public int GenerateRandomDependent()
        {
            var lowerBound = 0;
            var upperBound = 32;
            return Random.Next(lowerBound, upperBound);
        }

        /// <summary>
        /// Calculate benefits and net pay of employee
        /// Calculating benefits and Netpay and storing it in the list 0 index for benefits and 1 for netpay
        /// </summary>
        /// <param name="dependents"></param>
        /// <returns></returns>
        public List<Decimal> PaymentDetailsOfEmployee(int dependents)
        {
            // Calculate total benefits i.e 1000/26+ (500*26)* (no of dependents)
            List<Decimal> PayDetails = new List<Decimal>();
            decimal annualBenefits = Decimal.Divide(1000, 26);
            decimal dependentBenefits = Decimal.Divide(500m, 26) * dependents;
            decimal benefits = (annualBenefits + dependentBenefits);
            benefits = Math.Round(benefits, 2);
            PayDetails.Add(benefits);

            //Get net pay after subtracting benefits from gross per pay month
            decimal netPay = Math.Round(2000 - benefits, 2);
            PayDetails.Add(netPay);
            return PayDetails;
        }
    }
}
