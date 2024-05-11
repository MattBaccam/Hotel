using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessInterfaces
{
    public interface IRoomAccessor
    {
        Room SelectRoomByRoomID(int roomID);
        RoomType SelectRoomTypeByRoomTypeID(string roomTypeID);
        bool UpdateRoomStatus(int roomID, string newRoomStatus, string oldRoomStatus);
        bool UpdateRoomAvailability(int roomID, bool newRoomAvailability, bool oldRoomAvailability);
        bool SelectRoomAvailability(int roomID, DateTime checkIn, DateTime checkOut);
        List<Room> SelectRoomAvailabilityExceptReservation(int reservationID, DateTime checkIn, DateTime checkOut);
        List<Room> SelectRoomsAvailable(DateTime checkIn, DateTime checkOut);
        List<Room> SelectRoomsByStatus(string status);
        bool UpdateRoom(Room newRoom, Room oldRoom);
        bool UpdateRoomType(RoomType newRoomType, RoomType oldRoomType);
        List<Room> SelectAllRooms();
    }
}
