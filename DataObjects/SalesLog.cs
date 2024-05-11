using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class SalesLog
    {
        public int LogID { get; set; }
        public int EmployeeID { get; set; }
        public int GuestID { get; set; }
        public DateTime TimeOfSale { get; set; }
        public string PantryID { get; set; }
        public int SoldPrice { get; set; }
        public int ItemAmount { get; set; }
    }

    public class SalesLogVM : SalesLog
    {
        public Employee Employee { get; set; }
        public string EmployeeName { get; set; }
        public string GuestName { get; set; }
        public Guest Guest { get; set; }
    }
}
