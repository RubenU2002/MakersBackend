using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> CreateUserAsync(UserDto userDto)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

            var user = new User(userDto.Name, userDto.Email, userDto.Role, passwordHash);
            var createdUser = await _userRepository.AddAsync(user);
            return new UserDto
            {
                Id = createdUser.Id,
                Name = createdUser.Name,
                Email = createdUser.Email,
                Role = createdUser.Role
            };
        }

        public async Task<UserDto> GetUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return null;
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            };
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Role = u.Role
            });
        }

        public async Task<UserDto> UpdateUserAsync(UserDto userDto)
        {
            var user = await _userRepository.GetByIdAsync(userDto.Id);
            if (user == null) throw new KeyNotFoundException("User not found");

            user.UpdateEmail(userDto.Email);
            var updatedUser = await _userRepository.UpdateAsync(user);
            return new UserDto
            {
                Id = updatedUser.Id,
                Name = updatedUser.Name,
                Email = updatedUser.Email,
                Role = updatedUser.Role
            };
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            return await _userRepository.DeleteAsync(id);
        }
    }
}
