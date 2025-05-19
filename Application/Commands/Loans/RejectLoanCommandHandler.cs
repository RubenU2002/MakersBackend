using Application.Interfaces;
using Domain.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.Loans;

public class RejectLoanCommandHandler : IRequestHandler<RejectLoanCommand, bool>
{
    private readonly ILoanRepository _loanRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;

    public RejectLoanCommandHandler(ILoanRepository loanRepository, IUnitOfWork unitOfWork, ICacheService cacheService)
    {
        _loanRepository = loanRepository;
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
    }

    public async Task<bool> Handle(RejectLoanCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var loan = await _loanRepository.GetByIdAsync(request.Id);
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
}