using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeSystemAdminPanel.Models
{
    public class Bicycle
    {
        public int Id { get; init; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int StationId { get; init; }
    }
}