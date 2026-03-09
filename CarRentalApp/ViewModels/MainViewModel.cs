using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;
using CarRentalApp.Models;

namespace CarRentalApp.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {

        public ObservableCollection<Car> Cars { get; set; }

        public MainViewModel()
        {
            // temp dane
            Cars = new ObservableCollection<Car>
            {
                new Car {
                    Brand = "Audi", Model = "RS5", FuelType = "Benzyna", GearboxType = "Automatyczna",
                    PricePerDay = 150, IsAvailable = true,
                    ImagePath = "https://freepngimg.com/thumb/audi/35227-5-audi-rs5-red.png"
                },
                new Car {
                    Brand = "Tesla", Model = "Model S", FuelType = "Elektryczny", GearboxType = "Automatyczna",
                    PricePerDay = 250, IsAvailable = true,
                    ImagePath = "https://freepngimg.com/thumb/audi/35227-5-audi-rs5-red.png"
                },
                new Car {
                    Brand = "BMW", Model = "X5", FuelType = "Diesel", GearboxType = "Automatyczna",
                    PricePerDay = 300, IsAvailable = false,
                    ImagePath = "https://freepngimg.com/thumb/audi/35227-5-audi-rs5-red.png"
                },
                new Car {
                    Brand = "Mazda", Model = "6", FuelType = "Benzyna", GearboxType = "Manualna",
                    PricePerDay = 120, IsAvailable = true,
                    ImagePath = "https://freepngimg.com/thumb/audi/35227-5-audi-rs5-red.png"
                }
            };
        }

        [RelayCommand]
        private void Logout()
        {
            var loginView = new Views.LoginView();
            loginView.Show();

            foreach (Window window in Application.Current.Windows)
            {
                if (window.DataContext == this)
                {
                    window.Close();
                    break;
                }
            }
        }
    }
}