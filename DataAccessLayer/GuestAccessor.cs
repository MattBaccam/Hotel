using DataAccessInterfaces;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace DataAccessLayer
{
    public class GuestAccessor : IGuestAccessor
    {
        public int AuthenticateGuestWithEmailAndPasswordHash(string email, string passwordHash)
        {
            int rows = 0;
            var connection = SqlConnectionProvider.GetConnection();

            var commandText = "sp_authenticate_guest";

            var cmd = new SqlCommand(commandText, connection);

            cmd.CommandType = CommandType.StoredProcedure;



            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@PasswordHash", SqlDbType.NVarChar, 100);

            cmd.Parameters["@Email"].Value = email;
            cmd.Parameters["@PasswordHash"].Value = passwordHash;

            try
            {
                connection.Open();

                rows = Convert.ToInt32(cmd.ExecuteScalar());

                return rows;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        public bool InsertGuest(Guest guest)
        {
            var connection = SqlConnectionProvider.GetConnection();
            var commandText = "sp_insert_guest";
            var cmd = new SqlCommand(commandText, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 25);
            cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 25);
            cmd.Parameters.Add("@Phone", SqlDbType.NVarChar, 25);
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100);

            cmd.Parameters["@FirstName"].Value = guest.FirstName;
            cmd.Parameters["@LastName"].Value = guest.LastName;
            cmd.Parameters["@Phone"].Value = guest.Phone;
            cmd.Parameters["@Email"].Value = guest.Email;

            try
            {
                connection.Open();

                var rows = cmd.ExecuteNonQuery();
                if (rows == 0)
                {
                    throw new ArgumentException("Failed to create the guest");
                }
                return rows == 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        public Guest SelectGuestByEmail(string email)
        {
            var guest = new Guest();
            var connection = SqlConnectionProvider.GetConnection();
            var commandText = "sp_select_guest_by_email";
            var cmd = new SqlCommand(commandText, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100);
            cmd.Parameters["@Email"].Value = email;
            try
            {
                connection.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        guest.GuestID = reader.GetInt32(0);
                        guest.FirstName = reader.GetString(1);
                        guest.LastName = reader.GetString(2);
                        guest.Phone = reader.GetString(3);
                        guest.Email = reader.GetString(4);
                    }
                    return guest;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }
        public Guest SelectGuestByPhone(string phone)
        {
            var guest = new Guest();
            var connection = SqlConnectionProvider.GetConnection();
            var commandText = "sp_select_guest_by_phone";
            var cmd = new SqlCommand(commandText, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Phone", SqlDbType.NVarChar, 100);
            cmd.Parameters["@Phone"].Value = phone;
            try
            {
                connection.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        guest.GuestID = reader.GetInt32(0);
                        guest.FirstName = reader.GetString(1);
                        guest.LastName = reader.GetString(2);
                        guest.Phone = reader.GetString(3);
                        guest.Email = reader.GetString(4);
                    }
                    return guest;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        public Guest SelectGuestByID(int guestId)
        {
            var guest = new Guest();
            var connection = SqlConnectionProvider.GetConnection();
            var commandText = "sp_select_guest_by_guestID";
            var cmd = new SqlCommand(commandText, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@GuestID", SqlDbType.Int);
            cmd.Parameters["@GuestID"].Value = guestId;
            try
            {
                connection.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        guest.GuestID = reader.GetInt32(0);
                        guest.FirstName = reader.GetString(1);
                        guest.LastName = reader.GetString(2);
                        guest.Phone = reader.GetString(3);
                        guest.Email = reader.GetString(4);
                    }
                    return guest;
                }
                else
                {
                    throw new ArgumentException("Guest not found");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        public List<Guest> SelectGuestsByFirstName(string firstName)
        {
            var guests = new List<Guest>();
            var connection = SqlConnectionProvider.GetConnection();
            var commandText = "sp_select_guest_by_firstname";
            var cmd = new SqlCommand(commandText, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 25);
            cmd.Parameters["@FirstName"].Value = firstName;
            try
            {
                connection.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var guest = new Guest();
                        guest.GuestID = reader.GetInt32(0);
                        guest.FirstName = reader.GetString(1);
                        guest.LastName = reader.GetString(2);
                        guest.Phone = reader.GetString(3);
                        guest.Email = reader.GetString(4);
                        guests.Add(guest);
                    }
                    return guests;
                }
                else
                {
                    throw new ArgumentException("Guests not found under first name");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        public List<Guest> SelectGuestsByFirstNameLastName(string firstName, string lastName)
        {
            var guests = new List<Guest>();
            var connection = SqlConnectionProvider.GetConnection();
            var commandText = "sp_select_guest_by_firstname_lastname";
            var cmd = new SqlCommand(commandText, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 25);
            cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 25);
            cmd.Parameters["@FirstName"].Value = firstName;
            cmd.Parameters["@LastName"].Value = lastName;
            try
            {
                connection.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var guest = new Guest();    
                        guest.GuestID = reader.GetInt32(0);
                        guest.FirstName = reader.GetString(1);
                        guest.LastName = reader.GetString(2);
                        guest.Phone = reader.GetString(3);
                        guest.Email = reader.GetString(4);
                        guests.Add(guest);
                    }
                    return guests;
                }
                else
                {
                    throw new ArgumentException("Guests not found under first name and last name");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        public bool UpdateGuest(Guest newGuest, Guest oldGuest)
        {
            var connection = SqlConnectionProvider.GetConnection();
            var commandText = "sp_update_guest";
            var cmd = new SqlCommand(commandText, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@GuestID", SqlDbType.Int);
            cmd.Parameters.Add("@NewFirstName", SqlDbType.NVarChar, 25);
            cmd.Parameters.Add("@NewLastName", SqlDbType.NVarChar, 25);
            cmd.Parameters.Add("@NewPhone", SqlDbType.NVarChar, 25);
            cmd.Parameters.Add("@NewEmail", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@OldFirstName", SqlDbType.NVarChar, 25);
            cmd.Parameters.Add("@OldLastName", SqlDbType.NVarChar, 25);
            cmd.Parameters.Add("@OldPhone", SqlDbType.NVarChar, 25);
            cmd.Parameters.Add("@OldEmail", SqlDbType.NVarChar, 100);

            cmd.Parameters["@GuestID"].Value = oldGuest.GuestID;
            cmd.Parameters["@NewFirstName"].Value = newGuest.FirstName;
            cmd.Parameters["@NewLastName"].Value = newGuest.LastName;
            cmd.Parameters["@NewPhone"].Value = newGuest.Phone;
            cmd.Parameters["@NewEmail"].Value = newGuest.Email;

            cmd.Parameters["@OldFirstName"].Value = oldGuest.FirstName;
            cmd.Parameters["@OldLastName"].Value = oldGuest.LastName;
            cmd.Parameters["@OldPhone"].Value = oldGuest.Phone;
            cmd.Parameters["@OldEmail"].Value = oldGuest.Email;

            try
            {
                connection.Open();

                var rows = cmd.ExecuteNonQuery();
                if(rows == 0)
                {
                    throw new ArgumentException("Failed to update the guest");
                }
                return rows == 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
