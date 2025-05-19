using MediatR;

namespace Application.Commands.Loans
{
    public class ApproveLoanCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}