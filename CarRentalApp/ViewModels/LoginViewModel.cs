using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Linq;
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
                ErrorMessage = "Wpisz login i hasło!";
                return;
            }

           
            
            if (Username == "admin" && password == "admin")
            {
                OpenWorkerPanel();
            }
            
            else if (Username == "klient" && password == "klient")
            {
                OpenClientPanel();
            }
            else
            {
                ErrorMessage = "Błędny login lub hasło!";
            }
        }

        
        [RelayCommand]
        private void OpenRegistration()
        {
            
            var registerWindow = new RegisterView();
            registerWindow.ShowDialog();
        }

        private void OpenWorkerPanel()
        {
            var mainView = new MainView();
            mainView.Show();
            CloseLoginWindow();
        }

        private void OpenClientPanel()
        {
            MessageBox.Show("Witaj");
            
        }

        private void CloseLoginWindow()
        {
            Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is LoginView)?.Close();
        }
    }
}