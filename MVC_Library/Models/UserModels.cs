using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using MVC_Library.Repositories;

namespace MVC_Library.Models
{
    public class UserModels
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public String Password { get; set; }
        public bool RememberMe { get; set; }

        public List<KeyValuePair<String, DateTime>> BooksTaken { get; set; }

        public UserModels()
        {
            BooksTaken = new List<KeyValuePair<String, DateTime>>();
        }
    }
}
