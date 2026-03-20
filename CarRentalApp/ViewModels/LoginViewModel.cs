using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Linq;
using CarRentalApp.Data;
using CarRentalApp.Models;
using CarRentalApp.Views;

namespace CarRentalApp.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string _username = "";

        [ObservableProperty]
        private string _errorMessage = "";

        
        [RelayCommand]
        private void Login(object parameter)
        {
            
            var passwordBox = parameter as System.Windows.Controls.PasswordBox;
            string password = passwordBox?.Password ?? "";

           
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(password))
            {
                ErrorMessage = "Wprowadź login i hasło!";
                return;
            }

            try
            {
                using var context = new AppDbContext();


                var client = context.Clients.FirstOrDefault(c => c.Username == Username && c.Password == password);

                if (client != null)
                {
                    MessageBox.Show($"Witaj {client.FirstName}! Logowanie pomyślne (Klient).");
                    UserSession.CurrentClient = client;
                    OpenMainWindow();
                    return;
                }


                var worker = context.Workers.FirstOrDefault(w => w.Username == Username && w.Password == password);

                if (worker != null)
                {
                    MessageBox.Show($"Witaj {worker.FirstName}! Zalogowano jako Pracownik.");
                    UserSession.CurrentWorker = worker;
                    OpenMainWindow();
                    return;
                }


                ErrorMessage = "Błędny login lub hasło!";
            }
            catch (System.Exception ex)
            {
                ErrorMessage = "Błąd połączenia z bazą danych!";
                MessageBox.Show($"Szczegóły błędu: {ex.Message}");
            }
        }

        
        [RelayCommand]
        private void OpenRegistration()
        {
           
            var registerView = new RegisterView();
            registerView.ShowDialog();
        }

       
        private void OpenMainWindow()
        {
            var mainView = new MainView();
            mainView.Show();

           
            var currentWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is LoginView);
            currentWindow?.Close();
        }
    }
}