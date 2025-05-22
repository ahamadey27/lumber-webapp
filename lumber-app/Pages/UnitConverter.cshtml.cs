// Pages/UnitConverter.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using lumber_app.Services; // For our static UnitConverter

namespace lumber_app.Pages
{
    public class UnitConverterModel : PageModel
    {
        [BindProperty]
        [Required]
        public double? InputValue { get; set; }

        [BindProperty]
        [Required]
        public string FromUnit { get; set; } = "ft";

        [BindProperty]
        [Required]
        public string ToUnit { get; set; } = "in";

        public double? Result { get; set; }
        public string? ErrorMessage { get; set; }

        public List<SelectListItem> AvailableUnits { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "in", Text = "Inches (in)" },
            new SelectListItem { Value = "ft", Text = "Feet (ft)" },
            new SelectListItem { Value = "m", Text = "Meters (m)" },
            new SelectListItem { Value = "cm", Text = "Centimeters (cm)" }
            // Add more units as desired
        };

        public void OnGet()
        {
            // Optional: Set default values or load something on page load
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (InputValue.HasValue)
            {
                try
                {
                    double valueInInches = Services.UnitConverter.ConvertToInches(InputValue.Value, FromUnit);
                    Result = Services.UnitConverter.ConvertFromInches(valueInInches, ToUnit);
                    ErrorMessage = null;
                }
                catch (ArgumentException ex)
                {
                    Result = null;
                    ErrorMessage = ex.Message;
                }
            }
            return Page();
        }
    }
}
