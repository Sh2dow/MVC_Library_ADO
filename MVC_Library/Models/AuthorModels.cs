using MVC_Library.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVC_Library.Models
{
    public class AuthorModels
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }

        public List<BookModels> BookList { get; set; }

        public List<AuthorModels> Authors { get; set; }

        public AuthorModels()
        {
            BookList = new List<BookModels>();
        }

        //public override string ToString()
        //{
        //    return Name.ToString();
        //}
    }
}