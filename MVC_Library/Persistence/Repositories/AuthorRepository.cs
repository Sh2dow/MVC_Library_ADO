using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using MVC_LIbrary.Models;

namespace MVC_LIbrary.Persistence.Repositories
{
    public class AuthorRepository  : IDisposable
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

        public bool Create(Author author)
        {
            var insertSql = new StringBuilder();
            insertSql.Append("INSERT INTO Authors(Name, Country) VALUES ");
            insertSql.Append("('" + author.Name + "',");
            insertSql.Append("'" + author.Country + "');");

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
                    "DELETE FROM Authors WHERE Id = @Id ;",
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

        public IEnumerable<Author> GetAll()
        {
            var returnAuthors = new List<Author>();

            try
            {
                var cmd = new SqlCommand("SELECT * FROM Authors ;", connection);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    returnAuthors.Add(ParseReader(reader));
                }

                return returnAuthors;
            }
            catch (Exception)
            {
                return returnAuthors;
            }
        }

        public bool Edit(Author editAuthor)
        {
            try
            {
                var cmd = new SqlCommand("UPDATE Authors SET Name = @Name," +
                                         "Country = @Country " +
                                         "WHERE Id = @Id ;",
                                         connection);

                cmd.Parameters.Add(new SqlParameter { ParameterName = "Id", Value = editAuthor.Id });
                cmd.Parameters.Add(new SqlParameter { ParameterName = "Name", Value = editAuthor.Name });
                cmd.Parameters.Add(new SqlParameter { ParameterName = "Country", Value = editAuthor.Country });

                return cmd.ExecuteNonQuery() == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Author Details(int id)
        {
            try
            {
                var cmd = new SqlCommand(
                    "SELECT * FROM Authors WHERE Id = @Id ;",
                    connection
                );

                cmd.Parameters.Add(new SqlParameter { ParameterName = "Id", Value = id });
                reader = cmd.ExecuteReader();

                var details = new Author();
                while (reader.Read())
                {
                    details = ParseReader(reader);
                }

                return details;
            }
            catch (Exception)
            {
                return new Author();
            }
        }

        private static Author ParseReader(IDataRecord rdr)
        {
            return new Author
            {
                Id = (int)rdr["Id"],
                Name = rdr["Name"].ToString(),
                Country = rdr["Country"].ToString(),
            };
        }
    }
}