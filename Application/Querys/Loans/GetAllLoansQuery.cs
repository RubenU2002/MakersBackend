using Application.DTOs;
using MediatR;

namespace Application.Querys.Loans
{
    public class GetAllLoansQuery : IRequest<IEnumerable<LoanDto>>
    {
    }
}