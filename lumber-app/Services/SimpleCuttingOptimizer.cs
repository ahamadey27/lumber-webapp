using lumber_app.Models;
using System.Collections.Generic;
using System.Linq;

namespace lumber_app.Services
{
    // Represents a piece of an available board, its current length, and cuts made from it.
    public class BoardPiece
    {
        public Board OriginalBoard { get; }
        public int OriginalBoardIndex { get; } // Index in the initial list of board types
        public int PieceInstanceId { get; } // Unique ID for this specific piece (e.g. if BoardType1 has Qty 5, there are 5 pieces)
        public double CurrentLengthInches { get; set; }
        public List<OptimizedCutDetail> CutsMade { get; } = new List<OptimizedCutDetail>();

        public BoardPiece(Board originalBoard, int originalBoardIndex, double initialLengthInches, int pieceInstanceId)
        {
            OriginalBoard = originalBoard;
            OriginalBoardIndex = originalBoardIndex;
            CurrentLengthInches = initialLengthInches;
            PieceInstanceId = pieceInstanceId;
        }
    }

    public class OptimizedCutDetail // More detailed than the result one for internal tracking
    {
        public DesiredCut DesiredCut { get; set; } = new DesiredCut();
        public double LengthInches { get; set; }
    }


    public class SimpleCuttingOptimizer : ICuttingOptimizer
    {
        public CutPlanResult OptimizeCuts(
            List<Board> availableBoardTypes, // Renamed for clarity
            List<DesiredCut> desiredCutTypes) // Renamed for clarity
        {
            var result = new CutPlanResult();

            // 1. Expand available boards by quantity and convert to inches
            var availableBoardPieces = new List<BoardPiece>();
            int pieceInstanceCounter = 0;
            for (int i = 0; i < availableBoardTypes.Count; i++)
            {
                var boardType = availableBoardTypes[i];
                for (int j = 0; j < boardType.Quantity; j++)
                {
                    availableBoardPieces.Add(new BoardPiece(boardType, i, boardType.LengthInInches, pieceInstanceCounter++));
                }
            }

            // 2. Expand desired cuts by quantity, convert to inches, and sort (e.g., by length descending)
            var allDesiredCutInstances = new List<DesiredCut>();
            foreach (var cutType in desiredCutTypes.OrderByDescending(c => c.LengthInInches))
            {
                for (int i = 0; i < cutType.Quantity; i++)
                {
                    // Create a new instance for each quantity to track them individually
                    allDesiredCutInstances.Add(new DesiredCut
                    {
                        Id = cutType.Id, // Keep original ID for grouping later if needed
                        length = cutType.length, // Corrected from Length to length
                        LengthUnit = cutType.LengthUnit,
                        Quantity = 1 // We are expanding, so each instance is 1
                    });
                }
            }

            double totalRequestedLengthInches = allDesiredCutInstances.Sum(c => c.LengthInInches);
            double totalAvailableLengthInches = availableBoardPieces.Sum(bp => bp.CurrentLengthInches);

            // 3. Perform the cutting (Greedy First Fit Decreasing Height - FFDH variation)
            foreach (var desiredCutInstance in allDesiredCutInstances)
            {
                double cutLengthInches = desiredCutInstance.LengthInInches;
                // BoardPiece? bestFitBoardPiece = null; // For Best Fit - Commented out to remove CS0219 warning
                BoardPiece? firstFitBoardPiece = null; // For First Fit

                // Option 1: First Fit (from longest available piece that fits)
                firstFitBoardPiece = availableBoardPieces
                    .Where(bp => bp.CurrentLengthInches >= cutLengthInches)
                    .OrderByDescending(bp => bp.CurrentLengthInches) // Try on longest boards first
                    .FirstOrDefault();

                // Option 2: Best Fit (minimizes immediate waste for this cut)
                // bestFitBoardPiece = availableBoardPieces
                //    .Where(bp => bp.CurrentLengthInches >= cutLengthInches)
                //    .OrderBy(bp => bp.CurrentLengthInches - cutLengthInches) // Smallest leftover
                //    .FirstOrDefault();

                var boardToCutFrom = firstFitBoardPiece; // Choose strategy: firstFitBoardPiece or bestFitBoardPiece

                if (boardToCutFrom != null)
                {
                    result.OptimizedCuts.Add(new OptimizedCut
                    {
                        OriginalDesiredCut = desiredCutInstance, // This is an instance
                        QuantityToCut = 1, // We are processing individual cut instances
                        SourceBoard = boardToCutFrom.OriginalBoard,
                        SourceBoardOriginalIndex = boardToCutFrom.OriginalBoardIndex,
                        CutLengthInches = cutLengthInches
                    });
                    boardToCutFrom.CurrentLengthInches -= cutLengthInches;
                    boardToCutFrom.CutsMade.Add(new OptimizedCutDetail { DesiredCut = desiredCutInstance, LengthInches = cutLengthInches });
                }
                else
                {
                    result.AdditionalMaterialNeededInches += cutLengthInches; // This cut couldn't be made
                }
            }

            // 4. Calculate results
            if (result.AdditionalMaterialNeededInches > 0)
            {
                result.Message = $"Not enough material. Additional needed: {result.AdditionalMaterialNeededFormatted}.";
            }
            else
            {
                result.Message = "Optimization complete. All requested cuts planned.";
            }

            // Calculate total waste from used pieces
            // Waste = Sum of (OriginalLength - Sum of CutsMadeFromIt) for all pieces that had cuts made
            // Or, more simply for this greedy approach: Total initial length of pieces that were touched MINUS total length of useful cuts.
            // This doesn't account for entire boards that were untouched.
            double lengthOfUsedCuts = result.OptimizedCuts.Sum(oc => oc.CutLengthInches);
            double originalLengthOfBoardsTouched = availableBoardPieces
                .Where(bp => bp.CutsMade.Any())
                .Sum(bp => bp.OriginalBoard.LengthInInches); // Sum of original lengths of pieces that were cut

            if (result.AdditionalMaterialNeededInches == 0) // Only calculate waste if no extra material is needed
            {
                // Total waste is the sum of all remaining small pieces on boards that were cut.
                result.TotalWasteInches = availableBoardPieces
                    .Where(bp => bp.CutsMade.Any()) // Only consider boards that were actually used
                    .Sum(bp => bp.CurrentLengthInches); // The remaining length on these boards is waste
            }
            else
            {
                result.TotalWasteInches = 0; // If we need more material, current "waste" isn't the primary concern.
            }


            // Populate remaining boards (pieces with length > 0)
            result.RemainingBoards = availableBoardPieces
                .Where(bp => bp.CurrentLengthInches > 0.01) // Consider a small tolerance for "usable"
                .Select(bp => new Board
                {
                    Id = bp.OriginalBoard.Id, // Link back to original type
                    Length = bp.CurrentLengthInches, // This is remaining length
                    LengthUnit = "in", // It's in inches now
                    Quantity = 1 // Each remaining piece is individual
                })
                .ToList();


            // Consolidate OptimizedCuts for display if desired (e.g., group by desired cut type and source board type)
            // The current display logic in the Razor page does some grouping.

            return result;
        }
    }
}