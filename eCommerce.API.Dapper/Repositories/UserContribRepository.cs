using Dapper;
using eCommerce.API.Dapper.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper.Contrib.Extensions;

namespace eCommerce.API.Dapper.Repositories {
    public class UserContribRepository : IUserRepository {

        private IDbConnection _connection;
        private IDbTransaction _transaction;
        private string _command = "SELECT Id, Nome AS Name, Email, Sexo AS Gender, RG, CPF, "
        + "Filiacao AS Filiation, Situacao AS Situation, DataCad AS RegDate FROM Usuarios";


        public UserContribRepository() {
            _connection = new SqlConnection("Data Source=SERVIDOR\\SQLSERVER;Initial Catalog=eCommerce;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        //Dapper - MER <-> POO
        public List<User> GetUsers() {
            return _connection.GetAll<User>().ToList();
        }

        public User GetUser(int id) {
            return _connection.Get<User>(id);
        }

        public void InsertUser(User user) {
            user.Id = Convert.ToInt32(_connection.Insert(user));
        }

        public void UpdateUser(User user) {
            _connection.Update(user);
        }


        public void DeleteUser(int id) {
            _connection.Delete(GetUser(id));
        }
    }
}
