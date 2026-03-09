using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Entities;
using Domain.Enums;
using Services.Interfaces;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LoansTracker.ViewModels
{
    public partial class LoanFormViewModel: ObservableObject
    {
        public IService<Person> _personService;
        public IService<Loan> _loanService;
        public Action? CloseAction { get; set; }
        public event Action<string, MessageType>? ShowMessageRequested;
        public ICommand SaveCommand { get; }

        public LoanFormViewModel( IService<Person> personService, IService<Loan> loanService)
        {
            _personService = personService;
            _loanService = loanService;
            SaveCommand = new RelayCommand(Save);
            InitializeAsync();
        }
        public ObservableCollection<Person> Persons { get; set; } = new ObservableCollection<Person>();
        [ObservableProperty] private Person? selectedPerson;
        [ObservableProperty] public DateTime date =  DateTime.Now;
        [ObservableProperty] private double amount = 0;
        [ObservableProperty] private string description = string.Empty;
        private async void InitializeAsync()
        {
            await Loadpersons();
        }
        private async void Save()
        {
            try
            {
                var loan = new Loan
                {
                    PersonId = selectedPerson.Id,
                    Date = DateOnly.FromDateTime(date),
                    Amount = amount,
                    Description = description
                };
                await _loanService.AddAsync(loan);
                // Close the window if Save succeeds
                CloseAction?.Invoke();
                ShowMessageRequested?.Invoke("Loan created successfully!", MessageType.Success);
            }
            catch (Exception ex)
            {
                ShowMessageRequested?.Invoke($"Error creating loan: {ex.Message}", MessageType.Error);
                return;
            }
        }

        private async Task Loadpersons()
        {
            var persons = await _personService.GetAllAsync();
            foreach (var person in persons)
            {
                Persons.Add(person);
            }
        }
    }
}