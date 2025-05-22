# Project: Lumber Optimizer Web App

**Goal:** To create a web application using ASP.NET Core Razor Pages that provides unit conversion utilities and optimizes cutting plans for lumber based on available stock and desired cuts. This project aims to minimize waste and assist users in efficiently planning their lumber usage.

---

# Components

## Environment/Hosting
- **Local Development Machine (Windows/macOS/Linux)**
  - .NET SDK (8.0 or 7.0)
  - IDE: Visual Studio 2022 (Community Edition) or Visual Studio Code with C# Dev Kit extension
  - Git for version control (implied)
- **Cloud Hosting:** Azure App Service (Free Tier recommended for initial deployment)

## Software Components

### Web Application Backend & Frontend
- **Framework:** ASP.NET Core Razor Pages
- **Language:** C#
- **Frontend Rendering:** Razor Syntax, HTML5, CSS (likely Bootstrap via project default), JavaScript (for dynamic UI elements)

### Core Logic Services
- **Unit Conversion:** Custom static `UnitConverter` class
- **Cutting Optimization:** Custom `ICuttingOptimizer` service with a `SimpleCuttingOptimizer` implementation.

---

# Data Model (Core Classes)

## `Board.cs` (Model)
- `Id` (int) - Identifier for list management in UI.
- `Length` (double) - Length of the board.
- `LengthUnit` (string) - Unit of the length (e.g., "ft", "in", "m", "cm"). Default: "ft".
- `Quantity` (int) - Number of such boards available.
- `LengthInInches` (double, read-only property) - Helper to get length in inches, using `UnitConverter`.

## `DesiredCut.cs` (Model)
- `Id` (int) - Identifier for list management in UI.
- `Length` (double) - Length of the desired cut.
- `LengthUnit` (string) - Unit of the length. Default: "ft".
- `Quantity` (int) - Number of such cuts desired.
- `LengthInInches` (double, read-only property) - Helper to get length in inches, using `UnitConverter`.

## `UnitConverter.cs` (Service - Static Class)
- Provides static methods for converting lengths between different units (ft, in, m, cm), primarily to and from inches.
- Includes `ConvertToInches(double value, string unit)`
- Includes `ConvertFromInches(double inches, string targetUnit)`
- Includes `FormatInchesToFeetAndInches(double totalInches)`

## `ICuttingOptimizer.cs` (Service - Interface & Supporting Classes)

### `CutPlanResult` Class
- `OptimizedCuts` (List<OptimizedCut>) - List of planned cuts.
- `RemainingBoards` (List<Board>) - List of boards with leftover pieces.
- `TotalWasteInches` (double) - Total estimated waste in inches.
- `AdditionalMaterialNeededInches` (double) - Additional material required in inches.
- `Message` (string) - Status message for the optimization process.
- `AdditionalMaterialNeededFormatted` (string, read-only property) - Formatted string for additional material.
- `TotalWasteFormatted` (string, read-only property) - Formatted string for total waste.

### `OptimizedCut` Class
- `OriginalDesiredCut` (DesiredCut) - The original desired cut specification.
- `QuantityToCut` (int) - The quantity of this specific cut from the source board.
- `SourceBoard` (Board) - The available board type from which the cut is made.
- `SourceBoardOriginalIndex` (int) - Index to identify the original board type if multiple are identical.
- `CutLengthInches` (double) - The length of the cut in inches.

### `ICuttingOptimizer` Interface
- Method: `CutPlanResult OptimizeCuts(List<Board> availableBoards, List<DesiredCut> desiredCuts)`

### `BoardPiece.cs` (Helper Class for Optimizer Implementation)
- `OriginalBoard` (Board)
- `OriginalBoardIndex` (int)
- `PieceInstanceId` (int)
- `CurrentLengthInches` (double)
- `CutsMade` (List<OptimizedCutDetail>)

### `OptimizedCutDetail.cs` (Helper Class for Optimizer Implementation)
- `DesiredCut` (DesiredCut)
- `LengthInches` (double)

---

# Development Plan

## Phase 1: Project Setup & Core Models
- [x] Install .NET SDK and choose IDE.
- [x] Sign up for a free Azure account.
- [x] Create ASP.NET Core Razor Pages Project (`LumberOptimizerWeb`).
- [x] Define Core Models:
    - [ ] `Models/Board.cs`
    - [ ] `Models/DesiredCut.cs`
- [x] Create placeholder `Services/UnitConverter.cs`.
- [x] Review basic project structure (`Pages`, `wwwroot`, `Program.cs`).

## Phase 2: Unit Converter Feature
- [x] Create Razor Page `Pages/UnitConverter.cshtml` (and `.cs`).
- [x] Design Unit Converter UI in `UnitConverter.cshtml`.
- [x] Implement logic in `UnitConverter.cshtml.cs` PageModel, utilizing `Services.UnitConverter`.
    - [x] Include properties for `InputValue`, `FromUnit`, `ToUnit`, `Result`, `ErrorMessage`.
    - [x] Populate `AvailableUnits` (SelectListItem).
    - [x] Implement `OnGet()` and `OnPost()` handlers.
- [x] Add navigation link to Unit Converter in `Pages/Shared/_Layout.cshtml`.

## Phase 3: Board-Cut Optimizer - Service Interface & Data Structures
- [x] Define `Services/ICuttingOptimizer.cs` interface and supporting classes:
    - [x] `CutPlanResult` class.
    - [x] `OptimizedCut` class.
- [ ] Create a basic implementation `Services/SimpleCuttingOptimizer.cs` (initial placeholder logic).
- [ ] Register the `ICuttingOptimizer` service in `Program.cs` (`builder.Services.AddScoped<ICuttingOptimizer, SimpleCuttingOptimizer>();`).

## Phase 4: Board-Cut Optimizer - UI for Input
- [ ] Create Razor Page `Pages/BoardOptimizer.cshtml` (and `.cs`).
- [ ] Design Optimizer Input UI in `BoardOptimizer.cshtml`:
    - [ ] Form for `AvailableBoards` with dynamic row adding/removing (Length, Unit, Quantity).
    - [ ] Form for `DesiredCuts` with dynamic row adding/removing (Length, Unit, Quantity).
    - [ ] Section to display `CutResult` (Message, Additional Material, Waste, Cutting Plan table, Remaining Boards list).
- [ ] Implement JavaScript for dynamic row adding/removing in `@section Scripts`.
- [ ] Implement logic in `BoardOptimizer.cshtml.cs` PageModel:
    - [ ] Inject `ICuttingOptimizer`.
    - [ ] Bind properties for `AvailableBoards` (List<Board>), `DesiredCuts` (List<DesiredCut>).
    - [ ] Property for `CutResult` (CutPlanResult).
    - [ ] Populate `AvailableUnits` (SelectListItem).
    - [ ] Implement `OnGet()` (initialize with empty rows if needed).
    - [ ] Implement `OnPost()` to call the optimizer service and handle results/validation.
- [ ] Add navigation link to Board Optimizer in `Pages/Shared/_Layout.cshtml`.

## Phase 5: Board-Cut Optimizer - Algorithm Implementation (Core Logic)
- [ ] Refine `Services/SimpleCuttingOptimizer.cs` with a more functional cutting algorithm (e.g., First Fit Decreasing Height variation).
    - [ ] Introduce internal helper classes if needed (e.g., `BoardPiece`, `OptimizedCutDetail`).
    - [ ] Standardize units to inches for calculations.
    - [ ] Expand board and cut quantities into individual instances.
    - [ ] Sort desired cuts (e.g., by length descending).
    - [ ] Iterate through cuts and find suitable available board pieces.
    - [ ] Update remaining lengths of board pieces.
    - [ ] Track waste and shortfall accurately.
    - [ ] Populate `CutPlanResult` with detailed `OptimizedCuts`, `RemainingBoards`, `TotalWasteInches`, and `AdditionalMaterialNeededInches`.

## Phase 6: Board-Cut Optimizer - Refining Results Display
- [ ] Review and enhance the results display section in `BoardOptimizer.cshtml`.
- [ ] Ensure `CutPlanResult.OptimizedCuts` are presented clearly, possibly grouped.
- [ ] Ensure `CutPlanResult.RemainingBoards` lists usable offcuts effectively.
- [ ] Verify messages for shortfalls, completion, and errors are user-friendly.
- [ ] Consider edge cases in display (no cuts possible, perfect fit, no inputs).

## Phase 7: Integration and Styling
- [ ] Ensure consistent styling using Bootstrap classes.
- [ ] Customize CSS in `wwwroot/css/site.css` if needed.
- [ ] Review User Experience (UX) for intuitiveness and clarity.
- [ ] Implement robust error handling beyond basic form validation.

## Phase 8: Testing
- [ ] **Unit Converter Testing:**
    - [ ] Test various unit conversions (zero, positive, large values).
    - [ ] Test invalid inputs.
- [ ] **Board Optimizer Testing:**
    - [ ] **Input Validation:** Non-numeric, negative, zero quantities/lengths, row limits.
    - [ ] **Algorithm Logic (Critical Scenarios):**
        - [ ] Perfect fit.
        - [ ] Not enough material.
        - [ ] Waste generation.
        - [ ] Multiple boards and multiple cut types.
        - [ ] Small cuts from large boards.
        - [ ] Large cuts from small boards.
    - [ ] **UI Testing:** Dynamic rows, results display, messages.
- [ ] **Cross-Browser Testing (Basic):** Chrome, Firefox, Edge.

## Phase 9: Deployment to Azure App Service (Free Tier)
- [ ] Create Azure App Service resource in the Azure portal.
    - [ ] Configure Runtime stack (.NET 8 or 7), OS, Region.
    - [ ] Select Free (F1) pricing tier.
- [ ] Publish application from Visual Studio to Azure App Service.
- [ ] Test deployed application thoroughly on Azure.
- [ ] Consider CI/CD for future updates (optional).

---