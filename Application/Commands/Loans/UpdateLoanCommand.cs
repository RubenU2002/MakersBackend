using Application.DTOs;
using MediatR;

namespace Application.Commands.Loans;

public class UpdateLoanCommand : IRequest<LoanDto>
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
}