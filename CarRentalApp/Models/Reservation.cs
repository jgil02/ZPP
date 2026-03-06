using System;

namespace CarRentalApp.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        public string CarVin { get; set; } = string.Empty;
        public virtual Car Car { get; set; } = null!;

        public int ClientId { get; set; }
        public virtual Client Client { get; set; } = null!;

        public int WorkerId { get; set; }
        public virtual Worker Worker { get; set; } = null!;


        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalPrice { get; set; }
    }
}