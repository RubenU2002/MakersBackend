using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Commands.Auth.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IUnitOfWork _unitOfWork;
        
        public RegisterCommandHandler(IUserRepository userRepository,
                                      IJwtTokenService jwtTokenService,
                                      IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
            _unitOfWork = unitOfWork;
        }

        public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                throw new System.Exception("User already exists with this email.");
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var newUser = new User(request.Name, request.Email, request.Role, passwordHash);
            await _unitOfWork.BeginTransactionAsync();
            var createdUser = await _userRepository.AddAsync(newUser);
            await _unitOfWork.CommitAsync();
            string token = _jwtTokenService.GenerateToken(createdUser.Email, createdUser.Role);

            return new AuthResponseDto
            {
                UserId = createdUser.Id,
                Email = createdUser.Email,
                Role = createdUser.Role,
                Token = token
            };
        }
    }
}