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
    public class RoomManager : IRoomManager
    {
        private IRoomAccessor _roomAccessor = new RoomAccessor();

        public Room GetRoom(int roomID)
        {
            try
            {
                return _roomAccessor.SelectRoomByRoomID(roomID);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public RoomType GetRoomType(string roomTypeID)
        {
            try
            {
                return _roomAccessor.SelectRoomTypeByRoomTypeID(roomTypeID);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public RoomVM GetRoomVM(int roomID)
        {
            try
            {
                var room = GetRoom(roomID);
                var roomVM = new RoomVM();
                var roomType = GetRoomType(room.RoomTypeID);
                roomVM.RoomID = room.RoomID;
                roomVM.RoomTypeID = room.RoomTypeID;
                roomVM.RoomAvailability = room.RoomAvailability;
                roomVM.RoomStatus = room.RoomStatus;
                roomVM.RoomType = roomType;
                roomVM.RoomPrice = roomType.RoomPrice;
                return roomVM;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool SaveRoomStatus(int roomID, string oldStatus, string newStatus)
        {
            try
            {
                return _roomAccessor.UpdateRoomStatus(roomID, newStatus, oldStatus);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<RoomVM> GetAllRooms()
        {
            try
            {
                var roomList = new List<RoomVM>();
                foreach (var room in _roomAccessor.SelectAllRooms())
                {
                    roomList.Add(GetRoomVM(room.RoomID));
                }
                return roomList;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool SelectRoomAvailability(int roomID, DateTime checkIn, DateTime checkOut)
        {
            try
            {
                return _roomAccessor.SelectRoomAvailability(roomID, checkIn, checkOut);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<RoomVM> GetRoomsAvailable(DateTime checkIn, DateTime checkOut)
        {
            try
            {
                var roomList = new List<RoomVM>();
                foreach (var room in _roomAccessor.SelectRoomsAvailable(checkIn, checkOut))
                {
                    roomList.Add(GetRoomVM(room.RoomID));
                } 
                return roomList;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<RoomVM> GetRoomsByStatus(string status)
        {
            try
            {
                var roomListVM = new List<RoomVM>();
                var roomList = _roomAccessor.SelectRoomsByStatus(status);
                roomList.ForEach(r => roomListVM.Add(new RoomVM()
                {
                    RoomID = r.RoomID,
                    RoomTypeID = r.RoomTypeID,
                    RoomAvailability = r.RoomAvailability,
                    RoomStatus = r.RoomStatus,
                    RoomPrice = int.Parse(GetRoomType(r.RoomTypeID).RoomPrice.ToString()),
                    RoomType = GetRoomType(r.RoomTypeID),
                }));
                return roomListVM;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool UpdateRoom(Room newRoom, Room oldRoom)
        {
            try
            {
                return _roomAccessor.UpdateRoom(newRoom, oldRoom);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool UpdateRoomAvailability(int roomID, bool newRoomAvailability, bool oldRoomAvailability)
        {
            try
            {
                return _roomAccessor.UpdateRoomAvailability(roomID, newRoomAvailability, oldRoomAvailability);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool UpdateRoomType(RoomType newRoomType, RoomType oldRoomType)
        {
            try
            {
                return _roomAccessor.UpdateRoomType(newRoomType, oldRoomType);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<RoomVM> GetRoomsAvailableExceptReservation(int reservationID, DateTime checkIn, DateTime checkOut)
        {
            try
            {
                var roomListVM = new List<RoomVM>();
                var roomList = _roomAccessor.SelectRoomAvailabilityExceptReservation(reservationID, checkIn, checkOut);
                roomList.ForEach(r => roomListVM.Add(new RoomVM()
                {
                    RoomID = r.RoomID,
                    RoomTypeID = r.RoomTypeID,
                    RoomAvailability = r.RoomAvailability,
                    RoomStatus = r.RoomStatus,
                    RoomPrice = int.Parse(GetRoomType(r.RoomTypeID).RoomPrice.ToString()),
                    RoomType = GetRoomType(r.RoomTypeID),
                }));
                return roomListVM;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
