using MediatR;

namespace Application.Commands.Loans;

public class RejectLoanCommand : IRequest<bool>
{
    public int Id { get; set; }
}