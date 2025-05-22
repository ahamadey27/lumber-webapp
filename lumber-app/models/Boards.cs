namespace lumber_app.Models
{
    public class Board
    {
        public int Id { get; set; } //For list management in UI
        public double Length { get; set; } //Store in const unit 
        public string LengthUnit { get; set; } = "ft"; //default unit
        public int Quantity { get; set; }

        //Help to get length in in inches for calculations
        public double LengthInInches
        {
            get
            {
                return UnitConvert.ConvertToInches(Length, LengthUnit);
            }
        }
    }
}