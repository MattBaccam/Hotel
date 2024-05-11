using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class RoomCharge
    {
        public int RoomChargeID { get; set; }
        public int ReservationID { get; set; }
    }
    public class RoomChargeVM : RoomCharge
    {
        public List<RoomChargeItemVM> RoomChargeItems { get; set; }
    }
}
