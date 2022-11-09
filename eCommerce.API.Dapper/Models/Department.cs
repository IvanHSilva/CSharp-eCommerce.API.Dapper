using System.Collections.Generic;

namespace eCommerce.API.Dapper.Models {
    public class Department {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }

        public Department() { }

        public Department(int id, string name) {
            Id = id;
            Name = name;
        }
    }
}
