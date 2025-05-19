using Application.Services;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanUserSystem.Tests.ApplicationTests
{
    public class LoanServiceTests
    {
        private readonly Mock<ILoanRepository> _loanRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<Application.Interfaces.ICacheService> _cacheServiceMock;
        private readonly LoanService _loanService;

        public LoanServiceTests()
        {
            _loanRepositoryMock = new Mock<ILoanRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _cacheServiceMock = new Mock<Application.Interfaces.ICacheService>();

            _loanService = new LoanService(_loanRepositoryMock.Object, _unitOfWorkMock.Object, _cacheServiceMock.Object);
        }

        [Fact]
        public async Task ApproveLoanAsync_WithValidLoan_ShouldApproveLoan()
        {
            var loan = new Loan(1000, 1);
            _loanRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(loan);
            _loanRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Loan>())).ReturnsAsync(loan);
            _unitOfWorkMock.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);
            _cacheServiceMock.Setup(c => c.RemoveAsync(It.IsAny<string>())).Returns(Task.CompletedTask);

            var result = await _loanService.ApproveLoanAsync(loan.Id);

            Assert.True(result);
            Assert.Equal(LoanStatus.Approved, loan.Status);
        }

        [Fact]
        public async Task UpdateLoanAsync_WhenLoanNotFound_ShouldThrowKeyNotFoundException()
        {
            _loanRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Loan)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _loanService.UpdateLoanAsync(new Application.DTOs.LoanDto { Id = 1, Amount = 1500 }));
        }

        [Fact]
        public async Task UpdateLoanAsync_WithInvalidAmount_ShouldThrowBusinessException()
        {
            var loan = new Loan(1000, 1);
            _loanRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(loan);

            var invalidLoanDto = new Application.DTOs.LoanDto { Id = loan.Id, Amount = 0 };
            await Assert.ThrowsAsync<BusinessException>(() => _loanService.UpdateLoanAsync(invalidLoanDto));
        }
    }
}
