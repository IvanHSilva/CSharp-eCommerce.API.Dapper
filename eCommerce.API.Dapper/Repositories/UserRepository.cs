using eCommerce.API.Dapper.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace eCommerce.API.Dapper.Repositories {
    public class UserRepository : IUserRepository {

        private IDbConnection _connection;
        public UserRepository() {

            _connection = new SqlConnection("Data Source=SERVIDOR\\SQLSERVER;Initial Catalog=eCommerce;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        private static List<User> _dbUsers = new List<User>() {
            new User(1, "José Rodrigues", "jrodrigues@gmail.com"),
            new User(2, "Maria Teresa", "marite@gmail.com"),
            new User(3, "Ronaldo Amaral", "roamaral@gmail.com"),
            new User(4, "Ana Clarice Mendes", "anacmendes@gmail.com"),
            new User(5, "Xavier Oliveria", "xaoliver@gmail.com"),
        };

        public List<User> GetUsers() {
            return _dbUsers;
        }

        public User GetUser(int id) {
            return _dbUsers.FirstOrDefault(u => u.Id == id);
        }

        public void InsertUser(User user) {
            User lastUser = _dbUsers.LastOrDefault();
            if (lastUser == null) user.Id = 1; else user.Id = lastUser.Id + 1;
            _dbUsers.Add(user);
        }

        public void UpdateUser(User user) {
            _dbUsers.Remove(_dbUsers.FirstOrDefault(u => u.Id == user.Id));
            _dbUsers.Add(user);
        }

        public void DeleteUser(int id) {
            _dbUsers.Remove(_dbUsers.FirstOrDefault(u => u.Id == id));
        }

    }
}
