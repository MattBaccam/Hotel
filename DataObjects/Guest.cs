using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class Guest
    {
        public int GuestID { get; set; }

        [Required(ErrorMessage = "Required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Required")]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(@"^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]\d{3}[\s.-]\d{4}$", ErrorMessage = "Please provide a valid phone number")]
        public string Phone { get; set; }

        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please provide a valid email")]
        public string Email { get; set; }

        public Guest DeepCopy()
        {
            Guest deepcopyGuest = new Guest()
            {
                GuestID = this.GuestID,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Email = this.Email,
                Phone = this.Phone
            };
            return deepcopyGuest;
        }
    }
}
