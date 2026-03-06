using System.ComponentModel.DataAnnotations;

namespace CarRentalApp.Models
{
    public class Car
    {
        public string Vin { get; set; } = string.Empty;

        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int SeatsCount { get; set; }
        public int DoorsCount { get; set; }
        public string GearboxType { get; set; } = string.Empty;
        public string FuelType { get; set; } = string.Empty;
        public string BodyType { get; set; } = string.Empty;
        public int TrunkCapacity { get; set; }


        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>(); //relacja 1 do wiel

    }
}