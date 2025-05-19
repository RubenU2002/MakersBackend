using Domain.Exceptions;

namespace Domain.Entities
{
    public enum LoanStatus
    {
        Pending,
        Approved,
        Rejected
    }

    public class Loan
    {
        public int Id { get; private set; }
        public decimal Amount { get; private set; }
        public LoanStatus Status { get; private set; }
        public int UserId { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public Loan(decimal amount, int userId)
        {
            if (amount <= 0)
                throw new ArgumentException("Loan amount must be greater than zero", nameof(amount));

            Amount = amount;
            UserId = userId;
            Status = LoanStatus.Pending;
            CreatedAt = DateTime.UtcNow;
        }

        public void Approve()
        {
            if (Status != LoanStatus.Pending)
                throw new InvalidOperationException("Only pending loans can be approved");

            Status = LoanStatus.Approved;
        }

        public void Reject()
        {
            if (Status != LoanStatus.Pending)
                throw new InvalidOperationException("Only pending loans can be rejected");

            Status = LoanStatus.Rejected;
        }

        public void UpdateAmount(decimal newAmount)
        {
            if (newAmount <= 0)
                throw new BusinessException("Loan amount must be greater than zero");
            if (newAmount == Amount)
                throw new BusinessException("New amount must be different from the current amount");

            Amount = newAmount;
        }
    }
}
