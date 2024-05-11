using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayerInterfaces
{
    public interface IGuestManager
    {
        Guest GetGuestByID(int guestID);
        Guest GetGuestByEmail(string email);
        Guest GetGuestByPhone(string phone);
        bool DoesEmailExist(string email);
        List<Guest> GetGuestByFirstName(string firstName);
        List<Guest> GetGuestByFirstNameLastName(string firstName, string lastName);
        bool SaveGuestInfo(Guest newGuest, Guest oldGuest);
        bool CreateGuest(Guest guest);
        bool AuthenticateGuest(string email, string password);
    }
}
