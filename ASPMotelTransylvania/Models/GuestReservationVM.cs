using DataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ASPScenicHotel.Models
{
    public class GuestReservationVM
    {
        public Guest Guest { get; set; }
        public Room Room { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public string ReservationStatus { get; set; }
        public int AdultAmount { get; set; }
        public int ChildAmount { get; set; }
        public string Comments { get; set; }
        public int Price { get; set; }
    }
}