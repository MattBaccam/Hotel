using DataObjects;
using LogicLayer;
using LogicLayerInterfaces;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ASPScenicHotel.Controllers
{
    [Authorize]
    public class ReservationController : Controller
    {
        IReservationManager _reservationManager = new ReservationManager();
        IRoomManager _roomManager = new RoomManager();

        // GET: GuestReservations/Reschedule/reservationID
        public ActionResult Reschedule(string reservationID)
        {
            var rooms = new List<RoomVM>();
            try
            {
                ApplicationUserManager manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = manager.FindById(User.Identity.GetUserId());
                var reservation = _reservationManager.GetReservationByID(int.Parse(reservationID));
                ViewBag.Reservation = reservation;
                ViewBag.CheckIn = reservation.CheckIn.ToString("MM/dd/yyyy");
                ViewBag.CheckOut = reservation.CheckOut.ToString("MM/dd/yyyy");
                rooms = _roomManager.GetRoomsAvailableExceptReservation(reservation.ReservationID, reservation.CheckIn, reservation.CheckOut);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(rooms);
        }

        // POST: GuestReservations/RoomsAvailable
        [HttpPost]
        public ActionResult Reschedule(FormCollection collection, string reservationID)
        {
            var roomList = new List<RoomVM>();
            try
            {
                if (ModelState.IsValid)
                {
                    DateTime checkIn = DateTime.Parse(collection["checkIn"]);
                    DateTime checkOut = DateTime.Parse(collection["checkOut"]);
                    roomList = _roomManager.GetRoomsAvailable(checkIn, checkOut);
                    ViewBag.Reservation = _reservationManager.GetReservationByID(int.Parse(reservationID));
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

        // GET: Guest/ConfirmReschedule/reservationID&roomID&checkIn&checkOut
        public ActionResult ConfirmReschedule(string reservationID, string roomID, string checkIn, string checkOut)
        {
            var guestReservationVM = new ReservationVM();
            try
            {
                ApplicationUserManager manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = manager.FindById(User.Identity.GetUserId());
                guestReservationVM = _reservationManager.GetReservationVM(int.Parse(reservationID));
                if (int.Parse(roomID) == guestReservationVM.RoomID)
                {
                    if (_reservationManager.SaveReservationForReschedule(int.Parse(reservationID), DateTime.Parse(checkIn), DateTime.Parse(checkOut), guestReservationVM.CheckIn, guestReservationVM.CheckOut))
                    {
                        if(user.PositionTitle == "Admin" || user.PositionTitle == "Front Desk Agent")
                        {
                            return RedirectToAction("ReservationDetails", "EmployeeLanding", new { reservationID = int.Parse(reservationID) });
                        }
                        else if(user.PositionTitle == "Guest")
                        {
                            return RedirectToAction("ReservationDetails", "Guest", new { reservationID = int.Parse(reservationID) });
                        }
                    }
                }
                else
                {
                    if (_reservationManager.SaveReservationForRescheduleNewRoom(int.Parse(reservationID), int.Parse(roomID), DateTime.Parse(checkIn), DateTime.Parse(checkOut), guestReservationVM.RoomID, guestReservationVM.CheckIn, guestReservationVM.CheckOut))
                    {
                        if (user.PositionTitle == "Admin" || user.PositionTitle == "Front Desk Agent")
                        {
                            return RedirectToAction("ReservationDetails", "EmployeeLanding", new { reservationID = int.Parse(reservationID) });
                        }
                        else if (user.PositionTitle == "Guest")
                        {
                            return RedirectToAction("ReservationDetails", "Guest", new { reservationID = int.Parse(reservationID) });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("DefaultErrorPage", "Error");
        }


        // GET: Guest/ConfirmReschedule/reservationID&roomID&checkIn&checkOut
        public ActionResult Cancel(string reservationID)
        {
            var reservation = new ReservationVM();
            try
            {
                ApplicationUserManager manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = manager.FindById(User.Identity.GetUserId());
                reservation = _reservationManager.GetReservationVM(int.Parse(reservationID));
                if (_reservationManager.SaveReservationForCancel(reservation.ReservationID, reservation.ReservationStatus))
                {
                    if (user.PositionTitle == "Admin" || user.PositionTitle == "Front Desk Agent")
                    {
                        return RedirectToAction("ReservationDetails", "EmployeeLanding", new { reservationID = int.Parse(reservationID) });
                    }
                    else if (user.PositionTitle == "Guest")
                    {
                        return RedirectToAction("ReservationDetails", "Guest", new { reservationID = int.Parse(reservationID) });
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("DefaultErrorPage", "Error");
        }
    }
}
