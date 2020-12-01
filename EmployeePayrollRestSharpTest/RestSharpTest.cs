using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using System.Net;
using Newtonsoft.Json;
using System.Collections.Generic;
using EmployeePayrollRestSharp;
using Newtonsoft.Json.Linq;

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
        /// <summary>
        /// TC-2 
        /// On adding the employee using rest API after the data addition should return the Employee data.
        /// </summary>
        [TestMethod]
        public void OnAddingEmployee_ShouldReturnAddedEmployee()
        {
            //Arrange
            RestRequest restRequest = new RestRequest("employees/", Method.POST);
            /// Creating reference of json object
            JObject jObject = new JObject();
            /// Adding the data attribute with data elements
            jObject.Add("name", "Kunal");
            jObject.Add("Salary", "11000");
            /// Adding parameter to the rest request
            restRequest.AddParameter("application/json", jObject, ParameterType.RequestBody);
            //Act
            IRestResponse restResponse = restClient.Execute(restRequest);
            //Assert
            Assert.AreEqual(restResponse.StatusCode, HttpStatusCode.Created);
            /// Getting the recently added data as json format and then deserialise it to Employee object.
            EmployeeModel dataResponse = JsonConvert.DeserializeObject<EmployeeModel>(restResponse.Content);
            Assert.AreEqual("Kunal", dataResponse.name);
            Assert.AreEqual("11000", dataResponse.salary);
        }
    }
}
