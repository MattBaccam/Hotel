using DataAccessInterfaces;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessFakes
{
    public class GuestAccessorFake : IGuestAccessor
    {
        private List<Guest> _guests = new List<Guest>();
        private int _nextGuestID = 1;

        public GuestAccessorFake()
        {
            // Add some dummy guest data
            _guests.Add(new Guest
            {
                GuestID = _nextGuestID++,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Phone = "123-456-7890"
            });

            _guests.Add(new Guest
            {
                GuestID = _nextGuestID++,
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@example.com",
                Phone = "987-654-3210"
            });
        }

        public int AuthenticateGuestWithEmailAndPasswordHash(string email, string passwordHash)
        {
            // Dummy authentication logic
            var guest = _guests.FirstOrDefault(g => g.Email == email);
            if (guest != null)
            {
                // For simplicity, just return the guest's ID
                return guest.GuestID;
            }
            return -1; // Indicate failure
        }

        public bool InsertGuest(Guest guest)
        {
            guest.GuestID = _nextGuestID++;
            _guests.Add(guest);
            return true;
        }

        public Guest SelectGuestByEmail(string email)
        {
            return _guests.FirstOrDefault(g => g.Email == email);
        }

        public Guest SelectGuestByID(int guestId)
        {
            return _guests.FirstOrDefault(g => g.GuestID == guestId);
        }

        public Guest SelectGuestByPhone(string phone)
        {
            return _guests.FirstOrDefault(g => g.Phone == phone);
        }

        public List<Guest> SelectGuestsByFirstName(string firstName)
        {
            return _guests.Where(g => g.FirstName == firstName).ToList();
        }

        public List<Guest> SelectGuestsByFirstNameLastName(string firstName, string lastName)
        {
            return _guests.Where(g => g.FirstName == firstName && g.LastName == lastName).ToList();
        }

        public bool UpdateGuest(Guest newGuest, Guest oldGuest)
        {
            var existingGuest = _guests.FirstOrDefault(g => g.GuestID == oldGuest.GuestID);
            if (existingGuest != null)
            {
                // Update guest information
                existingGuest.FirstName = newGuest.FirstName;
                existingGuest.LastName = newGuest.LastName;
                existingGuest.Email = newGuest.Email;
                existingGuest.Phone = newGuest.Phone;
                return true;
            }
            return false; // Indicate failure
        }
    }

}
