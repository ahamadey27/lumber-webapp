# Lumber Optimizer Web App
Efficient lumber cutting optimization and unit conversion tool built with ASP.NET Core Razor Pages

## Project Overview
The Lumber Optimizer Web App is a specialized tool designed for carpenters, woodworkers, and DIY enthusiasts to optimize lumber usage and minimize waste. Built with ASP.NET Core Razor Pages, this application provides intelligent cutting optimization algorithms and comprehensive unit conversion utilities to help users plan their lumber projects efficiently.

## Features
- **Unit Converter**: Convert between different measurement units (feet, inches, meters, centimeters)
- **Board-Cut Optimizer**: Intelligent cutting plan generation to minimize waste
- **Multi-unit Support**: Work with mixed units across different lumber pieces
- **Waste Calculation**: Detailed waste analysis and material efficiency reporting
- **Dynamic Input Management**: Add/remove lumber boards and desired cuts dynamically
- **Comprehensive Results**: Detailed cutting plans, remaining materials, and optimization statistics
- **Responsive Design**: Works seamlessly across desktop and mobile devices
- **Real-time Calculations**: Instant optimization results and unit conversions

## How It Was Built
- **Backend**: ASP.NET Core Razor Pages (.NET 8.0)
- **Frontend**: Server-rendered Razor Pages with Bootstrap for responsive UI
- **Languages**: C# (backend logic), HTML5, CSS3, JavaScript (dynamic UI interactions)
- **Optimization Algorithm**: Custom Greedy Algorithm implementation for lumber cutting optimization
- **Architecture**: Service-oriented design with dependency injection
- **Deployment**: Azure App Service ready with cloud deployment configuration

## Core Components

### Models
- **Board**: Represents available lumber with length, unit, and quantity
- **DesiredCut**: Represents required cuts with specifications
- **CutPlanResult**: Comprehensive optimization results with waste analysis

### Services
- **UnitConverter**: Static utility for length unit conversions
- **ICuttingOptimizer**: Interface for cutting optimization algorithms
- **SimpleCuttingOptimizer**: Implementation of efficient cutting algorithm

### Pages
- **Unit Converter**: Interactive tool for measurement conversions
- **Board Optimizer**: Main optimization interface for lumber cutting plans
- **Responsive Layout**: Consistent navigation and mobile-friendly design

## Algorithm Features
The cutting optimization algorithm includes:
- **Greedy First Fit Decreasing**: Optimized greedy algorithm for minimal waste
- **Multi-board Support**: Handle various lumber sizes and quantities
- **Precise Calculations**: Accurate waste and efficiency calculations
- **Material Planning**: Identifies additional material requirements
- **Detailed Reporting**: Complete cutting plans with source board tracking

## Getting Started

### Prerequisites
- .NET 8.0 SDK or later
- Visual Studio 2022 (Community Edition) or VS Code with C# Dev Kit
- Git for version control

### Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/lumber-webapp.git
   cd lumber-webapp/lumber-app
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Run the application:
   ```bash
   dotnet run
   ```

4. Open your browser and navigate to `https://localhost:7000` (or the port shown in the console)

### Usage
1. **Unit Converter**: Enter values and select units for instant conversions
2. **Board Optimizer**: 
   - Add your available lumber boards (length, unit, quantity)
   - Specify your desired cuts (length, unit, quantity)
   - Click "Optimize" to generate the most efficient cutting plan
   - Review waste analysis and remaining materials

## Deployment
The application is configured for easy deployment to Azure App Service:

1. Create an Azure App Service resource
2. Configure for .NET 8.0 runtime
3. Deploy directly from Visual Studio or via Azure DevOps
4. The Free (F1) tier is sufficient for initial deployment

## Technical Highlights
- **Clean Architecture**: Separation of concerns with service layers
- **Dependency Injection**: Modern ASP.NET Core DI patterns
- **Responsive Design**: Bootstrap-based mobile-first UI
- **Input Validation**: Comprehensive client and server-side validation
- **Error Handling**: Robust error management and user feedback
- **Performance Optimized**: Efficient algorithms for real-time calculations

## Development Status
This project demonstrates:
- ✅ Complete unit conversion functionality
- ✅ Advanced cutting optimization algorithm
- ✅ Dynamic UI with JavaScript interactions
- ✅ Comprehensive testing scenarios
- ✅ Production-ready deployment configuration
- ✅ Professional code organization and documentation

## Use Cases
- **Professional Carpenters**: Optimize material usage for construction projects
- **Woodworking Hobbyists**: Plan cuts for furniture and craft projects
- **Construction Planning**: Minimize waste and reduce material costs
- **Educational Tool**: Learn about optimization algorithms and web development
- **Portfolio Project**: Demonstrates full-stack ASP.NET Core development skills

---

This project showcases modern web development practices with ASP.NET Core, practical algorithm implementation, and real-world problem-solving for the woodworking and construction industries. Perfect for developers looking to demonstrate both technical skills and practical application development.