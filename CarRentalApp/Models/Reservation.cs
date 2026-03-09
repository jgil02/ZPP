using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRentalApp.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }

        public string CarVin { get; set; } = string.Empty;
        [ForeignKey(nameof(CarVin))]
        public virtual Car Car { get; set; } = null!;

        public int ClientId { get; set; }
        [ForeignKey(nameof(ClientId))]
        public virtual Client Client { get; set; } = null!;

        public int WorkerId { get; set; }
        [ForeignKey(nameof(WorkerId))]
        public virtual Worker Worker { get; set; } = null!;

        [Required(ErrorMessage = "StartDate type name cannot be empty!")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "EndDate type name cannot be empty!")]
        public DateTime EndDate { get; set; }
        public decimal TotalPrice { get; set; }
    }
}