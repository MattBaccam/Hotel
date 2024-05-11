using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayerInterfaces
{
    public interface IEmployeeManager
    {
        Employee LoginEmployee(string email, string password);
        bool CreateNewEmployee(Employee employee);
        bool DoesEmailExist(string email);
        EmployeeVM GetEmployeeVM(int employeeID);
        bool AuthenticateEmployee(string email, string password);
        bool ResetPassword(int employeeID, string oldPassword, string newPassword);
        Employee GetEmployeeByEmail(string email);
        Employee GetEmployeeByID(int employeeID);
        List<Position> GetPositions();
        bool UpdateEmployeeContactInformation(Employee newEmployee, Employee oldEmployee);
    }
}
