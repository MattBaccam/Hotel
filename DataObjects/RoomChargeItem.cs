using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class RoomChargeItem
    {
        public int RoomChargeItemID { get; set; }
        public int RoomChargeID { get; set; }
        public string PantryID { get; set; }
        public int ItemAmount { get; set; }
        public bool Active { get; set; }
    }

    public class RoomChargeItemVM : RoomChargeItem
    {
        public Pantry Pantry { get; set; }
        public int ItemPrice { get; set; }
    }
}
