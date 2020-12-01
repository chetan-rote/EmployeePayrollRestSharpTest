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
            Assert.AreEqual(5, dataResponse.Count);
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
        /// <summary>
        /// Given the multiple data on post should return total count.
        /// </summary>
        [TestMethod]
        public void GivenMultipleData_OnPost_ShouldReturn_TotalCount()
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();
            employeeList.Add(new EmployeeModel { name = "Rohit", salary = "12000" });
            employeeList.Add(new EmployeeModel { name = "Sachin", salary = "11500" });
            employeeList.ForEach(employeeData =>
            {
                RestRequest restRequest = new RestRequest("/employees", Method.POST);
                /// Creating reference of json object
                JObject jObject = new JObject();
                /// Adding the data attribute with data elements
                jObject.Add("name", employeeData.name);
                jObject.Add("Salary", employeeData.salary);
                /// Adding parameter to the rest request jObject - contains the parameter list of the json database
                restRequest.AddParameter("application/json", jObject, ParameterType.RequestBody);
                //Act
                IRestResponse restResponse = restClient.Execute(restRequest);
                //Assert
                Assert.AreEqual(restResponse.StatusCode, HttpStatusCode.Created);
                EmployeeModel dataResponse = JsonConvert.DeserializeObject<EmployeeModel>(restResponse.Content);
                Assert.AreEqual(employeeData.name, dataResponse.name);
                Assert.AreEqual(employeeData.salary, dataResponse.salary);
            });
            IRestResponse response = GetEmployeeList();
            List<EmployeeModel> dataResponse = JsonConvert.DeserializeObject<List<EmployeeModel>>(response.Content);
            Assert.AreEqual(7, dataResponse.Count);
        }

        [TestMethod]
        public void GivenEmployee_OnUpdate_ShouldReturnUpdatedEmployee()
        {
            //Arrange
            RestRequest restRequest = new RestRequest("/employees/7", Method.PUT);
            /// Creating reference of json object
            JObject jObject = new JObject();
            /// Adding the data attribute with data elements
            jObject.Add("name", "Sachin");
            jObject.Add("salary", "13500");
            /// Adding parameter to the rest request jObject contains the parameter list of the json database
            restRequest.AddParameter("application/json", jObject, ParameterType.RequestBody);
            //Act
            IRestResponse restResponse = restClient.Execute(restRequest);
            //Assert
            Assert.AreEqual(restResponse.StatusCode, System.Net.HttpStatusCode.OK);
            EmployeeModel dataResponse = JsonConvert.DeserializeObject<EmployeeModel>(restResponse.Content);
            Assert.AreEqual("Sachin", dataResponse.name);
            Assert.AreEqual("13500", dataResponse.salary);
            System.Console.WriteLine(restResponse.Content);
        }
    }
}
