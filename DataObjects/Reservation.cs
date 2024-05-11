using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class Reservation
    {
        public int ReservationID { get; set; }
        public int GuestID { get; set; }
        public int RoomID { get; set; }
        public string ReservationStatus { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        [DataType(DataType.MultilineText)]
        public string Comments { get; set; }
        public int AdultAmount { get; set; }
        public int ChildAmount { get; set; }
        public bool Paid { get; set; }  
    }
    public class ReservationVM : Reservation
    {
        public string Name { get; set; }    
        public Guest Guest { get; set; }
        public RoomVM Room { get; set; }
        public RoomChargeVM RoomCharges { get; set; }
    }
}
