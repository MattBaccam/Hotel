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
    public class GuestManagerTests
    {
        private GuestManager _guestManager;
        private GuestAccessorFake _guestAccessorFake;

        [TestInitialize]
        public void TestInitialize()
        {
            _guestAccessorFake = new GuestAccessorFake();//Added this to differ away from having some methods that check to see if accessor fake is being used, rather have the method itsself called directly
            _guestManager = new GuestManager(_guestAccessorFake);
        }

        [TestMethod]
        public void TestAuthenticateGuest()
        {
            string email = "john.doe@example.com";
            string password = "P@ssw0rd";

            bool isAuthenticated = _guestManager.AuthenticateGuest(email, password);

            Assert.IsTrue(isAuthenticated);
        }

        [TestMethod]
        public void TestCreateGuest()
        {
            Guest newGuest = new Guest
            {
                FirstName = "Alice",
                LastName = "Wonderland",
                Email = "alice.wonderland@example.com",
                Phone = "555-555-5555"
            };

            bool isCreated = _guestManager.CreateGuest(newGuest);

            Assert.IsTrue(isCreated);
            Assert.AreEqual(3, _guestAccessorFake.SelectGuestsByFirstName("Alice")[0].GuestID);
        }

        [TestMethod]
        public void TestDoesEmailExist()
        {
            string email = "john.doe@example.com";

            bool exists = _guestManager.DoesEmailExist(email);

            Assert.IsTrue(exists);
        }

        [TestMethod]
        public void TestGetGuestByEmail()
        {
            string email = "john.doe@example.com";

            Guest guest = _guestManager.GetGuestByEmail(email);

            Assert.IsNotNull(guest);
            Assert.AreEqual("John", guest.FirstName);
        }

        [TestMethod]
        public void TestGetGuestByPhone()
        {
            string phone = "123-456-7890";

            Guest guest = _guestManager.GetGuestByPhone(phone);

            Assert.IsNotNull(guest);
            Assert.AreEqual("John", guest.FirstName);
        }

        [TestMethod]
        public void TestGetGuestByFirstName()
        {
            string firstName = "John";

            List<Guest> guests = _guestManager.GetGuestByFirstName(firstName);

            Assert.IsNotNull(guests);
            Assert.AreEqual(1, guests.Count);
        }

        [TestMethod]
        public void TestGetGuestByFirstNameLastName()
        {
            string firstName = "John";
            string lastName = "Doe";

            List<Guest> guests = _guestManager.GetGuestByFirstNameLastName(firstName, lastName);

            Assert.IsNotNull(guests);
            Assert.AreEqual(1, guests.Count);
        }

        [TestMethod]
        public void TestGetGuestByID()
        {
            int guestID = 1;

            Guest guest = _guestManager.GetGuestByID(guestID);

            Assert.IsNotNull(guest);
            Assert.AreEqual("John", guest.FirstName);
        }

        [TestMethod]
        public void TestSaveGuestInfo()
        {
            Guest oldGuest = _guestManager.GetGuestByID(1);
            Guest newGuest = new Guest
            {
                GuestID = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@newemail.com",
                Phone = "123-456-7890"
            };

            bool isUpdated = _guestManager.SaveGuestInfo(newGuest, oldGuest);

            Assert.IsTrue(isUpdated);
            Assert.AreEqual("john.doe@newemail.com", _guestManager.GetGuestByID(1).Email);
        }
    }

}
