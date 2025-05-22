namespace lumber_app.Models
{
    public class DesiredCut
    {
        public int Id { get; set; } //For list management in UI
        public double length { get; set; }
        public string LengthUnit { get; set; } = "ft";
        public int Quantity { get; set; }

        public double LengthInInches
        {
            get
            {
                return UnitConverter.ConvertToInches(length, LengthUnit);
            }
        }
    }
}