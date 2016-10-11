using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using MVC_Library.Models;
using MVC_Library.Repositories.DAL;

namespace MVC_Library.Repositories
{
    public class UserRepository : Repository<UserModels>
    {
        public bool IsValid(UserModels user)
        {
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT[Name] FROM[Users] WHERE[Name] = @Name AND[Password] = @Password;";
                cmd.AddParameter("Name", user.Name);
                cmd.AddParameter("Password", user.Password);
                return cmd.ExecuteReader().HasRows;
            }
        }

        public bool Create(UserModels user)
        {
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"INSERT INTO Users(Name, Email, Password) VALUES(@Name, @Email, @Password);";
                cmd.AddParameter("Name", user.Name);
                cmd.AddParameter("Email", user.Email);
                cmd.AddParameter("Password", user.Password);
                return cmd.ExecuteNonQuery() == 1;
            }
        }

        public bool Delete(int userId)
        {
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"DELETE FROM Users WHERE UserId = @UserId;";
                cmd.AddParameter("UserId", userId);
                return cmd.ExecuteNonQuery() == 1;
            }
        }

        public IEnumerable<UserModels> GetAll()
        {
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT * FROM Users";
                return ToList(cmd);
            }
        }

        public bool Edit(UserModels user)
        {
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"UPDATE Users SET Name = @Name, Email = @Email, Password = @Password WHERE UserId = @UserId;";
                cmd.AddParameter("@UserId", user.Id);
                cmd.AddParameter("@Name", user.Name);
                cmd.AddParameter("@Email", user.Email);
                cmd.AddParameter("@Password", user.Password);
                return cmd.ExecuteNonQuery() == 1;
            }
        }

        public UserModels Details(int userId)
        {
            var user = new UserModels();
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT * FROM Users WHERE UserId = @UserId;";
                cmd.AddParameter("UserId", userId);
                using (reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Map(reader, user);
                    }
                }
            }
            return user;
        }

        public UserModels Details(string userName)
        {
            var user = new UserModels();
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT * FROM Users WHERE Name = @Name;";
                cmd.AddParameter("Name", userName);
                using (reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Map(reader, user);
                    }
                }
            }
            return user;
        }

        

        protected override void Map(IDataRecord record, UserModels user)
        {
            user.Id = (int)record["UserId"];
            user.Name = record["Name"].ToString();
            user.Email = record["Email"].ToString();
            user.Password = record["Password"].ToString();
        }
    }
}