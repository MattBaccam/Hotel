using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessInterfaces
{
    public interface IReservationAccessor
    {
        List<Reservation> SelectReservationsByAsc(int guestID);
        List<Reservation> SelectReservationsByDesc(int guestID);
        List<Reservation> SelectReservationsForGuestByStatus(int guestID, string status);
        List<Events> SelectEventsByStatus(string status);
        List<Reservation> SelectReservationsByStatus(string status);
        List<Reservation> SelectReservationsByGuestID(int guestID);
        Reservation SelectReservationByID(int reservationID);
        List<Reservation> SelectReservationByAllFields(string firstName, string lastName, DateTime checkIn, DateTime checkOut);
        List<Reservation> SelectReservationByFirstNameAndCheckInCheckOut(string firstName, DateTime checkIn, DateTime checkOut);
        List<Reservation> SelectReservationByLastNameAndCheckInCheckOut(string lastName, DateTime checkIn, DateTime checkOut);
        List<Reservation> SelectReservationByFirstName(string firstName);
        List<Reservation> SelectReservationByLastName(string lastName);
        List<Reservation> SelectReservationByCheckInAndCheckOut(DateTime checkIn, DateTime checkOut);
        List<Reservation> SelectReservationByCheckIn(DateTime checkIn);
        List<Reservation> SelectReservationByCheckOut(DateTime checkOut);
        List<Reservation> SelectReservationByFirstNameCheckIn(string firstName, DateTime checkIn);
        List<Reservation> SelectReservationByFirstNameCheckOut(string firstName, DateTime checkOut);
        List<Reservation> SelectReservationByLastNameCheckIn(string lastName, DateTime checkIn);
        List<Reservation> SelectReservationByLastNameCheckOut(string lastName, DateTime checkOut);
        Reservation SelectReservationByFirstNameLastNameRoomID(string firstName, string lastName, int roomID);
        Reservation SelectReservationByFirstNameLastName(string firstName, string lastName);
        Reservation SelectReservationByRoomID(int roomID);
        bool UpdateReservationComments(int reservationID, string newComments);
        bool UpdateReservationForCheckIn(int reservationID, DateTime newCheckIn, DateTime oldCheckIn);
        bool UpdateReservationForCheckOut(int reservationID, DateTime newCheckOut, DateTime oldCheckOut);
        bool UpdateReservation(Reservation newReservation, Reservation oldReservation);
        bool UpdateReservationForReschedule(int reservationID, DateTime newCheckIn, DateTime newCheckOut, DateTime oldCheckIn, DateTime oldCheckOut);
        bool UpdateReservationForRescheduleNewRoom(int reservationID, int newRoomID, DateTime newCheckIn, DateTime newCheckOut, int oldRoomID, DateTime oldCheckIn, DateTime oldCheckOut);
        bool UpdateReservationForCancel(int reservationID, string oldReservationStatus);
        bool InsertReservation(Reservation reservation);
        bool UpdateReservationChildAdultAmount(int reservationID, int newAdultAmount, int newChildAmount, int oldAdultAmount, int oldChildAmount);
    }
}
