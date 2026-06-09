using CarRentalApp.Data;
using CarRentalApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;

namespace CarRentalApp.ViewModels
{
    public partial class ChangeEndDateViewModel : BaseViewModel
    {
        private readonly int _reservationId;
        private readonly string _vin;
        private readonly DateTime _currentEnd;

        [ObservableProperty]
        private DateTime _newEndDate;

        public DateTime MinDate { get; } = DateTime.Now.Date;
        public DateTime? MaxAllowedDate { get; private set; }

        public ChangeEndDateViewModel(int reservationId, string vin, DateTime currentEnd)
        {
            _reservationId = reservationId;
            _vin = vin;
            _currentEnd = currentEnd.Date;
            NewEndDate = currentEnd.Date;

            ComputeMaxAllowedDate();
        }

        private void ComputeMaxAllowedDate()
        {
            using var context = new AppDbContext();
            var next = context.Reservations
                .Where(r => r.CarVin.Trim() == _vin.Trim() && r.Id != _reservationId && r.StartDate.Date > _currentEnd)
                .OrderBy(r => r.StartDate)
                .FirstOrDefault();

            if (next != null)
            {
                MaxAllowedDate = next.StartDate.Date.AddDays(-1);
            }
            else
            {
                MaxAllowedDate = null;
            }
        }

        [RelayCommand]
        private void Save(Window window)
        {
            if (NewEndDate.Date < DateTime.Now.Date)
            {
                MessageBox.Show("Data zwrotu nie mo¿e byæ wczeœniejsza ni¿ dzisiaj.", "B³¹d daty", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MaxAllowedDate.HasValue && NewEndDate.Date > MaxAllowedDate.Value)
            {
                MessageBox.Show($"Auto ma kolejn¹ rezerwacjê. Maksymalna dostêpna data: {MaxAllowedDate:yyyy-MM-dd}", "Konflikt z rezerwacj¹", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using var context = new AppDbContext();
                var reservation = context.Reservations.Include(r => r.CarFleet).ThenInclude(cf => cf.Car).FirstOrDefault(r => r.Id == _reservationId);
                if (reservation == null)
                {
                    MessageBox.Show("Nie odnaleziono rezerwacji.", "B³¹d", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                reservation.EndDate = NewEndDate.Date;

                var car = context.CarFleets.Include(cf => cf.Car).FirstOrDefault(cf => cf.Vin.Trim() == _vin.Trim())?.Car;
                decimal pricePerDay = car?.PricePerDay ?? 0m;
                int days = (reservation.EndDate.Date - reservation.StartDate.Date).Days + 1;
                if (days < 1) days = 1;
                reservation.TotalPrice = days * pricePerDay;

                context.SaveChanges();

                MessageBox.Show("Termin zwrotu zosta³ zaktualizowany.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                window?.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"B³¹d podczas zapisu: {ex.Message}", "B³¹d", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private void Cancel(Window window)
        {
            window?.Close();
        }
    }
}