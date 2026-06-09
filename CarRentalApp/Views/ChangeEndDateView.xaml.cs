using System;
using System.Windows;
using CarRentalApp.ViewModels;

namespace CarRentalApp.Views
{
    public partial class ChangeEndDateView : Window
    {
        public ChangeEndDateView(int reservationId, string vin, DateTime currentEnd)
        {
            try
            {
                InitializeComponent();
                DataContext = new ChangeEndDateViewModel(reservationId, vin, currentEnd);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ChangeEndDateView ctor error:\n{ex.Message}\n\n{ex}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw; 
            }
        }
    }
}