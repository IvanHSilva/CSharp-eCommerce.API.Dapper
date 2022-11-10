using eCommerce.API.Dapper.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace eCommerce.API.Dapper.Repositories {
    public class UserRepository : IUserRepository {

        private IDbConnection _connection;
        private string _command = "SELECT Id, Nome AS Name, Email, Sexo AS Gender, RG, CPF, "
        + "Filiacao AS Filiation, Situacao AS Situation, DataCad AS RegDate FROM Usuarios";


        public UserRepository() {
            _connection = new SqlConnection("Data Source=SERVIDOR\\SQLSERVER;Initial Catalog=eCommerce;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        //Dapper - MER <-> POO
        public List<User> GetUsers() {
            return _connection.Query<User>(_command).ToList();
        }

        public User GetUser(int id) {
            _command += " WHERE Id = @Id";
            return _connection.QueryFirstOrDefault<User>(_command, new { Id = id });
        }

        public void InsertUser(User user) {
            _command = "INSERT INTO Usuarios (Nome, EMail, Sexo, RG, CPF, Filiacao, Situacao, DataCad) ";
            _command += "VALUES (@Name, @EMail, @Gender, @RG, @CPF, @Filiation, @Situation, @RegDate); ";
            _command += "SELECT CAST(SCOPE_IDENTITY() AS INT);";
            user.Id = _connection.Query<int>(_command, user).Single();
        }

        public void UpdateUser(User user) {
            _command = "UPDATE Usuarios SET Nome = @Name, EMail = @EMail, Sexo = @Gender, RG = @RG, CPF = @CPF, ";
            _command += "Filiacao = @Filiation, Situacao = @Situation, DataCad = @RegDate WHERE Id = @Id";
            _connection.Execute(_command, user);
        }

        public void DeleteUser(int id) {
            _dbUsers.Remove(_dbUsers.FirstOrDefault(u => u.Id == id));
        }

        private static List<User> _dbUsers = new List<User>() {
            new User(1, "José Rodrigues", "jrodrigues@gmail.com"),
            new User(2, "Maria Teresa", "marite@gmail.com"),
            new User(3, "Ronaldo Amaral", "roamaral@gmail.com"),
            new User(4, "Ana Clarice Mendes", "anacmendes@gmail.com"),
            new User(5, "Xavier Oliveria", "xaoliver@gmail.com"),
        };
    }
}
