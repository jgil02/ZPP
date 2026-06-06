using CarRentalApp.Data;
using CarRentalApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Linq;

namespace CarRentalApp.ViewModels
{
    public partial class AddCarViewModel : BaseViewModel
    {
        
        [ObservableProperty] private string _idCar = "";
        [ObservableProperty] private string _brand = "";
        [ObservableProperty] private string _model = "";
        [ObservableProperty] private decimal _pricePerDay;
        [ObservableProperty] private string _imagePath = "";
        [ObservableProperty] private int _seatsCount = 5;
        [ObservableProperty] private string _segment = "";
        [ObservableProperty] private int _doorsCount = 5;
        [ObservableProperty] private string _gearboxType = "";
        [ObservableProperty] private string _fuelType = "";
        [ObservableProperty] private string _bodyType = "";
        [ObservableProperty] private int _trunkCapacity;
        [ObservableProperty] private double _engineCapacity;

        
        [ObservableProperty] private string _vin = "";
        [ObservableProperty] private string _registrationNumber = "";
        [ObservableProperty] private int _mileage;

        [RelayCommand]
        private void SaveCar()
        {
            
            if (string.IsNullOrWhiteSpace(IdCar) || string.IsNullOrWhiteSpace(Vin) || string.IsNullOrWhiteSpace(RegistrationNumber))
            {
                MessageBox.Show("Pola: Id-Modelu, VIN oraz Numer Rejestracyjny są wymagane!");
                return;
            }

            try
            {
                using (var context = new AppDbContext())
                {
                    
                    var existingCarModel = context.Cars.FirstOrDefault(c => c.IdCar == IdCar);

                    if (existingCarModel == null)
                    {
                        
                        existingCarModel = new Car
                        {
                            IdCar = IdCar,
                            Brand = Brand,
                            Model = Model,
                            PricePerDay = PricePerDay,
                            ImagePath = ImagePath,
                            SeatsCount = SeatsCount,
                            Segment = Segment,
                            DoorsCount = DoorsCount,
                            GearboxType = GearboxType,
                            FuelType = FuelType,
                            BodyType = BodyType,
                            TrunkCapacity = TrunkCapacity,
                            EngineCapacity = EngineCapacity
                        };
                        context.Cars.Add(existingCarModel);
                    }

                    
                    var newFleetItem = new CarFleet
                    {
                        Vin = Vin,
                        RegistrationNumber = RegistrationNumber,
                        CarId = IdCar, 
                        Mileage = Mileage,
                        IsAvailable = true
                    };

                    context.CarFleets.Add(newFleetItem);
                    context.SaveChanges();
                }

                MessageBox.Show($"Sukces! Dodano model {Brand} {Model} (VIN: {Vin})");
                ClearForm();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Błąd bazy danych: {ex.Message}");
            }
        }

        private void ClearForm()
        {
            IdCar = Brand = Model = ImagePath = Segment = GearboxType = FuelType = BodyType = Vin = RegistrationNumber = "";
            PricePerDay = 0;
            SeatsCount = 0;
            DoorsCount = 0;
            TrunkCapacity = 0;
            EngineCapacity = 0;
            Mileage = 0;
        }
    }
}