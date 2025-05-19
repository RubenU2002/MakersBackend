using Application.Interfaces;
using Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.Loans;

public class DeleteLoanCommandHandler : IRequestHandler<DeleteLoanCommand, bool>
{
    private readonly ILoanRepository _loanRepository;
    private readonly ICacheService _cacheService;

    public DeleteLoanCommandHandler(ILoanRepository loanRepository, ICacheService cacheService)
    {
        _loanRepository = loanRepository;
        _cacheService = cacheService;
    }

    public async Task<bool> Handle(DeleteLoanCommand request, CancellationToken cancellationToken)
    {
        var result = await _loanRepository.DeleteAsync(request.Id);
        if (result)
        {
            await _cacheService.RemoveAsync($"Loan_{request.Id}");
        }
        return result;
    }
}