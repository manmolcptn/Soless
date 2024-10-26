using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolessBackend.Interfaces;
using SolessBackend.Models;
using SolessBackend.DTO;
using SolessBackend.DataMappers;
using Microsoft.AspNetCore.Authorization;

namespace SolessBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        //Inyecciones
        private readonly IUserRepository _userRepository;
        private readonly UserMapper _mapper;
        public UserController(IUserRepository userRepository, UserMapper userMapper) 
        { 
            _userRepository = userRepository;
            _mapper = userMapper;
        }

        //GetALLUsers
        [HttpGet]
        public async Task<IActionResult> GetUsersAsync()
        {
            // Comprobación de errores de ModelState
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Intentar obtener los usuarios desde el repositorio
                var users = await _userRepository.GetUsersAsync();

                // Comprobar si la lista de usuarios es nula o está vacía
                if (users == null || !users.Any())
                {
                    return NotFound("No users found.");
                }

                //Creacion del user DTO por cada User en la base de datos
                IEnumerable<UserDTO> usersDTO = _mapper.usersToDTO(users);

                return Ok(usersDTO);
            }
            catch (Exception ex)
            {
                // Captura cualquier error inesperado y devuelve una respuesta de error 500
                // Podrías agregar aquí algún log de error
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserAsync(int id)
        {
            // Verificar si el ID es válido
            if (id <= 0)
            {
                return BadRequest("Invalid user ID.");
            }

            try
            {
                // Intentar obtener el usuario desde el repositorio
                var user = await _userRepository.GetUserByIdAsync(id);

                // Comprobar si el usuario no existe
                if (user == null)
                {
                    return NotFound($"User with ID {id} not found.");
                }

                //Crear UserDTO segun el User encontrado
                UserDTO userDTO = _mapper.userToDTO(user);

                return Ok(userDTO);
            }
            catch (Exception ex)
            {
                // Capturar cualquier error inesperado y devolver una respuesta de error 500
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] User userToAdd)
        {
            if (userToAdd == null)
    {
        return BadRequest("User data is required.");
    }

    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    // Verificar si el usuario ya existe
    var existingUser = await _userRepository.GetUserByEmailAsync(userToAdd.Email);
    if (existingUser != null)
    {
        return Conflict("A user with this email already exists.");
    }

    try
    {
        // Agregar usuario a la base de datos
        await _userRepository.AddUserAsync(userToAdd);
    }
    catch (Exception ex)
    {
        // Capturar y devolver un error interno si algo falla
        return StatusCode(500, "Internal server error: " + ex.Message);
    }

    // Retornar el usuario recién creado
    return CreatedAtAction(nameof(GetUserAsync), new { id = userToAdd.Id }, userToAdd);
        }

    }
}
