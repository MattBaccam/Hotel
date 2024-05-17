using DataAccessInterfaces;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessFakes
{
    public class RoomAccessorFake : IRoomAccessor
    {
        public static List<Room> _rooms = new List<Room>();
        public static List<RoomVM> _roomVMs = new List<RoomVM>();
        public static List<RoomType> _roomTypes = new List<RoomType>();

        public RoomAccessorFake() 
        {
            _roomTypes.Add(new RoomType { RoomTypeID = "Single", RoomPrice = 100, RoomDescription = "Standard Room" });
            _roomTypes.Add(new RoomType { RoomTypeID = "Double", RoomPrice = 150, RoomDescription = "Deluxe Room" });
            _roomTypes.Add(new RoomType { RoomTypeID = "Single", RoomPrice = 200, RoomDescription = "Suite" });

            _rooms.Add(new Room { RoomID = 1, RoomTypeID = "Single", RoomAvailability = true, RoomStatus = "Clean" });
            _rooms.Add(new Room { RoomID = 2, RoomTypeID = "Double", RoomAvailability = true, RoomStatus = "Clean" });
            _rooms.Add(new Room { RoomID = 3, RoomTypeID = "Single", RoomAvailability = true, RoomStatus = "Clean" });
            _rooms.Add(new Room { RoomID = 4, RoomTypeID = "Double", RoomAvailability = false, RoomStatus = "Occupied" });
            _rooms.Add(new Room { RoomID = 5, RoomTypeID = "Single", RoomAvailability = true, RoomStatus = "Clean" });

            foreach (var room in _rooms)
            {
                var roomType = _roomTypes.Find(rt => rt.RoomTypeID == room.RoomTypeID);
                _roomVMs.Add(new RoomVM
                {
                    RoomID = room.RoomID,
                    RoomTypeID = room.RoomTypeID,
                    RoomAvailability = room.RoomAvailability,
                    RoomStatus = room.RoomStatus,
                    RoomPrice = roomType.RoomPrice,
                    RoomType = roomType
                });
            }
        }

        public List<Room> SelectAllRooms()
        {
            return _rooms;
        }

        public bool SelectRoomAvailability(int roomID, DateTime checkIn, DateTime checkOut)
        {
            // Get the room by ID
            var room = _rooms.FirstOrDefault(r => r.RoomID == roomID);
            if (room == null)
                return false;

            // Check room availability within the given time frame
            return !_roomVMs.Any(r => r.RoomID == roomID && r.RoomAvailability == false);
        }

        public List<Room> SelectRoomAvailabilityExceptReservation(int reservationID, DateTime checkIn, DateTime checkOut)
        {
            return _rooms
                .Where(r => r.RoomAvailability != false && r.RoomID != reservationID)
                .Distinct()
                .ToList();
        }

        public Room SelectRoomByRoomID(int roomID)
        {
            return _rooms.FirstOrDefault(r => r.RoomID == roomID);
        }

        public List<Room> SelectRoomsAvailable(DateTime checkIn, DateTime checkOut)
        {
            // Get the reserved rooms within the time frame
            var reservedRoomIDs = _roomVMs
                .Where(r => !r.RoomAvailability)
                .Select(r => r.RoomID)
                .Distinct()
                .ToList();

            // Get the available rooms excluding the reserved rooms
            return _rooms
                .Where(r => !reservedRoomIDs.Contains(r.RoomID))
                .ToList();
        }

        public List<Room> SelectRoomsByStatus(string status)
        {
            return _rooms.Where(r => r.RoomStatus == status).ToList();
        }

        public RoomType SelectRoomTypeByRoomTypeID(string roomTypeID)
        {
            return _roomTypes.FirstOrDefault(rt => rt.RoomTypeID == roomTypeID);
        }

        public bool UpdateRoom(Room newRoom, Room oldRoom)
        {
            var index = _rooms.FindIndex(r => r.RoomID == oldRoom.RoomID);
            if (index == -1)
                return false;

            _rooms[index] = newRoom;
            return true;
        }

        public bool UpdateRoomAvailability(int roomID, bool newRoomAvailability, bool oldRoomAvailability)
        {
            var room = _rooms.FirstOrDefault(r => r.RoomID == roomID);
            if (room == null || room.RoomAvailability != oldRoomAvailability)
                return false;

            room.RoomAvailability = newRoomAvailability;
            return true;
        }

        public bool UpdateRoomStatus(int roomID, string newRoomStatus, string oldRoomStatus)
        {
            var room = _rooms.FirstOrDefault(r => r.RoomID == roomID);
            if (room == null || room.RoomStatus != oldRoomStatus)
                return false;

            room.RoomStatus = newRoomStatus;
            return true;
        }

        public bool UpdateRoomType(RoomType newRoomType, RoomType oldRoomType)
        {
            var index = _roomTypes.FindIndex(rt => rt.RoomTypeID == oldRoomType.RoomTypeID);
            if (index == -1)
                return false;

            _roomTypes[index] = newRoomType;
            return true;
        }

    }
}
