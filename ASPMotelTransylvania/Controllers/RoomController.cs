using DataObjects;
using LogicLayer;
using LogicLayerInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASPScenicHotel.Controllers
{
    public class RoomController : Controller
    {
        IRoomManager _roomManager = new RoomManager();

        // GET: Room
        public ActionResult Index()
        {
            var rooms = new List<RoomVM>();
            try
            {
                rooms = _roomManager.GetAllRooms();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(rooms);
        }

        // GET: Room/Details/roomID
        public ActionResult Details(string roomID)
        {
            var room = new Room();
            if(roomID != null)
            {
                try
                {
                    room = _roomManager.GetRoom(int.Parse(roomID));                    
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                }
            }
            return View(room);
        }


        // GET: GuestReservations/RoomsAvailable
        public ActionResult RoomsAvailable()
        {
            return View();
        }

        // POST: GuestReservations/RoomsAvailable
        [HttpPost]
        public ActionResult RoomsAvailable(FormCollection collection)
        {
            var roomList = new List<RoomVM>();
            try
            {
                if (ModelState.IsValid)
                {
                    DateTime checkIn = DateTime.Parse(collection["checkIn"]);
                    DateTime checkOut = DateTime.Parse(collection["checkOut"]);

                    roomList = _roomManager.GetRoomsAvailable(checkIn, checkOut);

                    ViewBag.CheckIn = checkIn.ToString("MM/dd/yyyy");
                    ViewBag.CheckOut = checkOut.ToString("MM/dd/yyyy");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(roomList);
        }

        //// GET: Room/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Room/Create
        //[HttpPost]
        //public ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Room/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: Room/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Room/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: Room/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
