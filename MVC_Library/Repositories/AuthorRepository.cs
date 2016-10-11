using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using MVC_Library.Repositories;
using MVC_Library.Repositories.DAL;
using MVC_Library.Models;
using System.Diagnostics;

namespace MVC_Library.Repositories
{
    public class AuthorRepository : Repository<AuthorModels>
    {
        public bool Create(AuthorModels author)
        {
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"INSERT INTO Authors(Name, Country) VALUES (@Name, @Country);";
                cmd.AddParameter("Name", author.Name);
                cmd.AddParameter("Country", author.Country);
                return cmd.ExecuteNonQuery() == 1;
            }
        }

        public bool Delete(int authorId)
        {
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"DELETE FROM Authors WHERE AuthorId = @AuthorId;";
                cmd.AddParameter("AuthorId", authorId);
                return cmd.ExecuteNonQuery() == 1;
            }
        }

        public bool Edit(AuthorModels author)
        {
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"UPDATE Authors SET Name = @Name, Country = @Country WHERE AuthorId = @AuthorId;";
                cmd.AddParameter("AuthorId", author.Id);
                cmd.AddParameter("Name", author.Name);
                cmd.AddParameter("Country", author.Country);
                return cmd.ExecuteNonQuery() == 1;
            }
        }

        public AuthorModels Details(int authorId)
        {
            var author = new AuthorModels();
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT * FROM Authors WHERE AuthorId = @AuthorId;";
                cmd.AddParameter("AuthorId", authorId);
                using (reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Map(reader, author);
                    }
                }
            }
            return author;
        }

        public IEnumerable<AuthorModels> GetAll()
        {
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT * FROM Authors";
                return ToList(cmd);
            }
        }

        public IEnumerable<AuthorModels> FindAuthorsByBook(int bookId)
        {
            List<int> AuthorIds = new List<int>();
            List<AuthorModels> authors = new List<AuthorModels>();
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT AuthorId FROM BookAuthors WHERE BookId = @BookId;";
                cmd.Parameters.Add(new SqlParameter { ParameterName = "BookId", Value = bookId });
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        AuthorIds.Add((int)reader["AuthorId"]);
                    }
                }
            }
            foreach (var i in AuthorIds)
            {
                authors.Add(Details(i));
            }
            return authors;
        }

        protected override void Map(IDataRecord record, AuthorModels author)
        {
            author.Id = (int)record["AuthorId"];
            author.Name = record["Name"].ToString();
            author.Country = record["Country"].ToString();
        }
    }
}