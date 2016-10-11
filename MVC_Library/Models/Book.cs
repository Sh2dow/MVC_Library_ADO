using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVC_LIbrary.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Year { get; set; }

        internal List<Book> Books = new List<Book>();
    }
}