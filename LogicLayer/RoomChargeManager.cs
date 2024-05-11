using DataAccessInterfaces;
using DataAccessLayer;
using DataObjects;
using LogicLayerInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class RoomChargeManager : IRoomChargeManager
    {
        private IRoomChargeAccessor _roomChargeAccessor;

        public RoomChargeManager()
        {
            _roomChargeAccessor = new RoomChargeAccessor();
        }

        public RoomChargeManager(IRoomChargeAccessor roomAccessorFake)
        {
            _roomChargeAccessor = roomAccessorFake;
        }

        public List<RoomChargeItem> GetRoomChargeItems(int roomChargeID)
        {
            try
            {
                return _roomChargeAccessor.SelectRoomChargeItems(roomChargeID);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<RoomChargeItemVM> GetRoomChargeItemsVM(int roomChargeID)
        {
            try
            {
                var itemsList = GetRoomChargeItems(roomChargeID);
                var itemsListVM = new List<RoomChargeItemVM>();
                foreach (var item in itemsList)
                {
                    var itemVM = new RoomChargeItemVM()
                    {
                        RoomChargeItemID = item.RoomChargeItemID,
                        RoomChargeID = item.RoomChargeID,
                        PantryID = item.PantryID,
                        ItemAmount = item.ItemAmount,
                        Active = item.Active
                    };
                    try
                    {
                        itemVM.Pantry = GetPantryItem(item.PantryID);
                        itemVM.ItemPrice = itemVM.Pantry.ItemPrice;
                        itemsListVM.Add(itemVM);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                return itemsListVM;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public RoomCharge GetRoomCharge(int reservationID)
        {
            try
            {
                return _roomChargeAccessor.SelectRoomCharge(reservationID);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public RoomChargeVM GetRoomChargeVM(int reservationID, int roomID)
        {
            try
            {
                var roomChargeVM = new RoomChargeVM();
                if (CheckForRoomCharges(reservationID))
                {
                    var roomCharge = GetRoomCharge(reservationID);
                    roomChargeVM.RoomChargeID = roomCharge.RoomChargeID;
                    roomChargeVM.ReservationID = roomCharge.ReservationID;
                    roomChargeVM.RoomChargeItems = GetRoomChargeItemsVM(roomCharge.RoomChargeID);
                }
                return roomChargeVM;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CreateRoomCharge(int reservationID)
        {
            try
            {
                return _roomChargeAccessor.CreateRoomCharge(reservationID);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool PostRoomCharges(int roomCharge, List<Pantry> cart)
        {
            bool result = false;
            try
            {
                cart.ForEach(item =>
                {
                    result = _roomChargeAccessor.InsertRoomChargeItems(roomCharge, item);
                    result = _roomChargeAccessor.UpdatePantryQuantity(item.PantryID, item.ItemAmount, GetPantryItem(item.PantryID).ItemAmount);
                });
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool CreateSalesLog(SalesLog salesLog)
        {
            try
            {
               return _roomChargeAccessor.InsertSalesLogs(salesLog);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<SalesLogVM> GetSalesLogList()
        {
            var salesLogList = new List<SalesLogVM>();
            try
            {
                var salesLogs = _roomChargeAccessor.SelectSalesLogs();
                var employeeManager = new EmployeeManager();
                var guestManager = new GuestManager();
                salesLogs.ForEach(s => salesLogList.Add(new SalesLogVM()
                {
                    LogID = s.LogID,
                    EmployeeID = s.EmployeeID,
                    GuestID = s.GuestID,
                    TimeOfSale = s.TimeOfSale,
                    PantryID = s.PantryID,
                    SoldPrice = s.SoldPrice,
                    ItemAmount = s.ItemAmount,
                    Employee = employeeManager.GetEmployeeByID(s.EmployeeID),
                    EmployeeName = $"{employeeManager.GetEmployeeByID(s.EmployeeID).FirstName} {employeeManager.GetEmployeeByID(s.EmployeeID).LastName}",
                    Guest = guestManager.GetGuestByID(s.GuestID),
                    GuestName = $"{guestManager.GetGuestByID(s.GuestID).FirstName} {guestManager.GetGuestByID(s.GuestID).LastName}"

            }));
                return salesLogList;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Pantry GetPantryItem(string pantryID)
        {
            try
            {
                return _roomChargeAccessor.SelectPantryItem(pantryID);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool RefundRoomCharge(int roomChargeItemID, int roomChargeID)
        {
            try
            {
                return _roomChargeAccessor.UpdateRoomChargeItemForRefund(roomChargeItemID, roomChargeID, false, true);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<RoomChargeItemVM> GetActiveRoomChargeItems(int roomChargeID)
        {
            try
            {
                var itemsList = _roomChargeAccessor.SelectActiveRoomChargeItems(roomChargeID);
                var itemsListVM = new List<RoomChargeItemVM>();
                foreach (var item in itemsList)
                {
                    var pantryItem = GetPantryItem(item.PantryID);
                    var itemVM = new RoomChargeItemVM()
                    {
                        RoomChargeItemID = item.RoomChargeItemID,
                        RoomChargeID = item.RoomChargeID,
                        PantryID = pantryItem.PantryID,
                        ItemAmount = item.ItemAmount,
                        Active = item.Active
                    };
                    try
                    {
                        itemVM.Pantry = GetPantryItem(item.PantryID);
                        itemVM.ItemPrice = itemVM.Pantry.ItemPrice * item.ItemAmount;
                        itemsListVM.Add(itemVM);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                return itemsListVM;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CheckForRoomCharges(int reservationID)
        {
            try
            {
                return _roomChargeAccessor.AuthenticateRoomCharges(reservationID);
            }
            catch (Exception ex) 
            {

                throw ex;
            }
        }

        public bool UpdateInventory(List<Pantry> newInventory, List<Pantry> oldInventory)
        {
            try
            {
                var result = false;
                newInventory.ForEach(p =>
                {
                    result = _roomChargeAccessor.UpdatePantryQuantityAndPrice(p, oldInventory.FirstOrDefault(pI => pI.PantryID == p.PantryID));
                });
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<Pantry> GetPantryItems()
        {
            try
            {
                return _roomChargeAccessor.SelectPantryItems();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
