using DataAccessInterfaces;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessFakes
{
    public class ReservationAccessorFake : IReservationAccessor
    {
        public static List<Reservation> _reservations = new List<Reservation>();
        public static List<ReservationVM> _reservationVMs = new List<ReservationVM>();
        public ReservationAccessorFake()
        {
            _reservations.Add(new Reservation
            {
                ReservationID = 1,
                GuestID = 1,
                RoomID = 1,
                ReservationStatus = "Confirmed",
                CheckIn = DateTime.Today.AddDays(1),
                CheckOut = DateTime.Today.AddDays(3),
                Comments = "No special requests",
                AdultAmount = 2,
                ChildAmount = 0,
                Paid = true
            });

            _reservations.Add(new Reservation
            {
                ReservationID = 2,
                GuestID = 2,
                RoomID = 2,
                ReservationStatus = "Confirmed",
                CheckIn = DateTime.Today.AddDays(5),
                CheckOut = DateTime.Today.AddDays(7),
                Comments = "Non-smoking room",
                AdultAmount = 1,
                ChildAmount = 1,
                Paid = false
            });

            _reservations.Add(new Reservation
            {
                ReservationID = 3,
                GuestID = 3,
                RoomID = 3,
                ReservationStatus = "Confirmed",
                CheckIn = DateTime.Today.AddDays(10),
                CheckOut = DateTime.Today.AddDays(12),
                Comments = "Late check-in requested",
                AdultAmount = 2,
                ChildAmount = 1,
                Paid = true
            });

            _reservations.Add(new Reservation
            {
                ReservationID = 4,
                GuestID = 4,
                RoomID = 4,
                ReservationStatus = "Confirmed",
                CheckIn = DateTime.Today.AddDays(15),
                CheckOut = DateTime.Today.AddDays(17),
                Comments = "Early check-out requested",
                AdultAmount = 1,
                ChildAmount = 0,
                Paid = false
            });
            _reservationVMs.Add(new ReservationVM
            {
                ReservationID = 1,
                GuestID = 1,
                RoomID = 1,
                ReservationStatus = "Confirmed",
                CheckIn = DateTime.Today.AddDays(1),
                CheckOut = DateTime.Today.AddDays(3),
                Comments = "No special requests",
                AdultAmount = 2,
                ChildAmount = 0,
                Paid = true,
                Guest = new Guest
                {
                    GuestID = 1,
                    FirstName = "John",
                    LastName = "Doe"
                }
            });

            _reservationVMs.Add(new ReservationVM
            {
                ReservationID = 2,
                GuestID = 2,
                RoomID = 2,
                ReservationStatus = "Confirmed",
                CheckIn = DateTime.Today.AddDays(5),
                CheckOut = DateTime.Today.AddDays(7),
                Comments = "Non-smoking room",
                AdultAmount = 1,
                ChildAmount = 1,
                Paid = false,
                Guest = new Guest
                {
                    GuestID = 2,
                    FirstName = "Jane",
                    LastName = "Smith"
                }
            });

            _reservationVMs.Add(new ReservationVM
            {
                ReservationID = 3,
                GuestID = 3,
                RoomID = 3,
                ReservationStatus = "Confirmed",
                CheckIn = DateTime.Today.AddDays(10),
                CheckOut = DateTime.Today.AddDays(12),
                Comments = "Late check-in requested",
                AdultAmount = 2,
                ChildAmount = 1,
                Paid = true,
                Guest = new Guest
                {
                    GuestID = 3,
                    FirstName = "Michael",
                    LastName = "Johnson"
                }
            });

            _reservationVMs.Add(new ReservationVM
            {
                ReservationID = 4,
                GuestID = 4,
                RoomID = 4,
                ReservationStatus = "Confirmed",
                CheckIn = DateTime.Today.AddDays(15),
                CheckOut = DateTime.Today.AddDays(17),
                Comments = "Early check-out requested",
                AdultAmount = 1,
                ChildAmount = 0,
                Paid = false,
                Guest = new Guest
                {
                    GuestID = 4,
                    FirstName = "Emily",
                    LastName = "Davis"
                }
            });

        }

        public bool InsertReservation(Reservation reservation)
        {
            _reservations.Add(reservation);
            return true;
        }

        public List<Events> SelectEventsByStatus(string status)
        {
            var events = new List<Events>();
            _reservationVMs.ForEach(r => 
            {
                if(r.ReservationStatus == status)
                {
                    events.Add(new Events() { id = r.ReservationID.ToString(), url = $"MOCKURL?reservationID={r.ReservationID}", title = $"{r.Guest.FirstName} {r.Guest.LastName}", start = r.CheckIn.ToString(), end = r.CheckOut.ToString(), color = "#0000" });

                }
            });
            return events;
        }

        public List<Reservation> SelectReservationByAllFields(string firstName, string lastName, DateTime checkIn, DateTime checkOut)
        {
            var reservationList = _reservationVMs
                .Where(r => r.Guest.FirstName == firstName && r.Guest.LastName == lastName && r.CheckIn == checkIn && r.CheckOut == checkOut)
                .ToList();
            return reservationList.ConvertAll(reservation => (Reservation)reservation);
        }

        public List<Reservation> SelectReservationByCheckIn(DateTime checkIn)
        {
            return _reservations.Where(r => r.CheckIn == checkIn).ToList();
        }

        public List<Reservation> SelectReservationByCheckInAndCheckOut(DateTime checkIn, DateTime checkOut)
        {
            return _reservations
                .Where(r => r.CheckIn >= checkIn && r.CheckOut <= checkOut)
                .ToList();
        }

        public List<Reservation> SelectReservationByCheckOut(DateTime checkOut)
        {
            return _reservations.Where(r => r.CheckOut == checkOut).ToList();
        }

        public List<Reservation> SelectReservationByFirstName(string firstName)
        {
            var reservationList = _reservationVMs.Where(r => r.Guest.FirstName == firstName).ToList();
            return reservationList.ConvertAll(reservation => (Reservation)reservation);
        }


        public List<Reservation> SelectReservationByFirstNameAndCheckInCheckOut(string firstName, DateTime checkIn, DateTime checkOut)
        {
            return _reservationVMs
                .Where(r => r.Guest.FirstName == firstName && r.CheckIn >= checkIn && r.CheckOut <= checkOut)
                .ToList().ConvertAll(r => (Reservation)r);
        }

        public List<Reservation> SelectReservationByFirstNameCheckIn(string firstName, DateTime checkIn)
        {
            return _reservationVMs
                .Where(r => r.Guest.FirstName == firstName && r.CheckIn == checkIn)
                .ToList().ConvertAll(r => (Reservation)r);
        }

        public List<Reservation> SelectReservationByFirstNameCheckOut(string firstName, DateTime checkOut)
        {
            return _reservationVMs
                .Where(r => r.Guest.FirstName == firstName && r.CheckOut == checkOut)
                .ToList().ConvertAll(r => (Reservation)r);
        }

        public Reservation SelectReservationByFirstNameLastName(string firstName, string lastName)
        {
            return _reservationVMs
                .FirstOrDefault(r => r.Guest.FirstName == firstName && r.Guest.LastName == lastName);
        }

        public Reservation SelectReservationByFirstNameLastNameRoomID(string firstName, string lastName, int roomID)
        {
            return _reservationVMs
                .FirstOrDefault(r => r.Guest.FirstName == firstName && r.Guest.LastName == lastName && r.RoomID == roomID);
        }

        public Reservation SelectReservationByID(int reservationID)
        {
            return _reservations.FirstOrDefault(r => r.ReservationID == reservationID);
        }

        public List<Reservation> SelectReservationByLastName(string lastName)
        {
            return _reservationVMs.Where(r => r.Guest.LastName == lastName).ToList().ConvertAll(r => (Reservation)r);
        }

        public List<Reservation> SelectReservationByLastNameAndCheckInCheckOut(string lastName, DateTime checkIn, DateTime checkOut)
        {
            return _reservationVMs
                .Where(r => r.Guest.LastName == lastName && r.CheckIn >= checkIn && r.CheckOut <= checkOut)
                .ToList().ConvertAll(r => (Reservation)r);
        }

        public List<Reservation> SelectReservationByLastNameCheckIn(string lastName, DateTime checkIn)
        {
            return _reservationVMs
                .Where(r => r.Guest.LastName == lastName && r.CheckIn == checkIn)
                .ToList().ConvertAll(r => (Reservation)r);
        }

        public List<Reservation> SelectReservationByLastNameCheckOut(string lastName, DateTime checkOut)
        {
            return _reservationVMs
                .Where(r => r.Guest.LastName == lastName && r.CheckOut == checkOut)
                .ToList().ConvertAll(r => (Reservation)r);
        }


        public Reservation SelectReservationByRoomID(int roomID)
        {
            return _reservations.FirstOrDefault(r => r.RoomID == roomID);
        }

        public List<Reservation> SelectReservationsByAsc(int guestID)
        {
            return _reservations
                .Where(r => r.GuestID == guestID)
                .OrderBy(r => r.CheckIn)
                .ToList();
        }

        public List<Reservation> SelectReservationsByDesc(int guestID)
        {
            return _reservations
                .Where(r => r.GuestID == guestID)
                .OrderByDescending(r => r.CheckIn)
                .ToList();
        }

        public List<Reservation> SelectReservationsByGuestID(int guestID)
        {
            return _reservations.Where(r => r.GuestID == guestID).ToList();
        }

        public List<Reservation> SelectReservationsByStatus(string status)
        {
            return _reservations.Where(r => r.ReservationStatus == status).ToList();
        }

        public List<Reservation> SelectReservationsForGuestByStatus(int guestID, string status)
        {
            return _reservations.Where(r => r.GuestID == guestID && r.ReservationStatus == status).ToList();
        }

        public bool UpdateReservation(Reservation newReservation, Reservation oldReservation)
        {
            var existingReservation = _reservations.FirstOrDefault(r => r.ReservationID == oldReservation.ReservationID);
            if (existingReservation != null)
            {
                existingReservation.CheckIn = newReservation.CheckIn;
                existingReservation.CheckOut = newReservation.CheckOut;
                existingReservation.ReservationStatus = newReservation.ReservationStatus;
                existingReservation.Comments = newReservation.Comments;
                existingReservation.AdultAmount = newReservation.AdultAmount;
                existingReservation.ChildAmount = newReservation.ChildAmount;
                existingReservation.Paid = newReservation.Paid;
                return true;
            }
            return false;
        }
        public bool UpdateReservationChildAdultAmount(int reservationID, int newAdultAmount, int newChildAmount, int oldAdultAmount, int oldChildAmount)
        {
            var reservation = _reservations.FirstOrDefault(r => r.ReservationID == reservationID);
            if (reservation != null)
            {
                if (reservation.AdultAmount == oldAdultAmount && reservation.ChildAmount == oldChildAmount)
                {
                    reservation.AdultAmount = newAdultAmount;
                    reservation.ChildAmount = newChildAmount;
                    return true;
                }
            }
            return false;
        }

        public bool UpdateReservationComments(int reservationID, string newComments)
        {
            var reservation = _reservations.FirstOrDefault(r => r.ReservationID == reservationID);
            if (reservation != null)
            {
                reservation.Comments = newComments;
                return true;
            }
            return false;
        }

        public bool UpdateReservationForCancel(int reservationID, string oldReservationStatus)
        {
            var reservation = _reservations.FirstOrDefault(r => r.ReservationID == reservationID && r.ReservationStatus == oldReservationStatus);
            if (reservation != null)
            {
                reservation.ReservationStatus = "Cancelled";
                return true;
            }
            return false;
        }

        public bool UpdateReservationForCheckIn(int reservationID, DateTime newCheckIn, DateTime oldCheckIn)
        {
            var reservation = _reservations.FirstOrDefault(r => r.ReservationID == reservationID && r.CheckIn == oldCheckIn);
            if (reservation != null)
            {
                reservation.CheckIn = newCheckIn;
                return true;
            }
            return false;
        }

        public bool UpdateReservationForCheckOut(int reservationID, DateTime newCheckOut, DateTime oldCheckOut)
        {
            var reservation = _reservations.FirstOrDefault(r => r.ReservationID == reservationID && r.CheckOut == oldCheckOut);
            if (reservation != null)
            {
                reservation.CheckOut = newCheckOut;
                return true;
            }
            return false;
        }

        public bool UpdateReservationForReschedule(int reservationID, DateTime newCheckIn, DateTime newCheckOut, DateTime oldCheckIn, DateTime oldCheckOut)
        {
            var reservation = _reservations.FirstOrDefault(r => r.ReservationID == reservationID && r.CheckIn == oldCheckIn && r.CheckOut == oldCheckOut);
            if (reservation != null)
            {
                reservation.CheckIn = newCheckIn;
                reservation.CheckOut = newCheckOut;
                return true;
            }
            return false;
        }

        public bool UpdateReservationForRescheduleNewRoom(int reservationID, int newRoomID, DateTime newCheckIn, DateTime newCheckOut, int oldRoomID, DateTime oldCheckIn, DateTime oldCheckOut)
        {
            var reservation = _reservations.FirstOrDefault(r => r.ReservationID == reservationID && r.RoomID == oldRoomID && r.CheckIn == oldCheckIn && r.CheckOut == oldCheckOut);
            if (reservation != null)
            {
                reservation.RoomID = newRoomID;
                reservation.CheckIn = newCheckIn;
                reservation.CheckOut = newCheckOut;
                return true;
            }
            return false;
        }
    }
}
