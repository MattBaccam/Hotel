using DataAccessFakes;
using DataObjects;
using LogicLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogicLayerTests
{
    [TestClass]
    public class ReservationManagerTests
    {
        private ReservationManager _reservationManager;
        private ReservationAccessorFake _reservationAccessorFake;

        [TestInitialize]
        public void TestSetup()
        {
            _reservationManager = new ReservationManager(new ReservationAccessorFake());
            _reservationAccessorFake = new ReservationAccessorFake();//Added this to differ away from having some methods that check to see if accessor fake is being used, rather have the method itsself called directly
        }

        [TestMethod]
        public void TestGetReservationByID()
        {
            int reservationID = 1;

            var reservation = _reservationManager.GetReservationByID(reservationID);

            Assert.IsNotNull(reservation);
            Assert.AreEqual(reservationID, reservation.ReservationID);
        }

        [TestMethod]
        public void TestGetEventsByStatus()
        {
            string status = "Pending";

            var events = _reservationManager.GetEventsByStatus(status);

            Assert.IsTrue(events.Count <= 0);
        }

        [TestMethod]
        public void TestGetReservationsByStatus()
        {
            string status = "Pending";

            var reservations = _reservationManager.GetReservationsByStatus(status);

            Assert.IsTrue(reservations.Count <= 0);
        }

        [TestMethod]
        public void TestSaveReservationComments()
        {
            int reservationID = 1;
            string newComments = "Updated comments";

            bool result = _reservationManager.SaveReservationComments(reservationID, newComments);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestSaveReservationForCheckIn()
        {
            int reservationID = 1;
            DateTime newCheckIn = DateTime.Today;
            DateTime oldCheckIn = DateTime.Today.AddDays(1);

            bool result = _reservationManager.SaveReservationForCheckIn(reservationID, newCheckIn, oldCheckIn);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestSaveReservationForCheckOut()
        {
            int reservationID = 1;
            DateTime newCheckOut = DateTime.Today.AddDays(4);
            DateTime oldCheckOut = DateTime.Today.AddDays(3);

            bool result = _reservationAccessorFake.UpdateReservationForCheckOut(reservationID, newCheckOut, oldCheckOut);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestSaveReservation()
        {
            var newReservation = new Reservation() { ReservationID = 1, GuestID = 1 };
            var oldReservation = new Reservation() { ReservationID = 1, GuestID = 1 };

            bool result = _reservationManager.SaveReservation(newReservation, oldReservation);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestSaveReservationForReschedule()
        {
            int reservationID = 1;
            DateTime newCheckIn = DateTime.Today.AddDays(2);
            DateTime newCheckOut = DateTime.Today.AddDays(4);
            DateTime oldCheckIn = DateTime.Today.AddDays(1);
            DateTime oldCheckOut = DateTime.Today.AddDays(3);

            bool result = _reservationManager.SaveReservationForReschedule(reservationID, newCheckIn, newCheckOut, oldCheckIn, oldCheckOut);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestSaveReservationForCancel()
        {
            int reservationID = 1;
            string oldReservationStatus = "Pending";

            bool result = _reservationManager.SaveReservationForCancel(reservationID, oldReservationStatus);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestCreateReservation()
        {
            var reservation = new Reservation() { ReservationID = 3, GuestID = 1 };

            bool result = _reservationManager.CreateReservation(reservation);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestGetReservationsByNameAndRoomID()
        {
            string firstName = "John";
            string lastName = "Doe";
            int roomID = 1;

            var reservation = _reservationAccessorFake.SelectReservationByFirstNameLastNameRoomID(firstName, lastName, roomID);

            Assert.IsNotNull(reservation);
        }

        [TestMethod]
        public void TestGetReservationByRoomID()
        {
            int roomID = 1;

            var reservation = _reservationAccessorFake.SelectReservationByRoomID(roomID);

            Assert.IsNotNull(reservation);
        }

        [TestMethod]
        public void TestGetReservationsByGuestID()
        {
            int guestID = 1;

            var reservations = _reservationAccessorFake.SelectReservationsByGuestID(guestID);

            Assert.IsNotNull(reservations);
            Assert.IsTrue(reservations.Count > 0);
        }

        [TestMethod]
        public void TestGetReservationsByAsc()
        {
            int guestID = 1;

            var reservations = _reservationAccessorFake.SelectReservationsByAsc(guestID);

            Assert.IsNotNull(reservations);
            Assert.IsTrue(reservations.Count > 0);
        }

        [TestMethod]
        public void TestGetReservationsByDesc()
        {
            int guestID = 1;

            var reservations = _reservationAccessorFake.SelectReservationsByDesc(guestID);

            Assert.IsNotNull(reservations);
            Assert.IsTrue(reservations.Count > 0);
        }

        [TestMethod]
        public void TestGetReservationsForGuestByStatus()
        {
            int guestID = 1;
            string status = "Pending";

            var reservations = _reservationAccessorFake.SelectReservationsForGuestByStatus(guestID, status);

            Assert.IsTrue(reservations.Count <= 0);
        }

        [TestMethod]
        public void TestSaveReservationForRescheduleNewRoom()
        {
            int reservationID = 1;
            int newRoomID = 2;
            DateTime newCheckIn = DateTime.Today.AddDays(1);
            DateTime newCheckOut = DateTime.Today.AddDays(2);
            int oldRoomID = 1;
            DateTime oldCheckIn = DateTime.Today.AddDays(1);
            DateTime oldCheckOut = DateTime.Today.AddDays(3);

            bool result = _reservationAccessorFake.UpdateReservationForRescheduleNewRoom(reservationID, newRoomID, newCheckIn, newCheckOut, oldRoomID, oldCheckIn, oldCheckOut);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestUpdateReservationChildAdultAmount()
        {
            int reservationID = 1;
            int newAdultAmount = 2;
            int newChildAmount = 1;
            int oldAdultAmount = 2;
            int oldChildAmount = 0;

            bool result = _reservationAccessorFake.UpdateReservationChildAdultAmount(reservationID, newAdultAmount, newChildAmount, oldAdultAmount, oldChildAmount);

            Assert.IsTrue(result);
        }
    }
}
