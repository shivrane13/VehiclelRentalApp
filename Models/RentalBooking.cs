using System.ComponentModel.DataAnnotations.Schema;
using VehiclelRentalApp.Areas.Identity.Data;

namespace VehiclelRentalApp.Models
{
    public class RentalBooking
    {
        public int Id { get; set; } // Primary Key
        public int VehicleId { get; set; } // Foreign Key
        public Vehicle? Vehicle { get; set; }
        public string? userId { get; set; } // Foreign Key
        public ApplicationUser? User { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalCost { get; set; } // Calculated cost
        public string? Status { get; set; } // e.g., "Confirmed", "Cancelled"
        public bool IsPaid { get; set; }
    }
}