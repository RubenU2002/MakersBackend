using System;
using Application.DTOs;
using MediatR;

namespace Application.Querys.Auth;

public class LoginQuery : IRequest<AuthResponseDto>
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
