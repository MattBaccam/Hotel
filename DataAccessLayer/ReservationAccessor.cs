using DataAccessInterfaces;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataAccessLayer
{
    public class ReservationAccessor : IReservationAccessor
    {

        public Reservation SelectReservationByID(int reservationID)
        {
            var reservation = new Reservation();

            var connection = SqlConnectionProvider.GetConnection();

            var commandText = "sp_select_reservations_by_reservationID";

            var cmd = new SqlCommand(commandText, connection);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@ReservationID", SqlDbType.Int);

            cmd.Parameters["@ReservationID"].Value = reservationID;

            try
            {
                connection.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        reservation = new Reservation()
                        {
                            ReservationID = reader.GetInt32(0),
                            GuestID = reader.GetInt32(1),
                            RoomID = reader.GetInt32(2),
                            ReservationStatus = reader.GetString(3),
                            CheckIn = reader.GetDateTime(4),
                            CheckOut = reader.GetDateTime(5),
                            Comments = reader.GetString(6),
                            AdultAmount = reader.GetInt32(7),
                            ChildAmount = reader.GetInt32(8),
                            Paid = reader.GetBoolean(9)
                        };
                    }
                    return reservation;
                }
                else
                {
                    throw new ArgumentException("Reservation not found");
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

        public List<Events> SelectEventsByStatus(string status)
        {
            List<Events> eventsList = new List<Events>();

            var connection = SqlConnectionProvider.GetConnection();

            var commandText = "sp_select_events_by_status";

            var cmd = new SqlCommand(commandText, connection);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@EventsStatus", SqlDbType.NVarChar, 9);

            cmd.Parameters["@EventsStatus"].Value = status;

            try
            {
                connection.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var eventJson = new Events()
                        {
                            id = reader.GetInt32(0).ToString(),
                            title = $"{reader.GetString(1)} {reader.GetString(2)}",
                            start = reader.GetDateTime(3).ToString("yyyy-MM-dd"),
                            end = reader.GetDateTime(4).ToString(),
                            url = $"/EmployeeLanding/ReservationDetails?reservationID={reader.GetInt32(0)}"
                        };
                        switch (reader.GetString(5))
                        {
                            case "Due In":
                                eventJson.color = "#077e8c";
                                break;
                            case "Due Out":
                                eventJson.color = "#f7cb73";
                                break;
                            case "Out":
                                eventJson.color = "#f29339";
                                break;
                            case "Canceled":
                                eventJson.color = "#d9512c";
                                break;
                        }
                        eventsList.Add(eventJson);
                    }
                    return eventsList;
                }
                else
                {
                    throw new ArgumentException("No reservations under selected status");
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

        public List<Reservation> SelectReservationsByStatus(string status)
        {
            List<Reservation> reservations = new List<Reservation>();

            var connection = SqlConnectionProvider.GetConnection();

            var commandText = "sp_select_reservations_by_status";

            var cmd = new SqlCommand(commandText, connection);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@ReservationStatus", SqlDbType.NVarChar, 9);

            cmd.Parameters["@ReservationStatus"].Value = status;

            try
            {
                connection.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var reservation = new Reservation()
                        {
                            ReservationID = reader.GetInt32(0),
                            GuestID = reader.GetInt32(1),
                            RoomID = reader.GetInt32(2),
                            ReservationStatus = reader.GetString(3),
                            CheckIn = reader.GetDateTime(4),
                            CheckOut = reader.GetDateTime(5),
                            Comments = reader.GetString(6),
                            AdultAmount = reader.GetInt32(7),
                            ChildAmount = reader.GetInt32(8),
                            Paid = reader.GetBoolean(9)
                        };
                        reservations.Add(reservation);
                    }
                    return reservations;
                }
                else
                {
                    throw new ArgumentException("No reservations under selected status");
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


        public List<Reservation> SelectReservationsByGuestID(int guestID)
        {
            List<Reservation> reservations = new List<Reservation>();

            var connection = SqlConnectionProvider.GetConnection();

            var commandText = "sp_select_reservations_by_guestID";

            var cmd = new SqlCommand(commandText, connection);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@GuestID", SqlDbType.Int);

            cmd.Parameters["@GuestID"].Value = guestID;

            try
            {
                connection.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var reservation = new Reservation()
                        {
                            ReservationID = reader.GetInt32(0),
                            GuestID = reader.GetInt32(1),
                            RoomID = reader.GetInt32(2),
                            ReservationStatus = reader.GetString(3),
                            CheckIn = reader.GetDateTime(4),
                            CheckOut = reader.GetDateTime(5),
                            Comments = reader.GetString(6),
                            AdultAmount = reader.GetInt32(7),
                            ChildAmount = reader.GetInt32(8),
                            Paid = reader.GetBoolean(9)
                        };
                        reservations.Add(reservation);
                    }
                    return reservations;
                }
                else
                {
                    throw new ArgumentException("No reservations under selected guest");
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

        public bool UpdateReservation(Reservation newReservation, Reservation oldReservation)
        {
            var connection = SqlConnectionProvider.GetConnection();

            var commandText = "sp_update_reservation";

            var cmd = new SqlCommand(commandText, connection);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@ReservationID", SqlDbType.Int);
            cmd.Parameters.Add("@GuestID", SqlDbType.Int);
            cmd.Parameters.Add("@NewRoomID", SqlDbType.Int);
            cmd.Parameters.Add("@NewCheckIn", SqlDbType.Date);
            cmd.Parameters.Add("@NewCheckOut", SqlDbType.Date);
            cmd.Parameters.Add("@NewAdultAmount", SqlDbType.Int);
            cmd.Parameters.Add("@NewChildAmount", SqlDbType.Int);

            cmd.Parameters.Add("@OldRoomID", SqlDbType.Int);
            cmd.Parameters.Add("@OldCheckIn", SqlDbType.Date);
            cmd.Parameters.Add("@OldCheckOut", SqlDbType.Date);
            cmd.Parameters.Add("@OldAdultAmount", SqlDbType.Int);
            cmd.Parameters.Add("@OldChildAmount", SqlDbType.Int);

            cmd.Parameters["@ReservationID"].Value = oldReservation.ReservationID;
            cmd.Parameters["@GuestID"].Value = oldReservation.GuestID;
            cmd.Parameters["@NewRoomID"].Value = newReservation.RoomID;
            cmd.Parameters["@NewCheckIn"].Value = newReservation.CheckIn;
            cmd.Parameters["@NewCheckOut"].Value = newReservation.CheckOut;
            cmd.Parameters["@NewAdultAmount"].Value = newReservation.AdultAmount;
            cmd.Parameters["@NewChildAmount"].Value = newReservation.ChildAmount;

            cmd.Parameters["@OldRoomID"].Value = oldReservation.RoomID;
            cmd.Parameters["@OldCheckIn"].Value = oldReservation.CheckIn;
            cmd.Parameters["@OldCheckOut"].Value = oldReservation.CheckOut;
            cmd.Parameters["@OldAdultAmount"].Value = oldReservation.AdultAmount;
            cmd.Parameters["@OldChildAmount"].Value = oldReservation.ChildAmount;

            try
            {
                //open the connection
                connection.Open();

                // execute
                var rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                {
                    throw new ArgumentException("Failed to update the reservation");
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

        public bool UpdateReservationForCheckIn(int reservationID, DateTime newCheckIn, DateTime oldCheckIn)
        {
            var connection = SqlConnectionProvider.GetConnection();

            var commandText = "sp_update_reservation_for_checkin";

            var cmd = new SqlCommand(commandText, connection);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@ReservationID", SqlDbType.Int);
            cmd.Parameters.Add("@NewCheckIn", SqlDbType.Date);
            cmd.Parameters.Add("@OldCheckIn", SqlDbType.Date);

            cmd.Parameters["@ReservationID"].Value = reservationID;
            cmd.Parameters["@NewCheckIn"].Value = newCheckIn;
            cmd.Parameters["@OldCheckIn"].Value = oldCheckIn;

            try
            {
                connection.Open();

                var rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                {
                    throw new ArgumentException("Failed to update the reservation for check in");
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

        public bool UpdateReservationForCheckOut(int reservationID, DateTime newCheckOut, DateTime oldCheckOut)
        {
            var connection = SqlConnectionProvider.GetConnection();

            var commandText = "sp_update_reservation_for_checkout";

            var cmd = new SqlCommand(commandText, connection);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@ReservationID", SqlDbType.Int);
            cmd.Parameters.Add("@NewCheckOut", SqlDbType.Date);
            cmd.Parameters.Add("@OldCheckOut", SqlDbType.Date);

            cmd.Parameters["@ReservationID"].Value = reservationID;
            cmd.Parameters["@NewCheckOut"].Value = newCheckOut;
            cmd.Parameters["@OldCheckOut"].Value = oldCheckOut;

            try
            {
                connection.Open();

                // execute
                var rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                {
                    throw new ArgumentException("Failed to update the reservation for check out");
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

        public bool UpdateReservationForReschedule(int reservationID, DateTime newCheckIn, DateTime newCheckOut, DateTime oldCheckIn, DateTime oldCheckOut)
        {
            var connection = SqlConnectionProvider.GetConnection();

            var commandText = "sp_update_reservation_reschedule";

            var cmd = new SqlCommand(commandText, connection);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@ReservationID", SqlDbType.Int);
            cmd.Parameters.Add("@NewCheckIn", SqlDbType.Date);
            cmd.Parameters.Add("@NewCheckOut", SqlDbType.Date);

            cmd.Parameters.Add("@OldCheckIn", SqlDbType.Date);
            cmd.Parameters.Add("@OldCheckOut", SqlDbType.Date);

            cmd.Parameters["@ReservationID"].Value = reservationID;
            cmd.Parameters["@NewCheckIn"].Value = newCheckIn;
            cmd.Parameters["@NewCheckOut"].Value = newCheckOut;

            cmd.Parameters["@OldCheckIn"].Value = oldCheckIn;
            cmd.Parameters["@OldCheckOut"].Value = oldCheckOut;
            try
            {
                connection.Open();

                var rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                {
                    throw new ArgumentException("Failed to update the reservation for reschedule");
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

        public bool UpdateReservationComments(int reservationID, string newComments)
        {
            var connection = SqlConnectionProvider.GetConnection();

            var commandText = "sp_update_reservation_comments";

            var cmd = new SqlCommand(commandText, connection);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@ReservationID", SqlDbType.Int);
            cmd.Parameters.Add("@NewComments", SqlDbType.Text);

            cmd.Parameters["@ReservationID"].Value = reservationID;
            cmd.Parameters["@NewComments"].Value = newComments;

            try
            {
                connection.Open();

                var rows = cmd.ExecuteNonQuery();

                if(rows == 0)
                {
                    throw new ArgumentException("Failed to update the comments of the reservation");
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

        public bool UpdateReservationForCancel(int reservationID, string oldReservationStatus)
        {
            var connection = SqlConnectionProvider.GetConnection();

            var commandText = "sp_update_reservation_for_cancel";

            var cmd = new SqlCommand(commandText, connection);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@ReservationID", SqlDbType.Int);
            cmd.Parameters.Add("@OldReservationStatus", SqlDbType.NVarChar, 9);

            cmd.Parameters["@ReservationID"].Value = reservationID;
            cmd.Parameters["@OldReservationStatus"].Value = oldReservationStatus;

            try
            {
                connection.Open();

                var rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                {
                    throw new ArgumentException("Failed to update the reservation for cancel");
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

        public List<Reservation> SelectReservationByAllFields(string firstName, string lastName, DateTime checkIn, DateTime checkOut)
        {
            List<Reservation> reservations = new List<Reservation>();

            var connection = SqlConnectionProvider.GetConnection();

            var commandText = "sp_select_reservations_by_search_all";

            var cmd = new SqlCommand(commandText, connection);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 25);
            cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 25);
            cmd.Parameters.Add("@CheckIn", SqlDbType.Date);
            cmd.Parameters.Add("@CheckOut", SqlDbType.Date);

            cmd.Parameters["@FirstName"].Value = firstName;
            cmd.Parameters["@LastName"].Value = lastName;
            cmd.Parameters["@CheckIn"].Value = checkIn;
            cmd.Parameters["@CheckOut"].Value = checkOut;

            try
            {
                connection.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var reservation = new Reservation()
                        {
                            ReservationID = reader.GetInt32(0),
                            GuestID = reader.GetInt32(1),
                            RoomID = reader.GetInt32(2),
                            ReservationStatus = reader.GetString(3),
                            CheckIn = reader.GetDateTime(4),
                            CheckOut = reader.GetDateTime(5),
                            Comments = reader.GetString(6),
                            AdultAmount = reader.GetInt32(7),
                            ChildAmount = reader.GetInt32(8),
                            Paid = reader.GetBoolean(9)
                        };
                        reservations.Add(reservation);
                    }
                    return reservations;
                }
                else
                {
                    throw new ArgumentException("No reservations under search");
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

        public List<Reservation> SelectReservationByFirstNameAndCheckInCheckOut(string firstName, DateTime checkIn, DateTime checkOut)
        {
            List<Reservation> reservations = new List<Reservation>();

            var connection = SqlConnectionProvider.GetConnection();

            var commandText = "sp_select_reservations_by_search_firstname_checkin_checkout";

            var cmd = new SqlCommand(commandText, connection);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 25);
            cmd.Parameters.Add("@CheckIn", SqlDbType.Date);
            cmd.Parameters.Add("@CheckOut", SqlDbType.Date);

            cmd.Parameters["@FirstName"].Value = firstName;
            cmd.Parameters["@CheckIn"].Value = checkIn;
            cmd.Parameters["@CheckOut"].Value = checkOut;

            try
            {
                connection.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var reservation = new Reservation()
                        {
                            ReservationID = reader.GetInt32(0),
                            GuestID = reader.GetInt32(1),
                            RoomID = reader.GetInt32(2),
                            ReservationStatus = reader.GetString(3),
                            CheckIn = reader.GetDateTime(4),
                            CheckOut = reader.GetDateTime(5),
                            Comments = reader.GetString(6),
                            AdultAmount = reader.GetInt32(7),
                            ChildAmount = reader.GetInt32(8),
                            Paid = reader.GetBoolean(9)
                        };
                        reservations.Add(reservation);
                    }
                    return reservations;
                }
                else
                {
                    throw new ArgumentException("No reservations under search");
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

        public List<Reservation> SelectReservationByLastNameAndCheckInCheckOut(string lastName, DateTime checkIn, DateTime checkOut)
        {
            List<Reservation> reservations = new List<Reservation>();

            var connection = SqlConnectionProvider.GetConnection();

            var commandText = "sp_select_reservations_by_search_lastname_checkin_checkout";

            var cmd = new SqlCommand(commandText, connection);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 25);
            cmd.Parameters.Add("@CheckIn", SqlDbType.Date);
            cmd.Parameters.Add("@CheckOut", SqlDbType.Date);

            cmd.Parameters["@LastName"].Value = lastName;
            cmd.Parameters["@CheckIn"].Value = checkIn;
            cmd.Parameters["@CheckOut"].Value = checkOut;

            try
            {
                connection.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var reservation = new Reservation()
                        {
                            ReservationID = reader.GetInt32(0),
                            GuestID = reader.GetInt32(1),
                            RoomID = reader.GetInt32(2),
                            ReservationStatus = reader.GetString(3),
                            CheckIn = reader.GetDateTime(4),
                            CheckOut = reader.GetDateTime(5),
                            Comments = reader.GetString(6),
                            AdultAmount = reader.GetInt32(7),
                            ChildAmount = reader.GetInt32(8),
                            Paid = reader.GetBoolean(9)
                        };
                        reservations.Add(reservation);
                    }
                    return reservations;
                }
                else
                {
                    throw new ArgumentException("No reservations under search");
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

        public List<Reservation> SelectReservationByFirstName(string firstName)
        {
            List<Reservation> reservations = new List<Reservation>();

            var connection = SqlConnectionProvider.GetConnection();

            var commandText = "sp_select_reservations_by_search_firstname";

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
                        var reservation = new Reservation()
                        {
                            ReservationID = reader.GetInt32(0),
                            GuestID = reader.GetInt32(1),
                            RoomID = reader.GetInt32(2),
                            ReservationStatus = reader.GetString(3),
                            CheckIn = reader.GetDateTime(4),
                            CheckOut = reader.GetDateTime(5),
                            Comments = reader.GetString(6),
                            AdultAmount = reader.GetInt32(7),
                            ChildAmount = reader.GetInt32(8),
                            Paid = reader.GetBoolean(9)
                        };
                        reservations.Add(reservation);
                    }
                    return reservations;
                }
                else
                {
                    throw new ArgumentException("No reservations under search");
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

        public List<Reservation> SelectReservationByLastName(string lastName)
        {
            List<Reservation> reservations = new List<Reservation>();

            var connection = SqlConnectionProvider.GetConnection();

            var commandText = "sp_select_reservations_by_search_lastname";

            var cmd = new SqlCommand(commandText, connection);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 25);

            cmd.Parameters["@LastName"].Value = lastName;

            try
            {
                connection.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var reservation = new Reservation()
                        {
                            ReservationID = reader.GetInt32(0),
                            GuestID = reader.GetInt32(1),
                            RoomID = reader.GetInt32(2),
                            ReservationStatus = reader.GetString(3),
                            CheckIn = reader.GetDateTime(4),
                            CheckOut = reader.GetDateTime(5),
                            Comments = reader.GetString(6),
                            AdultAmount = reader.GetInt32(7),
                            ChildAmount = reader.GetInt32(8),
                            Paid = reader.GetBoolean(9)
                        };
                        reservations.Add(reservation);
                    }
                    return reservations;
                }
                else
                {
                    throw new ArgumentException("No reservations under search");
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

        public List<Reservation> SelectReservationByCheckInAndCheckOut(DateTime checkIn, DateTime checkOut)
        {
            List<Reservation> reservations = new List<Reservation>();

            var connection = SqlConnectionProvider.GetConnection();

            var commandText = "sp_select_reservations_by_search_checkin_checkout";

            var cmd = new SqlCommand(commandText, connection);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@CheckIn", SqlDbType.Date);
            cmd.Parameters.Add("@CheckOut", SqlDbType.Date);

            cmd.Parameters["@CheckIn"].Value = checkIn;
            cmd.Parameters["@CheckOut"].Value = checkOut;

            try
            {
                connection.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var reservation = new Reservation()
                        {
                            ReservationID = reader.GetInt32(0),
                            GuestID = reader.GetInt32(1),
                            RoomID = reader.GetInt32(2),
                            ReservationStatus = reader.GetString(3),
                            CheckIn = reader.GetDateTime(4),
                            CheckOut = reader.GetDateTime(5),
                            Comments = reader.GetString(6),
                            AdultAmount = reader.GetInt32(7),
                            ChildAmount = reader.GetInt32(8),
                            Paid = reader.GetBoolean(9)
                        };
                        reservations.Add(reservation);
                    }
                    return reservations;
                }
                else
                {
                    throw new ArgumentException("No reservations under search");
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

        public List<Reservation> SelectReservationByCheckIn(DateTime checkIn)
        {
            List<Reservation> reservations = new List<Reservation>();

            var connection = SqlConnectionProvider.GetConnection();

            var commandText = "sp_select_reservations_by_search_checkin";

            var cmd = new SqlCommand(commandText, connection);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@CheckIn", SqlDbType.Date);

            cmd.Parameters["@CheckIn"].Value = checkIn;

            try
            {
                connection.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var reservation = new Reservation()
                        {
                            ReservationID = reader.GetInt32(0),
                            GuestID = reader.GetInt32(1),
                            RoomID = reader.GetInt32(2),
                            ReservationStatus = reader.GetString(3),
                            CheckIn = reader.GetDateTime(4),
                            CheckOut = reader.GetDateTime(5),
                            Comments = reader.GetString(6),
                            AdultAmount = reader.GetInt32(7),
                            ChildAmount = reader.GetInt32(8),
                            Paid = reader.GetBoolean(9)
                        };
                        reservations.Add(reservation);
                    }
                    return reservations;
                }
                else
                {
                    throw new ArgumentException("No reservations under search");
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

        public List<Reservation> SelectReservationByCheckOut(DateTime checkOut)
        {
            List<Reservation> reservations = new List<Reservation>();

            var connection = SqlConnectionProvider.GetConnection();

            var commandText = "sp_select_reservations_by_search_checkout";

            var cmd = new SqlCommand(commandText, connection);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@CheckOut", SqlDbType.Date);

            cmd.Parameters["@CheckOut"].Value = checkOut;

            try
            {
                connection.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var reservation = new Reservation()
                        {
                            ReservationID = reader.GetInt32(0),
                            GuestID = reader.GetInt32(1),
                            RoomID = reader.GetInt32(2),
                            ReservationStatus = reader.GetString(3),
                            CheckIn = reader.GetDateTime(4),
                            CheckOut = reader.GetDateTime(5),
                            Comments = reader.GetString(6),
                            AdultAmount = reader.GetInt32(7),
                            ChildAmount = reader.GetInt32(8),
                            Paid = reader.GetBoolean(9)
                        };
                        reservations.Add(reservation);
                    }
                    return reservations;
                }
                else
                {
                    throw new ArgumentException("No reservations under search");
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
        public List<Reservation> SelectReservationByFirstNameCheckIn(string firstName, DateTime checkIn)
        {
            List<Reservation> reservations = new List<Reservation>();

            var connection = SqlConnectionProvider.GetConnection();

            var commandText = "sp_select_reservations_by_search_firstname_checkin";

            var cmd = new SqlCommand(commandText, connection);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 25);
            cmd.Parameters.Add("@CheckIn", SqlDbType.Date);

            cmd.Parameters["@FirstName"].Value = firstName;
            cmd.Parameters["@CheckIn"].Value = checkIn;

            try
            {
                connection.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var reservation = new Reservation()
                        {
                            ReservationID = reader.GetInt32(0),
                            GuestID = reader.GetInt32(1),
                            RoomID = reader.GetInt32(2),
                            ReservationStatus = reader.GetString(3),
                            CheckIn = reader.GetDateTime(4),
                            CheckOut = reader.GetDateTime(5),
                            Comments = reader.GetString(6),
                            AdultAmount = reader.GetInt32(7),
                            ChildAmount = reader.GetInt32(8),
                            Paid = reader.GetBoolean(9)
                        };
                        reservations.Add(reservation);
                    }
                    return reservations;
                }
                else
                {
                    throw new ArgumentException("No reservations under search");
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
        public List<Reservation> SelectReservationByFirstNameCheckOut(string firstName, DateTime checkOut)
        {
            List<Reservation> reservations = new List<Reservation>();

            var connection = SqlConnectionProvider.GetConnection();

            var commandText = "sp_select_reservations_by_search_firstname_checkout";

            var cmd = new SqlCommand(commandText, connection);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 25);
            cmd.Parameters.Add("@CheckOut", SqlDbType.Date);

            cmd.Parameters["@FirstName"].Value = firstName;
            cmd.Parameters["@CheckOut"].Value = checkOut;

            try
            {
                connection.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var reservation = new Reservation()
                        {
                            ReservationID = reader.GetInt32(0),
                            GuestID = reader.GetInt32(1),
                            RoomID = reader.GetInt32(2),
                            ReservationStatus = reader.GetString(3),
                            CheckIn = reader.GetDateTime(4),
                            CheckOut = reader.GetDateTime(5),
                            Comments = reader.GetString(6),
                            AdultAmount = reader.GetInt32(7),
                            ChildAmount = reader.GetInt32(8),
                            Paid = reader.GetBoolean(9)
                        };
                        reservations.Add(reservation);
                    }
                    return reservations;
                }
                else
                {
                    throw new ArgumentException("No reservations under search");
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
        public List<Reservation> SelectReservationByLastNameCheckIn(string lastName, DateTime checkIn)
        {
            List<Reservation> reservations = new List<Reservation>();

            var connection = SqlConnectionProvider.GetConnection();

            var commandText = "sp_select_reservations_by_search_lastname_checkin";

            var cmd = new SqlCommand(commandText, connection);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 25);
            cmd.Parameters.Add("@CheckIn", SqlDbType.Date);

            cmd.Parameters["@LastName"].Value = lastName;
            cmd.Parameters["@CheckIn"].Value = checkIn;

            try
            {
                connection.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var reservation = new Reservation()
                        {
                            ReservationID = reader.GetInt32(0),
                            GuestID = reader.GetInt32(1),
                            RoomID = reader.GetInt32(2),
                            ReservationStatus = reader.GetString(3),
                            CheckIn = reader.GetDateTime(4),
                            CheckOut = reader.GetDateTime(5),
                            Comments = reader.GetString(6),
                            AdultAmount = reader.GetInt32(7),
                            ChildAmount = reader.GetInt32(8),
                            Paid = reader.GetBoolean(9)
                        };
                        reservations.Add(reservation);
                    }
                    return reservations;
                }
                else
                {
                    throw new ArgumentException("No reservations under search");
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
        public List<Reservation> SelectReservationByLastNameCheckOut(string lastName, DateTime checkOut)
        {
            List<Reservation> reservations = new List<Reservation>();

            var connection = SqlConnectionProvider.GetConnection();

            var commandText = "sp_select_reservations_by_search_lastname_checkout";

            var cmd = new SqlCommand(commandText, connection);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 25);
            cmd.Parameters.Add("@CheckOut", SqlDbType.Date);

            cmd.Parameters["@LastName"].Value = lastName;
            cmd.Parameters["@CheckOut"].Value = checkOut;

            try
            {
                connection.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var reservation = new Reservation()
                        {
                            ReservationID = reader.GetInt32(0),
                            GuestID = reader.GetInt32(1),
                            RoomID = reader.GetInt32(2),
                            ReservationStatus = reader.GetString(3),
                            CheckIn = reader.GetDateTime(4),
                            CheckOut = reader.GetDateTime(5),
                            Comments = reader.GetString(6),
                            AdultAmount = reader.GetInt32(7),
                            ChildAmount = reader.GetInt32(8),
                            Paid = reader.GetBoolean(9)
                        };
                        reservations.Add(reservation);
                    }
                    return reservations;
                }
                else
                {
                    throw new ArgumentException("No reservations under search");
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

        public bool InsertReservation(Reservation reservation)
        {
            var connection = SqlConnectionProvider.GetConnection();

            var commandText = "sp_insert_reservation";

            var cmd = new SqlCommand(commandText, connection);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@GuestID", SqlDbType.Int);
            cmd.Parameters.Add("@RoomID", SqlDbType.Int);
            cmd.Parameters.Add("@ReservationStatus", SqlDbType.NVarChar, 9);
            cmd.Parameters.Add("@CheckIn", SqlDbType.Date);
            cmd.Parameters.Add("@CheckOut", SqlDbType.Date);
            cmd.Parameters.Add("@Comments", SqlDbType.Text);
            cmd.Parameters.Add("@AdultAmount", SqlDbType.Int);
            cmd.Parameters.Add("@ChildAmount", SqlDbType.Int);
            cmd.Parameters.Add("@Paid", SqlDbType.Bit);

            cmd.Parameters["@GuestID"].Value = reservation.GuestID;
            cmd.Parameters["@RoomID"].Value = reservation.RoomID;
            cmd.Parameters["@ReservationStatus"].Value = reservation.ReservationStatus;
            cmd.Parameters["@CheckIn"].Value = reservation.CheckIn;
            cmd.Parameters["@CheckOut"].Value = reservation.CheckOut;
            cmd.Parameters["@Comments"].Value = reservation.Comments;
            cmd.Parameters["@AdultAmount"].Value = reservation.AdultAmount;
            cmd.Parameters["@ChildAmount"].Value = reservation.ChildAmount;
            cmd.Parameters["@Paid"].Value = reservation.Paid;

            try
            {
                connection.Open();

                var rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                {
                    throw new ArgumentException("Failed to create the reservation");
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

        public Reservation SelectReservationByFirstNameLastNameRoomID(string firstName, string lastName, int roomID)
        {
            Reservation reservation = new Reservation();

            var connection = SqlConnectionProvider.GetConnection();

            var commandText = "sp_select_reservations_by_firstname_lastname_roomID";

            var cmd = new SqlCommand(commandText, connection);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 25);
            cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 25);
            cmd.Parameters.Add("@RoomID", SqlDbType.Int);

            cmd.Parameters["@FirstName"].Value = firstName;
            cmd.Parameters["@LastName"].Value = lastName;
            cmd.Parameters["@RoomID"].Value = roomID;

            try
            {
                connection.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                            reservation.ReservationID = reader.GetInt32(0);
                            reservation.GuestID = reader.GetInt32(1);
                            reservation.RoomID = reader.GetInt32(2);
                            reservation.ReservationStatus = reader.GetString(3);
                            reservation.CheckIn = reader.GetDateTime(4);
                            reservation.CheckOut = reader.GetDateTime(5);
                            reservation.Comments = reader.GetString(6);
                            reservation.AdultAmount = reader.GetInt32(7);
                            reservation.ChildAmount = reader.GetInt32(8);
                            reservation.Paid = reader.GetBoolean(9);
                    }
                    return reservation;
                }
                else
                {
                    throw new ArgumentException("No reservations under search");
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

        public Reservation SelectReservationByFirstNameLastName(string firstName, string lastName)
        {
            Reservation reservation = new Reservation();

            var connection = SqlConnectionProvider.GetConnection();

            var commandText = "sp_select_reservations_by_firstname_lastname";

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
                        reservation.ReservationID = reader.GetInt32(0);
                        reservation.GuestID = reader.GetInt32(1);
                        reservation.RoomID = reader.GetInt32(2);
                        reservation.ReservationStatus = reader.GetString(3);
                        reservation.CheckIn = reader.GetDateTime(4);
                        reservation.CheckOut = reader.GetDateTime(5);
                        reservation.Comments = reader.GetString(6);
                        reservation.AdultAmount = reader.GetInt32(7);
                        reservation.ChildAmount = reader.GetInt32(8);
                        reservation.Paid = reader.GetBoolean(9);
                    }
                    return reservation;
                }
                else
                {
                    throw new ArgumentException("No reservations under search");
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

        public Reservation SelectReservationByRoomID(int roomID)
        {
            Reservation reservation = new Reservation();

            var connection = SqlConnectionProvider.GetConnection();

            var commandText = "sp_select_reservations_by_roomID";

            var cmd = new SqlCommand(commandText, connection);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@RoomID", SqlDbType.Int);

            cmd.Parameters["@RoomID"].Value = roomID;

            try
            {
                connection.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        reservation.ReservationID = reader.GetInt32(0);
                        reservation.GuestID = reader.GetInt32(1);
                        reservation.RoomID = reader.GetInt32(2);
                        reservation.ReservationStatus = reader.GetString(3);
                        reservation.CheckIn = reader.GetDateTime(4);
                        reservation.CheckOut = reader.GetDateTime(5);
                        reservation.Comments = reader.GetString(6);
                        reservation.AdultAmount = reader.GetInt32(7);
                        reservation.ChildAmount = reader.GetInt32(8);
                        reservation.Paid = reader.GetBoolean(9);
                    }
                    return reservation;
                }
                else
                {
                    throw new ArgumentException("No reservations under search");
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

        public List<Reservation> SelectReservationsByAsc(int guestID)
        {
            List<Reservation> reservations = new List<Reservation>();

            var connection = SqlConnectionProvider.GetConnection();

            var commandText = "sp_select_guest_reservations_ascending";

            var cmd = new SqlCommand(commandText, connection);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@GuestID", SqlDbType.Int);

            cmd.Parameters["@GuestID"].Value = guestID;

            try
            {
                connection.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var reservation = new Reservation()
                        {
                            ReservationID = reader.GetInt32(0),
                            GuestID = reader.GetInt32(1),
                            RoomID = reader.GetInt32(2),
                            ReservationStatus = reader.GetString(3),
                            CheckIn = reader.GetDateTime(4),
                            CheckOut = reader.GetDateTime(5),
                            Comments = reader.GetString(6),
                            AdultAmount = reader.GetInt32(7),
                            ChildAmount = reader.GetInt32(8),
                            Paid = reader.GetBoolean(9)
                        };
                        reservations.Add(reservation);
                    }
                    return reservations;
                }
                else
                {
                    throw new ArgumentException("No reservations under selected status");
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

        public List<Reservation> SelectReservationsByDesc(int guestID)
        {
            List<Reservation> reservations = new List<Reservation>();

            var connection = SqlConnectionProvider.GetConnection();

            var commandText = "sp_select_guest_reservations_descending";

            var cmd = new SqlCommand(commandText, connection);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@GuestID", SqlDbType.Int);

            cmd.Parameters["@GuestID"].Value = guestID;

            try
            {
                connection.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var reservation = new Reservation()
                        {
                            ReservationID = reader.GetInt32(0),
                            GuestID = reader.GetInt32(1),
                            RoomID = reader.GetInt32(2),
                            ReservationStatus = reader.GetString(3),
                            CheckIn = reader.GetDateTime(4),
                            CheckOut = reader.GetDateTime(5),
                            Comments = reader.GetString(6),
                            AdultAmount = reader.GetInt32(7),
                            ChildAmount = reader.GetInt32(8),
                            Paid = reader.GetBoolean(9)
                        };
                        reservations.Add(reservation);
                    }
                    return reservations;
                }
                else
                {
                    throw new ArgumentException("No reservations under selected status");
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

        public List<Reservation> SelectReservationsForGuestByStatus(int guestID, string status)
        {
            List<Reservation> reservations = new List<Reservation>();

            var connection = SqlConnectionProvider.GetConnection();

            var commandText = "sp_select_guest_reservations_by_reservation_status";

            var cmd = new SqlCommand(commandText, connection);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@GuestID", SqlDbType.Int);

            cmd.Parameters.Add("@ReservationStatus", SqlDbType.NVarChar, 9);

            cmd.Parameters["@GuestID"].Value = guestID;
            cmd.Parameters["@ReservationStatus"].Value = status;

            try
            {
                connection.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var reservation = new Reservation()
                        {
                            ReservationID = reader.GetInt32(0),
                            GuestID = reader.GetInt32(1),
                            RoomID = reader.GetInt32(2),
                            ReservationStatus = reader.GetString(3),
                            CheckIn = reader.GetDateTime(4),
                            CheckOut = reader.GetDateTime(5),
                            Comments = reader.GetString(6),
                            AdultAmount = reader.GetInt32(7),
                            ChildAmount = reader.GetInt32(8),
                            Paid = reader.GetBoolean(9)
                        };
                        reservations.Add(reservation);
                    }
                    return reservations;
                }
                else
                {
                    throw new ArgumentException("No reservations under selected status");
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

        public bool UpdateReservationForRescheduleNewRoom(int reservationID, int newRoomID, DateTime newCheckIn, DateTime newCheckOut, int oldRoomID, DateTime oldCheckIn, DateTime oldCheckOut)
        {
            var connection = SqlConnectionProvider.GetConnection();

            var commandText = "sp_update_reservation_reschedule_new_room";

            var cmd = new SqlCommand(commandText, connection);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@ReservationID", SqlDbType.Int);
            cmd.Parameters.Add("@NewRoomID", SqlDbType.Int);
            cmd.Parameters.Add("@NewCheckIn", SqlDbType.Date);
            cmd.Parameters.Add("@NewCheckOut", SqlDbType.Date);

            cmd.Parameters.Add("@OldRoomID", SqlDbType.Int);
            cmd.Parameters.Add("@OldCheckIn", SqlDbType.Date);
            cmd.Parameters.Add("@OldCheckOut", SqlDbType.Date);

            cmd.Parameters["@ReservationID"].Value = reservationID;
            cmd.Parameters["@NewRoomID"].Value = newRoomID;
            cmd.Parameters["@NewCheckIn"].Value = newCheckIn;
            cmd.Parameters["@NewCheckOut"].Value = newCheckOut;

            cmd.Parameters["@OldRoomID"].Value = oldRoomID;
            cmd.Parameters["@OldCheckIn"].Value = oldCheckIn;
            cmd.Parameters["@OldCheckOut"].Value = oldCheckOut;
            try
            {
                connection.Open();

                var rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                {
                    throw new ArgumentException("Failed to update the reservation for reschedule with new room");
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

        public bool UpdateReservationChildAdultAmount(int reservationID, int newAdultAmount, int newChildAmount, int oldAdultAmount, int oldChildAmount)
        {
            var connection = SqlConnectionProvider.GetConnection();

            var commandText = "sp_update_child_adult_amount";

            var cmd = new SqlCommand(commandText, connection);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@ReservationID", SqlDbType.Int);
            cmd.Parameters.Add("@NewAdultAmount", SqlDbType.Int);
            cmd.Parameters.Add("@NewChildAmount", SqlDbType.Int);

            cmd.Parameters.Add("@OldAdultAmount", SqlDbType.Int);
            cmd.Parameters.Add("@OldChildAmount", SqlDbType.Int);

            cmd.Parameters["@ReservationID"].Value = reservationID;
            cmd.Parameters["@NewAdultAmount"].Value = newAdultAmount;
            cmd.Parameters["@NewChildAmount"].Value = newChildAmount;

            cmd.Parameters["@OldAdultAmount"].Value = oldAdultAmount;
            cmd.Parameters["@OldChildAmount"].Value = oldChildAmount;

            try
            {
                connection.Open();

                var rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                {
                    throw new ArgumentException("Failed to update the reservation for new adult and child amounts");
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
