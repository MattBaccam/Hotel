using DataObjects;
using LogicLayer;
using LogicLayerInterfaces;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASPScenicHotel.Models;
using Microsoft.Ajax.Utilities;
using System.Threading.Tasks;

namespace ASPScenicHotel.Controllers
{
    [Authorize(Roles = "Housekeeper, Front Desk Agent, Admin")]
    public class EmployeeLandingController : Controller
    {
        IEmployeeManager _employeeManager = new EmployeeManager();
        IReservationManager _reservationManager = new ReservationManager();
        IRoomManager _roomManager = new RoomManager();
        IGuestManager _guestManager = new GuestManager();

        // GET: EmployeeLanding
        public ActionResult Index()
        {
            return View();
        }

        #region Reservations
        [Authorize(Roles = "Front Desk Agent, Admin")]
        // GET: Reservations
        public ActionResult Reservations(string filterID)
        {
            var events = new List<Events>();
            try
            {
                if(string.IsNullOrEmpty(filterID))
                {
                    filterID = "All";
                }
                events = _reservationManager.GetEventsByStatus(filterID);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(events);
        }

        [Authorize(Roles = "Front Desk Agent, Admin")]
        // GET: ReservationDetails?reservationID
        public ActionResult ReservationDetails(string reservationID)
        {
            if (reservationID == null)
            {
                return RedirectToAction("Reservations", "EmployeeLanding");
            }
            try
            {
                int.Parse(reservationID);
            }
            catch (Exception)
            {
                return RedirectToAction("Reservations", "EmployeeLanding");
            }
            var reservation = new ReservationVM();
            try
            {
                reservation = _reservationManager.GetReservationVM(int.Parse(reservationID));
                ViewBag.Price = _roomManager.GetRoomType(reservation.Room.RoomTypeID).RoomPrice * int.Parse(reservation.CheckOut.Subtract(reservation.CheckIn).TotalDays.ToString());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(reservation);
        }

        [Authorize(Roles = "Front Desk Agent, Admin")]
        // GET: ReservationDetails?reservationID
        public ActionResult ReservationForm(string roomID, string checkIn, string checkOut)
        {
            if (roomID == null || checkIn == null || checkOut == null)
            {
                return RedirectToAction("Reservations", "EmployeeLanding");
            }
            var reservation = new ReservationVM();
            var room = new RoomVM();
            reservation.Room = room;
            try
            {
                reservation.Room = _roomManager.GetRoomVM(int.Parse(roomID));
                reservation.CheckIn = DateTime.Parse(checkIn);
                reservation.CheckOut = DateTime.Parse(checkOut);
                ViewBag.Price = _roomManager.GetRoomType(reservation.Room.RoomTypeID).RoomPrice * int.Parse(reservation.CheckOut.Subtract(reservation.CheckIn).TotalDays.ToString());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(reservation);
        }

        [Authorize(Roles = "Front Desk Agent, Admin")]
        [HttpPost]
        // GET: ReservationDetails?reservationID
        public async Task<ActionResult> ReservationForm(FormCollection collection)
        {
            var reservation = new ReservationVM();
            var guest = new Guest();
            try
            {
                reservation.Guest = guest;
                reservation.Guest.FirstName = collection["Guest.FirstName"];
                reservation.Guest.LastName = collection["Guest.LastName"];
                reservation.Guest.Email = collection["Guest.Email"];
                reservation.Guest.Phone = collection["Guest.Phone"];

                reservation.CheckIn = DateTime.Parse(collection["CheckIn"]);
                reservation.CheckOut = DateTime.Parse(collection["CheckOut"]);

                var room = _roomManager.GetRoomVM(int.Parse(collection["Room.RoomID"]));
                reservation.Room = room;
                ViewBag.Price = _roomManager.GetRoomType(reservation.Room.RoomTypeID).RoomPrice * int.Parse(reservation.CheckOut.Subtract(reservation.CheckIn).TotalDays.ToString());

                if (ModelState.IsValid)
                {
                    var guestByEmail = _guestManager.GetGuestByEmail(collection["Guest.Email"]);
                    var guestByPhone = _guestManager.GetGuestByPhone(collection["Guest.Phone"]);
                    if (guestByEmail != null)
                    {
                        ViewBag.EmailExists = "Email already exists";
                    }
                    if (guestByPhone != null)
                    {
                        ViewBag.PhoneExists = "Phone already exists";
                    }
                    if (guestByEmail == null && guestByPhone == null)
                    {
                        var newGuest = new Guest()
                        {
                            FirstName = collection["Guest.FirstName"],
                            LastName = collection["Guest.LastName"],
                            Phone = collection["Guest.Phone"],
                            Email = collection["Guest.Email"]
                        };
                        if (_guestManager.CreateGuest(newGuest))
                        {
                            newGuest = _guestManager.GetGuestByEmail(newGuest.Email);
                            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                            var user = new ApplicationUser()
                            {
                                PositionTitle = "Guest",
                                FirstName = newGuest.FirstName,
                                LastName = newGuest.LastName,
                                UserName = newGuest.Email,
                                Email = newGuest.Email,
                                AppID = newGuest.GuestID
                            };
                            var result = await userManager.CreateAsync(user);
                            if (result.Succeeded)
                            {
                                userManager.AddToRole(user.Id, "Guest");
                                guest = _guestManager.GetGuestByEmail(collection["Guest.Email"]);
                                if (CreateReservation())
                                {
                                    return RedirectToAction("Reservations", "EmployeeLanding", new {filterID = "Due In"});
                                }
                                else
                                {
                                    ViewBag.Error = "Failed to create reservation";
                                }
                            }
                            else
                            {
                                ViewBag.Error = "Failed to create guest Identity";
                            }
                        }
                        else
                        {
                            ViewBag.Error = "Failed to create guest";
                        }
                    }
                    else
                    {
                        if (guestByEmail != null && guestByPhone != null)
                        {
                            if (guestByEmail.GuestID == guestByPhone.GuestID)
                            {
                                guest = guestByEmail;
                                if (CreateReservation())
                                {
                                    return RedirectToAction("Reservations", "EmployeeLanding", new { reservationID = reservation.ReservationID });
                                }
                                else
                                {
                                    ViewBag.Error = "Failed to create reservation";
                                }
                            }
                        }
                    }

                    bool CreateReservation()//Created this because the flow of the logic does not need it to execute sometimes
                    {
                        return _reservationManager.CreateReservation(new Reservation()
                        {
                            GuestID = guest.GuestID,
                            RoomID = room.RoomID,
                            ReservationStatus = "Due In",
                            CheckIn = DateTime.Parse(collection["CheckIn"]),
                            CheckOut = DateTime.Parse(collection["CheckOut"]),
                            Comments = collection["Comments"],
                            AdultAmount = int.Parse(collection["AdultAmount"]),
                            ChildAmount = int.Parse(collection["ChildAmount"]),
                            Paid = false
                        });
                    }

                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(reservation);
        }

        [Authorize(Roles = "Front Desk Agent, Admin")]
        //Get: CheckIn
        public ActionResult CheckIn(string reservationID, string checkIn)
        {
            if (reservationID == null || checkIn == null)
            {
                return RedirectToAction("Reservations", "EmployeeLanding");
            }
            try
            {
                var reservation = _reservationManager.GetReservationByID(int.Parse(reservationID));
                var checkInDT = DateTime.Parse(checkIn);
                if (checkInDT.Date < DateTime.Now.Date)//Early 
                {
                    var roomsAvailable = _roomManager.GetRoomsAvailable(checkInDT, reservation.CheckIn.AddDays(-1));//Gets the early check in day to the day before they check in to see if anyone else is in there
                    if(roomsAvailable != null)
                    {
                        var result = false;
                        roomsAvailable.ForEach(room =>
                        {
                            if(room.RoomID == reservation.RoomID)
                            {
                                result = true;
                            }
                        });
                        if (result)
                        {
                            if(_reservationManager.SaveReservationForCheckIn(int.Parse(reservationID), checkInDT, reservation.CheckIn))
                            {
                                return RedirectToAction("ReservationDetails", new {reservationID = reservationID});
                            }
                        }
                    }
                }
                else
                {
                    if(_reservationManager.SaveReservationForCheckIn(int.Parse(reservationID), checkInDT, reservation.CheckIn))
                    {
                        return RedirectToAction("ReservationDetails", new { reservationID = reservationID });
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("DefaultErrorPage", "Error");
        }

        [Authorize(Roles = "Front Desk Agent, Admin")]
        //Get: CheckIn
        public ActionResult Receipt(string reservationID)
        {
            if (reservationID == null)
            {
                return RedirectToAction("Reservations", "EmployeeLanding");
            }
            var reservation = new ReservationVM();
            try
            {
                reservation = _reservationManager.GetReservationVM(int.Parse(reservationID));
                ViewBag.Price = _roomManager.GetRoomType(reservation.Room.RoomTypeID).RoomPrice * int.Parse(reservation.CheckOut.Subtract(reservation.CheckIn).TotalDays.ToString());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(reservation);
        }

        [Authorize(Roles = "Front Desk Agent, Admin")]
        //Get: CheckOut
        public ActionResult CheckOut(string reservationID, string checkOut)
        {
            if (reservationID == null || checkOut == null)
            {
                return RedirectToAction("Reservations", "EmployeeLanding");
            }
            try
            {
                var reservation = _reservationManager.GetReservationByID(int.Parse(reservationID));
                var checkOutDT = DateTime.Parse(checkOut);
                if (_reservationManager.SaveReservationForCheckOut(int.Parse(reservationID), checkOutDT, reservation.CheckOut))
                {
                    return RedirectToAction("ReservationDetails", new { reservationID = reservationID });
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("DefaultErrorPage", "Error");
        }

        [Authorize(Roles = "Front Desk Agent, Admin")]
        //GET: UpdateGuest
        public ActionResult UpdateGuest(string guestID, string reservationID)
        {
            if (reservationID == null || guestID == null)
            {
                return RedirectToAction("Reservations", "EmployeeLanding");
            }
            var guest = new Guest();
            try
            {
                guest = _guestManager.GetGuestByID(int.Parse(guestID));
                if(reservationID != null)
                {
                    ViewBag.ReservationID = reservationID;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View(guest);
        }

        [Authorize(Roles = "Front Desk Agent, Admin")]
        [HttpPost]
        public ActionResult UpdateGuest(FormCollection collection, string reservationID, string guestID)
        {
            var currentGuest = new Guest();
            ViewBag.ReservationID = reservationID;
            if (ModelState.IsValid)
            {
                ApplicationUserManager manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                currentGuest = _guestManager.GetGuestByID(int.Parse(guestID));
                try
                {
                    var newGuest = new Guest()
                    {
                        FirstName = collection["FirstName"],
                        LastName = collection["LastName"],
                        Phone = collection["Phone"],
                        Email = collection["Email"]
                    };
                    var userId = manager.Users
                    .Where(m => m.Email == currentGuest.Email)
                    .Select(m => m.Id)
                    .SingleOrDefault();//Couldnt figure out a better way to do this so this is going to have to do for now (Shouldnt be an issue because the scale is still small)
                    var newUser = manager.FindById(userId);
                    if(newUser != null)
                    {
                        newUser.FirstName = newGuest.FirstName;
                        newUser.LastName = newGuest.LastName;
                        newUser.PhoneNumber = newGuest.Phone;
                        newUser.Email = newGuest.Email;
                        if (manager.Update(newUser).Succeeded)
                        {
                            if (_guestManager.SaveGuestInfo(newGuest, currentGuest))
                            {
                                return RedirectToAction("ReservationDetails", "EmployeeLanding", new { reservationID = reservationID });
                            }
                        }
                        else
                        {
                            throw new Exception("Failed to update guest");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                }
            }
            return View(currentGuest);
        }
        #endregion
        
        #region Housekeeping
        // GET: EmployeeLanding/Housekeeping
        public ActionResult RoomManagement()
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
            return RedirectToAction("Index", "Room", rooms);
        }

        // GET: EmployeeLanding/EditRoom
        public ActionResult EditRoom(string roomID)
        {
            if (roomID == null)
            {
                return RedirectToAction("Index", "Room");
            }
            var room = new RoomVM();
            try
            {
                room = _roomManager.GetRoomVM(int.Parse(roomID));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(room);
        }

        // POST: EmployeeLanding/EditRoom
        [HttpPost]
        public ActionResult EditRoom(FormCollection collection)
        {
            var room = new RoomVM();
            try
            {
                var ogRoom = _roomManager.GetRoomVM(int.Parse(collection["RoomID"]));
                if(_roomManager.UpdateRoom(new Room() {RoomTypeID = ogRoom.RoomTypeID, RoomStatus = collection["RoomStatus"], RoomAvailability = collection["RoomAvailability"] == "true" ? true : false }, ogRoom))
                {
                    return RedirectToAction("Index", "Room");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(room);
        }
        #endregion
    }
}
