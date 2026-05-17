using CarRentalApp.Data;
using CarRentalApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using BCrypt.Net;

namespace CarRentalApp.ViewModels
{
    public partial class AddClientViewModel : BaseViewModel
    {
        [ObservableProperty] private string _username = "";
        [ObservableProperty] private string _firstName = "";
        [ObservableProperty] private string _lastName = "";
        [ObservableProperty] private string _email = "";
        [ObservableProperty] private string _phone = "";
        [ObservableProperty] private string _street = "";
        [ObservableProperty] private string _houseNumber = "";
        [ObservableProperty] private string _postalCode = "";
        [ObservableProperty] private string _city = "";

        [RelayCommand]
        private void SaveClient(object parameter)
        {
            
            var values = parameter as object[];
            if (values == null || values.Length < 2) return;

            var pb1 = values[0] as PasswordBox;
            var pb2 = values[1] as PasswordBox;

            string pass = pb1?.Password ?? "";
            string confirmPass = pb2?.Password ?? "";

            
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(pass) || string.IsNullOrWhiteSpace(FirstName))
            {
                MessageBox.Show("Pola: Login, Hasło oraz Imię są wymagane!");
                return;
            }

            if (pass != confirmPass)
            {
                MessageBox.Show("Hasła nie są identyczne!");
                return;
            }

            try
            {
                using (var context = new AppDbContext())
                {
                    if (context.Clients.Any(c => c.Username == Username))
                    {
                        MessageBox.Show("Ten login jest już zajęty!");
                        return;
                    }

                    var client = new Client
                    {
                        Username = Username,
                        Password = BCrypt.Net.BCrypt.HashPassword(pass),
                        FirstName = FirstName,
                        LastName = LastName,
                        Email = Email ?? "",
                        Phone = Phone ?? "",
                        Street = Street ?? "",
                        HouseNumber = HouseNumber ?? "",
                        PostalCode = PostalCode ?? "",
                        City = City ?? "",
                        Country = "Polska"
                    };

                    context.Clients.Add(client);
                    context.SaveChanges();
                }

                MessageBox.Show("Klient został dodany pomyślnie!");

                
                ClearForm(pb1, pb2);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Błąd: {ex.Message}");
            }
        }

        private void ClearForm(PasswordBox p1, PasswordBox p2)
        {
            Username = FirstName = LastName = Email = Phone = Street = HouseNumber = PostalCode = City = "";
            if (p1 != null) p1.Password = "";
            if (p2 != null) p2.Password = "";
        }
    }
}