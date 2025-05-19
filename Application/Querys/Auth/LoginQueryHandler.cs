using Application.DTOs;
using Application.Interfaces;
using Application.Querys.Auth;
using Domain.Interfaces;
using MediatR;

namespace Application.Commands.Auth.Login
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, AuthResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtTokenService;
        public LoginQueryHandler(IUserRepository userRepository,
                                  IJwtTokenService jwtTokenService)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
        }
        public async Task<AuthResponseDto> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null)
            {
                throw new System.Exception("Invalid email or password.");
            }

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!isValidPassword)
            {
                throw new System.Exception("Invalid email or password.");
            }

            string token = _jwtTokenService.GenerateToken(user.Email, user.Role);

            return new AuthResponseDto
            {
                UserId = user.Id,
                Email = user.Email,
                Role = user.Role,
                Token = token
            };
        }
    }
}