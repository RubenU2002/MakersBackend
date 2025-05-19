using Application.DTOs;
using Application.Interfaces;
using Domain.Interfaces;
using MediatR;

namespace Application.Querys.Loans;

public class GetLoanQueryHandler : IRequestHandler<GetLoanQuery, LoanDto>
{
    private readonly ILoanRepository _loanRepository;
    private readonly ICacheService _cacheService;

    public GetLoanQueryHandler(ILoanRepository loanRepository, ICacheService cacheService)
    {
        _loanRepository = loanRepository;
        _cacheService = cacheService;
    }

    public async Task<LoanDto> Handle(GetLoanQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"Loan_{request.Id}";
        var cachedLoan = await _cacheService.GetAsync<LoanDto>(cacheKey);
        if (cachedLoan != null)
            return cachedLoan;

        var loan = await _loanRepository.GetByIdAsync(request.Id);
        if (loan == null) return null;

        var loanDto = new LoanDto
        {
            Id = loan.Id,
            Amount = loan.Amount,
            Status = loan.Status.ToString(),
            UserId = loan.UserId,
            CreatedAt = loan.CreatedAt
        };
        
        await _cacheService.SetAsync(cacheKey, loanDto, TimeSpan.FromMinutes(5));
        return loanDto;
    }
}