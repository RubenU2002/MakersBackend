using Application.DTOs;
using MediatR;

namespace Application.Commands.Loans;

public class CreateLoanCommand : IRequest<LoanDto>
{
    public decimal Amount { get; set; }
    public int UserId { get; set; }
}