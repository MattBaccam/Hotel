using DataAccessInterfaces;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessFakes
{
    public class EmployeeAccessorFake : IEmployeeAccessor
    {
        private List<Employee> _employees = new List<Employee>();
        private Dictionary<Employee, string> _passwordHashes = new Dictionary<Employee, string>();
        private int _nextEmployeeID = 1;

        public EmployeeAccessorFake()
        {
            _employees.Add(new Employee
            {
                EmployeeID = _nextEmployeeID++,
                PositionID = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Phone = "123-456-7890"
            });

            _employees.Add(new Employee
            {
                EmployeeID = _nextEmployeeID++,
                PositionID = 2,
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@example.com",
                Phone = "987-654-3210"
            });

            _passwordHashes.Add(_employees[0], "b03ddf3ca2e714a6548e7495e2a03f5e824eaac9837cd7f159c67b90fb4b7342");
            _passwordHashes.Add(_employees[1], "e7cf3ef4f17c3999a94f2c6f612e8a888e5b1026878e4e19398b23bd38ec221a");
        }

        public int AuthenticateUserWithEmailAndPasswordHash(string email, string passwordHash)
        {
            var employee = _passwordHashes.FirstOrDefault(emp => emp.Key.Email == email && emp.Value == passwordHash);
            if(employee.Key != null)
            {
                return 1;
            }
            return 0;
        }

        public int InsertEmployee(Employee employee)
        {
            employee.EmployeeID = _nextEmployeeID++;
            _employees.Add(employee);
            return 1;
        }

        public List<Position> SelectAllPositions()
        {
            return new List<Position>
            {
                new Position { PositionID = 1, PositionTitle = "Admin" },
                new Position { PositionID = 2, PositionTitle = "Front Desk Manager" },
                new Position { PositionID = 3, PositionTitle = "Housekeeper" }
            };
        }

        public Employee SelectEmployeeByEmail(string email)
        {
            return _employees.FirstOrDefault(e => e.Email == email);
        }

        public Employee SelectEmployeeByID(int employeeID)
        {
            return _employees.FirstOrDefault(e => e.EmployeeID == employeeID);
        }

        public bool UpdateEmployee(Employee newEmployee, Employee oldEmployee)
        {
            var existingEmployee = _employees.FirstOrDefault(e => e.EmployeeID == oldEmployee.EmployeeID);
            if (existingEmployee != null)
            {
                existingEmployee.FirstName = newEmployee.FirstName;
                existingEmployee.LastName = newEmployee.LastName;
                existingEmployee.Email = newEmployee.Email;
                existingEmployee.Phone = newEmployee.Phone;
                existingEmployee.PositionID = newEmployee.PositionID;
                return true;
            }
            return false;
        }

        public int UpdatePasswordHash(int employeeID, string oldPasswordHash, string newPasswordHash)
        {
            var employee = _passwordHashes.FirstOrDefault(emp => emp.Key.EmployeeID == employeeID && emp.Value == oldPasswordHash);
            if(employee.Key != null)
            {
                _passwordHashes[employee.Key] = newPasswordHash;
                return 1;
            }
            return 0;
        }
    }
}
