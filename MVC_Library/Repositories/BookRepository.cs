using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using MVC_Library.Models;
using MVC_Library.Repositories.DAL;

namespace MVC_Library.Repositories
{
    public class BookRepository : Repository<BookModels>
    {
        public bool Create(BookModels book)
        {
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"INSERT INTO Books(Title, Year, Quantity) VALUES (@Title, @Year, @Quantity);";
                cmd.AddParameter("Title", book.Title);
                cmd.AddParameter("Year", book.Year);
                cmd.AddParameter("Quantity", book.Quantity);
                return cmd.ExecuteNonQuery() == 1;
            }
        }

        public bool Delete(int bookId)
        {
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"DELETE FROM Books WHERE BookId = @BookId;";
                cmd.AddParameter("BookId", bookId);
                return cmd.ExecuteNonQuery() == 1;
            }
        }

        public bool Edit(BookModels book)
        {
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"UPDATE Books SET Title = @Title, Year = @Year, Quantity = @Quantity WHERE BookId = @BookId;";
                cmd.AddParameter("BookId", book.Id);
                cmd.AddParameter("Title", book.Title);
                cmd.AddParameter("Year", book.Year);
                cmd.AddParameter("Quantity", book.Quantity);
                return cmd.ExecuteNonQuery() == 1;
            }
        }

        public BookModels Details(int bookId)
        {
            var book = new BookModels();
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT * FROM Books WHERE BookId = @BookId;";
                cmd.AddParameter("BookId", bookId);
                using (reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Map(reader, book);
                    }
                }
            }
            return book;
        }

        public IEnumerable<BookModels> FindBooks()
        {
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT * FROM Books";
                return ToList(cmd);
            }
        }

        public IEnumerable<BookModels> FindBooksByAuthor(int authorId)
        {
            List<int> bookIds = new List<int>();
            List<BookModels> books = new List<BookModels>();
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT BookId FROM BookAuthors WHERE AuthorId = @AuthorId;";
                cmd.AddParameter("AuthorId", authorId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        bookIds.Add((int)reader["BookId"]);
                    }
                }
            }
            foreach (var i in bookIds)
            {
                books.Add(Details(i));
            }
            return books;
        }

        public string GetBookById(int bookId)
        {
            string title = "";
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT Title FROM Books WHERE BookId = @BookId;";
                cmd.AddParameter("BookId", bookId);
                using (reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        title = (string)reader["Title"];
                    }
                }
            }
            return title;
        }

        public int GetBookByTitle(string title)
        {
            int id = -1;
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT BookId FROM Books WHERE Title = @Title;";
                cmd.AddParameter("Title", title);
                using (reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        id = (int)reader["BookId"];
                    }
                }
            }
            return id;
        }

        public bool BindAuthorToBook(int bookId, int authorId)
        {
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"INSERT INTO BookAuthors(BookId, AuthorId) VALUES (@BookId, @AuthorId);";
                cmd.AddParameter("BookId", bookId);
                cmd.AddParameter("AuthorId", authorId);
                return cmd.ExecuteNonQuery() == 1;
            }
        }

        public IEnumerable<KeyValuePair<String, DateTime>> FindTakenBooks(int userId)
        {
            List<KeyValuePair<int, DateTime>> temp = new List<KeyValuePair<int, DateTime>>();
            List<KeyValuePair<String, DateTime>> takenbooks = new List<KeyValuePair<String, DateTime>>();
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT BookId, Date FROM BooksTaken WHERE UserId = @UserId;";
                cmd.AddParameter("UserId", userId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        temp.Add(new KeyValuePair<int, DateTime>((int)reader["BookId"], (DateTime)reader["Date"]));
                    }
                }
            }
            foreach (var i in temp)
            {
                takenbooks.Add(new KeyValuePair<String,DateTime>(GetBookById(i.Key), i.Value));
            }
            return takenbooks;
        }

        public bool BindBookToUser(int bookId, string username)
        {
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"INSERT INTO BooksTaken(BookId, Date, UserId) VALUES (@BookId, @Date,
                                    (SELECT UserId FROM Users WHERE Name = @UserName));";
                cmd.AddParameter("BookId", bookId);
                cmd.AddParameter("UserName", username);
                cmd.AddParameter("Date", DateTime.Now);
                return cmd.ExecuteNonQuery() == 1;
            }
        }
        
        protected override void Map(IDataRecord record, BookModels book)
        {
            book.Id = (int)record["BookId"];
            book.Title = record["Title"].ToString();
            book.Year = record["Year"].ToString();
            book.Quantity = (int)record["Quantity"];
        }
    }
}