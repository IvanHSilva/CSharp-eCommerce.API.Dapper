using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.API.Dapper.Models {
    public class User {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public User() {}

        public User(int id, string name, string email) {
            Id = id;
            Name = name;
            Email = email;
        }
    }
}
