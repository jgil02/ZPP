using System.ComponentModel.DataAnnotations;

namespace CarRentalApp.Models
{
    public class Worker
    {
        [Key]
        public int WorkerID { get; set; }
        [Required(ErrorMessage = "Login cannot be empty!")]
        [StringLength(30)]
        public string Username { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password cannot be empty!")]
        [StringLength(30)]
        public string Password { get; set; } = string.Empty;
        [Required(ErrorMessage = "First name cannot be empty!")]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Last name cannot be empty!")]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Position cannot be empty!")]
        [StringLength(50)]
        public string Position { get; set; } = string.Empty;

        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    }
}
