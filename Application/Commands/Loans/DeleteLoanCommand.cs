using MediatR;

namespace Application.Commands.Loans;

public class DeleteLoanCommand : IRequest<bool>
{
    public int Id { get; set; }
}