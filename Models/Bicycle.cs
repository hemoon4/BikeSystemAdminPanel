using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeSystemAdminPanel.Models
{
    public class Bicycle
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public override string ToString() {
            return $"{Id} {Name} {Type}";
        }
    }
}