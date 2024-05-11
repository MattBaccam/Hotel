using DataAccessInterfaces;
using DataAccessLayer;
using DataObjects;
using LogicLayerInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class GuestManager : IGuestManager
    {
		private IGuestAccessor _guestAccessor = new GuestAccessor();
		public GuestManager() 
		{
            _guestAccessor = new GuestAccessor();
        }

        public bool AuthenticateGuest(string email, string password)
        {
            password = Helpers.HashSha256(password);
            return _guestAccessor.AuthenticateGuestWithEmailAndPasswordHash(email, password) == 1;
        }

        public bool CreateGuest(Guest guest)
        {
            try
            {
               return _guestAccessor.InsertGuest(guest);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool DoesEmailExist(string email)
        {
            try
            {
                return _guestAccessor.SelectGuestByEmail(email) != null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Guest GetGuestByEmail(string email)
        {
            try
            {
                return _guestAccessor.SelectGuestByEmail(email);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public Guest GetGuestByPhone(string phone)
        {
            try
            {
                return _guestAccessor.SelectGuestByPhone(phone);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<Guest> GetGuestByFirstName(string firstName)
        {
            try
            {
                return _guestAccessor.SelectGuestsByFirstName(firstName);
            }
            catch (Exception ex)
            { 
                throw ex;
            }
        }

        public List<Guest> GetGuestByFirstNameLastName(string firstName, string lastName)
        {
            try
            {
                return _guestAccessor.SelectGuestsByFirstNameLastName(firstName, lastName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Guest GetGuestByID(int guestID)
        {
			try
			{
				return _guestAccessor.SelectGuestByID(guestID);
            }
			catch (Exception ex)
			{

				throw ex;
			}
        }

        public bool SaveGuestInfo(Guest newGuest, Guest oldGuest)
        {
			try
			{
				return _guestAccessor.UpdateGuest(newGuest, oldGuest);
			}
			catch (Exception ex)
			{
				throw ex;
			}
        }
    }
}
