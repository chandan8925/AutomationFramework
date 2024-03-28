using Newtonsoft.Json.Linq;
using RestSharp;

namespace BenefitsDashboardAPITests.APICalls
{
    public class EmployeeAPICalls : APITestFactory
    {
        RestClient? client;
        public string BaseAPIUrl;
        public string APIToken;
        public EmployeeAPICalls(string baseUrl, string token)
        {
            BaseAPIUrl = baseUrl;
            APIToken = token;
            client = new RestClient(BaseAPIUrl);
        }

        /// <summary>
        /// Get List Of All Employees
        /// </summary>
        /// <returns></returns>
        public RestResponse GetAllEmployees()
        {
            RestRequest restRequest = new RestRequest(BaseAPIUrl, Method.Get);
            restRequest.AddHeader("Authorization", APIToken);
            RestResponse restResponse = client!.Execute(restRequest);
            return restResponse;
        }

        /// <summary>
        /// Get employee by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RestResponse GetEmployeeById(string id)
        {
            RestRequest restRequest = new RestRequest($"{BaseAPIUrl}/{id}", Method.Get);
            restRequest.AddHeader("Authorization", APIToken);
            RestResponse restResponse = client!.Execute(restRequest);
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
            RestRequest request = new RestRequest(BaseAPIUrl, Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", APIToken);
            var body = new JObject(
                new JProperty("firstName", firstName),
                new JProperty("lastName", lastName),
                new JProperty("dependants", dependents)
            );

            request.AddStringBody(body.ToString(), DataFormat.Json);
            RestResponse response = client!.Execute(request);
            return response;
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
            
            RestRequest restRequest = new RestRequest(BaseAPIUrl, Method.Put);
            var body = new JObject(
               new JProperty("firstName", firstName),
               new JProperty("lastName", lastName),
               new JProperty("dependants", dependents),
               new JProperty("id", id)
           );
            restRequest.AddStringBody(body.ToString(), DataFormat.Json);
            restRequest.AddHeader("Authorization", APIToken);
            RestResponse restResponse = client!.Execute(restRequest);
            return restResponse;
        }

        /// <summary>
        /// Delete Employees By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RestResponse DeleteEmployeeById(string id)
        {
            RestRequest restRequest = new RestRequest($"{BaseAPIUrl}/{id}", Method.Delete);
            restRequest.AddHeader("Authorization", APIToken);
            RestResponse restResponse = client!.Execute(restRequest);
            return restResponse;
        }
    }
}
