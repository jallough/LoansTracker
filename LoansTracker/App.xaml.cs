using Domain.Entities;
using Domain.Interfaces;
using LoansTracker.ViewModels;
using LoansTracker.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Persistence.DataContext;
using Persistence.SQL;
using Services.Implementations;
using Services.Interfaces;
using System.Windows;

namespace LoansTracker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider? Services { get; private set; }
        public App()
        {
            var services = new ServiceCollection();
            // Add logging
            services.AddLogging(config =>
            {
                config.ClearProviders();
                config.SetMinimumLevel(LogLevel.Information);
                config.AddNLog();
            });
            // DbContext
            services.AddDbContext<LoansDbContext>(options =>
                options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=LoansDb;Trusted_Connection=True;"));

            // Repositories
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<ILoanRepository, LoanRepository>();

            // Services
            services.AddScoped<IService<Person>, PersonService>();
            services.AddScoped<IService<Loan>, LoanService>();

            // ViewModels
            services.AddTransient<PersonFormViewModel>();
            services.AddTransient<LoanFormViewModel>();
            services.AddTransient<LoansViewModel>();

            // Views
            services.AddTransient<MainWindow>();
            services.AddTransient<PersonForm>();
            services.AddTransient<LoanForm>();
            services.AddTransient<LoansView>();



            Services = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Show main window
            var main = Services.GetRequiredService<MainWindow>();
            main.Show();
        }
    }

}
