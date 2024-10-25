using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolessBackend.Interfaces;
using SolessBackend.Models;

namespace SolessBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository) 
        { 
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult GetUsers() 
        {
            var users = _userRepository.GetUsersAsync;

            if (!ModelState.IsValid) 
            { 
                return BadRequest(ModelState);
            }

            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(int id) 
        { 
            var user = _userRepository.GetUserByIdAsync(id);

            return Ok(user);
        }

        [HttpPost]
        public IActionResult AddUser([FromBody] User userToAdd)
        {
            _userRepository.AddUserAsync(userToAdd);
            
            return Created();
        }

    }
}
