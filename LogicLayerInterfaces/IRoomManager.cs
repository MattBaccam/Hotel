using DataObjects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayerInterfaces
{
    public interface IRoomManager
    {
        Room GetRoom(int roomID);
        RoomType GetRoomType(string roomTypeID);
        RoomVM GetRoomVM(int roomID);
        bool SaveRoomStatus(int roomID, string newStatus, string oldStatus);
        List<RoomVM> GetRoomsAvailable(DateTime checkIn, DateTime checkOut);
        List<RoomVM> GetRoomsAvailableExceptReservation(int reservationID, DateTime checkIn, DateTime checkOut);
        bool SelectRoomAvailability(int roomID, DateTime checkIn, DateTime checkOut);
        bool UpdateRoom(Room newRoom, Room oldRoom);
        bool UpdateRoomType(RoomType newRoomType, RoomType oldRoomType);
        bool UpdateRoomAvailability(int roomID,bool newRoomAvailability, bool oldRoomAvailability);
        List<RoomVM> GetRoomsByStatus(string status);
        List<RoomVM> GetAllRooms();
    }
}
