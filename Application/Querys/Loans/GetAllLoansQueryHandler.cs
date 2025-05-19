using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Querys.Loans;

public class GetAllLoansQueryHandler : IRequestHandler<GetAllLoansQuery, IEnumerable<LoanDto>>
{
    private readonly ILoanRepository _loanRepository;

    public GetAllLoansQueryHandler(ILoanRepository loanRepository)
    {
        _loanRepository = loanRepository;
    }

    public async Task<IEnumerable<LoanDto>> Handle(GetAllLoansQuery request, CancellationToken cancellationToken)
    {
        var loans = await _loanRepository.GetAllAsync();
        return loans.Select(loan => new LoanDto
        {
            Id = loan.Id,
            Amount = loan.Amount,
            Status = loan.Status.ToString(),
            UserId = loan.UserId,
            CreatedAt = loan.CreatedAt
        });
    }
}