using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using MVC_LIbrary.Persistence;

namespace MVC_LIbrary.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public String Password { get; set; }
        public bool RememberMe { get; set; }

        public List<Book> Books;

        //public bool IsValid(string _email, string _password)
        //{
        //    using (var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        //    {
        //        string _sql = @"SELECT [Name] FROM [Users] WHERE [Email] = @e AND [Password] = @p";
        //        var cmd = new SqlCommand(_sql, sqlConnection);
        //        cmd.Parameters
        //            .Add(new SqlParameter("@e", SqlDbType.NVarChar))
        //            .Value = _email;
        //        cmd.Parameters
        //            .Add(new SqlParameter("@p", SqlDbType.NVarChar))
        //            .Value = _password;
        //        sqlConnection.Open();
        //        var reader = cmd.ExecuteReader();
        //        if (reader.HasRows)
        //        {
        //            reader.Dispose();
        //            cmd.Dispose();
        //            return true;
        //        }
        //        else
        //        {
        //            reader.Dispose();
        //            cmd.Dispose();
        //            return false;
        //        }
        //    }
        //}
    }
}
