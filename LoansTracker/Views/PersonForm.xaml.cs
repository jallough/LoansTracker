using LoansTracker.ViewModels;
using System.Windows;

namespace LoansTracker.Views
{
    public partial class PersonForm : Window
    {
        public PersonForm(PersonFormViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
            vm.ShowMessageRequested += (message, type) =>
            {
                switch (type)
                {
                    case Domain.Enums.MessageType.Success:
                        MessageBox.Show(message, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case Domain.Enums.MessageType.Error:
                        MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                }
            };
            vm.CloseAction = new Action(this.Close);
        }
    }
}