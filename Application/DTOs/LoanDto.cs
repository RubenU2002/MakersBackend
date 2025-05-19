using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class LoanDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string? Status { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
