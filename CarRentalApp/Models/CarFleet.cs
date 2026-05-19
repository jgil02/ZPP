using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalApp.Models
{
    public partial class CarFleet : ObservableObject
    {
        [Key]
        [StringLength(17)]
        public string Vin { get; set; } = string.Empty;
        [Required(ErrorMessage = "RegistrationNumber name cannot be empty!")]
        [StringLength(8)]
        public string RegistrationNumber { get; set; } = string.Empty;

        public bool IsAvailable { get; set; } = true;
        public string CarId { get; set; } = string.Empty;

        public int Mileage { get; set; }

        [ForeignKey(nameof(CarId))]
        public virtual Car Car { get; set; } = null!;
        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();


        private bool _isSelectedForCompare;
        [NotMapped]
        public bool IsSelectedForCompare
        {
            get => _isSelectedForCompare;
            set => SetProperty(ref _isSelectedForCompare, value);
        }
    }

}
