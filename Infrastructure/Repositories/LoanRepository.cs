using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class LoanRepository : ILoanRepository
    {
        private readonly ApplicationDbContext _context;

        public LoanRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Loan> AddAsync(Loan loan)
        {
            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();
            return loan;
        }

        public async Task<Loan> GetByIdAsync(int id)
        {
            return await _context.Loans.FindAsync(id);
        }

        public async Task<IEnumerable<Loan>> GetAllAsync()
        {
            return await _context.Loans.ToListAsync();
        }

        public async Task<Loan> UpdateAsync(Loan loan)
        {
            _context.Loans.Update(loan);
            await _context.SaveChangesAsync();
            return loan;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var loan = await _context.Loans.FindAsync(id);
            if (loan == null)
                return false;

            _context.Loans.Remove(loan);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
