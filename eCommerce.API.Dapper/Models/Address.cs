namespace eCommerce.API.Dapper.Models {
    public class Address {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string Comp { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        public User User { get; set; }

        public Address() { }

        public Address(int id, int userId, string description, string street, string number, string comp, string district, string city, string state, string zipCode) {
            Id = id;
            UserId = userId;
            Description = description;
            Street = street;
            Number = number;
            Comp = comp;
            District = district;
            City = city;
            State = state;
            ZipCode = zipCode;
        }
    }
}
