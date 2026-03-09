using LoansTracker.ViewModels;
using System.Windows;

namespace LoansTracker.Views
{
    public partial class LoansView : Window
    {
        public LoansView(LoansViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}