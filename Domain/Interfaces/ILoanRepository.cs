using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ILoanRepository
    {
        Task<Loan> AddAsync(Loan loan);
        Task<Loan> GetByIdAsync(int id);
        Task<IEnumerable<Loan>> GetAllAsync();
        Task<Loan> UpdateAsync(Loan loan);
        Task<bool> DeleteAsync(int id);
    }
}
