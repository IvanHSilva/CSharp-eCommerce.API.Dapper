using eCommerce.API.Dapper.Models;
using eCommerce.API.Dapper.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.API.Dapper.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase {

        private IUserRepository _repository;

        public UsersController() {
            _repository = new UserRepository();
        }

        [HttpGet]
        public IActionResult Get() {
            // Ok converts a object in json format
            return Ok(_repository.GetUsers()); 
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id) {
            User user = _repository.GetUser(id);
            if (user == null) return NotFound(); else return Ok(user);
        }

        [HttpPost]
        public IActionResult Insert([FromBody] User user) {
            _repository.InsertUser(user);
            return Ok(user);
        }

        [HttpPut]
        public IActionResult Update([FromBody] User user) {
            _repository.UpdateUser(user);
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id) {
            _repository.DeleteUser(id);
            return Ok();
        }
    }
}
