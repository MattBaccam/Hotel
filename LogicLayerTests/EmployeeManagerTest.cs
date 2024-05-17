using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DataObjects;
using LogicLayerInterfaces;
using LogicLayer;
using DataAccessFakes;

namespace LogicLayerTests
{
    [TestClass]
    public class EmployeeManagerTests
    {
        private IEmployeeManager _employeeManager;

        [TestInitialize]
        public void TestSetUp()
        {
            _employeeManager = new EmployeeManager(new EmployeeAccessorFake());
        }

        [TestMethod]
        public void TestAuthenticateUserWithEmailAndPasswordHash()
        {
            string email = "john.doe@example.com";
            string correctPasswordHash = "P@ssw0rd";

            bool authenticatedEmployeeID = _employeeManager.AuthenticateEmployee(email, correctPasswordHash);

            Assert.IsTrue(authenticatedEmployeeID);
        }

        [TestMethod]
        public void TestInsertEmployee()
        {
            var employee = new Employee
            {
                FirstName = "Test",
                LastName = "Employee",
                Email = "test.employee@example.com",
                Phone = "123-456-7890",
                PositionID = 1
            };

            bool newEmployeeID = _employeeManager.CreateNewEmployee(employee);

            Assert.IsTrue(newEmployeeID);
        }

        [TestMethod]
        public void TestSelectAllPositions()
        {
            var positions = _employeeManager.GetPositions();

            Assert.IsNotNull(positions);
            Assert.AreEqual(3, positions.Count);
        }

        [TestMethod]
        public void TestSelectEmployeeByEmail()
        {
            string email = "john.doe@example.com";

            var employee = _employeeManager.GetEmployeeByEmail(email);

            Assert.IsNotNull(employee);
            Assert.AreEqual(email, employee.Email);
        }

        [TestMethod]
        public void TestSelectEmployeeByID()
        {
            int employeeID = 1;

            var employee = _employeeManager.GetEmployeeByID(employeeID);

            Assert.IsNotNull(employee);
            Assert.AreEqual(employeeID, employee.EmployeeID);
        }

        [TestMethod]
        public void TestUpdateEmployee()
        {
            var oldEmployee = _employeeManager.GetEmployeeByID(1);
            var newEmployee = new Employee()
            {
                EmployeeID = oldEmployee.EmployeeID,
                FirstName = "Updated",
                LastName = "Employee",
                Email = "updated.employee@example.com",
                Phone = "987-654-3210",
                PositionID = oldEmployee.PositionID
            };

            bool isUpdated = _employeeManager.UpdateEmployeeInformation(newEmployee, oldEmployee);

            Assert.IsTrue(isUpdated);
            var updatedEmployee = _employeeManager.GetEmployeeByID(1);
            Assert.AreEqual(newEmployee.FirstName, updatedEmployee.FirstName);
        }

        [TestMethod]
        public void TestUpdatePasswordHash()
        {
            int employeeID = 1;
            string oldPasswordHash = "P@ssw0rd";
            string newPasswordHash = "newpasswordhash";

            bool updated = _employeeManager.ResetPassword(employeeID, oldPasswordHash, newPasswordHash);

            Assert.IsTrue(updated);
        }
    }
}

