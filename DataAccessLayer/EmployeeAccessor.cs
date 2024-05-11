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
    public class EmployeeAccessor : IEmployeeAccessor
    {
        public int AuthenticateUserWithEmailAndPasswordHash(string email, string passwordHash)
        {
            int rows = 0;
            var connection = SqlConnectionProvider.GetConnection();

            var commandText = "sp_authenticate_employee";

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

        public int InsertEmployee(Employee employee)
        {
            int rows = 0;
            var connection = SqlConnectionProvider.GetConnection();

            var commandText = "sp_insert_employee";

            var cmd = new SqlCommand(commandText, connection);

            cmd.CommandType = CommandType.StoredProcedure;

            /*
              @PositionID
	        , @FirstName
	        , @LastName
	        , @Phone
	        , @Email	
             */

            cmd.Parameters.Add("@PositionID", SqlDbType.Int);
            cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 25);
            cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 25);
            cmd.Parameters.Add("@Phone", SqlDbType.NVarChar, 25);
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100);

            cmd.Parameters["@PositionID"].Value = employee.PositionID;
            cmd.Parameters["@FirstName"].Value = employee.FirstName;
            cmd.Parameters["@LastName"].Value = employee.LastName;
            cmd.Parameters["@Phone"].Value = employee.Phone;
            cmd.Parameters["@Email"].Value = employee.Email;

            try
            {
                connection.Open();

                rows = cmd.ExecuteNonQuery();
                if (rows == 0)
                {
                    throw new ArgumentException("Failed to create new employee");
                }

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

        public List<Position> SelectAllPositions()
        {
            List<Position> positions = new List<Position>();

            var connection = SqlConnectionProvider.GetConnection();

            var commandText = "sp_select_positions";

            var cmd = new SqlCommand(commandText, connection);

            try
            {
                connection.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var position = new Position(); 
                        position.PositionID = reader.GetInt32(0);
                        position.PositionTitle = reader.GetString(1);
                        position.PositionDescription = reader.GetString(2);
                        positions.Add(position);
                    }
                }
                else
                {
                    throw new ArgumentException("Positions not found");
                }
                return positions;
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

        public Employee SelectEmployeeByEmail(string email)
        {
            Employee employee = new Employee();

            var connection = SqlConnectionProvider.GetConnection();

            var commandText = "sp_select_employee_by_email";

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
                        employee.PositionID = reader.GetInt32(0);
                        employee.EmployeeID = reader.GetInt32(1);
                        employee.FirstName = reader.GetString(2);
                        employee.LastName = reader.GetString(3);
                        employee.Phone = reader.GetString(4);
                        employee.Email = reader.GetString(5);
                    }
                    return employee;
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

        public Employee SelectEmployeeByID(int employeeID)
        {
            var employee = new Employee();
            var connection = SqlConnectionProvider.GetConnection();
            var commandText = "sp_select_employee_by_id";
            var cmd = new SqlCommand(commandText, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@EmployeeID", SqlDbType.Int);
            cmd.Parameters["@EmployeeID"].Value = employeeID;
            try
            {
                connection.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        employee.EmployeeID = reader.GetInt32(0);
                        employee.PositionID = reader.GetInt32(1);
                        employee.FirstName = reader.GetString(2);
                        employee.LastName = reader.GetString(3);
                        employee.Phone = reader.GetString(4);
                        employee.Email = reader.GetString(5);
                    }
                    return employee;
                }
                else
                {
                    throw new ArgumentException("Employee not found");
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

        public bool UpdateEmployeeContactInformation(Employee newEmployee, Employee oldEmployee)
        {
            var connection = SqlConnectionProvider.GetConnection();
            var commandText = "sp_update_employee_contact_information";
            var cmd = new SqlCommand(commandText, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@EmployeeID", SqlDbType.Int);
            cmd.Parameters.Add("@NewFirstName", SqlDbType.NVarChar, 25);
            cmd.Parameters.Add("@NewLastName", SqlDbType.NVarChar, 25);
            cmd.Parameters.Add("@NewPhone", SqlDbType.NVarChar, 25);
            cmd.Parameters.Add("@NewEmail", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@OldFirstName", SqlDbType.NVarChar, 25);
            cmd.Parameters.Add("@OldLastName", SqlDbType.NVarChar, 25);
            cmd.Parameters.Add("@OldPhone", SqlDbType.NVarChar, 25);
            cmd.Parameters.Add("@OldEmail", SqlDbType.NVarChar, 100);

            cmd.Parameters["@EmployeeID"].Value = oldEmployee.EmployeeID;
            cmd.Parameters["@NewFirstName"].Value = newEmployee.FirstName;
            cmd.Parameters["@NewLastName"].Value = newEmployee.LastName;
            cmd.Parameters["@NewPhone"].Value = newEmployee.Phone;
            cmd.Parameters["@NewEmail"].Value = newEmployee.Email;

            cmd.Parameters["@OldFirstName"].Value = oldEmployee.FirstName;
            cmd.Parameters["@OldLastName"].Value = oldEmployee.LastName;
            cmd.Parameters["@OldPhone"].Value = oldEmployee.Phone;
            cmd.Parameters["@OldEmail"].Value = oldEmployee.Email;

            try
            {
                connection.Open();

                var rows = cmd.ExecuteNonQuery();
                if (rows == 0)
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

        public int UpdatePasswordHash(int employeeID, string oldPasswordHash, string newPasswordHash)
        {

            int rows = 0;
            var connection = SqlConnectionProvider.GetConnection();

            var commandText = "sp_update_passwordhash";

            var cmd = new SqlCommand(commandText, connection);

            cmd.CommandType = CommandType.StoredProcedure;


            cmd.Parameters.Add("@EmployeeID", SqlDbType.Int);
            cmd.Parameters.Add("@NewPasswordHash", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@OldPasswordHash", SqlDbType.NVarChar, 100);

            cmd.Parameters["@EmployeeID"].Value = employeeID;
            cmd.Parameters["@OldPasswordHash"].Value = oldPasswordHash;
            cmd.Parameters["@NewPasswordHash"].Value = newPasswordHash;

            try
            {
                connection.Open();

                rows = cmd.ExecuteNonQuery();
                if (rows == 0)
                {
                    throw new ArgumentException("Bad email or password");
                }

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
    }
}
