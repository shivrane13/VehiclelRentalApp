using System.ComponentModel.DataAnnotations.Schema;

namespace VehiclelRentalApp.Models
{
    public class VehicalBookingViewModel
    {
        public int Id { get; set; } // Primary Key
        public string? Make { get; set; } // e.g., "Toyota"
        public string? Model { get; set; } // e.g., "Corolla"
        public string? RegistrationNumber { get; set; } // e.g., "MH12AB1234"
        public int? Capacity { get; set; } // e.g., number of seats
        public string? FuelType { get; set; } // e.g., "Petrol", "Diesel", "Electric"
        public bool IsAvailable { get; set; } // True if available for rent
        [Column(TypeName = "decimal(18,2)")]
        public decimal DailyRate { get; set; } // Cost per day
        [Column(TypeName = "decimal(18,2)")]
        public decimal HourlyRate { get; set; } // Cost per hour
        public string? ImageUrl { get; set; }    
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
