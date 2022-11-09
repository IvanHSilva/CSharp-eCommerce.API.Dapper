namespace eCommerce.API.Dapper.Models {
    public class Contact {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Phone { get; set; }
        public string CellPhone { get; set; }

        public User User { get; set; }

        public Contact() { }

        public Contact(int id, int userId, string phone, string cellPhone) {
            Id = id;
            UserId = userId;
            Phone = phone;
            CellPhone = cellPhone;
        }
    }
}
