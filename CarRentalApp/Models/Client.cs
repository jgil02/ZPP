using System.ComponentModel.DataAnnotations;

namespace CarRentalApp.Models
{
    public class Client
    {
        [Key]
        public int ClientID { get; set; }
        [Required(ErrorMessage = "First name cannot be empty!")]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Last name cannot be empty!")]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Street cannot be empty!")]
        [StringLength(50)]
        public string Street { get; set; } = string.Empty;
        [Required(ErrorMessage = "House number cannot be empty!")]
        [StringLength(50)]
        public string HouseNumber { get; set; } = string.Empty;

        public string? ApartmentNumber { get; set; }
        [Required(ErrorMessage = "Postal code cannot be empty!")]
        [StringLength(8)]
        public string PostalCode { get; set; } = string.Empty;
        [Required(ErrorMessage = "City cannot be empty!")]
        [StringLength(30)]
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = "Polska";
        [Required(ErrorMessage = "Email cannot be empty!")]
        [StringLength(50)]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Phone cannot be empty!")]
        [StringLength(15)]
        public string Phone { get; set; } = string.Empty;

        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
