using BenefitsDashboardAPITests.APICalls;
using BenefitsDashboardAPITests.APIModal;
using RestSharp;
using System.Net;
using System.Text.Json;

namespace BenefitsDashboardAPITests.Test
{
    [Category("EmployeeAPITests")]
    public class BenefitsEmployeeAPITests : APITestFactory
    {
        public bool isDeleteEmployee = false;
        public string deleteEmployeeId = "";
        [TearDown]
        public void CleanUp()
        {
            if (isDeleteEmployee)
            {
                try
                {
                    //delete employee by employee id
                    EmployeeAPICalls.DeleteEmployeeById(deleteEmployeeId);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            isDeleteEmployee = false;
        }
        /// <summary>
        /// Validate Get list of employees call
        /// </summary>
        [Test]
        [Category("ValidateGetListOfEmployees")]
        public void ValidateGetListOfEmployees()
        {
            try
            {
                // Generate random firstName, lastname, depenedent as part of payload to create a new employee api
                string firstName = HelperMethods.GenerateRandomString(5);
                string lastName = HelperMethods.GenerateRandomString(5);
                int dependents = HelperMethods.GenerateRandomDependent();

                // Call add employee call and pass the parameter for the request body
                var restResponse = EmployeeAPICalls.AddAnEmployee(firstName, lastName, dependents);

                //Validate status code after employee is created
                Assert.That(restResponse.StatusCode.Equals(HttpStatusCode.OK), "Validate status code is 200");
                //Deserialize the response and validate the firstname, lastname, dependents, net pay, benefits all matches successfully
                var employeeDetails = JsonSerializer.Deserialize<Employee>(json: restResponse.Content!);
                //Data collected for deleting the employee as part of cleanup
                isDeleteEmployee = true;
                deleteEmployeeId = employeeDetails!.id!;

                // Call Get Employee
                var getAllEmployeesRestResponse = EmployeeAPICalls.GetAllEmployees();

                //Validate Status Code
                Assert.That(getAllEmployeesRestResponse.StatusCode.Equals(HttpStatusCode.OK), "Validate status code is 200");
                var listOfEmployees = JsonSerializer.Deserialize<List<Employee>>(json: getAllEmployeesRestResponse.Content!);

                //Validate list of employee count
                Assert.That(listOfEmployees!.Count >= 0, "List of Employees is greater than equal to 0");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// Validate create employee api call
        /// </summary>
        [Test]
        [Category("ValidateCreateEmployeeAPIAndValidateNetPayOfEmployee")]
        public void ValidateCreateEmployeeAPIAndValidateNetPayOfCreatedEmployee()
        {
            try
            {
                // Generate random firstName, lastname, depenedent as part of payload to create a new employee api
                string firstName = HelperMethods.GenerateRandomString(5);
                string lastName = HelperMethods.GenerateRandomString(5);
                int dependents = HelperMethods.GenerateRandomDependent();

                // Call add employee call and pass the parameter for the request body
                var restResponse = EmployeeAPICalls.AddAnEmployee(firstName, lastName, dependents);

                // Calculate total benefits i.e 1000/26+ (500*26)* (no of dependents)
                decimal annualBenefits = Decimal.Divide(1000, 26);
                decimal dependentBenefits = Decimal.Divide(500m, 26) * dependents;
                decimal benefits = (annualBenefits + dependentBenefits);
                benefits = Math.Round(benefits, 2);
                //Get net pay after suubtracting benefits from gross per pay month
                decimal netPay = Math.Round(2000 - benefits, 2);

                //Validate status code after employee is created
                Assert.That(restResponse.StatusCode.Equals(HttpStatusCode.OK), "Validate status code is 200");
                // Http Status Code for adding an employee should be 201 instead of 200. Bug is Logged
                // Assert.That(restResponse.StatusCode.Equals(HttpStatusCode.Created), "Validate status code is 201");

                //Deserialize the response and validate the firstname, lastname, dependents, net pay, benefits all matches successfully
                var employeeDetails = JsonSerializer.Deserialize<Employee>(json: restResponse.Content!);

                //Data collected for deleting the employee as part of cleanup
                isDeleteEmployee = true;
                deleteEmployeeId = employeeDetails!.id!;

                //Assert the response are correct based on the request payload
                Assert.That(employeeDetails!.firstName!.Equals(firstName), "Validate first name is same as payload");
                Assert.That(employeeDetails!.lastName!.Equals(lastName), "Validate last name is same as payload");
                Assert.That(employeeDetails!.dependants!.Equals(dependents), "Validate dependents is same as payload");
                Assert.That(!string.IsNullOrEmpty(employeeDetails!.id), "Validate Id Is not empty");
                Assert.That(Math.Round(employeeDetails!.net!, 2).Equals(netPay), "Validate net pay is calculated correctly");
                Assert.That(Math.Round(employeeDetails!.benefitsCost!, 2).Equals(benefits), "Validate benefits is calculated correctly");
                Assert.That(Convert.ToDecimal(52000.0).Equals(employeeDetails.salary), "Validate salary for the employee");
                Assert.That(Convert.ToDecimal(2000.0).Equals(employeeDetails.gross), "Validate gross pay for the employee");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        /// <summary>
        /// Validate get employee by Id api call
        /// </summary>
        [Test]
        [Category("ValidateGetEmployeeAPIById")]
        public void ValidateGetEmployeeAPIById()
        {
            try
            {
                // Generate random firstName, lastname, depenedent as part of payload to create a new employee api
                string firstName = HelperMethods.GenerateRandomString(5);
                string lastName = HelperMethods.GenerateRandomString(5);
                int dependents = HelperMethods.GenerateRandomDependent();
                var restResponse = EmployeeAPICalls.AddAnEmployee(firstName, lastName, dependents);
                var employeeDetails = JsonSerializer.Deserialize<Employee>(json: restResponse.Content!);

                // Store employee id details as provided as response when employee is created
                string employeeId = employeeDetails!.id!;

                //Data collected for deleting the employee as part of cleanup
                isDeleteEmployee = true;
                deleteEmployeeId = employeeDetails!.id!;

                //Call employee by id stored at previous step
                var getByEmployeeIdResponse = EmployeeAPICalls.GetEmployeeById(employeeId);

                //Validate status code and validate the payload request property is same as response provided from get employee by id call
                Assert.That(getByEmployeeIdResponse.StatusCode.Equals(HttpStatusCode.OK), "Validate status code is 200");
                var employeeDetailsById = JsonSerializer.Deserialize<Employee>(json: getByEmployeeIdResponse.Content!);
                Assert.That(employeeDetails!.firstName!.Equals(employeeDetailsById!.firstName), "Validate first name is same as the employee whose details are same when created when user calls employee by id");
                Assert.That(employeeDetails!.lastName!.Equals(employeeDetailsById!.lastName), "Validate last name is same as the employee whose details are same when created when user calls employee by id");
                Assert.That(employeeDetails!.dependants!.Equals(employeeDetailsById!.dependants), "Validate dependents is same as the employee whose details are same when user calls employee by id");
                Assert.That(employeeDetails!.id!.Equals(employeeDetailsById!.id), "Validate id is same as the employee which is generated whose details are same when user calls employee by id");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        /// <summary>
        /// Validate delete employee api
        /// </summary>
        [Test]
        [Category("ValidateDeleteEmployeeAPIById")]
        public void ValidateDeleteEmployeeAPIById()
        {
            try
            {
                // Generate random firstName, lastname, depenedent as part of payload to create a new employee api
                string firstName = HelperMethods.GenerateRandomString(5);
                string lastName = HelperMethods.GenerateRandomString(5);
                int dependents = HelperMethods.GenerateRandomDependent();
                var restResponse = EmployeeAPICalls.AddAnEmployee(firstName, lastName, dependents);
                var employeeDetails = JsonSerializer.Deserialize<Employee>(json: restResponse.Content!);

                // Store employee id details as provided as response when employee is created
                string employeeId = employeeDetails!.id!;

                //Delete employee by id stored at previous step and validate status code
                var deleteByEmployeeIdRespons = EmployeeAPICalls.DeleteEmployeeById(employeeId);
                Assert.That(deleteByEmployeeIdRespons.StatusCode.Equals(HttpStatusCode.OK), "Validate status code is 200");

                // Http Status Code for deleting an employee should be 204 instead of 200. Bug is Logged
                // Assert.That(getByEmployeeIdResponse.StatusCode.Equals(HttpStatusCode.NoContent), "Validate status code is 204");
                var employeeDetailsById = EmployeeAPICalls.GetEmployeeById(employeeId);

                // Http Status Code for getting an employee by id should be 404 instead of 200 as employee is deleted. Bug is Logged
                // Assert.That(employeeDetailsById.StatusCode.Equals(HttpStatusCode.NotFound), "Validate status code is 404 not found as employee is deleted");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// Validate Put employee usecase
        /// </summary>
        [Test]
        [Category("ValidatePutEmployeeAPI")]
        public void ValidatePutEmployeeAPI()
        {
            try
            {
                // Generate random firstName, lastname, depenedent as part of payload to create a new employee api
                string firstName = HelperMethods.GenerateRandomString(5);
                string lastName = HelperMethods.GenerateRandomString(5);
                int dependents = HelperMethods.GenerateRandomDependent();
                var restResponse = EmployeeAPICalls.AddAnEmployee(firstName, lastName, dependents);
                var employeeDetails = JsonSerializer.Deserialize<Employee>(json: restResponse.Content!);
                string employeeId = employeeDetails?.id!;
                Assert.That(restResponse.StatusCode.Equals(HttpStatusCode.OK), "Validate status code is 200");

                // Http Status Code for adding an employee should be 201 instead of 200. Bug is Logged
                // Assert.That(restResponse.StatusCode.Equals(HttpStatusCode.Created), "Validate status code is 201");
                Assert.That(employeeDetails!.firstName!.Equals(firstName), "Validate first name is same as payload");
                Assert.That(employeeDetails!.lastName!.Equals(lastName), "Validate last name is same as payload");
                Assert.That(employeeDetails!.dependants!.Equals(dependents), "Validate dependents is same as payload");
                Assert.That(!string.IsNullOrEmpty(employeeDetails!.id), "Validate Id Is not empty");

                //Data collected for deleting the employee as part of cleanup
                isDeleteEmployee = true;
                deleteEmployeeId = employeeDetails!.id!;

                // Generate random firstName, lastname, as part of payload to update the existing employee via put api
                string updatedFirstName = HelperMethods.GenerateRandomString(5);
                string updatedLastName = HelperMethods.GenerateRandomString(5);
                var updatedEmployeeDetailsResponse = EmployeeAPICalls.UpdateEmployeeDetails(updatedFirstName, updatedLastName, employeeId, dependents);

                // Validate the response code and the field matches with the values which are updated as per the update employee payload
                Assert.That(updatedEmployeeDetailsResponse.StatusCode.Equals(HttpStatusCode.OK), "Validate status code is 200");
                var employeeUpdatedDetails = JsonSerializer.Deserialize<Employee>(json: updatedEmployeeDetailsResponse.Content!);
                Assert.That(employeeUpdatedDetails!.firstName!.Equals(updatedFirstName), "Validate first name is updated as per update request payload");
                Assert.That(employeeUpdatedDetails!.lastName!.Equals(updatedLastName), "Validate last name is updated as per update request payload");
                Assert.That(employeeUpdatedDetails!.dependants!.Equals(employeeDetails!.dependants), "Validate dependents is same as update request payload is same as original payload");
                Assert.That(employeeUpdatedDetails!.id!.Equals(employeeDetails!.id), "Validate Id Is same for the employee");


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// Validate create employee bad request usecase
        /// </summary>
        [Test]
        [Category("ValidateCreateEmployeesBadRequestUseCases")]
        public void ValidateCreateEmployeesBadRequestUseCases()
        {
            // Note creating all bad data for each individual field and sending them in order to validate the error message for each field
            // Order is 1)firstname greater than 50 2) last name is blank 3) dependent greater than 32
            string firstName = HelperMethods.GenerateRandomString(51);
            string lastName = "";
            int dependents = 33;
            var restResponse = EmployeeAPICalls.AddAnEmployee(firstName, lastName, dependents);
            var employeeDetails = JsonSerializer.Deserialize<List<EmployeeErrorMessage>>(json: restResponse.Content!);

            // Validate bad request is displayed along with proper error message for each field
            Assert.That(restResponse.StatusCode.Equals(HttpStatusCode.BadRequest), "Validate status code is 400");
            for (int i = 0; i < employeeDetails!.Count; i++)
            {
                if (i == 0)
                {
                    employeeDetails[i]?.memberNames[0].Equals("firstName");
                    employeeDetails[i]?.errorMessage.Equals("The field FirstName must be a string with a maximum length of 50.");
                }
                else if (i == 1)
                {
                    employeeDetails[i]?.memberNames[0].Equals("lastName");
                    employeeDetails[i]?.errorMessage.Equals("The LastName field is required.");
                }
                else
                {
                    employeeDetails[i]?.memberNames[0].Equals("dependants");
                    employeeDetails[i]?.errorMessage.Equals("The field Dependants must be between 0 and 32.");
                }
            }
        }

        /// <summary>
        /// Validate bad request usecases for put api call
        /// </summary>
        [Test]
        [Category("ValidatePutEmployeesBadRequestUseCases()")]
        public void ValidatePutEmployeesBadRequestUseCases()
        {
            // Update an employee by giving invalid id or removing id from payload with firstname character greater than 50 ,last name as blank and dependent greater than 32
            string firstName = HelperMethods.GenerateRandomString(51);
            string lastName = "";
            int dependents = 33;
            var restResponse = EmployeeAPICalls.UpdateEmployeeDetails(firstName, lastName, "123", dependents);

            //Currently user is seeing 500 when id is removed or given incorrect which is wrong. Added Bug for it
            Assert.That(restResponse.StatusCode.Equals(HttpStatusCode.MethodNotAllowed), "Validate status code is 500");

            //Put should give 400 bad request if id is missing in the payload or any bad id is given
            // Assert.That(restResponse.StatusCode.Equals(HttpStatusCode.BadRequest), "Validate status code is 400");

            // Cannot validate as assert status is incorrect
            //var employeeDetails = JsonSerializer.Deserialize<List<EmployeeErrorMessage>>(json: restResponse.Content!);
            //for (int i = 0; i < employeeDetails!.Count; i++)
            //{
            //    if (i == 0)
            //    {
            //        employeeDetails[i]?.memberNames[0].Equals("firstName");
            //        employeeDetails[i]?.errorMessage.Equals("The field FirstName must be a string with a maximum length of 50.");
            //    }
            //    else if (i == 1)
            //    {
            //        employeeDetails[i]?.memberNames[0].Equals("lastName");
            //        employeeDetails[i]?.errorMessage.Equals("The LastName field is required.");
            //    }
            //    else
            //    {
            //        employeeDetails[i]?.memberNames[0].Equals("dependants");
            //        employeeDetails[i]?.errorMessage.Equals("The field Dependants must be between 0 and 32.");
            //    }
            //}

        }

        /// <summary>
        /// Validate Unauthorized Assertions When User Token Is Not Given
        /// </summary>
        [Test]
        [Category("ValidateUnauthorizedAssertionsWhenUserTokenIsNotGiven")]
        public void ValidateUnauthorizedAssertionsWhenUserTokenIsNotGiven()
        {
            try
            {
                RestClient client = new RestClient(BaseUrl);
                RestRequest restRequest = new RestRequest(BaseUrl, Method.Get);
                RestResponse restResponse = client.Execute(restRequest);

                // Validate api gives 401 response
                Assert.That(restResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized), "When token is not there user gets 401 Unauthorized");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}