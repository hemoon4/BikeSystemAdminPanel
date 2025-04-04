using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeSystemAdminPanel.Models
{
    public class Station
    {
        public int Id { get; init; }
        public string Address {get; set; }
        public int NumberOfBicyclesHold { get; set; }
        public override string ToString() {
            return $"{Id} {Address} {NumberOfBicyclesHold}";
        }
    }
}