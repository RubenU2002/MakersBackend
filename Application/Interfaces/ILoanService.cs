using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ILoanService
    {
        Task<LoanDto> CreateLoanAsync(LoanDto loanDto);
        Task<LoanDto> GetLoanAsync(int id);
        Task<IEnumerable<LoanDto>> GetAllLoansAsync();
        Task<LoanDto> UpdateLoanAsync(LoanDto loanDto);
        Task<bool> DeleteLoanAsync(int id);
        Task<bool> ApproveLoanAsync(int id);
        Task<bool> RejectLoanAsync(int id);
    }
}
