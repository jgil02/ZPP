using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Linq;
using CarRentalApp.Models;

namespace CarRentalApp.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string _username = "";

        [ObservableProperty]
        private string _password = "";

        [ObservableProperty]
        private string _errorMessage = "";

        [RelayCommand]
        private void Login()
        {
            
            if (Username == "admin" && Password == "admin")
            {
                OpenWorkerPanel();
                return;
            }

            
            if (Username == "klient" && Password == "klient")
            {
                OpenClientPanel();
                return;
            }

            ErrorMessage = "Nieprawidłowe dane logowania!";
        }

        private void OpenWorkerPanel()
        {
            var mainView = new Views.MainView(); 
            mainView.Show();
            CloseLoginWindow();
        }

        private void OpenClientPanel()
        {
            
            MessageBox.Show("Zalogowano jako Klient! Tu otworzy się Twój panel rezerwacji.");
            
        }

        private void CloseLoginWindow()
        {
            
            foreach (Window window in Application.Current.Windows)
            {
                if (window.DataContext == this)
                {
                    window.Close();
                    break;
                }
            }
        }
    }
}