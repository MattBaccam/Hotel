using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LogicLayer;
using DataObjects;
using ASPScenicHotel.Migrations;
using ASPScenicHotel.Models;
using LogicLayerInterfaces;

namespace ASPScenicHotel.Controllers
{
    [Authorize(Roles = "Guest")]
    public class GuestController : Controller
    {
        IGuestManager _guestManager = new GuestManager();
        IRoomManager _roomManager = new RoomManager();
        IReservationManager _reservationManager = new ReservationManager();
        // GET: Guest
        public ActionResult Index()
        {
            var guest = new Guest();
            try
            {
                ApplicationUserManager manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = manager.FindById(User.Identity.GetUserId());
                guest = _guestManager.GetGuestByID(user.AppID);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(guest);
        }

        // GET: Guest/Edit/
        [Authorize(Roles = "Guest, Front Desk Agent")]
        public ActionResult Edit()
        {
            var guest = new Guest();
            try
            {
                ApplicationUserManager manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = manager.FindById(User.Identity.GetUserId());
                guest = _guestManager.GetGuestByID(user.AppID);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(guest);
        }

        // POST: Guest/Edit/5
        [HttpPost]
        public ActionResult Edit(FormCollection collection)
        {
            var currentGuest = new Guest();
            if (ModelState.IsValid)
            {
                ApplicationUserManager manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = manager.FindById(User.Identity.GetUserId());
                currentGuest = _guestManager.GetGuestByID(user.AppID);
                try
                {
                    var newGuest = new Guest()
                    {
                        FirstName = collection["FirstName"],
                        LastName = collection["LastName"],
                        Phone = collection["Phone"],
                        Email = collection["Email"]
                    };
                    var newUser = user;
                    newUser.FirstName = newGuest.FirstName;
                    newUser.LastName = newGuest.LastName;
                    newUser.PhoneNumber = newGuest.Phone;
                    newUser.Email = newGuest.Email;
                    if (manager.Update(newUser).Succeeded)
                    {
                        if (_guestManager.SaveGuestInfo(newGuest, currentGuest))
                        {
                            return RedirectToAction("Index", "Guest");
                        }
                    }
                    else
                    {
                        throw new Exception("Failed to update guest");
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                }
            }
            return View(currentGuest);
        }

        // GET: Guest/Reservations
        public ActionResult Reservations(string filterID = null)
        {
            var reservations = new List<ReservationVM>();
            try
            {
                ApplicationUserManager manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = manager.FindById(User.Identity.GetUserId());
                if (filterID != null)
                {
                    if (filterID == "Due In" || filterID == "Due Out" || filterID == "Out" || filterID == "Cancelled")
                    {
                        reservations = _reservationManager.GetReservationsForGuestByStatus(user.AppID, filterID);
                    }
                    else if (filterID == "Ascending")
                    {
                        reservations = _reservationManager.GetReservationsByAsc(user.AppID);
                    }
                    else if (filterID == "Descending")
                    {
                        reservations = _reservationManager.GetReservationsByDesc(user.AppID);
                    }
                }
                else
                {
                    reservations = _reservationManager.GetReservationsByGuestID(user.AppID);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(reservations);
        }

        // GET: Guest/ConfirmReservation/roomID
        public ActionResult ConfirmReservation(string roomID, string checkIn, string checkOut)
        {
            var _guestReservationVM = new ReservationVM();
            try
            {
                ApplicationUserManager manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = manager.FindById(User.Identity.GetUserId());
                _guestReservationVM.Guest = _guestManager.GetGuestByID(user.AppID);
                _guestReservationVM.Room = _roomManager.GetRoomVM(_roomManager.GetRoom(int.Parse(roomID)).RoomID);
                _guestReservationVM.CheckIn = DateTime.Parse(checkIn);
                _guestReservationVM.CheckOut = DateTime.Parse(checkOut);
                ViewBag.Price = _guestReservationVM.Room.RoomType.RoomPrice * int.Parse(_guestReservationVM.CheckOut.Subtract(_guestReservationVM.CheckIn).TotalDays.ToString());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(_guestReservationVM);
        }


        // POST: Guest/ConfirmReservation/roomID
        [HttpPost]
        public ActionResult ConfirmReservation(FormCollection collection)
        {
            var _guestReservationVM = new ReservationVM();
            try
            {
                ApplicationUserManager manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = manager.FindById(User.Identity.GetUserId());

                _guestReservationVM.Guest = _guestManager.GetGuestByID(user.AppID);
                _guestReservationVM.Room = _roomManager.GetRoomVM(_roomManager.GetRoom(int.Parse(collection["Room.RoomID"])).RoomID);
                _guestReservationVM.CheckIn = DateTime.Parse(collection["CheckIn"]);
                _guestReservationVM.CheckOut = DateTime.Parse(collection["CheckOut"]);
                ViewBag.Price = _roomManager.GetRoomType(_guestReservationVM.Room.RoomTypeID).RoomPrice * int.Parse(_guestReservationVM.CheckOut.Subtract(_guestReservationVM.CheckIn).TotalDays.ToString());
                _guestReservationVM.Comments = collection["Comments"];
                _guestReservationVM.AdultAmount = int.Parse(collection["AdultAmount"]);
                _guestReservationVM.ChildAmount = int.Parse(collection["ChildAmount"]);

                if (ModelState.IsValid)
                {

                    var reservation = new Reservation()
                    {
                        GuestID = _guestReservationVM.Guest.GuestID,
                        RoomID = _guestReservationVM.Room.RoomID,
                        CheckIn = _guestReservationVM.CheckIn,
                        CheckOut = _guestReservationVM.CheckOut,
                        Comments = _guestReservationVM.Comments,
                        AdultAmount = _guestReservationVM.AdultAmount,
                        ChildAmount = _guestReservationVM.ChildAmount,
                        ReservationStatus = "Due In"
                    };
                    if (_reservationManager.CreateReservation(reservation))
                    {
                        return RedirectToAction("Reservations", "Guest");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }
            return View(_guestReservationVM);
        }


        // GET: Guest/Details/reservationID
        public ActionResult ReservationDetails(string reservationID)
        {
            var _guestReservationVM = new ReservationVM();
            try
            {
                ApplicationUserManager manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = manager.FindById(User.Identity.GetUserId());
                _guestReservationVM = _reservationManager.GetReservationVM(int.Parse(reservationID));
                ViewBag.Price = _roomManager.GetRoomType(_guestReservationVM.Room.RoomTypeID).RoomPrice * int.Parse(_guestReservationVM.CheckOut.Subtract(_guestReservationVM.CheckIn).TotalDays.ToString());
                _guestReservationVM = _reservationManager.GetReservationVM(int.Parse(reservationID));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(_guestReservationVM);
        }


        // GET: Guest/Edit/reservationID
        public ActionResult EditReservation(string reservationID)
        {
            var _guestReservationVM = new ReservationVM();
            try
            {
                ApplicationUserManager manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = manager.FindById(User.Identity.GetUserId());
                _guestReservationVM = _reservationManager.GetReservationVM(int.Parse(reservationID));
                ViewBag.Price = _guestReservationVM.Room.RoomType.RoomPrice * int.Parse(_guestReservationVM.CheckOut.Subtract(_guestReservationVM.CheckIn).TotalDays.ToString());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(_guestReservationVM);
        }


        // POST: Guest/Details/reservationID
        [HttpPost]
        public ActionResult EditReservation(FormCollection collection, string reservationID)
        {
            var _guestReservationVM = new ReservationVM();
            try
            {

                if (ModelState.IsValid)
                {
                    ApplicationUserManager manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                    var user = manager.FindById(User.Identity.GetUserId());
                    var comments = collection["Comments"];
                    var newAdultAmount = int.Parse(collection["AdultAmount"]);
                    var newChildAmount = int.Parse(collection["ChildAmount"]);
                    _guestReservationVM = _reservationManager.GetReservationVM(int.Parse(reservationID));
                    ViewBag.Price = _roomManager.GetRoomType(_guestReservationVM.Room.RoomTypeID).RoomPrice * int.Parse(_guestReservationVM.CheckOut.Subtract(_guestReservationVM.CheckIn).TotalDays.ToString());

                    if (!_reservationManager.SaveReservationComments(int.Parse(reservationID), comments))
                    {
                        ViewBag.Error = "Failed to save comments";
                    }
                    if (newAdultAmount != _guestReservationVM.AdultAmount || newChildAmount != _guestReservationVM.ChildAmount)
                    {
                        if (!_reservationManager.UpdateReservationChildAdultAmount(_guestReservationVM.ReservationID, newAdultAmount, newChildAmount, _guestReservationVM.AdultAmount, _guestReservationVM.ChildAmount))
                        {
                            ViewBag.Error = ViewBag.Error != null ? ViewBag.Error += ", child and adult amount" : "Failed to save child and adult amount";
                        }
                    }
                    if (ViewBag.Error == null)
                    {
                        return RedirectToAction("ReservationDetails", "Guest", new { reservationID = _guestReservationVM.ReservationID });
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ViewBag.Error != null ? ViewBag.Error += $"\n {ex.Message}" : ex.Message;
            }
            return View(_guestReservationVM);
        }
    }
}
