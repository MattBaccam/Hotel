using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessInterfaces
{
    public interface IEmployeeAccessor
    {
        int AuthenticateUserWithEmailAndPasswordHash(string email, string passwordHash);
        int InsertEmployee(Employee employee);
        Employee SelectEmployeeByEmail(string email);
        Employee SelectEmployeeByID(int employeeID);
        bool UpdateEmployeeContactInformation(Employee newEmployee, Employee oldEmployee);
        int UpdatePasswordHash(int employeeID, string oldPasswordHash, string newPasswordHash);
        List<Position> SelectAllPositions();
    }
}
