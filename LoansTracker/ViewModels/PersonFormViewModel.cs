using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Entities;
using Domain.Enums;
using Services.Interfaces;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LoansTracker.ViewModels
{
    public partial class PersonFormViewModel : ObservableObject
    {
        private readonly IService<Person> _personService;
        public Action? CloseAction { get; set; }
        public ICommand SaveCommand { get; }
        public event Action<string, MessageType>? ShowMessageRequested;

        public PersonFormViewModel(IService<Person> personService)
        {
            _personService = personService;
            SaveCommand = new RelayCommand(AddPerson);
        }
        [ObservableProperty] public string name;
        [ObservableProperty] public string surname;

        public async void AddPerson()
        {
            try
            {
                var author = new Person
                {
                    Name = name,
                    Surname = surname
                };
                await _personService.AddAsync(author);
                // Close the window if Save succeeds
                CloseAction?.Invoke();
                ShowMessageRequested?.Invoke("Author created successfully!", MessageType.Success);
            }
            catch (Exception ex)
            {
                ShowMessageRequested?.Invoke($"Error creating author: {ex.Message}", MessageType.Error);
            }
        }
    }
}
