using eCommerce.API.Dapper.Models;
using System.Collections.Generic;
using System.Linq;

namespace eCommerce.API.Dapper.Repositories {
    public class UserRepository : IUserRepository {

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
