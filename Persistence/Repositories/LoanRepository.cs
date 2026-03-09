using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence.DataContext;


namespace Persistence.SQL
{
    public class LoanRepository : SQLRepository<Loan>, ILoanRepository
    {
        private readonly ILogger<LoanRepository> _logger;
        private readonly LoansDbContext _dbContext;

        public LoanRepository(ILogger<LoanRepository> logger, LoansDbContext dbContext): base(dbContext, logger)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public new async Task<IEnumerable<Loan>> GetAllAsync()
        {
            return await _dbContext.Loans
                .Include(l=>l.Person)
                .OrderByDescending(l=>l.Date).ThenByDescending(l=>l.Amount)
                .ToListAsync();
        }

        public new async Task<Loan?> GetByIdAsync(long id)
        {
            return await _dbContext.Loans
                .Include(l => l.Person)
                .FirstOrDefaultAsync(l => l.Id == id);
        }
    }
}
