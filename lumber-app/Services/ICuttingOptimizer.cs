using lumber_app.Models;
using System.Collections.Generic; //provides lists and keyvalue pairs

namespace lumber_app.Services
{
    public class CutPlanResult
    {
        public List<OptimizedCut> OptimizedCuts { get; set; } = new List<OptimizedCut>();
        public List<Board> RemainingBoards { get; set; } = new List<Board>(); //Boards with leftover pieces
        public double TotalWasteInches { get; set; }
        public double AdditionalMaterialNeededInches { get; set; }
        public string Message { get; set; } = string.Empty;

        public string AdditionalMaterialNeededFormatted =>
            Services.UnitConverter.FormatInchesToFeetAndInches(AdditionalMaterialNeededInches);

        public string TotalWasteFormatted =>
            Services.UnitConverter.FormatInchesToFeetAndInches(TotalWasteInches);
    }

    public class OptimizedCut
    {
        public DesiredCut OriginalDesiredCut { get; set; } = new DesiredCut();
        public int QuantityToCut { get; set; }
        public Board SourceBoard { get; set; } = new Board(); //Takes board from models and makes new object 
        public int SourceBoardOriginalIndex { get; set; } // To identify original board if multiple are identical
        public double CutLengthToInches { get; set; }
    }

    public interface ICuttingOptimizer
    {
        CutPlanResult OptimizeCuts( //Hold output of algorimth and detials which cut is taken from which board source
            List<Board> availableBoards,
            List<DesiredCut> desiredCuts);
    }
}