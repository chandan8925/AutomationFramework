using BenefitsDashboardAPITests.APIModal;
using RestSharp;

namespace BenefitsDashboardAPITests.APICalls
{
    public class EmployeeAPICalls : APITestFactory
    {
        RestClient? client = new RestClient();
        public string BaseAPIUrl;
        public string APIToken;
        public EmployeeAPICalls(string baseUrl, string token)
        {
            BaseAPIUrl = baseUrl;
            APIToken = token;
        }

        /// <summary>
        /// Get List Of All Employees
        /// </summary>
        /// <returns></returns>
        public RestResponse GetAllEmployees()
        {
            client = new RestClient(BaseAPIUrl);
            RestRequest restRequest = new RestRequest(BaseAPIUrl, Method.Get);
            restRequest.AddHeader("Authorization", APIToken);
            RestResponse restResponse = client.Execute(restRequest);
            return restResponse;
        }

        /// <summary>
        /// Get employee by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RestResponse GetEmployeeById(string id)
        {
            client = new RestClient(BaseAPIUrl);
            RestRequest restRequest = new RestRequest($"{BaseAPIUrl}/{id}", Method.Get);
            restRequest.AddHeader("Authorization", APIToken);
            RestResponse restResponse = client.Execute(restRequest);
            return restResponse;
        }

        /// <summary>
        /// Add an employee
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="dependents"></param>
        /// <returns></returns>
        public RestResponse AddAnEmployee(string firstName = null, string lastName = null, int dependents = 0)
        {
            var employeeDetailsPayload = new CreateUpdateEmployeeRequest
            {
                firstName = firstName,
                lastName = lastName,
                dependants = dependents,
            };
            client = new RestClient(BaseAPIUrl);
            RestRequest restRequest = new RestRequest(BaseAPIUrl, Method.Post);
            restRequest.AddBody(employeeDetailsPayload, ContentType.Json);
            restRequest.AddHeader("Authorization", APIToken);
            RestResponse restResponse = client.Execute(restRequest);
            return restResponse;
        }

        /// <summary>
        /// Update employee details of an employee
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="id"></param>
        /// <param name="dependents"></param>
        /// <returns></returns>
        public RestResponse UpdateEmployeeDetails(string firstName = null, string lastName = null, string id = null, int dependents = 0)
        {
            var employeeDetailsPayload = new CreateUpdateEmployeeRequest
            {
                id = id,
                firstName = firstName,
                lastName = lastName,
                dependants = dependents,
            };
            client = new RestClient(BaseAPIUrl);
            RestRequest restRequest = new RestRequest(BaseAPIUrl, Method.Put);
            restRequest.AddBody(employeeDetailsPayload, ContentType.Json);
            restRequest.AddHeader("Authorization", APIToken);
            RestResponse restResponse = client.Execute(restRequest);
            return restResponse;
        }

        /// <summary>
        /// Delete Employees By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RestResponse DeleteEmployeeById(string id)
        {
            client = new RestClient(BaseAPIUrl);
            RestRequest restRequest = new RestRequest($"{BaseAPIUrl}/{id}", Method.Delete);
            restRequest.AddHeader("Authorization", APIToken);
            RestResponse restResponse = client.Execute(restRequest);
            return restResponse;
        }
    }
}
