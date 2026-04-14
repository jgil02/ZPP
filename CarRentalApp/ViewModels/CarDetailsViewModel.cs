using CarRentalApp.Data;
using CarRentalApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Linq;

namespace CarRentalApp.ViewModels
{
    public partial class CarDetailsViewModel : BaseViewModel
    {
        [ObservableProperty]
        private Car _selectedCar;

        public CarDetailsViewModel(string carId)
        {
            using (var context = new AppDbContext())
            {
                SelectedCar = context.Cars.FirstOrDefault(c => c.IdCar == carId);
            }
        }
    }
}