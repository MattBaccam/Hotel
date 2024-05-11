using DataAccessInterfaces;
using DataAccessLayer;
using DataObjects;
using LogicLayerInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class ReservationManager : IReservationManager
    {
        private ReservationAccessor _reservationAccessor = new ReservationAccessor();

        public Reservation GetReservationByID(int reservationID)
        {
            try
            {
                return _reservationAccessor.SelectReservationByID(reservationID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Events> GetEventsByStatus(string status)
        {
            try
            {
                return _reservationAccessor.SelectEventsByStatus(status);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ReservationVM> GetReservationsByStatus(string status)
        {
            try
            {
                var reservationList = _reservationAccessor.SelectReservationsByStatus(status);
                var reservationVMList = new List<ReservationVM>();
                foreach (var reservation in reservationList)
                {
                    var reservationVM = GetReservationVM(reservation.ReservationID);
                    reservationVMList.Add(reservationVM); 
                }
                return reservationVMList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ReservationVM GetReservationVM(int reservationID)
        {
            try
            {
                var guestManager = new GuestManager();
                var roomManager = new RoomManager();
                var _roomChargeManager = new RoomChargeManager();
                var reservation = GetReservationByID(reservationID);
                var guest = guestManager.GetGuestByID(reservation.GuestID);
                var room = roomManager.GetRoomVM(reservation.RoomID);

                var reservationVM = new ReservationVM();
                reservationVM.ReservationID = reservation.ReservationID;
                reservationVM.GuestID = reservation.GuestID;
                reservationVM.RoomID = reservation.RoomID;
                reservationVM.ReservationStatus = reservation.ReservationStatus;
                reservationVM.CheckIn = reservation.CheckIn;
                reservationVM.CheckOut = reservation.CheckOut;
                reservationVM.Comments = reservation.Comments;
                reservationVM.AdultAmount = reservation.AdultAmount;
                reservationVM.ChildAmount = reservation.ChildAmount;
                reservationVM.Paid = reservation.Paid;
                reservationVM.Name = $"{guest.FirstName} {guest.LastName}";
                reservationVM.Room = room;
                reservationVM.Guest = guest;
                try
                {
                    reservationVM.RoomCharges = _roomChargeManager.GetRoomChargeVM(reservationVM.ReservationID, reservation.RoomID);
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                return reservationVM;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool SaveReservationComments(int reservationID, string newComments)
        {
            try
            {
                return _reservationAccessor.UpdateReservationComments(reservationID, newComments);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SaveReservationForCheckIn(int reservationID, DateTime newCheckIn, DateTime oldCheckIn)
        {
            try
            {
                return _reservationAccessor.UpdateReservationForCheckIn(reservationID, newCheckIn, oldCheckIn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SaveReservationForCheckOut(int reservationID, DateTime newCheckOut, DateTime oldCheckout)
        {
            try
            {
                var roomManager = new RoomManager();
                var reservation = GetReservationByID(reservationID);
                if (_reservationAccessor.UpdateReservationForCheckOut(reservationID, newCheckOut, oldCheckout))
                {
                    var room = roomManager.GetRoom(reservation.RoomID);
                    roomManager.SaveRoomStatus(room.RoomID, room.RoomStatus, "Dirty");
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SaveReservation(Reservation newReservation, Reservation oldReservation)
        {
            try
            {
                return _reservationAccessor.UpdateReservation(newReservation, oldReservation);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SaveReservationForReschedule(int reservationID, DateTime newCheckIn, DateTime newCheckOut, DateTime oldCheckIn, DateTime oldCheckOut)
        {
            try
            {
                return _reservationAccessor.UpdateReservationForReschedule(reservationID, newCheckIn, newCheckOut, oldCheckIn, oldCheckOut);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool SaveReservationForCancel(int reservationID, string oldReservationStatus)
        {
            try
            {
                return _reservationAccessor.UpdateReservationForCancel(reservationID, oldReservationStatus);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ReservationVM> GetReservationsViaSearch(string firstName = null, string lastName = null, DateTime? checkIn = null, DateTime? checkOut = null)
        {
            var reservationVMList = new List<ReservationVM>();
            if(firstName != string.Empty && lastName != string.Empty &&  checkIn != null && checkOut != null)
            {
                var list = _reservationAccessor.SelectReservationByAllFields(firstName, lastName, DateTime.Parse(checkIn.ToString()), DateTime.Parse(checkOut.ToString()));
                foreach(var reservation in list)
                {
                    reservationVMList.Add(GetReservationVM(reservation.ReservationID));
                }
            }
            else if (firstName != string.Empty && lastName == string.Empty && checkIn != null && checkOut != null)
            {
                var list = _reservationAccessor.SelectReservationByFirstNameAndCheckInCheckOut(firstName, DateTime.Parse(checkIn.ToString()), DateTime.Parse(checkOut.ToString()));
                foreach (var reservation in list)
                {
                    reservationVMList.Add(GetReservationVM(reservation.ReservationID));
                }
            }
            else if (firstName == string.Empty && lastName != string.Empty && checkIn != null && checkOut != null)
            {
                var list = _reservationAccessor.SelectReservationByLastNameAndCheckInCheckOut(lastName, DateTime.Parse(checkIn.ToString()), DateTime.Parse(checkOut.ToString()));
                foreach (var reservation in list)
                {
                    reservationVMList.Add(GetReservationVM(reservation.ReservationID));
                }
            }
            else if (firstName != string.Empty && lastName == string.Empty && checkIn == null && checkOut == null)
            {
                var list = _reservationAccessor.SelectReservationByFirstName(firstName);
                foreach (var reservation in list)
                {
                    reservationVMList.Add(GetReservationVM(reservation.ReservationID));
                }
            }
            else if (firstName == string.Empty && lastName != string.Empty && checkIn == null && checkOut == null)
            {
                var list = _reservationAccessor.SelectReservationByLastName(lastName);
                foreach (var reservation in list)
                {
                    reservationVMList.Add(GetReservationVM(reservation.ReservationID));
                }
            }
            else if (firstName == string.Empty && lastName == string.Empty && checkIn != null && checkOut != null)
            {
                var list = _reservationAccessor.SelectReservationByCheckInAndCheckOut(DateTime.Parse(checkIn.ToString()), DateTime.Parse(checkOut.ToString()));
                foreach (var reservation in list)
                {
                    reservationVMList.Add(GetReservationVM(reservation.ReservationID));
                }
            }
            else if (firstName == string.Empty && lastName == string.Empty && checkIn != null && checkOut == null)
            {
                var list = _reservationAccessor.SelectReservationByCheckIn(DateTime.Parse(checkIn.ToString()));
                foreach (var reservation in list)
                {
                    reservationVMList.Add(GetReservationVM(reservation.ReservationID));
                }
            }
            else if (firstName == string.Empty && lastName == string.Empty && checkIn == null && checkOut != null)
            {
                var list = _reservationAccessor.SelectReservationByCheckOut(DateTime.Parse(checkOut.ToString()));
                foreach (var reservation in list)
                {
                    reservationVMList.Add(GetReservationVM(reservation.ReservationID));
                }
            }
            else if (firstName != string.Empty && lastName == string.Empty && checkIn != null && checkOut == null)
            {
                var list = _reservationAccessor.SelectReservationByFirstNameCheckIn(firstName, DateTime.Parse(checkIn.ToString()));
                foreach (var reservation in list)
                {
                    reservationVMList.Add(GetReservationVM(reservation.ReservationID));
                }
            }
            else if (firstName != string.Empty && lastName == string.Empty && checkIn == null && checkOut != null)
            {
                var list = _reservationAccessor.SelectReservationByFirstNameCheckOut(firstName, DateTime.Parse(checkOut.ToString()));
                foreach (var reservation in list)
                {
                    reservationVMList.Add(GetReservationVM(reservation.ReservationID));
                }
            }
            else if (firstName == string.Empty && lastName != string.Empty && checkIn == null && checkOut == null)
            {
                var list = _reservationAccessor.SelectReservationByLastNameCheckIn(lastName, DateTime.Parse(checkIn.ToString()));
                foreach (var reservation in list)
                {
                    reservationVMList.Add(GetReservationVM(reservation.ReservationID));
                }
            }
            else if (firstName == string.Empty  && lastName != string.Empty && checkIn == null && checkOut == null)
            {
                var list = _reservationAccessor.SelectReservationByLastNameCheckIn(lastName, DateTime.Parse(checkOut.ToString()));
                foreach (var reservation in list)
                {
                    reservationVMList.Add(GetReservationVM(reservation.ReservationID));
                }
            }
            return reservationVMList;
        }

        public bool CreateReservation(Reservation reservation)
        {
            try
            {
                return _reservationAccessor.InsertReservation(reservation);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ReservationVM GetReservationsByNameAndRoomID(string firstName, string lastName, int roomID)
        {
            try
            {
                return GetReservationVM(_reservationAccessor.SelectReservationByFirstNameLastNameRoomID(firstName, lastName, roomID).ReservationID);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ReservationVM GetReservationByRoomID(int roomID)
        {
            try
            {
                return GetReservationVM(_reservationAccessor.SelectReservationByRoomID(roomID).ReservationID);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<ReservationVM> GetReservationsByGuestID(int guestID)
        {
            try
            {
                var reservationVMList = new List<ReservationVM>();
                var reservationList = _reservationAccessor.SelectReservationsByGuestID(guestID);
                foreach (var reservation in reservationList)
                {
                    var reservationVM = GetReservationVM(reservation.ReservationID);
                    reservationVMList.Add(reservationVM);
                }
                return reservationVMList;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<ReservationVM> GetReservationsByAsc(int guestID)
        {
            try
            {
                var reservationVMList = new List<ReservationVM>();
                var reservationList = _reservationAccessor.SelectReservationsByAsc(guestID);
                foreach (var reservation in reservationList)
                {
                    var reservationVM = GetReservationVM(reservation.ReservationID);
                    reservationVMList.Add(reservationVM);
                }
                return reservationVMList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ReservationVM> GetReservationsByDesc(int guestID)
        {
            try
            {
                var reservationVMList = new List<ReservationVM>();
                var reservationList = _reservationAccessor.SelectReservationsByDesc(guestID);
                foreach (var reservation in reservationList)
                {
                    var reservationVM = GetReservationVM(reservation.ReservationID);
                    reservationVMList.Add(reservationVM);
                }
                return reservationVMList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ReservationVM> GetReservationsForGuestByStatus(int guestID, string status)
        {
            try
            {
                var reservationVMList = new List<ReservationVM>();
                var reservationList = _reservationAccessor.SelectReservationsForGuestByStatus(guestID, status);
                foreach (var reservation in reservationList)
                {
                    var reservationVM = GetReservationVM(reservation.ReservationID);
                    reservationVMList.Add(reservationVM);
                }
                return reservationVMList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SaveReservationForRescheduleNewRoom(int reservationID, int newRoomID, DateTime newCheckIn, DateTime newCheckOut, int oldRoomID, DateTime oldCheckIn, DateTime oldCheckOut)
        {
            try
            {
                return _reservationAccessor.UpdateReservationForRescheduleNewRoom(reservationID, newRoomID, newCheckIn, newCheckOut, oldRoomID, oldCheckIn, oldCheckOut);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateReservationChildAdultAmount(int reservationID, int newAdultAmount, int newChildAmount, int oldAdultAmount, int oldChildAmount)
        {
            try
            {
                return _reservationAccessor.UpdateReservationChildAdultAmount(reservationID, newAdultAmount, newChildAmount, oldAdultAmount, oldChildAmount);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
