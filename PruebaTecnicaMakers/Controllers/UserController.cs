using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PruebaTecnicaMakers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserDto userDto)
        {
            var createdUser = await _userService.CreateUserAsync(userDto);
            return CreatedAtAction(nameof(Get), new { id = createdUser.Id }, createdUser);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _userService.GetUserAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserDto userDto)
        {
            if (id != userDto.Id) return BadRequest("Mismatched user ID.");
            var updatedUser = await _userService.UpdateUserAsync(userDto);
            if (updatedUser == null) return NotFound();
            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
