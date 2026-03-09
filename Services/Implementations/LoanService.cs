using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class LoanService(ILoanRepository loanRepo,IPersonRepository PersonRepo,ILogger<LoanService> logger):IService<Loan>
    {
        public async Task AddAsync(Loan loan)
        {
            try
            {
                if(loan.Date > DateOnly.FromDateTime(DateTime.Today))
                {
                    logger.LogError("Invalid StartDate");
                    throw new Exception("Date cannot be in the future");
                }
                if(loan.Amount <= 0)
                {
                    logger.LogError("Invalid Amount");
                    throw new Exception("Amount must be greater than zero");
                }

                var Person = await PersonRepo.GetByIdAsync(loan.PersonId);
                if (Person == null)
                {
                    logger.LogWarning("Person with ID {PersonId} not found", loan.PersonId);
                    throw new Exception($"Person with ID {loan.PersonId} not found");
                }

                await loanRepo.AddAsync(loan);
                logger.LogInformation("Loan created for Person {PersonId} at {Date} ",  loan.PersonId, loan.Date);   
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating Loan for Person {PersonId}: {Message}", loan.PersonId, ex.Message);
                throw new Exception(ex.Message, ex);
            }
            
        }

        public async Task<List<Loan>> GetAllAsync()
        {
            try
            {
                var loans = await loanRepo.GetAllAsync();
                logger.LogInformation("Retrieved {LoanCount} loans", loans.Count());
                return loans.ToList();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving loans: {Message}", ex.Message);
                throw new Exception("Error retrieving loans", ex);
            }
        }

        public async Task<Loan?> GetByIdAsync(long id)
        {
            try
            {
                var loan = await loanRepo.GetByIdAsync(id);
                if (loan != null)
                {
                    logger.LogInformation("Retrieved loan with ID {LoanId}", id);
                }
                else
                {
                    logger.LogWarning("Loan with ID {LoanId} not found", id);
                }
                return loan;
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Error retrieving loan with ID {LoanId}: {Message}", id, ex.Message);
                throw new Exception("Error retrieving loan", ex);
            }
        }

        public async Task RemoveAsync(long id)
        {
            try { 
                await loanRepo.RemoveAsync(id);
                logger.LogInformation("Removed loan with ID {LoanId}", id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error removing loan with ID {LoanId}: {Message}", id, ex.Message);
                throw new Exception("Error removing loan", ex);
            }
        }

        public Task UpdateAsync(Loan entity)
        {
            throw new NotImplementedException();
        }
    }
}
