using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Entities;
using Services.Interfaces;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LoansTracker.ViewModels
{
    public partial class LoansViewModel: ObservableObject
    {
        private readonly IService<Loan> _loanService;
        public double TotalAmount => Loans.Sum(l => l.Amount);
        public ICommand DeleteLoanCommand { get; }
        public ObservableCollection<Loan> Loans { get; } = new();

        public LoansViewModel(IService<Loan> loanService)
        {
            _loanService = loanService;
            DeleteLoanCommand = new RelayCommand<Loan>(DeleteLoan);
            LoadLoans();
        }

        private async void LoadLoans()
        {
            var items = await _loanService.GetAllAsync();
            Loans.Clear();

            foreach (var loan in items)
                Loans.Add(loan);
            OnPropertyChanged(nameof(TotalAmount));
        }
        private async void DeleteLoan(Loan loan)
        {
            try { 
                await _loanService.RemoveAsync(loan.Id);
                Loans.Remove(loan);
                OnPropertyChanged(nameof(TotalAmount));
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., show a message to the user)
                Console.WriteLine($"Error deleting loan: {ex.Message}");
                return;
            }
            
        }
    }
}
