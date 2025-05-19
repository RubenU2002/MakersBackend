using Domain.Entities;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanUserSystem.Tests.DomainTests
{
    public class LoanTests
    {
        [Fact]
        public void CreateLoan_WithValidAmount_ShouldSetPendingStatus()
        {
            var loan = new Loan(1000, 1);

            Assert.Equal(LoanStatus.Pending, loan.Status);
        }

        [Fact]
        public void ApproveLoan_WhenLoanIsPending_ShouldSetApprovedStatus()
        {
            var loan = new Loan(1000, 1);

            loan.Approve();

            Assert.Equal(LoanStatus.Approved, loan.Status);
        }

        [Fact]
        public void RejectLoan_WhenLoanIsPending_ShouldSetRejectedStatus()
        {
            var loan = new Loan(1000, 1);

            loan.Reject();

            Assert.Equal(LoanStatus.Rejected, loan.Status);
        }

        [Fact]
        public void ApproveLoan_WhenLoanIsNotPending_ShouldThrowException()
        {
            var loan = new Loan(1000, 1);
            loan.Approve();
            Assert.Throws<InvalidOperationException>(() => loan.Approve());
        }

        [Fact]
        public void UpdateAmount_WithSameAmount_ShouldThrowBusinessException()
        {
            var loan = new Loan(1000, 1);

            Assert.Throws<BusinessException>(() => loan.UpdateAmount(1000));
        }

        [Fact]
        public void UpdateAmount_WithInvalidAmount_ShouldThrowBusinessException()
        {
            var loan = new Loan(1000, 1);

            Assert.Throws<BusinessException>(() => loan.UpdateAmount(0));
        }
    }
}
