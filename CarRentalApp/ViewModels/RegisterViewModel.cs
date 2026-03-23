using CarRentalApp.Data;
using CarRentalApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls; 

namespace CarRentalApp.ViewModels
{
    public partial class RegisterViewModel : BaseViewModel
    {
        
        [ObservableProperty] private string _newUsername = "";
        [ObservableProperty] private bool _isRegisteringAsWorker;

        
        [ObservableProperty] private string _firstName = "";
        [ObservableProperty] private string _lastName = "";
        [ObservableProperty] private string _email = "";
        [ObservableProperty] private string _phone = "";

        
        [ObservableProperty] private string _street = "";
        [ObservableProperty] private string _houseNumber = "";
        [ObservableProperty] private string _apartmentNumber = "";
        [ObservableProperty] private string _postalCode = "";
        [ObservableProperty] private string _city = "";
        [ObservableProperty] private string _country = "Polska";

        [RelayCommand]
        private void Register(object parameter)
        {
            

            
            var values = parameter as object[];
            if (values == null || values.Length < 2)
            {
                MessageBox.Show("Błąd techniczny: Brak pól haseł.");
                return;
            }

            
            var passBox1 = values[0] as PasswordBox;
            var passBox2 = values[1] as PasswordBox;

            string actualPassword = passBox1?.Password ?? "";
            string confirmPassword = passBox2?.Password ?? "";

            
            if (string.IsNullOrWhiteSpace(NewUsername) ||
                string.IsNullOrWhiteSpace(actualPassword) ||
                string.IsNullOrWhiteSpace(confirmPassword) ||
                string.IsNullOrWhiteSpace(FirstName) ||
                string.IsNullOrWhiteSpace(LastName))
            {
                MessageBox.Show("Podstawowe dane (Login, Hasło, Imię, Nazwisko) są wymagane!");
                return;
            }

            
            if (actualPassword != confirmPassword)
            {
                MessageBox.Show("Wprowadzone hasła nie są identyczne! Proszę poprawić błąd.");
                return;
            }

            try
            {
                using (var context = new AppDbContext())
                {
                    if (IsRegisteringAsWorker)
                    {
                        // --- REJESTRACJA PRACOWNIKA ---
                        if (context.Workers.Any(w => w.Username == NewUsername))
                        {
                            MessageBox.Show("Ten login pracownika jest już zajęty!");
                            return;
                        }

                        var newWorker = new Worker
                        {
                            Username = NewUsername,
                            Password = actualPassword,
                            FirstName = FirstName,
                            LastName = LastName,
                            Position = "Pracownik"
                        };

                        context.Workers.Add(newWorker);
                    }
                    else
                    {
                        // --- REJESTRACJA KLIENTA ---
                        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Street) ||
                            string.IsNullOrWhiteSpace(HouseNumber) || string.IsNullOrWhiteSpace(City))
                        {
                            MessageBox.Show("Dla klienta wymagany jest pełny adres i e-mail!");
                            return;
                        }

                        if (context.Clients.Any(c => c.Username == NewUsername))
                        {
                            MessageBox.Show("Ten login klienta jest już zajęty!");
                            return;
                        }

                        var newClient = new Client
                        {
                            Username = NewUsername,
                            Password = actualPassword,
                            FirstName = FirstName,
                            LastName = LastName,
                            Email = Email,
                            Phone = Phone,
                            Street = Street,
                            HouseNumber = HouseNumber,
                            ApartmentNumber = ApartmentNumber,
                            PostalCode = PostalCode,
                            City = City,
                            Country = Country
                        };

                        context.Clients.Add(newClient);
                    }

                    context.SaveChanges();
                }

                string rola = IsRegisteringAsWorker ? "Pracownik" : "Klient";
                MessageBox.Show($"Zarejestrowano pomyślnie jako {rola}!");

                
                Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.DataContext == this)?.Close();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Błąd bazy danych: {ex.Message}");
            }
        }
    }
}