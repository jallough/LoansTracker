using Domain.Enums;
using LoansTracker.ViewModels;
using System.Windows;
using System.Windows.Interop;

namespace LoansTracker.Views
{
    public partial class LoanForm : Window
    {
        public LoanForm(LoanFormViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
            vm.ShowMessageRequested += (message,type) =>
            {
                switch (type)
                {
                    case MessageType.Success:
                        MessageBox.Show(message, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case MessageType.Error:
                        MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                    
                }
            };
            vm.CloseAction = new Action(this.Close);
        }

    }
}