using Application.DTOs;
using MediatR;

namespace Application.Querys.Loans;

public class GetLoanQuery : IRequest<LoanDto>
{
    public int Id { get; set; }
}