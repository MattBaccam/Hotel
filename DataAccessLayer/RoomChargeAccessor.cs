using DataAccessInterfaces;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class RoomChargeAccessor : IRoomChargeAccessor
    {
        public RoomCharge SelectRoomCharge(int reservationID)
        {
            var roomCharge = new RoomCharge();

            var connection = SqlConnectionProvider.GetConnection();

            var commandText = "sp_select_room_charge";

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
                        roomCharge.RoomChargeID = reader.GetInt32(0);
                        roomCharge.ReservationID = reader.GetInt32(1);
                    }
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
            return roomCharge;
        }

        public List<RoomChargeItem> SelectRoomChargeItems(int roomChargeID)
        {
            var roomChargeItems = new List<RoomChargeItem>();

            var connection = SqlConnectionProvider.GetConnection();

            var commandText = "sp_select_room_charge_items";

            var cmd = new SqlCommand(commandText, connection);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@RoomChargeID", SqlDbType.Int);

            cmd.Parameters["@RoomChargeID"].Value = roomChargeID;

            try
            {
                connection.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var roomChargeItem = new RoomChargeItem();
                        roomChargeItem.RoomChargeItemID = reader.GetInt32(0);
                        roomChargeItem.RoomChargeID = reader.GetInt32(1);
                        roomChargeItem.PantryID = reader.GetString(2);
                        roomChargeItem.ItemAmount = reader.GetInt32(3);
                        roomChargeItem.Active = reader.GetBoolean(4);
                        roomChargeItems.Add(roomChargeItem);
                    }
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
            return roomChargeItems;
        }

        public bool AuthenticateRoomCharges(int reservationID)
        {
            var connection = SqlConnectionProvider.GetConnection();

            var commandText = "sp_authenticate_room_charges";

            var cmd = new SqlCommand(commandText, connection);

            cmd.CommandType = CommandType.StoredProcedure;


            cmd.Parameters.Add("@ReservationID", SqlDbType.Int);

            cmd.Parameters["@ReservationID"].Value = reservationID;

            try
            {
                connection.Open();

                var reader = cmd.ExecuteReader();
                return reader.HasRows;
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

        public Pantry SelectPantryItem(string pantryID)
        {
            var pantry = new Pantry();

            var connection = SqlConnectionProvider.GetConnection();

            var commandText = "sp_select_pantry_item";

            var cmd = new SqlCommand(commandText, connection);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@PantryID", SqlDbType.NVarChar, 50);

            cmd.Parameters["@PantryID"].Value = pantryID;

            try
            {
                connection.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        pantry.PantryID = reader.GetString(0);
                        pantry.ItemPrice = reader.GetInt32(1);
                        pantry.ItemAmount = reader.GetInt32(2);
                    }
                }
                else
                {
                    throw new ArgumentException("Pantry item not found");
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
            return pantry;
        }

        public bool CreateRoomCharge(int reservationID)
        {

            int rows = 0;

            var connection = SqlConnectionProvider.GetConnection();

            var commandText = "sp_create_room_charge";

            var cmd = new SqlCommand(commandText, connection);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@ReservationID", SqlDbType.Int);

            cmd.Parameters["@ReservationID"].Value = reservationID;

            try
            {
                connection.Open();

                var reader = cmd.ExecuteNonQuery();
                return rows > 0;
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

        public bool InsertRoomChargeItems(int roomCharge, Pantry pantry)
        {
            var connection = SqlConnectionProvider.GetConnection();

            var commandText = "sp_insert_room_charge_items";

            var cmd = new SqlCommand(commandText, connection);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@RoomChargeID", SqlDbType.Int);
            cmd.Parameters.Add("@PantryID", SqlDbType.NVarChar, 50);
            cmd.Parameters.Add("@ItemAmount", SqlDbType.Int);

            cmd.Parameters["@RoomChargeID"].Value = roomCharge;
            cmd.Parameters["@PantryID"].Value = pantry.PantryID;
            cmd.Parameters["@ItemAmount"].Value = pantry.ItemAmount;

            try
            {
                connection.Open();

                var reader = cmd.ExecuteNonQuery();
                return reader > 0;
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

        public bool UpdatePantryQuantity(string pantryID, int newItemAmount, int oldItemAmount)
        {
            var connection = SqlConnectionProvider.GetConnection();

            var commandText = "sp_update_pantry_quantity";

            var cmd = new SqlCommand(commandText, connection);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@PantryID", SqlDbType.NVarChar, 50);
            cmd.Parameters.Add("@NewItemAmount", SqlDbType.Int);
            cmd.Parameters.Add("@OldItemAmount", SqlDbType.Int);

            cmd.Parameters["@PantryID"].Value = pantryID;
            cmd.Parameters["@NewItemAmount"].Value = newItemAmount;
            cmd.Parameters["@OldItemAmount"].Value = oldItemAmount;

            try
            {
                connection.Open();

                var reader = cmd.ExecuteNonQuery();
                return reader > 0;
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

        public List<SalesLog> SelectSalesLogs()
        {
            var salesLogList = new List<SalesLog>();

            var connection = SqlConnectionProvider.GetConnection();

            var commandText = "sp_select_saleslog";

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
                        var salesLog = new SalesLog();
                        salesLog.LogID = reader.GetInt32(0);
                        salesLog.EmployeeID = reader.GetInt32(1);
                        salesLog.GuestID = reader.GetInt32(2);
                        salesLog.TimeOfSale = reader.GetDateTime(3);
                        salesLog.PantryID = reader.GetString(4);
                        salesLog.SoldPrice = reader.GetInt32(5);
                        salesLog.ItemAmount = reader.GetInt32(6);
                        salesLogList.Add(salesLog);
                    }
                }
                else
                {
                    throw new ArgumentException("Sales log items not found");
                }
                return salesLogList;

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

        public bool InsertSalesLogs(SalesLog salesLog)
        {
            var connection = SqlConnectionProvider.GetConnection();

            var commandText = "sp_insert_into_saleslog";

            var cmd = new SqlCommand(commandText, connection);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@EmployeeID", SqlDbType.Int);
            cmd.Parameters.Add("@GuestID", SqlDbType.Int);
            cmd.Parameters.Add("@TimeOfSale", SqlDbType.DateTime);
            cmd.Parameters.Add("@PantryID", SqlDbType.NVarChar, 50);
            cmd.Parameters.Add("@SoldPrice", SqlDbType.Int);
            cmd.Parameters.Add("@ItemAmount", SqlDbType.Int);

            cmd.Parameters["@EmployeeID"].Value = salesLog.EmployeeID;
            cmd.Parameters["@GuestID"].Value = salesLog.GuestID;
            cmd.Parameters["@TimeOfSale"].Value = salesLog.TimeOfSale;
            cmd.Parameters["@PantryID"].Value = salesLog.PantryID;
            cmd.Parameters["@SoldPrice"].Value = salesLog.SoldPrice;
            cmd.Parameters["@ItemAmount"].Value = salesLog.ItemAmount;

            try
            {
                connection.Open();

                var reader = cmd.ExecuteNonQuery();
                return reader > 0;
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

        public bool UpdateRoomChargeItemForRefund(int roomChargeItemID, int roomChargeID, bool newActive, bool oldActive)
        {
            var connection = SqlConnectionProvider.GetConnection();

            var commandText = "sp_update_room_charge_items_for_refund";

            var cmd = new SqlCommand(commandText, connection);

            cmd.CommandType = CommandType.StoredProcedure;


            cmd.Parameters.Add("@RoomChargeItemID", SqlDbType.Int);
            cmd.Parameters.Add("@RoomChargeID", SqlDbType.Int);
            cmd.Parameters.Add("@NewActive", SqlDbType.Bit);
            cmd.Parameters.Add("@OldActive", SqlDbType.Bit);

            cmd.Parameters["@RoomChargeItemID"].Value = roomChargeItemID;
            cmd.Parameters["@RoomChargeID"].Value = roomChargeID;
            cmd.Parameters["@NewActive"].Value = newActive;
            cmd.Parameters["@OldActive"].Value = oldActive;

            try
            {
                connection.Open();

                var rows = cmd.ExecuteNonQuery();

                return rows > 0;
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

        public List<RoomChargeItem> SelectActiveRoomChargeItems(int roomChargeID)
        {
            var roomChargeItems = new List<RoomChargeItem>();

            var connection = SqlConnectionProvider.GetConnection();

            var commandText = "sp_select_active_room_charge_items";

            var cmd = new SqlCommand(commandText, connection);
            cmd.Parameters.Add("@RoomChargeID", SqlDbType.Int);
            cmd.Parameters["@RoomChargeID"].Value = roomChargeID;

            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                connection.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var roomChargeItem = new RoomChargeItem();
                        roomChargeItem.RoomChargeItemID = reader.GetInt32(0);
                        roomChargeItem.RoomChargeID = reader.GetInt32(1);
                        roomChargeItem.PantryID = reader.GetString(2);
                        roomChargeItem.ItemAmount = reader.GetInt32(3);
                        roomChargeItem.Active = reader.GetBoolean(4);
                        roomChargeItems.Add(roomChargeItem);
                    }
                }
                else
                {
                    throw new ArgumentException("Room charges not found");
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
            return roomChargeItems;
        }

        public bool UpdatePantryQuantityAndPrice(Pantry newRoomCharge, Pantry oldRoomCharge)
        {
            var connection = SqlConnectionProvider.GetConnection();
            var commandText = "sp_update_pantry_quantity_and_price";
            var cmd = new SqlCommand(commandText, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PantryID", SqlDbType.NVarChar, 9);
            cmd.Parameters.Add("@NewItemAmount", SqlDbType.Int);
            cmd.Parameters.Add("@NewItemPrice", SqlDbType.Int);
            cmd.Parameters.Add("@OldItemAmount", SqlDbType.Int);
            cmd.Parameters.Add("@OldItemPrice", SqlDbType.Int);

            cmd.Parameters["@PantryID"].Value = oldRoomCharge.PantryID;
            cmd.Parameters["@NewItemAmount"].Value = newRoomCharge.ItemAmount;
            cmd.Parameters["@NewItemPrice"].Value = newRoomCharge.ItemPrice;
            cmd.Parameters["@OldItemAmount"].Value = oldRoomCharge.ItemAmount;
            cmd.Parameters["@OldItemPrice"].Value = oldRoomCharge.ItemPrice;

            try
            {
                connection.Open();

                var rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                {
                    throw new ArgumentException("Failed to update the pantry");
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

        public List<Pantry> SelectPantryItems()
        {
            var pantryItems = new List<Pantry>();

            var connection = SqlConnectionProvider.GetConnection();

            var commandText = "sp_select_pantry_items";

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
                        var pantry = new Pantry();
                        pantry.PantryID = reader.GetString(0);
                        pantry.ItemPrice = reader.GetInt32(1);
                        pantry.ItemAmount = reader.GetInt32(2);
                        pantryItems.Add(pantry);
                    }
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
            return pantryItems;
        }
    }
}
