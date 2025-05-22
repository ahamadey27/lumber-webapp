namespace lumber_app.Services
{
    public class UnitConverter
    {
        //Basic conversion factors(to inches)
        private const double InchesPerFoot = 12.0;
        private const double InchesPerMeter = 39.3701;
        private const double InchesPerCentimeter = 0.393701;

        public static double ConvertToInches(double value, string unit)
        {
            return unit.ToLower()
            switch
            {
                "ft" or "feet" or "'" => value * InchesPerFoot,
                "in" or "inches" or "\"" => value,
                "m" or "meters" => value * InchesPerMeter,
                "cm" or "centimeters" => value * InchesPerCentimeter,
                _ => throw new ArgumentException("Unsupported unit for conversion to inches.")
            };
        }

        public static double ConvertFromInches(double inches, string targetUnit)
        {
            return targetUnit.ToLower()
            switch
            {

                "ft" or "feet" => inches / InchesPerFoot,
                "in" or "inc hes" => inches,
                "m" or "meters" => inches / InchesPerMeter,
                "cm" or "centimeters" => inches / InchesPerCentimeter,
                _ => throw new ArgumentException("Unsupported unit for conversion from inches.")

            };
        }

        //Add more conversions as needed 
        public static string FormatInchesToFeetAndInches(double totalInches)
        {
            if (totalInches < 0) totalInches = 0;
            double feet = Math.Floor(totalInches / InchesPerFoot);
            double remainingInches = Math.Round(totalInches % InchesPerFoot, 2); //second parameter round by 2 decimal places
            return $"{feet} ft {remainingInches} in";
        }
    }
}