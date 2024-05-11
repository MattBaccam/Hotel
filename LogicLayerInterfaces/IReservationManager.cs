using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayerInterfaces
{
    public interface IReservationManager
    {
        List<ReservationVM> GetReservationsByAsc(int guestID);
        List<ReservationVM> GetReservationsByDesc(int guestID);
        List<ReservationVM> GetReservationsForGuestByStatus(int guestID, string status);
        List<ReservationVM> GetReservationsByGuestID(int guestID);
        List<Events> GetEventsByStatus(string status);
        List<ReservationVM> GetReservationsByStatus(string status);
        List<ReservationVM> GetReservationsViaSearch(string firstName = null, string lastName = null, DateTime? checkIn = null, DateTime? checkOut = null);
        ReservationVM GetReservationsByNameAndRoomID(string firstName, string lastName, int roomID);
        ReservationVM GetReservationByRoomID(int roomID);
        ReservationVM GetReservationVM(int reservationID);
        Reservation GetReservationByID(int reservationID);
        bool SaveReservationComments(int reservationID, string newComments);
        bool SaveReservationForCheckIn(int reservationID, DateTime newCheckIn, DateTime oldCheckIn);
        bool SaveReservationForCheckOut(int reservationID, DateTime newCheckOut, DateTime oldCheckOut);
        bool SaveReservation(Reservation newReservation, Reservation oldReservation);
        bool SaveReservationForReschedule(int reservationID, DateTime newCheckIn, DateTime newCheckOut, DateTime oldCheckIn, DateTime oldCheckOut);
        bool SaveReservationForRescheduleNewRoom(int reservationID, int newRoomID, DateTime newCheckIn, DateTime newCheckOut, int oldRoomID, DateTime oldCheckIn, DateTime oldCheckOut);
        bool UpdateReservationChildAdultAmount(int reservationID, int newAdultAmount, int newChildAmount, int oldAdultAmount, int oldChildAmount);
        bool SaveReservationForCancel(int reservationID, string oldReservationStatus);
        bool CreateReservation(Reservation reservation);
    }
}
