using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;

        public LoanService(ILoanRepository loanRepository, IUnitOfWork unitOfWork, ICacheService cacheService)
        {
            _loanRepository = loanRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        public async Task<LoanDto> CreateLoanAsync(LoanDto loanDto)
        {
            var loan = new Loan(loanDto.Amount, loanDto.UserId);
            var createdLoan = await _loanRepository.AddAsync(loan);
            return MapToDto(createdLoan);
        }

        public async Task<LoanDto> GetLoanAsync(int id)
        {
            string cacheKey = $"Loan_{id}";
            var cachedLoan = await _cacheService.GetAsync<LoanDto>(cacheKey);
            if (cachedLoan != null)
                return cachedLoan;

            var loan = await _loanRepository.GetByIdAsync(id);
            if (loan == null) return null;

            var loanDto = MapToDto(loan);
            await _cacheService.SetAsync(cacheKey, loanDto, TimeSpan.FromMinutes(5));
            return loanDto;
        }

        public async Task<IEnumerable<LoanDto>> GetAllLoansAsync()
        {
            var loans = await _loanRepository.GetAllAsync();
            return loans.Select(loan => MapToDto(loan));
        }

        public async Task<LoanDto> UpdateLoanAsync(LoanDto loanDto)
        {
            var loan = await _loanRepository.GetByIdAsync(loanDto.Id);
            if (loan == null)
                throw new KeyNotFoundException("Loan not found");

            if (loan.Status != LoanStatus.Pending)
                throw new BusinessException("Only pending loans can be updated");

            if (loanDto.Amount <= 0)
                throw new BusinessException("Loan amount must be greater than zero");

            if (loanDto.Amount == loan.Amount)
                throw new BusinessException("New loan amount must be different from the current amount");

            loan.UpdateAmount(loanDto.Amount);

            var updatedLoan = await _loanRepository.UpdateAsync(loan);

            await _cacheService.RemoveAsync($"Loan_{loan.Id}");

            return MapToDto(updatedLoan);
        }

        public async Task<bool> DeleteLoanAsync(int id)
        {
            return await _loanRepository.DeleteAsync(id);
        }

        public async Task<bool> ApproveLoanAsync(int id)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var loan = await _loanRepository.GetByIdAsync(id);
                if (loan == null) throw new KeyNotFoundException("Loan not found");
                loan.Approve();
                await _loanRepository.UpdateAsync(loan);
                await _unitOfWork.CommitAsync();
                await _cacheService.RemoveAsync($"Loan_{loan.Id}");
                return true;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> RejectLoanAsync(int id)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var loan = await _loanRepository.GetByIdAsync(id);
                if (loan == null) throw new KeyNotFoundException("Loan not found");
                loan.Reject();
                await _loanRepository.UpdateAsync(loan);
                await _unitOfWork.CommitAsync();
                await _cacheService.RemoveAsync($"Loan_{loan.Id}");
                return true;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        private LoanDto MapToDto(Loan loan)
        {
            return new LoanDto
            {
                Id = loan.Id,
                Amount = loan.Amount,
                Status = loan.Status.ToString(),
                UserId = loan.UserId,
                CreatedAt = loan.CreatedAt
            };
        }
    }
}
