using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using MVC_LIbrary.Models;

namespace MVC_LIbrary.Persistence.Repositories
{
    public class BookRepository : IDisposable
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

        public bool Create(Book book)
        {
            var insertSql = new StringBuilder();
            insertSql.Append("INSERT INTO Users(Name, Email, Password) VALUES ");
            insertSql.Append("('" + book.Title + "',");
            insertSql.Append("'" + book.Year + "');");

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
                    "DELETE FROM Books WHERE Id = @Id ;",
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

        public IEnumerable<Book> GetAll()
        {
            var returnUsers = new List<Book>();

            try
            {
                var cmd = new SqlCommand("SELECT * FROM Books ;", connection);
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

        public bool Edit(Book editBook)
        {
            try
            {
                var cmd = new SqlCommand("UPDATE Books SET Title = @Title," +
                                         "Year = @Year " +
                                         "WHERE Id = @Id ;",
                                         connection);

                cmd.Parameters.Add(new SqlParameter { ParameterName = "Id", Value = editBook.Id });
                cmd.Parameters.Add(new SqlParameter { ParameterName = "Title", Value = editBook.Title });
                cmd.Parameters.Add(new SqlParameter { ParameterName = "Year", Value = editBook.Year });

                return cmd.ExecuteNonQuery() == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Book Details(int id)
        {
            try
            {
                var cmd = new SqlCommand(
                    "SELECT * FROM Books WHERE Id = @Id ;",
                    connection
                );

                cmd.Parameters.Add(new SqlParameter { ParameterName = "Id", Value = id });
                reader = cmd.ExecuteReader();

                var details = new Book();
                while (reader.Read())
                {
                    details = ParseReader(reader);
                }

                return details;
            }
            catch (Exception)
            {
                return new Book();
            }
        }

        private static Book ParseReader(IDataRecord rdr)
        {
            return new Book
            {
                Id = (int)rdr["Id"],
                Title = rdr["Title"].ToString(),
                Year = rdr["Year"].ToString(),
            };
        }
    }
}