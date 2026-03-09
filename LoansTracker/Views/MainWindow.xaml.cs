using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace LoansTracker.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CreatePerson_Click(object sender, RoutedEventArgs e)
        {
            var wnd = App.Services!.GetRequiredService<PersonForm>();
            wnd.Owner = this;
            wnd.ShowDialog();
        }

        private void CreateLoan_Click(object sender, RoutedEventArgs e)
        {
            var wnd = App.Services!.GetRequiredService<LoanForm>();
            wnd.Owner = this;
            wnd.ShowDialog();
        }

        private void ViewLoans_Click(object sender, RoutedEventArgs e)
        {
            var wnd = App.Services!.GetRequiredService<LoansView>();
            wnd.Owner = this;
            wnd.ShowDialog();
        }
    }
}