using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Services.Implementations;

namespace UnitTests
{
    public class LoanServiceUnitTests
    {
        private readonly ILoanRepository _loanRepo;
        private readonly IPersonRepository _bookRepo;
        private readonly ILogger<LoanService> _logger;
        private readonly LoanService _loanService;
        private Person samplePerson;
        public LoanServiceUnitTests()
        {
            // Arrange
        }
        [Fact]
        public void Test1()
        {

        }
    }
}
