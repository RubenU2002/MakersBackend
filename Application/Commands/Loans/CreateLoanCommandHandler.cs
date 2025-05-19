using Application.Commands.Loans;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Commands.Loans;

public class CreateLoanCommandHandler : IRequestHandler<CreateLoanCommand, LoanDto>
{
    private readonly ILoanRepository _loanRepository;

    public CreateLoanCommandHandler(ILoanRepository loanRepository)
    {
        _loanRepository = loanRepository;
    }

    public async Task<LoanDto> Handle(CreateLoanCommand request, CancellationToken cancellationToken)
    {
        var loan = new Loan(request.Amount, request.UserId);
        var createdLoan = await _loanRepository.AddAsync(loan);
        
        return new LoanDto
        {
            Id = createdLoan.Id,
            Amount = createdLoan.Amount,
            Status = createdLoan.Status.ToString(),
            UserId = createdLoan.UserId,
            CreatedAt = createdLoan.CreatedAt
        };
    }
}