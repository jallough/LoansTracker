using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Services.Implementations;

namespace UnitTests
{
    public class LoanServiceUnitTests
    {
        private readonly ILoanRepository _loanRepo;
        private readonly IPersonRepository _personRepo;
        private readonly ILogger<LoanService> _logger;
        private readonly LoanService _loanService;
        private Person samplePerson;
        public LoanServiceUnitTests()
        {
            _loanRepo = Substitute.For<ILoanRepository>();
            _personRepo = Substitute.For<IPersonRepository>();
            _logger = Substitute.For<ILogger<LoanService>>();

            _loanService = new LoanService(_loanRepo, _personRepo, _logger);
            samplePerson = new Person
            {
                Id = 1,
                Name = "John",
                Surname = "Doe"
            };
        }

        private Loan BuildValidLoan()
        {
            return new Loan
            {
                Id = 1,
                Person = samplePerson,
                PersonId = samplePerson.Id,
                Description = "Test Loan",
                Date = DateOnly.FromDateTime(DateTime.Today),
                Amount = 100.00
            };
        }

        [Fact]
        public async Task AddAsync_ShouldCreateLoanSuccessfully()
        {
            var loan = BuildValidLoan();

            _personRepo.GetByIdAsync(loan.PersonId).Returns(samplePerson);

            await _loanService.AddAsync(loan);

            await _loanRepo.Received(1).AddAsync(loan);
            _logger.Received().Log(
                LogLevel.Information,
                Arg.Any<EventId>(),
                Arg.Any<object>(),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception?, string>>());
        }


        [Fact]
        public async Task AddAsync_ShouldThrow_WhenDateInFuture()
        {
            var loan = BuildValidLoan();
            loan.Date = DateOnly.FromDateTime(DateTime.Today.AddDays(1));

            var ex = await Assert.ThrowsAsync<Exception>(() => _loanService.AddAsync(loan));
            Assert.Contains("Date cannot be in the future", ex.Message);
        }

        [Fact]
        public async Task AddAsync_ShouldThrow_WhenPersonNotFound()
        {
            var loan = BuildValidLoan();
            _personRepo.GetByIdAsync(loan.PersonId).Returns((Person?)null);

            var ex = await Assert.ThrowsAsync<Exception>(() => _loanService.AddAsync(loan));
            Assert.Contains($"Person with ID {loan.PersonId} not found", ex.Message);
            _logger.Received().Log(
                LogLevel.Warning,
                Arg.Any<EventId>(),
                Arg.Any<object>(),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception?, string>>());
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnLoans()
        {
            var loans = new List<Loan> { BuildValidLoan() };
            _loanRepo.GetAllAsync().Returns(loans);

            var result = await _loanService.GetAllAsync();

            Assert.Single(result);
            _logger.Received().Log(
                LogLevel.Information,
                Arg.Any<EventId>(),
                Arg.Any<object>(),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception?, string>>());
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnLoan_WhenExists()
        {
            var loan = BuildValidLoan();
            _loanRepo.GetByIdAsync(loan.Id).Returns(loan);

            var result = await _loanService.GetByIdAsync(loan.Id);

            Assert.Equal(loan.Id, result!.Id);
            _logger.Received().Log(
                LogLevel.Information,
                Arg.Any<EventId>(),
                Arg.Any<object>(),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception?, string>>());
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            _loanRepo.GetByIdAsync(1).Returns((Loan?)null);

            var result = await _loanService.GetByIdAsync(1);

            Assert.Null(result);
            _logger.Received().Log(
                LogLevel.Warning,
                Arg.Any<EventId>(),
                Arg.Any<object>(),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception?, string>>());
        }
    }
}
