using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayerInterfaces
{
    public interface IRoomChargeManager
    {
        RoomCharge GetRoomCharge(int reservationID);
        bool CreateRoomCharge(int reservationID);
        bool PostRoomCharges(int RoomCharge, List<Pantry> cart);
        List<RoomChargeItem> GetRoomChargeItems(int roomChargeID);
        List<Pantry> GetPantryItems();
        List<RoomChargeItemVM> GetActiveRoomChargeItems(int roomChargeID);
        List<RoomChargeItemVM> GetRoomChargeItemsVM(int roomChargeID);
        RoomChargeVM GetRoomChargeVM(int reservationID, int roomID);
        bool CreateSalesLog(SalesLog salesLog);
        List<SalesLogVM> GetSalesLogList();
        Pantry GetPantryItem(string pantryID);
        bool RefundRoomCharge(int roomChargeItemID, int roomChargeID);
        bool CheckForRoomCharges(int reservationID);
        bool UpdateInventory(List<Pantry> newInventory, List<Pantry> oldInventory);
    }
}
