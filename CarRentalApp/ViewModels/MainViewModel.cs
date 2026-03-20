using CarRentalApp.Data;
using CarRentalApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApp.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {
        
        
        [ObservableProperty]
        private bool _isCarsVisible = true; 

        [ObservableProperty]
        private bool _isHistoryVisible = false; 

        public ObservableCollection<CarFleet> Cars { get; set; } = new();
        public ObservableCollection<ReservationHistoryItem> ReservationsHistory { get; set; } = new();

        public MainViewModel()
        {          
            LoadCarsFromDatabase();
        }

        private void LoadCarsFromDatabase()
        {
            using (var context = new AppDbContext())
            {
                var data = context.CarFleets
                    .Include(c => c.Car)
                    .ToList();

                Cars = new ObservableCollection<CarFleet>(data);
            }
        }

        [RelayCommand]
        private void ShowCars()
        {
            IsCarsVisible = true;
            IsHistoryVisible = false;
        }


        [RelayCommand]
        private void ShowHistory()
        {
            
            if (UserSession.CurrentClient == null)
            {
                MessageBox.Show("Sesja wygasła lub nie jesteś zalogowany jako klient.");
                return;
            }

            try
            {
                using (var context = new AppDbContext())
                {
                    
                    var history = context.Reservations
                        .Where(r => r.ClientId == UserSession.CurrentClient.ClientID)
                        .Select(r => new ReservationHistoryItem
                        {
                            
                            CarName = r.CarFleet.Car.Brand + " " + r.CarFleet.Car.Model,
                            Vin = r.CarVin,
                            Dates = r.StartDate.ToString("dd.MM.yyyy") + " - " + r.EndDate.ToString("dd.MM.yyyy"),
                            TotalPrice = r.TotalPrice
                        })
                        .ToList();

                    
                    ReservationsHistory.Clear();
                    foreach (var item in history)
                    {
                        ReservationsHistory.Add(item);
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Błąd podczas pobierania historii: {ex.Message}");
            }

            
            IsCarsVisible = false;
            IsHistoryVisible = true;
        }

        [RelayCommand]
        private void OpenReservation(string vin)
        {

            var selectedCar = Cars.FirstOrDefault(c => c.Vin == vin);

            if (selectedCar != null)
            {
                var reservationView = new Views.ReservationView(selectedCar.Vin);
                reservationView.ShowDialog();
            }
        }

        [RelayCommand]
        private void Logout()
        {
            var loginView = new Views.LoginView();
            loginView.Show();

            Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is Views.MainView)?.Close();
        }

        public class ReservationHistoryItem
        {
            public string CarName { get; set; } = string.Empty;
            public string Vin { get; set; } = string.Empty;
            public string Dates { get; set; } = string.Empty;
            public decimal TotalPrice { get; set; }
        }
    }
}