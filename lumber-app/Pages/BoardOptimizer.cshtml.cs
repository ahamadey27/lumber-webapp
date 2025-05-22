// Pages/BoardOptimizer.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using lumber_app.Models;
using lumber_app.Services;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace lumber_app.Pages
{
    public class BoardOptimizerModel : PageModel
    {
        private readonly ICuttingOptimizer _optimizer;

        public BoardOptimizerModel(ICuttingOptimizer optimizer)
        {
            _optimizer = optimizer;
        }

        [BindProperty]
        public List<Board> AvailableBoards { get; set; } = new List<Board>();

        [BindProperty]
        public List<DesiredCut> DesiredCuts { get; set; } = new List<DesiredCut>();

        public CutPlanResult? CutResult { get; set; }

        public List<SelectListItem> AvailableUnits { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "in", Text = "Inches (in)" },
            new SelectListItem { Value = "ft", Text = "Feet (ft)" },
            new SelectListItem { Value = "m", Text = "Meters (m)" },
            new SelectListItem { Value = "cm", Text = "Centimeters (cm)" }
        };

        public void OnGet()
        {
            // Initialize with one empty entry for user to fill, if lists are empty
            // The JS will also handle this if the model starts empty.
            if (!AvailableBoards.Any())
            {
                // AvailableBoards.Add(new Board { Id = 1 }); // JS handles initial add
            }
            if (!DesiredCuts.Any())
            {
                // DesiredCuts.Add(new DesiredCut { Id = 1 }); // JS handles initial add
            }
        }

        public IActionResult OnPost()
        {
            // Server-side validation example for quantities (can also be done with attributes)
            for (int i = 0; i < AvailableBoards.Count; i++)
            {
                if (AvailableBoards[i].Quantity <= 0)
                    ModelState.AddModelError($"AvailableBoards[{i}].Quantity", "Quantity must be positive.");
                if (AvailableBoards[i].Length <= 0)
                    ModelState.AddModelError($"AvailableBoards[{i}].Length", "Length must be positive.");
            }
            for (int i = 0; i < DesiredCuts.Count; i++)
            {
                if (DesiredCuts[i].Quantity <= 0)
                    ModelState.AddModelError($"DesiredCuts[{i}].Quantity", "Quantity must be positive.");
                if (DesiredCuts[i].length <= 0) //### Check length as was changed from Length
                    ModelState.AddModelError($"DesiredCuts[{i}].length", "Length must be positive.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (AvailableBoards.Any() && DesiredCuts.Any())
            {
                // Assign temporary IDs if they are 0, for tracking in results if needed
                // The JS side gives temporary high IDs, so we can use those or re-assign
                int tempIdCounter = 1;
                AvailableBoards.ForEach(b => { if (b.Id == 0) b.Id = tempIdCounter++; });
                tempIdCounter = 1;
                DesiredCuts.ForEach(c => { if (c.Id == 0) c.Id = tempIdCounter++; });

                CutResult = _optimizer.OptimizeCuts(AvailableBoards, DesiredCuts);
            }
            else
            {
                // Handle case where one or both lists might be empty after submission
                // (e.g., if user removes all rows and submits)
                CutResult = new CutPlanResult { Message = "Please provide available boards and desired cuts." };
            }

            return Page();
        }
    }

}