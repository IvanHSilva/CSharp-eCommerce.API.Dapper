using Dapper;
using eCommerce.API.Dapper.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

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
            _command = "SELECT U.Id, Nome AS Name, Email, Sexo AS Gender, RG, CPF, Filiacao AS Filiation, ";
            _command += "Situacao AS Situation, DataCad AS RegDate, C.Id AS ContId, C.UsuId, C.Telefone AS Phone, ";
            _command += "C.Celular AS CellPhone FROM Usuarios AS U ";
            _command += "LEFT JOIN Contatos AS C ON C.UsuId = U.Id ";
            _command += " WHERE U.Id = @Id";
            return _connection.Query<User, Contact, User>(_command,
            (user, contact) => {
                user.Contact = contact;
                return user;
            },
                new { Id = id }, splitOn: "Id, UsuId").SingleOrDefault();
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
            _command = "DELETE FROM Usuarios  WHERE Id = @Id";
            _connection.Execute(_command, new { Id = id });
        }
    }
}
