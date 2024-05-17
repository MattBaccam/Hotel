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
    public class RoomChargeManagerTests
    {
        private IRoomChargeManager _roomChargeManager;

        [TestInitialize]
        public void TestSetUp()
        {
            _roomChargeManager = new RoomChargeManager(new RoomChargeAccessorFake());
        }

        [TestMethod]
        public void TestAuthenticateRoomCharges()
        {
            int reservationID = 101;

            bool authenticated = _roomChargeManager.CheckForRoomCharges(reservationID);

            Assert.IsTrue(authenticated);
        }

        [TestMethod]
        public void TestCreateRoomCharge()
        {
            int reservationID = 105;

            bool created = _roomChargeManager.CreateRoomCharge(reservationID);

            Assert.IsTrue(created);
        }

        [TestMethod]
        public void TestInsertRoomChargeItems()
        {
            int roomChargeID = 1;
            var pantry = new Pantry
            {
                PantryID = "Snack",
                ItemPrice = 10,
                ItemAmount = 5
            };

            bool inserted = _roomChargeManager.PostRoomCharges(roomChargeID, new List<Pantry> { pantry });

            Assert.IsTrue(inserted);
        }

        [TestMethod]
        public void TestInsertSalesLogs()
        {
            var salesLog = new SalesLog
            {
                LogID = 4,
                EmployeeID = 104,
                GuestID = 204,
                TimeOfSale = DateTime.Now,
                PantryID = "Snack",
                SoldPrice = 25,
                ItemAmount = 4
            };

            bool inserted = _roomChargeManager.CreateSalesLog(salesLog);

            Assert.IsTrue(inserted);
        }

        [TestMethod]
        public void TestSelectActiveRoomChargeItems()
        {
            int roomChargeID = 1;

            var activeItems = _roomChargeManager.GetActiveRoomChargeItems(roomChargeID);

            Assert.IsNotNull(activeItems);
            Assert.AreEqual(2, activeItems.Count);
        }

        [TestMethod]
        public void TestSelectPantryItem()
        {
            string pantryID = "Snack";

            var pantryItem = _roomChargeManager.GetPantryItem(pantryID);

            Assert.IsNotNull(pantryItem);
            Assert.AreEqual(20, pantryItem.ItemAmount);
        }

        [TestMethod]
        public void TestSelectPantryItems()
        {
            var pantryItems = _roomChargeManager.GetPantryItems();

            Assert.IsNotNull(pantryItems);
            Assert.AreEqual(4, pantryItems.Count);
        }

        [TestMethod]
        public void TestSelectRoomCharge()
        {
            int reservationID = 101;

            var roomCharge = _roomChargeManager.GetRoomCharge(reservationID);

            Assert.IsNotNull(roomCharge);
            Assert.AreEqual(1, roomCharge.RoomChargeID);
        }

        [TestMethod]
        public void TestSelectRoomChargeItems()
        {
            int roomChargeID = 1;

            var items = _roomChargeManager.GetRoomChargeItems(roomChargeID);

            Assert.IsNotNull(items);
            Assert.AreEqual(2, items.Count);
        }

        [TestMethod]
        public void TestUpdatePantryQuantity()
        {
            string pantryID = "Snack";
            int newItemAmount = 25;
            int oldItemAmount = 20;

            bool updated = _roomChargeManager.UpdateInventory(new List<Pantry> { new Pantry { PantryID = pantryID, ItemAmount = newItemAmount } }, new List<Pantry> { new Pantry { PantryID = pantryID, ItemAmount = oldItemAmount } });

            Assert.IsTrue(updated);
        }

        [TestMethod]
        public void TestUpdatePantryQuantityAndPrice()
        {
            var newPantryItem = new Pantry { PantryID = "Snack", ItemAmount = 25, ItemPrice = 12 };
            var oldPantryItem = new Pantry { PantryID = "Snack", ItemAmount = 20, ItemPrice = 10 };

            bool updated = _roomChargeManager.UpdateInventory(new List<Pantry> { newPantryItem }, new List<Pantry> { oldPantryItem });

            Assert.IsTrue(updated);
        }

        [TestMethod]
        public void TestUpdateRoomChargeItemForRefund()
        {
            int roomChargeItemID = 1;
            int roomChargeID = 1;

            bool refunded = _roomChargeManager.RefundRoomCharge(roomChargeItemID, roomChargeID);

            Assert.IsTrue(refunded);
        }
    }
}