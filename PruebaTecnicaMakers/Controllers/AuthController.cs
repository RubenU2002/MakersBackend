using Application.Commands.Auth.Register;
using Application.DTOs;
using Application.Interfaces;
using Application.Querys.Auth;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PruebaTecnicaMakers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterCommand command)
        {
            command.Role = "User";
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        
        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody]RegisterCommand command)
        {
            command.Role = "Admin";
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}