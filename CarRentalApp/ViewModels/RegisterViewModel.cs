using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Linq;
using CarRentalApp.Models;

namespace CarRentalApp.ViewModels
{
    
    public partial class RegisterViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string _newUsername = "";

        [ObservableProperty]
        private bool _isRegisteringAsWorker = false;

        
       
        [RelayCommand]
        private void Register(object parameter)
        {
            
            var passwordBox = parameter as System.Windows.Controls.PasswordBox;
            string actualPassword = passwordBox?.Password ?? "";

           
            if (string.IsNullOrWhiteSpace(NewUsername) || string.IsNullOrWhiteSpace(actualPassword))
            {
                MessageBox.Show("Wypełnij wszystkie pola!");
                return;
            }

           
            string rola = IsRegisteringAsWorker ? "Pracownik" : "Klient";
            MessageBox.Show($"Zarejestrowano pomyślnie użytkownika {NewUsername} jako: {rola}");

            
            Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.DataContext == this)?.Close();
        }
    }
}