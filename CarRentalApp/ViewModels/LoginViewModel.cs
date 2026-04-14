using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Linq;
using CarRentalApp.Data;
using CarRentalApp.Models;
using CarRentalApp.Views;
using BCrypt.Net; // DODANE: obsługa weryfikacji haszy

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

                // 1. WERYFIKACJA KLIENTA
                // Najpierw szukamy klienta tylko po nazwie użytkownika
                var client = context.Clients.FirstOrDefault(c => c.Username == Username);

                // Jeśli klient istnieje, sprawdzamy czy wpisane hasło pasuje do hasza w bazie
                if (client != null && BCrypt.Net.BCrypt.Verify(password, client.Password))
                {
                    MessageBox.Show($"Witaj {client.FirstName}! Logowanie pomyślne (Klient).");
                    UserSession.CurrentClient = client;
                    OpenMainWindow();
                    return;
                }

                // 2. WERYFIKACJA PRACOWNIKA
                // Szukamy pracownika tylko po nazwie użytkownika
                var worker = context.Workers.FirstOrDefault(w => w.Username == Username);

                // Jeśli pracownik istnieje, sprawdzamy czy wpisane hasło pasuje do hasza
                if (worker != null && BCrypt.Net.BCrypt.Verify(password, worker.Password))
                {
                    MessageBox.Show($"Witaj {worker.FirstName}! Zalogowano jako Pracownik.");
                    UserSession.CurrentWorker = worker;
                    OpenMainWindow();
                    return;
                }

                // Jeśli nie znaleziono dopasowania lub hasło było błędne
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