using Dapper;
using eCommerce.API.Dapper.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace eCommerce.API.Dapper.Repositories {
    public class UserRepository : IUserRepository {

        private IDbConnection _connection;
        private IDbTransaction _transaction;
        private string _command = "SELECT Id, Nome AS Name, Email, Sexo AS Gender, RG, CPF, "
        + "Filiacao AS Filiation, Situacao AS Situation, DataCad AS RegDate FROM Usuarios";


        public UserRepository() {
            _connection = new SqlConnection("Data Source=SERVIDOR\\SQLSERVER;Initial Catalog=eCommerce;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        //Dapper - MER <-> POO
        public List<User> GetUsers() {
            //return _connection.Query<User>(_command).ToList();
            List<User> users = new List<User>();

            _command = "SELECT U.Id, Nome AS Name, Email, Sexo AS Gender, RG, CPF, Filiacao AS Filiation, ";
            _command += "Situacao AS Situation, DataCad AS RegDate, C.Id, C.UsuId AS UserId, C.Telefone AS Phone, ";
            _command += "C.Celular AS CellPhone, E.Id, E.UsuId AS UserId, E.Descricao AS Description, E.Endereco AS Street, E.Numero AS Number, ";
            _command += "E.Complemento AS Comp, E.Bairro AS District, E.Cidade AS City, E.Estado as State, E.CEP AS ZipCode ";
            _command += "FROM Usuarios AS U ";
            _command += "LEFT JOIN Contatos AS C ON C.UsuId = U.Id ";
            _command += "LEFT JOIN Enderecos AS E ON E.UsuId = U.Id";
            //_command += " WHERE U.Id = @Id";

            _connection.Query<User, Contact, Address, User>(_command,
                (user, contact, address) => {
                    if (users.SingleOrDefault(u => u.Id == user.Id) == null) {
                        user.Addresses = new List<Address>();
                        user.Contact = contact;
                        users.Add(user);
                    } else {
                        user = users.SingleOrDefault(u => u.Id == user.Id);
                    }
                    user.Addresses.Add(address);
                    return user;
                }, splitOn: "UserId, Id, Id");
            return users;
        }
        public User GetUser(int id) {
            _command = "SELECT U.Id, Nome AS Name, Email, Sexo AS Gender, RG, CPF, Filiacao AS Filiation, ";
            _command += "Situacao AS Situation, DataCad AS RegDate, C.Id, C.UsuId AS UserId, C.Telefone AS Phone, ";
            _command += "C.Celular AS CellPhone FROM Usuarios AS U ";
            _command += "LEFT JOIN Contatos AS C ON C.UsuId = U.Id ";
            _command += " WHERE U.Id = @Id";
            return _connection.Query<User, Contact, User>(_command,
            map: (user, contact) => {
                user.Contact = contact;
                return user;
            },
            param: new { id }, splitOn: "UserId, Id").SingleOrDefault();
        }

        public void InsertUser(User user) {

            _connection.Open();
            _transaction = _connection.BeginTransaction();
            try {
                _command = "INSERT INTO Usuarios (Nome, EMail, Sexo, RG, CPF, Filiacao, Situacao, DataCad) ";
                _command += "VALUES (@Name, @EMail, @Gender, @RG, @CPF, @Filiation, @Situation, @RegDate); ";
                _command += "SELECT CAST(SCOPE_IDENTITY() AS INT);";
                user.Id = _connection.Query<int>(_command, user, _transaction).Single();

                if (user.Contact != null) {
                    user.Contact.UserId = user.Id;
                    _command = "INSERT INTO Contatos (UsuId, Telefone, Celular) ";
                    _command += "VALUES (@UserId, @Phone, @CellPhone); ";
                    _command += "SELECT CAST(SCOPE_IDENTITY() AS INT);";
                    user.Contact.Id = _connection.Query<int>(_command, user.Contact, _transaction).Single();
                }

                _transaction.Commit();
            } catch (Exception e) {
                string error = e.Message;
                _transaction.Rollback();
            } finally {
                _connection.Close();
            }
        }

        public void UpdateUser(User user) {

            _connection.Open();
            _transaction = _connection.BeginTransaction();
            try {

                _command = "UPDATE Usuarios SET Nome = @Name, EMail = @EMail, Sexo = @Gender, RG = @RG, CPF = @CPF, ";
                _command += "Filiacao = @Filiation, Situacao = @Situation, DataCad = @RegDate WHERE Id = @Id";
                _connection.Execute(_command, user, _transaction);

                if (user.Contact != null) {
                    _command = "UPDATE Contatos SET Telefone = @Phone, Celular = @CellPhone WHERE UsuId = @UserId";
                    _connection.Execute(_command, user.Contact, _transaction);
                }

                _transaction.Commit();
            } catch (Exception e) {
                string error = e.Message;
                _transaction.Rollback();
            } finally {
                _connection.Close();
            }
        }


        public void DeleteUser(int id) {
            _command = "DELETE FROM Usuarios  WHERE Id = @Id";
            _connection.Execute(_command, new { Id = id });
        }
    }
}
