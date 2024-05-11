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
    public class RoomAccessor : IRoomAccessor
    {
        public Room SelectRoomByRoomID(int roomID)
        {
            var room = new Room();
            var connection = SqlConnectionProvider.GetConnection();
            var commandText = "sp_select_room";
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
                        room.RoomID = reader.GetInt32(0);
                        room.RoomTypeID = reader.GetString(1);
                        room.RoomAvailability = reader.GetBoolean(2);
                        room.RoomStatus = reader.GetString(3);
                    }
                    return room;
                }
                else
                {
                    throw new ArgumentException("Room not found");
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

        public List<Room> SelectRoomsAvailable(DateTime checkIn, DateTime checkOut)
        {
            var rooms = new List<Room>();
            var connection = SqlConnectionProvider.GetConnection();
            var commandText = "sp_select_rooms_available";
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
                        var room = new Room();
                        room.RoomID = reader.GetInt32(0);
                        room.RoomTypeID = reader.GetString(1);
                        room.RoomAvailability = reader.GetBoolean(2);
                        room.RoomStatus = reader.GetString(3);
                        rooms.Add(room);
                    }
                    return rooms;
                }
                else
                {
                    throw new ArgumentException("Room not found");
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
        public bool SelectRoomAvailability(int roomID, DateTime checkIn, DateTime checkOut)
        {
            var rows = 0;
            var connection = SqlConnectionProvider.GetConnection();
            var commandText = "sp_select_room_availability";
            var cmd = new SqlCommand(commandText, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CheckIn", checkIn);
            cmd.Parameters.AddWithValue("@CheckOut", checkOut);
            cmd.Parameters.AddWithValue("@RoomID", roomID);

            try
            {
                connection.Open();

                rows += Convert.ToInt32(cmd.ExecuteScalar());

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
            return rows == 1;
        }

        public RoomType SelectRoomTypeByRoomTypeID(string roomTypeID)
        {
            var roomType = new RoomType();
            var connection = SqlConnectionProvider.GetConnection();
            var commandText = "sp_select_room_type";
            var cmd = new SqlCommand(commandText, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@RoomTypeId", SqlDbType.NVarChar, 75);
            cmd.Parameters["@RoomTypeId"].Value = roomTypeID;
            try
            {
                connection.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        roomType.RoomTypeID = reader.GetString(0);
                        roomType.RoomPrice = reader.GetInt32(1);
                        roomType.RoomDescription = reader.GetString(2);
                    }
                    return roomType;
                }
                else
                {
                    throw new ArgumentException("Room Type not found");
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

        public bool UpdateRoomStatus(int roomID, string newRoomStatus, string oldRoomStatus)
        {
            var rows = 0;
            var connection = SqlConnectionProvider.GetConnection();
            var commandText = "sp_update_room_status";
            var cmd = new SqlCommand(commandText, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@RoomID", SqlDbType.Int);
            cmd.Parameters.Add("@NewRoomStatus", SqlDbType.NVarChar, 9);
            cmd.Parameters.Add("@OldRoomStatus", SqlDbType.NVarChar, 9);

            cmd.Parameters["@RoomID"].Value = roomID;
            cmd.Parameters["@NewRoomStatus"].Value = newRoomStatus;
            cmd.Parameters["@OldRoomStatus"].Value = oldRoomStatus;
            try
            {
                connection.Open();

                rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                {
                    throw new ArgumentException("Failed to update the room status");
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

        public List<Room> SelectRoomsByStatus(string status)
        {
            var rooms = new List<Room>();
            var connection = SqlConnectionProvider.GetConnection();
            var commandText = "sp_select_rooms_by_status";
            var cmd = new SqlCommand(commandText, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@RoomStatus", SqlDbType.NVarChar, 9);

            cmd.Parameters["@RoomStatus"].Value = status;
            try
            {
                connection.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var room = new Room();
                        room.RoomID = reader.GetInt32(0);
                        room.RoomTypeID = reader.GetString(1);
                        room.RoomAvailability = reader.GetBoolean(2);
                        room.RoomStatus = reader.GetString(3);
                        rooms.Add(room);
                    }
                    return rooms;
                }
                else
                {
                    throw new ArgumentException("Rooms not foun under selected status");
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

        public bool UpdateRoom(Room newRoom, Room oldRoom)
        {
            var connection = SqlConnectionProvider.GetConnection();
            var commandText = "sp_update_room";
            var cmd = new SqlCommand(commandText, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@RoomID", oldRoom.RoomID);
            cmd.Parameters.AddWithValue("@NewRoomTypeID", newRoom.RoomTypeID);
            cmd.Parameters.AddWithValue("@NewRoomAvailability", newRoom.RoomAvailability);
            cmd.Parameters.AddWithValue("@NewRoomStatus", newRoom.RoomStatus);

            cmd.Parameters.AddWithValue("@OldRoomTypeID", oldRoom.RoomTypeID);
            cmd.Parameters.AddWithValue("@OldRoomAvailability", oldRoom.RoomAvailability);
            cmd.Parameters.AddWithValue("@OldRoomStatus", oldRoom.RoomStatus);

            try
            {
                connection.Open();

                var rows = cmd.ExecuteNonQuery();

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

        public bool UpdateRoomType(RoomType newRoomType, RoomType oldRoomType)
        {
            var connection = SqlConnectionProvider.GetConnection();
            var commandText = "sp_update_room_type";
            var cmd = new SqlCommand(commandText, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@RoomTypeID", oldRoomType.RoomTypeID);
            cmd.Parameters.AddWithValue("@NewRoomPrice", newRoomType.RoomPrice);
            cmd.Parameters.AddWithValue("@NewRoomDescription", newRoomType.RoomDescription);

            cmd.Parameters.AddWithValue("@OldRoomPrice", oldRoomType.RoomPrice);
            cmd.Parameters.AddWithValue("@OldRoomDescription", oldRoomType.RoomDescription);

            try
            {
                connection.Open();

                var rows = cmd.ExecuteNonQuery();
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

        public List<Room> SelectAllRooms()
        {
            var rooms = new List<Room>();
            var connection = SqlConnectionProvider.GetConnection();
            var commandText = "sp_select_all_rooms";
            var cmd = new SqlCommand(commandText, connection);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                connection.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var room = new Room();
                        room.RoomID = reader.GetInt32(0);
                        room.RoomTypeID = reader.GetString(1);
                        room.RoomAvailability = reader.GetBoolean(2);
                        room.RoomStatus = reader.GetString(3);
                        rooms.Add(room);
                    }
                    return rooms;
                }
                else
                {
                    throw new ArgumentException("Could not select all rooms");
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

        public bool UpdateRoomAvailability(int roomID, bool newRoomAvailability, bool oldRoomAvailability)
        {
            var rows = 0;
            var connection = SqlConnectionProvider.GetConnection();
            var commandText = "sp_update_room_availability";
            var cmd = new SqlCommand(commandText, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@RoomID", SqlDbType.Int);
            cmd.Parameters.Add("@NewRoomAvailability", SqlDbType.Bit);
            cmd.Parameters.Add("@OldRoomAvailability", SqlDbType.Bit);

            cmd.Parameters["@RoomID"].Value = roomID;
            cmd.Parameters["@NewRoomAvailability"].Value = newRoomAvailability;
            cmd.Parameters["@OldRoomAvailability"].Value = oldRoomAvailability;
            try
            {
                connection.Open();

                rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                {
                    throw new ArgumentException("Failed to update the room availability");
                }

                return rows == 1;

            }catch (Exception ex)
            {
                throw ex;
            }

         }

        public List<Room> SelectRoomAvailabilityExceptReservation(int reservationID, DateTime checkIn, DateTime checkOut)
        {
            var rooms = new List<Room>();
            var connection = SqlConnectionProvider.GetConnection();
            var commandText = "sp_select_rooms_available_for_reschedule_except";
            var cmd = new SqlCommand(commandText, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@CheckIn", SqlDbType.Date);
            cmd.Parameters.Add("@CheckOut", SqlDbType.Date);
            cmd.Parameters.Add("@ReservationID", SqlDbType.Int);
            cmd.Parameters["@CheckIn"].Value = checkIn;
            cmd.Parameters["@CheckOut"].Value = checkOut;
            cmd.Parameters["@ReservationID"].Value = reservationID;
            try
            {
                connection.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var room = new Room();
                        room.RoomID = reader.GetInt32(0);
                        room.RoomTypeID = reader.GetString(1);
                        room.RoomAvailability = reader.GetBoolean(2);
                        room.RoomStatus = reader.GetString(3);
                        rooms.Add(room);
                    }
                    return rooms;
                }
                else
                {
                    throw new ArgumentException("Room not found");
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
    }
}
