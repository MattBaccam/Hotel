using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessInterfaces
{
    public interface IGuestAccessor
    {
        int AuthenticateGuestWithEmailAndPasswordHash(string email, string passwordHash);
        bool InsertGuest(Guest guest);
        Guest SelectGuestByID(int guestId);
        Guest SelectGuestByEmail(string email);
        Guest SelectGuestByPhone(string phone);
        List<Guest> SelectGuestsByFirstName(string firstName);
        List<Guest> SelectGuestsByFirstNameLastName(string firstName, string lastName);
        bool UpdateGuest(Guest newGuest, Guest oldGuest);
    }
}
