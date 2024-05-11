using DataAccessInterfaces;
using DataAccessLayer;
using DataObjects;
using LogicLayerInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class EmployeeManager : IEmployeeManager
    { 
        //Dependancy inversion for the data provider 
        private IEmployeeAccessor _employeeAccessor = new EmployeeAccessor();


        public bool AuthenticateEmployee(string email, string password)
        {
            password = Helpers.HashSha256(password);
            return _employeeAccessor.AuthenticateUserWithEmailAndPasswordHash(email, password) == 1;
        }

        public Employee GetEmployeeByID(int employeeID)
        {
            try
            {
                return _employeeAccessor.SelectEmployeeByID(employeeID);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Employee LoginEmployee(string email, string password)
        {
            try
            {
                Employee employee = null;

                if (AuthenticateEmployee(email, password))
                {
                    try
                    {
                        employee = GetEmployeeByEmail(email);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else
                {
                    throw new ArgumentException("Incorrect login input");
                }
                return employee;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Authentication failed", ex);
            }
        }

        public bool ResetPassword(int employeeID, string oldPassword, string newPassword)
        {
            bool result = false;
            oldPassword = Helpers.HashSha256(oldPassword);
            newPassword = Helpers.HashSha256(newPassword);

            try
            {
                result = (1 == _employeeAccessor.UpdatePasswordHash(employeeID, oldPassword, newPassword));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public List<Position> GetPositions()
        {
            try
            {
                return _employeeAccessor.SelectAllPositions();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DoesEmailExist(string email)
        {
            try
            {
                return _employeeAccessor.SelectEmployeeByEmail(email) != null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Employee GetEmployeeByEmail(string email)
        {
            try
            {
                return _employeeAccessor.SelectEmployeeByEmail(email);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public EmployeeVM GetEmployeeVM(int employeeID)
        {
            try
            {
                var employee = GetEmployeeByID(employeeID);
                var employeeVM = new EmployeeVM()
                {
                    EmployeeID = employee.EmployeeID,
                    PositionID = employee.PositionID,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Phone = employee.Phone,
                    Email = employee.Email,
                    Position = GetPositions().FirstOrDefault(position => position.PositionID == employee.PositionID)
                };
                return employeeVM;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CreateNewEmployee(Employee employee)
        {
            try
            {
                return _employeeAccessor.InsertEmployee(employee) == 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateEmployeeContactInformation(Employee newEmployee, Employee oldEmployee)
        {
            try
            {
                return _employeeAccessor.UpdateEmployeeContactInformation(newEmployee, oldEmployee);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
