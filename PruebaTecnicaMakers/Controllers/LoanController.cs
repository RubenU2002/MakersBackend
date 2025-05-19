using Application.Commands.Loans;
using Application.DTOs;
using Application.Querys.Loans;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace PruebaTecnicaMakers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LoanController : ControllerBase
    {
        private readonly IMediator _mediator;
        public LoanController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLoanCommand command)
        {
            var createdLoan = await _mediator.Send(command); 
            return CreatedAtAction(nameof(Get), new { id = createdLoan.Id }, createdLoan);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetLoanQuery { Id = id };
            var loan = await _mediator.Send(query);
            if (loan == null) return NotFound();
            return Ok(loan);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllLoansQuery();
            var loans = await _mediator.Send(query);
            return Ok(loans);
        }
        [HttpGet("byUser/{userId}")]
        public async Task<IActionResult> GetLoanByUser(int userId)
        {
            var query = new GetAllLoansQuery();
            var loans = await _mediator.Send(query);
            var filteredLoans = loans.Where(l => l.UserId == userId).ToList();
            if(filteredLoans.Count == 0) return Ok(new List<LoanDto>());
            return Ok(filteredLoans);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateLoanCommand command)
        {
            if (id != command.Id) return BadRequest("Mismatched loan ID.");
            try
            {
                var updatedLoan = await _mediator.Send(command);
                return Ok(updatedLoan);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteLoanCommand { Id = id };
            var result = await _mediator.Send(command);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPost("{id}/approve")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Approve(int id)
        {
            var command = new ApproveLoanCommand { Id = id };
            try
            {
                var result = await _mediator.Send(command);
                return Ok(new { message = "Loan approved successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Loan approval failed: {ex.Message}" });
            }
        }

        [HttpPost("{id}/reject")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Reject(int id)
        {
            var command = new RejectLoanCommand { Id = id };
            try
            {
                var result = await _mediator.Send(command);
                return Ok(new { message = "Loan rejected successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Loan rejection failed: {ex.Message}" });
            }
        }
    }
}

