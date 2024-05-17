using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        public int PositionID { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please provide a valid email")]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]\d{3}[\s.-]\d{4}$", ErrorMessage = "Please provide a valid phone number")]
        public string Phone { get; set; }
        public Employee DeepCopy()
        {
            Employee deepcopyEmployee = new Employee()
            {
                EmployeeID = this.EmployeeID,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Email = this.Email,
                Phone = this.Phone,
                PositionID = this.PositionID
            };
            return deepcopyEmployee;
        }
    }
    public class EmployeeVM : Employee
    {
        [Required]
        public Position Position{ get; set; }
    }
}
