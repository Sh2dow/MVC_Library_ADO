using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MVC_Library.Repositories;
using System.Web.Mvc;

namespace MVC_Library.Models
{
    public class BookModels
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Year { get; set; }
        public int Quantity { get; set; }

        public List<AuthorModels> AuthorList { get; set; }
        public List<int> SelectedAuthors { get; set; }

        public List<BookModels> Books { get; set; }

        public BookModels()
        {
            SelectedAuthors = new List<int>();
            AuthorList = new List<AuthorModels>();
        }
    }
}