namespace CarRentalApp.Models
{
    public class Car
    {
        public int Id { get; set; } //lub VIN? 
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public decimal PricePerDay { get; set; }
        public bool IsAvailable { get; set; } = true;


    }
}