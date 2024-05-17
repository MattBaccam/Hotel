using DataAccessFakes;
using DataObjects;
using LogicLayer;
using LogicLayerInterfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
namespace LogicLayerTests
{
    [TestClass]
    public class RoomManagerTests
    {
        private IRoomManager _roomManager;
        private RoomAccessorFake _roomAccessorFake;

        [TestInitialize]
        public void TestSetUp()
        {
            _roomManager = new RoomManager(new RoomAccessorFake());
            _roomAccessorFake = new RoomAccessorFake();//Added this to differ away from having some methods that check to see if accessor fake is being used, rather have the method itsself called directly
        }

        [TestMethod]
        public void TestGetRoom()
        {
            int roomID = 1;

            var room = _roomManager.GetRoom(roomID);

            Assert.IsNotNull(room);
            Assert.AreEqual(roomID, room.RoomID);
        }

        [TestMethod]
        public void TestGetRoomType()
        {
            string roomTypeID = "Single";

            var roomType = _roomManager.GetRoomType(roomTypeID);

            Assert.IsNotNull(roomType);
            Assert.AreEqual(roomTypeID, roomType.RoomTypeID);
        }

        [TestMethod]
        public void TestGetRoomVM()
        {
            int roomID = 1;

            var roomVM = _roomManager.GetRoomVM(roomID);

            Assert.IsNotNull(roomVM);
            Assert.AreEqual(roomID, roomVM.RoomID);
            Assert.IsNotNull(roomVM.RoomType);
        }

        [TestMethod]
        public void TestSaveRoomStatus()
        {
            int roomID = 1;
            string oldStatus = "Clean";
            string newStatus = "Occupied";

            bool statusUpdated = _roomManager.SaveRoomStatus(roomID, oldStatus, newStatus);

            Assert.IsTrue(statusUpdated);
        }

        [TestMethod]
        public void TestGetAllRooms()
        {
            var rooms = _roomManager.GetAllRooms();

            Assert.IsNotNull(rooms);
        }

        [TestMethod]
        public void TestSelectRoomAvailability()
        {
            int roomID = 1;
            DateTime checkIn = DateTime.Now;
            DateTime checkOut = DateTime.Now.AddDays(1);

            bool isAvailable = _roomManager.SelectRoomAvailability(roomID, checkIn, checkOut);

            Assert.IsTrue(isAvailable);
        }

        [TestMethod]
        public void TestGetRoomsAvailable()
        {
            DateTime checkIn = DateTime.Now;
            DateTime checkOut = DateTime.Now.AddDays(1);

            var rooms = _roomAccessorFake.SelectRoomsAvailable(checkIn, checkOut);

            Assert.IsNotNull(rooms);
        }

        [TestMethod]
        public void TestGetRoomsByStatus()
        {
            string status = "Clean";

            var rooms = _roomAccessorFake.SelectRoomsByStatus(status);

            Assert.IsNotNull(rooms);
        }

        [TestMethod]
        public void TestUpdateRoom()
        {
            var oldRoom = new Room { RoomID = 1, RoomTypeID = "Single", RoomAvailability = true, RoomStatus = "Clean" };
            var newRoom = new Room { RoomID = 1, RoomTypeID = "Single", RoomAvailability = false, RoomStatus = "Occupied" };

            bool updated = _roomManager.UpdateRoom(newRoom, oldRoom);

            Assert.IsTrue(updated);
        }

        [TestMethod]
        public void TestUpdateRoomAvailability()
        {
            int roomID = 1;
            bool newAvailability = false;
            bool oldAvailability = true;

            bool availabilityUpdated = _roomManager.UpdateRoomAvailability(roomID, newAvailability, oldAvailability);

            Assert.IsTrue(availabilityUpdated);
        }

        [TestMethod]
        public void TestUpdateRoomType()
        {
            var oldRoomType = new RoomType { RoomTypeID = "Single", RoomPrice = 100, RoomDescription = "Standard Room" };
            var newRoomType = new RoomType { RoomTypeID = "Single", RoomPrice = 120, RoomDescription = "Updated Room" };

            bool updated = _roomManager.UpdateRoomType(newRoomType, oldRoomType);

            Assert.IsTrue(updated);
        }

        [TestMethod]
        public void TestGetRoomsAvailableExceptReservation()
        {
            int reservationID = 1;
            DateTime checkIn = DateTime.Now;
            DateTime checkOut = DateTime.Now.AddDays(1);

            var rooms = _roomAccessorFake.SelectRoomAvailabilityExceptReservation(reservationID, checkIn, checkOut);

            Assert.IsNotNull(rooms);
        }
    }
}