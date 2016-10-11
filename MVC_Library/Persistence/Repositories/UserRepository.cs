using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using MVC_LIbrary.Models;

namespace MVC_LIbrary.Persistence.Repositories
{
    public class UserRepository : IDisposable
    {
        private readonly SqlConnection connection = Database.GetConnection();
        private SqlDataReader reader;

        public void Dispose()
        {
            if (reader != null)
            {
                reader.Close();
            }
            if (connection != null)
            {
                connection.Close();
            }
        }

        public bool IsValid(User user)
        {
            string _sql = @"SELECT [Name] FROM [Users] WHERE [Email] = @e AND [Password] = @p";
            try
            {
                var cmd = new SqlCommand(_sql.ToString(), connection);
                cmd.Parameters
                    .Add(new SqlParameter("@e", SqlDbType.NVarChar))
                    .Value = user.Email;
                cmd.Parameters
                    .Add(new SqlParameter("@p", SqlDbType.NVarChar))
                    .Value = user.Password;
                return cmd.ExecuteReader().HasRows;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Create(User user)
        {
            var insertSql = new StringBuilder();
            insertSql.Append("INSERT INTO Users(Name, Email, Password) VALUES ");
            insertSql.Append("('" + user.Name + "',");
            insertSql.Append("'" + user.Email + "',");
            insertSql.Append("'" + user.Password + "');");

            try
            {
                var cmd = new SqlCommand(insertSql.ToString(), connection);
                return cmd.ExecuteNonQuery() == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var cmd = new SqlCommand(
                    "DELETE FROM Users WHERE Id = @Id ;",
                    connection
                );

                cmd.Parameters.Add(new SqlParameter("Id", id));
                reader = cmd.ExecuteReader();

                return cmd.ExecuteNonQuery() == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<User> GetAll()
        {
            var returnUsers = new List<User>();

            try
            {
                var cmd = new SqlCommand("SELECT * FROM Users ;", connection);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    returnUsers.Add(ParseReader(reader));
                }

                return returnUsers;
            }
            catch (Exception)
            {
                return returnUsers;
            }
        }

        public bool Edit(User editUser)
        {
            try
            {
                var cmd = new SqlCommand("UPDATE Users SET Name = @Name," +
                                         "Email = @Email, Password = @Password " +
                                         "WHERE Id = @Id ;",
                                         connection);

                cmd.Parameters.Add(new SqlParameter { ParameterName = "Id", Value = editUser.Id });
                cmd.Parameters.Add(new SqlParameter { ParameterName = "Name", Value = editUser.Name });
                cmd.Parameters.Add(new SqlParameter { ParameterName = "Email", Value = editUser.Email });
                cmd.Parameters.Add(new SqlParameter { ParameterName = "Password", Value = editUser.Password});

                return cmd.ExecuteNonQuery() == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public User Details(int id)
        {
            try
            {
                var cmd = new SqlCommand(
                    "SELECT * FROM Users WHERE Id = @Id ;",
                    connection
                );

                cmd.Parameters.Add(new SqlParameter { ParameterName = "Id", Value = id });
                reader = cmd.ExecuteReader();

                var details = new User();
                while (reader.Read())
                {
                    details = ParseReader(reader);
                }

                return details;
            }
            catch (Exception)
            {
                return new User();
            }
        }

        private static User ParseReader(IDataRecord rdr)
        {
            return new User
            {
                Id = (int)rdr["Id"],
                Name = rdr["Name"].ToString(),
                Email = rdr["Email"].ToString(),
                Password = rdr["Password"].ToString(),
            };
        }
    }
}