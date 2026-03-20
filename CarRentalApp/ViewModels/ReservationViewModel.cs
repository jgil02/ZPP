using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CarRentalApp.Data;
using CarRentalApp.Models;
using System;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApp.ViewModels
{
    public partial class ReservationViewModel : BaseViewModel
    {
        private readonly AppDbContext _context = new();
        private readonly decimal _pricePerDay;

        public string CarVin { get; }
        public string CarFullName { get; private set; } = string.Empty;

        [ObservableProperty]
        private DateTime _startDate = DateTime.Now;

        [ObservableProperty]
        private DateTime _endDate = DateTime.Now.AddDays(3); // Domyślnie na 3 dni

        [ObservableProperty]
        private decimal _totalPrice;

        public ReservationViewModel(string vin)
        {
            CarVin = vin;


            var carFleet = _context.CarFleets.Include(c => c.Car).FirstOrDefault(c => c.Vin == vin);
            if (carFleet != null)
            {
                _pricePerDay = carFleet.Car.PricePerDay;
                CarFullName = $"{carFleet.Car.Brand} {carFleet.Car.Model}";
            }

            CalculatePrice(); 
        }


        partial void OnStartDateChanged(DateTime value) => CalculatePrice();
        partial void OnEndDateChanged(DateTime value) => CalculatePrice();

        private void CalculatePrice()
        {
            int days = (EndDate - StartDate).Days;
            if (days < 1) days = 1; 

            TotalPrice = days * _pricePerDay;
        }

        [RelayCommand]
        private void ConfirmReservation(Window window)
        {

            if (UserSession.CurrentClient == null && UserSession.CurrentWorker == null)
            {
                MessageBox.Show("Błąd sesji: Nie jesteś zalogowany!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            if (StartDate.Date < DateTime.Now.Date)
            {
                MessageBox.Show("Data odbioru nie może być w przeszłości!", "Błąd daty", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (EndDate <= StartDate)
            {
                MessageBox.Show("Data zwrotu musi być późniejsza niż data odbioru!", "Błąd daty", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {

                bool isAlreadyReserved = _context.Reservations.Any(r =>
                    r.CarVin == CarVin &&
                    StartDate < r.EndDate &&
                    EndDate > r.StartDate);

                if (isAlreadyReserved)
                {
                    MessageBox.Show("Niestety, to auto jest już zarezerwowane w wybranym terminie. Wybierz inne daty.",
                                    "Brak dostępności", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return; 
                }

                var reservation = new Reservation
                {
                    CarVin = CarVin,
                    ClientId = UserSession.CurrentClient?.ClientID ?? 1,
                    WorkerId = UserSession.CurrentWorker?.WorkerID ?? 1,
                    StartDate = StartDate,
                    EndDate = EndDate,
                    TotalPrice = TotalPrice
                };

                _context.Reservations.Add(reservation);

                if (StartDate.Date == DateTime.Now.Date)
                {
                    var carFleet = _context.CarFleets.FirstOrDefault(c => c.Vin == CarVin);
                    if (carFleet != null) carFleet.IsAvailable = false;
                }

                _context.SaveChanges();

                MessageBox.Show("Rezerwacja zakończona sukcesem!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                window?.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas zapisu do bazy: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}