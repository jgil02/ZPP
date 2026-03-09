using System.ComponentModel.DataAnnotations;

namespace CarRentalApp.Models
{
    public class Car
    {
        [Key]
        public string Vin { get; set; } = string.Empty;
        [Required(ErrorMessage = "Brand name cannot be empty!")]
        [StringLength(50)]
        public string Brand { get; set; } = string.Empty;
        [Required(ErrorMessage = "Model name cannot be empty!")]
        [StringLength(50)]
        public string Model { get; set; } = string.Empty;

        public int SeatsCount { get; set; }
        public int DoorsCount { get; set; }
        [Required(ErrorMessage = "Gearbox type name cannot be empty!")]
        [StringLength(30)]
        public string GearboxType { get; set; } = string.Empty;
        [Required(ErrorMessage = "Fuel type name cannot be empty!")]
        [StringLength(30)]
        public string FuelType { get; set; } = string.Empty;
        [Required(ErrorMessage = "Body type name cannot be empty!")]
        [StringLength(30)]
        public string BodyType { get; set; } = string.Empty;
        public int TrunkCapacity { get; set; }
        public double EngineCapacity { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    }
}