using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeSystemAdminPanel.Models
{
    public class Rentals
    {
        public int Id { get; init; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string PhoneNumber {get; set; }
        public int BicycleId { get; set; }
        public int StationId { get; set; }
    }
}