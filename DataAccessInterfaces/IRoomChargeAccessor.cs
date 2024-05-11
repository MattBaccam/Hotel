using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessInterfaces
{
    public interface IRoomChargeAccessor
    {
        RoomCharge SelectRoomCharge(int reservationID);
        bool CreateRoomCharge(int reservationID);
        List<RoomChargeItem> SelectRoomChargeItems(int roomChargeID);
        List<RoomChargeItem> SelectActiveRoomChargeItems(int roomChargeID);
        bool InsertRoomChargeItems(int roomCharge, Pantry roomChargeItem);
        Pantry SelectPantryItem(string pantryID);
        List<Pantry> SelectPantryItems();
        bool AuthenticateRoomCharges(int reservationID);
        bool UpdatePantryQuantity(string pantryID, int newItemAmount, int oldItemAmount);
        List<SalesLog> SelectSalesLogs();
        bool InsertSalesLogs(SalesLog salesLog);
        bool UpdateRoomChargeItemForRefund(int roomChargeItemID, int roomChargeID ,bool newActive, bool oldActive);
        bool UpdatePantryQuantityAndPrice(Pantry newRoomCharge, Pantry oldRoomCharge);
    }
}
