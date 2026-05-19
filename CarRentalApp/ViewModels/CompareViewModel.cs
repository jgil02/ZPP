using CarRentalApp.Data;
using CarRentalApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;

namespace CarRentalApp.ViewModels
{
    public partial class CompareViewModel : BaseViewModel
    {
        public ObservableCollection<CarCompareItem> CarsToCompare { get; set; } = new();

        public CompareViewModel(List<string> carIds)
        {
            using var context = new AppDbContext();
            var cars = context.Cars.Where(c => carIds.Contains(c.IdCar)).ToList();

            if (cars.Any())
            {
                var minPrice = cars.Min(c => c.PricePerDay);
                var maxEngine = cars.Max(c => c.EngineCapacity);
                var maxTrunk = cars.Max(c => c.TrunkCapacity);
                var maxDoors = cars.Max(c => c.DoorsCount);
                var maxSeats = cars.Max(c => c.SeatsCount);

                foreach (var car in cars)
                {
                    CarsToCompare.Add(new CarCompareItem
                    {
                        Car = car,
                        PriceColor = car.PricePerDay == minPrice ? Brushes.Green : Brushes.Red,
                        EngineColor = car.EngineCapacity == maxEngine ? Brushes.Green : Brushes.Red,
                        TrunkColor = car.TrunkCapacity == maxTrunk ? Brushes.Green : Brushes.Red,
                        DoorsColor = car.DoorsCount == maxDoors ? Brushes.Green : Brushes.Red,
                        SeatsColor = car.SeatsCount == maxSeats ? Brushes.Green : Brushes.Red
                    });
                }
            }
        }
    }

    public class CarCompareItem
    {
        public Car Car { get; set; } = null!;
        public Brush PriceColor { get; set; } = Brushes.Black;
        public Brush EngineColor { get; set; } = Brushes.Black;
        public Brush TrunkColor { get; set; } = Brushes.Black;
        public Brush DoorsColor { get; set; } = Brushes.Black;
        public Brush SeatsColor { get; set; } = Brushes.Black;
    }
}