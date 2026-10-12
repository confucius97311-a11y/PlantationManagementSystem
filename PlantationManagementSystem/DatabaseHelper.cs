# Plantation Management System

A simple plantation management application built with C# and Windows Forms.

## Features
- Dashboard summary for plants, workers, plantation areas, and expenses
- Plant management: add, update, delete, and view plant records
- Worker management: add, update, delete, and view worker records
- Activity tracking for plantation tasks
- Expense tracking and total expense calculation
- Reports tab for viewing all major records
- SQLite database support for permanent storage

## Technology Stack
- C#
- Windows Forms
- .NET 8
- SQLite

## Project Structure
- `PlantationManagementSystem/Program.cs` - application entry point
- `PlantationManagementSystem/MainForm.cs` - main user interface and logic
- `PlantationManagementSystem/Models.cs` - domain models
- `PlantationManagementSystem/DatabaseHelper.cs` - SQLite database access
- `PlantationManagementSystem/PlantationManagementSystem.csproj` - project configuration

## Database
The project uses SQLite and creates the database file automatically:

`PlantationDB.sqlite`

This database is created in the output folder when the application starts.

## How to Run in Visual Studio
1. Open Visual Studio 2022.
2. Open the `PlantationManagementSystem.sln` file.
3. Restore NuGet packages.
4. Press `F5` to run the project.

## NuGet Package Required
- `Microsoft.Data.Sqlite`

## Notes
This is a beginner-friendly project designed for learning and mini-project development. You can extend it with:
- search filters
- charts
- export to Excel/CSV
- login system
- advanced validation

## License
This project is provided as a sample learning project.
