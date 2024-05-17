using DataAccessInterfaces;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessFakes
{
    public class RoomChargeAccessorFake : IRoomChargeAccessor
    {
        private List<RoomCharge> _roomCharges = new List<RoomCharge>();
        private List<RoomChargeItem> _roomChargeItems = new List<RoomChargeItem>();
        private List<Pantry> _pantryItems = new List<Pantry>();
        private List<SalesLog> _salesLogs = new List<SalesLog>();
        
        public RoomChargeAccessorFake() 
        {
            _roomCharges.Add(new RoomCharge { RoomChargeID = 1, ReservationID = 101 });
            _roomCharges.Add(new RoomCharge { RoomChargeID = 2, ReservationID = 102 });
            _roomCharges.Add(new RoomCharge { RoomChargeID = 3, ReservationID = 103 });

            _roomChargeItems.Add(new RoomChargeItem { RoomChargeItemID = 1, RoomChargeID = 1, PantryID = "Snack", ItemAmount = 2, Active = true });
            _roomChargeItems.Add(new RoomChargeItem { RoomChargeItemID = 2, RoomChargeID = 1, PantryID = "Snack", ItemAmount = 1, Active = true });
            _roomChargeItems.Add(new RoomChargeItem { RoomChargeItemID = 3, RoomChargeID = 2, PantryID = "Snack", ItemAmount = 3, Active = false });
            _roomChargeItems.Add(new RoomChargeItem { RoomChargeItemID = 4, RoomChargeID = 3, PantryID = "Snack", ItemAmount = 2, Active = true });

            _pantryItems.Add(new Pantry { PantryID = "Snack", ItemPrice = 10, ItemAmount = 20 });
            _pantryItems.Add(new Pantry { PantryID = "Chips", ItemPrice = 15, ItemAmount = 10 });
            _pantryItems.Add(new Pantry { PantryID = "Water", ItemPrice = 5, ItemAmount = 30 });
            _pantryItems.Add(new Pantry { PantryID = "Soda", ItemPrice = 8, ItemAmount = 25 });

            _salesLogs.Add(new SalesLog { LogID = 1, EmployeeID = 101, GuestID = 201, TimeOfSale = DateTime.Now, PantryID = "Snack", SoldPrice = 20, ItemAmount = 2 });
            _salesLogs.Add(new SalesLog { LogID = 2, EmployeeID = 102, GuestID = 202, TimeOfSale = DateTime.Now, PantryID = "Snack", SoldPrice = 30, ItemAmount = 1 });
            _salesLogs.Add(new SalesLog { LogID = 3, EmployeeID = 103, GuestID = 203, TimeOfSale = DateTime.Now, PantryID = "Snack", SoldPrice = 15, ItemAmount = 3 });
        }
        public bool AuthenticateRoomCharges(int reservationID)
        {
            return _roomCharges.Any(rc => rc.ReservationID == reservationID);
        }

        public bool CreateRoomCharge(int reservationID)
        {
            _roomCharges.Add(new RoomCharge { RoomChargeID = _roomCharges.Count + 1, ReservationID = reservationID });
            return true;
        }

        public bool InsertRoomChargeItems(int roomChargeID, Pantry pantry)
        {
            _roomChargeItems.Add(new RoomChargeItem()
            {
                RoomChargeItemID = 1,
                RoomChargeID = roomChargeID,
                PantryID = pantry.PantryID,
                ItemAmount = 1,
                Active = true
            });
            return true;
        }

        public bool InsertSalesLogs(SalesLog salesLog)
        {
            _salesLogs.Add(salesLog);
            return true;
        }

        public List<RoomChargeItem> SelectActiveRoomChargeItems(int roomChargeID)
        {
            return _roomChargeItems.Where(item => item.RoomChargeID == roomChargeID && item.Active).ToList();
        }

        public Pantry SelectPantryItem(string pantryID)
        {
            return _pantryItems.FirstOrDefault(p => p.PantryID == pantryID);
        }

        public List<Pantry> SelectPantryItems()
        {
            return _pantryItems;
        }

        public RoomCharge SelectRoomCharge(int reservationID)
        {
            return _roomCharges.FirstOrDefault(rc => rc.ReservationID == reservationID);
        }

        public List<RoomChargeItem> SelectRoomChargeItems(int roomChargeID)
        {
            return _roomChargeItems.Where(item => item.RoomChargeID == roomChargeID).ToList();
        }

        public List<SalesLog> SelectSalesLogs()
        {
            return _salesLogs;
        }

        public bool UpdatePantryQuantity(string pantryID, int newItemAmount, int oldItemAmount)
        {
            var pantryItem = _pantryItems.FirstOrDefault(p => p.PantryID == pantryID);
            if (pantryItem != null)
            {
                pantryItem.ItemAmount += newItemAmount - oldItemAmount;
                return true;
            }
            return false;
        }

        public bool UpdatePantryQuantityAndPrice(Pantry newPantryItem, Pantry oldPantryItem)
        {
            var pantryItem = _pantryItems.FirstOrDefault(p => p.PantryID == newPantryItem.PantryID);
            if (pantryItem != null)
            {
                pantryItem.ItemAmount = newPantryItem.ItemAmount;
                pantryItem.ItemPrice = newPantryItem.ItemPrice;
                return true;
            }
            return false;
        }

        public bool UpdateRoomChargeItemForRefund(int roomChargeItemID, int roomChargeID, bool newActive, bool oldActive)
        {
            var roomChargeItem = _roomChargeItems.FirstOrDefault(item => item.RoomChargeItemID == roomChargeItemID && item.RoomChargeID == roomChargeID && item.Active == oldActive);
            if (roomChargeItem != null)
            {
                roomChargeItem.Active = newActive;
                return true;
            }
            return false;
        }

    }
}