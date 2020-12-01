using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using System.Net;
using Newtonsoft.Json;
using System.Collections.Generic;
using EmployeePayrollRestSharp;

namespace EmployeePayrollRestSharpTest
{
    [TestClass]
    public class RestSharpTest
    {
        RestClient restClient = new RestClient("http://localhost:3000");
        /// <summary>
        /// Method to get the data in json format requested from the api's data hosting server
        /// </summary>
        /// <returns></returns>
        public IRestResponse GetEmployeeList()
        {
            //Arrange
            RestRequest restRequest = new RestRequest("/employees", Method.GET);
            //Act
            IRestResponse response = restClient.Execute(restRequest);
            //Returning Json result
            return response;
        }
        /// <summary>
        /// TC-1
        /// On calling the employee rest API return the list of the employee.
        /// </summary>
        [TestMethod]
        public void OnCallingEmployeeRestAPI_RetrivesAllData()
        {
            //Act
            IRestResponse restResponse = GetEmployeeList();
            //Assert
            Assert.AreEqual(restResponse.StatusCode, HttpStatusCode.OK);
            List<EmployeeModel> dataResponse = JsonConvert.DeserializeObject<List<EmployeeModel>>(restResponse.Content);
            Assert.AreEqual(3, dataResponse.Count);
            foreach (var employee in dataResponse)
            {
                System.Console.WriteLine($"ID: {employee.id}, Name: {employee.name}, Sallary: {employee.salary}");
            }
        }
    }
}
