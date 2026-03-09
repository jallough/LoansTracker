using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Persistence.DataContext
{
    public class LoansDbContextFactory : IDesignTimeDbContextFactory<LoansDbContext>
    {
        public LoansDbContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<LoansDbContext>()
                .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=LoansDb;Trusted_Connection=True;")
                .Options;

            return new LoansDbContext(options);
        }
    }
}
