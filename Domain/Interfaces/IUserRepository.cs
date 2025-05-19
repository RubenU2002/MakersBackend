using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> AddAsync(User user);
        Task<User> GetByIdAsync(int id);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> UpdateAsync(User user);
        Task<bool> DeleteAsync(int id);
        Task<User> GetByEmailAsync(string email);
    }
}
