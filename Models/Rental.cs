using System;

namespace BikeSystemAdminPanel.Models
{
    public class Rental
    {
        public int Id { get; init; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string UserPhoneNumber { get; set; } = string.Empty;
        public int StationId { get; set; }
        public int BicycleId { get; set; }
    }
}