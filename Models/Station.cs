namespace BikeSystemAdminPanel.Models
{
    public class Station
    {
        public int Id { get; init; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int NumberOfBicyclesHold { get; set; }
    }
}