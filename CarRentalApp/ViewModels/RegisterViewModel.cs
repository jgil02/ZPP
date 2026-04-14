using CarRentalApp.Data;
using CarRentalApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BCrypt.Net; // DODANE: obsługa haszowania

namespace CarRentalApp.ViewModels
{
    public partial class RegisterViewModel : BaseViewModel
    {
        // --- DANE KONTA ---
        [ObservableProperty] private string _newUsername = "";
        [ObservableProperty] private bool _isRegisteringAsWorker;

        // --- DANE OSOBOWE ---
        [ObservableProperty] private string _firstName = "";
        [ObservableProperty] private string _lastName = "";
        [ObservableProperty] private string _email = "";
        [ObservableProperty] private string _phone = "";

        // --- ADRES ---
        [ObservableProperty] private string _street = "";
        [ObservableProperty] private string _houseNumber = "";
        [ObservableProperty] private string _apartmentNumber = "";
        [ObservableProperty] private string _postalCode = "";
        [ObservableProperty] private string _city = "";
        [ObservableProperty] private string _country = "Polska";

        [RelayCommand]
        private void Register(object parameter)
        {
            // 1. Odbieranie danych z MultiBindingu (z RegisterView.xaml)
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

            // 2. Walidacja czy pola nie są puste
            if (string.IsNullOrWhiteSpace(NewUsername) ||
                string.IsNullOrWhiteSpace(actualPassword) ||
                string.IsNullOrWhiteSpace(confirmPassword) ||
                string.IsNullOrWhiteSpace(FirstName) ||
                string.IsNullOrWhiteSpace(LastName))
            {
                MessageBox.Show("Podstawowe dane (Login, Hasło, Imię, Nazwisko) są wymagane!");
                return;
            }

            // 3. Sprawdzenie czy hasła są identyczne
            if (actualPassword != confirmPassword)
            {
                MessageBox.Show("Wprowadzone hasła nie są identyczne! Proszę poprawić błąd.");
                return;
            }

            try
            {
                // --- KROK KLUCZOWY: HASZOWANIE HASŁA ---
                // Generujemy bezpieczny skrót (hash) zamiast zapisywać jawny tekst
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(actualPassword);

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
                            Password = hashedPassword, // ZAPISUJEMY HASH
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
                            Password = hashedPassword, // ZAPISUJEMY HASH
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
                MessageBox.Show($"Zarejestrowano pomyślnie jako {rola}! Twoje hasło zostało zabezpieczone.");

                // Zamknięcie okna rejestracji
                Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.DataContext == this)?.Close();
            }
            catch (System.Exception ex)
            {
                // Ta linia wyciąga prawdziwą przyczynę z wnętrza błędu
                string innerError = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

                MessageBox.Show($"Szczegóły błędu bazy: {innerError}");
            }
        }
    }
}