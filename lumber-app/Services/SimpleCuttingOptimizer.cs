using lumber_app.Models;
using System.Collections.Generic;
using System.Linq;

namespace lumber_app.Services
{
    // Helper class to track individual board pieces with mutable length
    internal class MutableBoardPiece
    {
        public required Board OriginalBoard { get; set; }
        public int OriginalIndex { get; set; }
        public double CurrentLengthInches { get; set; }
        public int PieceId { get; set; }
        public List<OptimizedCut> CutsMadeFromThisPiece { get; set; } = new List<OptimizedCut>();
    }

    public class SimpleCuttingOptimizer : ICuttingOptimizer
    {
        public CutPlanResult OptimizeCuts(
           List<Board> availableBoards,
           List<DesiredCut> desiredCuts)
        {
            var result = new CutPlanResult();
            // --- Initial Setup ---
            // Convert all board lengths to inches and expand quantities
            var allAvailableBoardPieces = new List<(Board originalBoard, int originalIndex, double lengthInches, int pieceId)>();
            int boardIdx = 0;
            int pieceCounter = 0;
            foreach (var board in availableBoards)
            {
                for (int i = 0; i < board.Quantity; i++)
                {
                    allAvailableBoardPieces.Add((board, boardIdx, board.LengthInInches, pieceCounter++));
                }
                boardIdx++;

            }

            // Convert all desired cut lengths to inches and sort by length (descending often good for greedy)
            // Also expand quantities
            var allDesiredCutPieces = new List<(DesiredCut originalCut, double lengthInches, int cutId)>();
            int cutIdx = 0;
            foreach (var cut in desiredCuts.OrderByDescending(c => c.LengthInInches))
            {
                for (int i = 0; i < cut.Quantity; i++)
                {
                    allDesiredCutPieces.Add((cut, cut.LengthInInches, cutIdx++));
                }
            }

            // --- Simple Greedy Algorithm (First Fit Decreasing Height) ---
            var tempBoardPieces = allAvailableBoardPieces.Select(b => new MutableBoardPiece // Changed from anonymous type
            {
                OriginalBoard = b.originalBoard,
                OriginalIndex = b.originalIndex,
                CurrentLengthInches = b.lengthInches,
                PieceId = b.pieceId
                // CutsMadeFromThisPiece is initialized to an empty list by the MutableBoardPiece class
            }).ToList();

            double totalDesiredLength = allDesiredCutPieces.Sum(c => c.lengthInches);
            double totalAvailableLength = allAvailableBoardPieces.Sum(b => b.lengthInches);
            result.AdditionalMaterialNeededInches = Math.Max(0, totalDesiredLength - totalAvailableLength);

            int desiredCutsFulfilled = 0;

            foreach (var desiredCutItem in allDesiredCutPieces)
            {
                bool cutMade = false;
                // Try to fit this cut into an existing board piece
                var bestFitBoard = tempBoardPieces // bestFitBoard is now a MutableBoardPiece
                    .Where(bp => bp.CurrentLengthInches >= desiredCutItem.lengthInches)
                    .OrderBy(bp => bp.CurrentLengthInches - desiredCutItem.lengthInches) // Best fit
                                                                                         // .ThenByDescending(bp => bp.CurrentLengthInches) // Or First fit from longest
                    .FirstOrDefault();

                if (bestFitBoard != null)
                {
                    var newOptimizedCut = new OptimizedCut
                    {
                        OriginalDesiredCut = desiredCutItem.originalCut,
                        QuantityToCut = 1, // Since we expanded cuts
                        SourceBoard = bestFitBoard.OriginalBoard,
                        SourceBoardOriginalIndex = bestFitBoard.OriginalIndex,
                        CutLengthToInches = desiredCutItem.lengthInches // Using CutLengthInches as per your visible code
                    };
                    result.OptimizedCuts.Add(newOptimizedCut);
                    
                    // Update the board piece directly
                    bestFitBoard.CurrentLengthInches -= desiredCutItem.lengthInches;
                    bestFitBoard.CutsMadeFromThisPiece.Add(newOptimizedCut); // Track the cut on this specific board piece

                    desiredCutsFulfilled++;
                    cutMade = true;
                }

                if (!cutMade)
                {
                    // This cut could not be made from available stock.
                    // The AdditionalMaterialNeededInches already accounts for total shortfall.
                    // You might want more granular tracking of unfulfilled cuts.
                }
            }

            if (desiredCutsFulfilled < allDesiredCutPieces.Count)
            {
                result.Message = $"Could not fulfill all desired cuts. {allDesiredCutPieces.Count - desiredCutsFulfilled} cuts remaining.";
            }
            else if (result.AdditionalMaterialNeededInches > 0)
            {
                result.Message = $"Not enough material. Additional needed: {result.AdditionalMaterialNeededFormatted}";
            }
            else
            {
                result.Message = "Optimization complete.";
            }

            // Calculate waste (this is also simplified)
            // True waste is (Total original length of boards used) - (Total length of useful cuts made from them)
            // Leftover pieces are also important.
            // This placeholder doesn't accurately calculate waste or remaining pieces yet.
            result.TotalWasteInches = Math.Max(0, totalAvailableLength - totalDesiredLength);
            if (result.AdditionalMaterialNeededInches > 0) result.TotalWasteInches = 0;


            // This is a VERY basic placeholder. The actual algorithm is the core challenge.
            // We will refine this in Phase 5.
            // For now, it just signals if there's enough total length.
            result.Message = "Optimizer logic is a placeholder. Actual cutting plan not yet generated.";
            return result;
        }

    }

    
}