using BenefitsDashboardAPITests.APIModal;
using BenefitsDashboardUITests.Helper;
using System.Net;
using System.Text.Json;

namespace BenefitsDashboardUITests.UITests
{
    [Category("EmployeeUITests")]
    public class BenefitsDashboardEmployeeUITests : UITestFactory
    {
        public bool isDeleteEmployee = false;
        public string deleteEmployeeName = "";

        [TearDown]
        public void CleanUp()
        {
            if (isDeleteEmployee)
            {
                try
                {
                    //Delete Employee From DashBoard
                    BenefitsDashboard.ClickOnDeleteIconButton(deleteEmployeeName);
                    DeleteModal.ClickOnDeleteButton();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            isDeleteEmployee = false;
            var employeeCount = GetEmployeeRowCount();
            if (employeeCount > 0)
            {
                var employeeDetails = EmployeeAPICalls.GetAllEmployees();
                var listOfEmployees = JsonSerializer.Deserialize<List<Employee>>(json: employeeDetails.Content!);
                for (int i = 0; i < listOfEmployees!.Count; i++)
                {
                    EmployeeAPICalls.DeleteEmployeeById(listOfEmployees[i].id!);
                }

            }
        }

        /// <summary>
        /// Benefit dashboard page validation
        /// </summary>
        [Test]
        [Category("VerifyBenefitPageDisplayed")]
        public void VerifyBenefitPageIsDisplayed()
        {
            //Get Page Title
            var benefitDashBoardTitle = GetTitle();

            //Verify page title and dashboard is displayed
            Assert.That(BenefitsDashboard.IsEmployeeDashboardDisplayed, "Benefit dashboard is displayed");
            Assert.That(benefitDashBoardTitle.Equals("Employees - Paylocity Benefits Dashboard"), "Page Title Should Match");
        }

        /// <summary>
        /// Add employee and validate net pay of the employee
        /// </summary>
        [Test]
        [Category("AddEmployeeAndVerifyDetailsInBenefitDashBoard")]
        public void AddEmployeeAndVerifyDetailsInBenefitDashBoard()
        {
            Assert.That(BenefitsDashboard.IsEmployeeDashboardDisplayed, "Benefit dashboard is displayed");

            //Add employee Modal is displayed on clicking add employee button
            BenefitsDashboard.ClickAddEmployeeButton();
            Assert.That(AddEditEmployeeModal.IsModalDisplayed(), "Add Employee Modal is Displayed When User clicks on add employee button");

            //Add details of employee In Add employee modal
            string firstName = HelperClass.GenerateRandomString(5);
            string lastName = HelperClass.GenerateRandomString(5);
            int dependents = HelperClass.GenerateRandomDependent();
            AddEditEmployeeModal.AddOrEditEmployee(firstName, lastName, dependents);
            Assert.That(AddEditEmployeeModal.IsModalDisplayed(false), Is.False, "Add Employee Modal is Not Displayed When User clicks on add button");

            //setting data for test cleanup
            deleteEmployeeName = firstName;
            isDeleteEmployee = true;

            // Using helper method to calculate benefits and net pay of employee
            /// Calculating benefits and Netpay and storing it in the list 
            /// 0 index is for benefits and 1 index is stored for netpay in the list
            var benefits = HelperClass.PaymentDetailsOfEmployee(dependents)[0];
            var netPay = HelperClass.PaymentDetailsOfEmployee(dependents)[1];

            // Step 1: Array Declaration 
            string[] expectedEmployeeDetails;

            // Step 2:Array Initialization of expected values for the employee 
            expectedEmployeeDetails = new string[7] { lastName, firstName, dependents.ToString(),
                "52000.00", "2000.00", benefits.ToString(), netPay.ToString() };

            //Validate expected values and the values displayed in dashboard for the employee matches
            ValidateGridDetails(firstName, expectedEmployeeDetails);
        }

        /// <summary>
        /// Update employee details and validate is updated correctly
        /// </summary>
        [Test]
        [Category("UpdateEmployeeAndVerifyDetailsInBenefitDashBoard")]
        public void UpdateEmployeeAndVerifyDetailsInBenefitDashBoard()
        {
            try
            {
                //Note - Using API to create api to save time as create employee via UI has been validated in last testCase
                // Generate random firstName, lastname, depenedent as part of payload to create a new employee api
                string firstName = HelperClass.GenerateRandomString(5);
                string lastName = HelperClass.GenerateRandomString(5);
                int dependents = HelperClass.GenerateRandomDependent();

                // Call add employee call and pass the parameter for the request body
                var restResponse = EmployeeAPICalls.AddAnEmployee(firstName, lastName, dependents);
                if(restResponse.StatusCode != HttpStatusCode.OK)
                {
                    BenefitsDashboard.ClickAddEmployeeButton();
                    AddEditEmployeeModal.AddOrEditEmployee(firstName, lastName, dependents);
                }
                else
                {
                    //Validate status code after employee is created
                    Assert.That(restResponse.StatusCode.Equals(HttpStatusCode.OK), "Validate status code is 200");
                    Driver.Navigate().Refresh();
                }
                // Using helper method to calculate benefits and net pay of employee
                /// Calculating benefits and Netpay and storing it in the list 
                /// 0 index is for benefits and 1 index is stored for netpay in the list
                var benefits = HelperClass.PaymentDetailsOfEmployee(dependents)[0];
                var netPay = HelperClass.PaymentDetailsOfEmployee(dependents)[1];

                // setting data for testcleanup
                deleteEmployeeName = firstName;
                isDeleteEmployee = true;

                // Step 1: Array Declaration 
                string[] expectedEmployeeDetails;

                // Step 2:Array Initialization of expected values for the employee 
                expectedEmployeeDetails = new string[7] { lastName, firstName, dependents.ToString(),
                "52000.00", "2000.00", benefits.ToString(), netPay.ToString() };

                //Validate expected values and the values displayed in dashboard for the employee matches
                ValidateGridDetails(firstName, expectedEmployeeDetails);

                //CLick on edit icon for the employee
                BenefitsDashboard.ClickOnEditIconButton(firstName);
                // Changing only dependent as name assertion will fail because first name and last name is switched

                //Generate a new dependent to update
                int updateDependents = HelperClass.GenerateRandomDependent();

                //Verify add/edit modal is displayed when user clicks on edit icon button for the employee
                Assert.That(AddEditEmployeeModal.IsModalDisplayed(), "Verify add/edit modal is displayed when user clicks on edit icon button for the employee");
                AddEditEmployeeModal.AddOrEditEmployee(dependent: updateDependents, isEdit: true);

                //Verify modal is closed and not displayed
                Assert.That(AddEditEmployeeModal.IsModalDisplayed(false), Is.False, "Verify add/edit modal is Not Displayed When User clicks on update button");

                // Using helper method to calculate benefits and net pay of employee
                /// Calculating benefits and Netpay and storing it in the list 
                /// 0 index is for benefits and 1 index is stored for netpay in the list
                var updatedBenefits = HelperClass.PaymentDetailsOfEmployee(updateDependents)[0];
                var updatedNetPay = HelperClass.PaymentDetailsOfEmployee(updateDependents)[1];

                // Step 1: Array Declaration 
                string[] expectedUpdatedEmployeeDetails;

                // Step 2:Array Initialization of expected values for the employee 
                expectedUpdatedEmployeeDetails = new string[7] { lastName, firstName, updateDependents.ToString(),
                "52000.00", "2000.00", updatedBenefits.ToString(), updatedNetPay.ToString() };

                //Validate expected values and the values displayed in dashboard for the employee matches
                ValidateGridDetails(firstName, expectedUpdatedEmployeeDetails);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// Delete Employee and verify it is not displayed in dashboard
        /// </summary>
        [Test]
        [Category("DeleteEmployeeAndVerifyItsNotDisplayedInBenefitDashBoard")]
        public void DeleteEmployeeAndVerifyItsNotDisplayedInBenefitDashBoard()
        {
            try
            {
                //Note - Using API to create api to save time as create employee via UI has been validated in previous testCase

                // Generate random firstName, lastname, depenedent as part of payload to create a new employee api
                string firstName = HelperClass.GenerateRandomString(5);
                string lastName = HelperClass.GenerateRandomString(5);
                int dependents = HelperClass.GenerateRandomDependent();

                // Call add employee call and pass the parameter for the request body
                var restResponse = EmployeeAPICalls.AddAnEmployee(firstName, lastName, dependents);
                if (restResponse.StatusCode != HttpStatusCode.OK)
                {
                    BenefitsDashboard.ClickAddEmployeeButton();
                    AddEditEmployeeModal.AddOrEditEmployee(firstName, lastName, dependents);
                }
                else
                {
                    //Validate status code after employee is created
                    Assert.That(restResponse.StatusCode.Equals(HttpStatusCode.OK), "Validate status code is 200");
                    Driver.Navigate().Refresh();

                }
                Assert.That(BenefitsDashboard.ValidateEmployeeDetailsIsDisplayedInDashBoard(firstName), "Employee details is displayed in dashboard");

                //CLick on delete icon of the employee
                BenefitsDashboard.ClickOnDeleteIconButton(firstName);
                var employeeCountBeforeDelete = GetEmployeeRowCount();
                //validate correct employee is deleted by checking the name
                Assert.That(DeleteModal.GetModalContentText().Contains(firstName + " " + lastName), "Validate corect employee is deleted.");

                //Click on Cancel Button
                DeleteModal.CancelDeleteModal();
                var employeeCountWhenDeleteIsCanceled = GetEmployeeRowCount();
                var isEmployeeDisplayed = BenefitsDashboard.ValidateEmployeeDetailsIsDisplayedInDashBoard(firstName);
                Assert.That(isEmployeeDisplayed, Is.True, "Employee details is displayed in dashboard as user did not delete the employee");
                Assert.That(employeeCountBeforeDelete.Equals(employeeCountWhenDeleteIsCanceled), "Employee count is same in the dashboard");

                //Click On Delete Button
                BenefitsDashboard.ClickOnDeleteIconButton(firstName);
                DeleteModal.ClickOnDeleteButton();

                // Verify the employee is not displayed in the dashboard
                for (int i = 0; i < 7; i++)
                {
                    if (isEmployeeDisplayed)
                    {
                        Driver.Navigate().Refresh();
                        isEmployeeDisplayed = BenefitsDashboard.ValidateEmployeeDetailsIsDisplayedInDashBoard(firstName);
                    }
                    else
                    {
                        break;
                    }
                }
                isEmployeeDisplayed = BenefitsDashboard.ValidateEmployeeDetailsIsDisplayedInDashBoard(firstName);
                Assert.That(isEmployeeDisplayed, Is.False, "Employee details is not displayed in dashboard");

                var employeeCountAfterDelete = GetEmployeeRowCount();
                Assert.That((employeeCountBeforeDelete - 1).Equals(employeeCountAfterDelete), "Employee count is decreased by 1 in the dashboard");


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }

}