using Application.DTOs;
using Application.Interfaces;
using Domain.Exceptions;
using Domain.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.Loans;

public class UpdateLoanCommandHandler : IRequestHandler<UpdateLoanCommand, LoanDto>
{
    private readonly ILoanRepository _loanRepository;
    private readonly ICacheService _cacheService;

    public UpdateLoanCommandHandler(ILoanRepository loanRepository, ICacheService cacheService)
    {
        _loanRepository = loanRepository;
        _cacheService = cacheService;
    }

    public async Task<LoanDto> Handle(UpdateLoanCommand request, CancellationToken cancellationToken)
    {
        var loan = await _loanRepository.GetByIdAsync(request.Id);
        if (loan == null)
            throw new KeyNotFoundException("Loan not found");

        if (loan.Status != Domain.Entities.LoanStatus.Pending)
            throw new BusinessException("Only pending loans can be updated");

        if (request.Amount <= 0)
            throw new BusinessException("Loan amount must be greater than zero");

        if (request.Amount == loan.Amount)
            throw new BusinessException("New loan amount must be different from the current amount");

        loan.UpdateAmount(request.Amount);

        var updatedLoan = await _loanRepository.UpdateAsync(loan);

        await _cacheService.RemoveAsync($"Loan_{loan.Id}");

        return new LoanDto
        {
            Id = updatedLoan.Id,
            Amount = updatedLoan.Amount,
            Status = updatedLoan.Status.ToString(),
            UserId = updatedLoan.UserId,
            CreatedAt = updatedLoan.CreatedAt
        };
    }
}