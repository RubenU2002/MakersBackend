using Application.DTOs;
using MediatR;

namespace Application.Commands.Auth.Register
{
    public class RegisterCommand : IRequest<AuthResponseDto>
    {
        public string? Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string? Role { get; set; }
    }
}