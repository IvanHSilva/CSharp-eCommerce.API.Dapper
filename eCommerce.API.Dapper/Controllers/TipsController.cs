using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using static Dapper.SqlMapper;
using eCommerce.API.Dapper.Models;
using System.Linq;
using System.Collections.Generic;

namespace eCommerce.API.Dapper.Controllers {
    
    [Route("api/[controller]")]
    [ApiController]
    
    public class TipsController : ControllerBase {
    
        private IDbConnection _connection;
        private IDbTransaction _transaction;

        private string _command = "";

        public TipsController() {
            _connection = new SqlConnection("Data Source=SERVIDOR\\SQLSERVER;Initial Catalog=eCommerce;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        [HttpGet("{id}")]
        public ActionResult Get(int id) {
            _command =  "SELECT * FROM Usuarios WHERE Id = @Id;" +
                        "SELECT * FROM Contatos WHERE UsuId = @Id;" +
                        "SELECT * FROM Enderecos WHERE UsuId = @Id;" +
                        "SELECT D.* FROM UsuDeptos UD INNER JOIN Departamentos D ON UD.DeptoId = D.Id WHERE UD.UsuId = @Id;";
            using (GridReader multResultSets = _connection.QueryMultiple(_command, new { Id = id })) {
                User user = multResultSets.Read<User>().SingleOrDefault();
                Contact contact = multResultSets.Read<Contact>().SingleOrDefault();
                List<Address> addresses = multResultSets.Read<Address>().ToList();
                List<Department> departments = multResultSets.Read<Department>().ToList();

                if (user != null) {
                    user.Contact = contact;
                    user.Addresses = addresses;
                    user.Departments = departments;
                    return Ok(user);
                }
            }

            return NotFound();
        }

        [HttpGet("stored/users")]
        public IActionResult StoredGet() {
            //executa a Stored Procedure SelecionarUsuarios
            IEnumerable<User> users = _connection.Query<User>("SelecionarUsuarios", commandType: CommandType.StoredProcedure);
            return Ok(users);
        }

        [HttpGet("stored/users/{id}")]
        public IActionResult StoredGet(int id) {
            //_connection.Query<User>("exec SelecionarUsuario 1");
            IEnumerable<User> user = _connection.Query<User>("SelecionarUsuario",new { Id = id }, commandType: CommandType.StoredProcedure);
            return Ok(user);
        }
    }
}