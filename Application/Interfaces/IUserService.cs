using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> CreateUserAsync(UserDto userDto);
        Task<UserDto> GetUserAsync(int id);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> UpdateUserAsync(UserDto userDto);
        Task<bool> DeleteUserAsync(int id);
    }
}
