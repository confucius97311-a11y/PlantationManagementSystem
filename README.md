# Plantation Management System

A beginner-friendly Windows Forms C# project for managing plantation records, workers, activities, expenses, and reports using SQLite.

## Features

- Dashboard with total plants, workers, activities, and expenses
- Plant management: add, view, edit, and delete
- Worker management: track names, phone numbers, job roles, and salaries
- Activity management: watering, fertilizing, pruning, and harvesting
- Expense management: record and calculate total plantation expenses
- Reports: view detailed records for plants, workers, activities, and expenses
- SQLite database for permanent data storage
- Green-themed, simple user interface

## Project Structure

- `PlantationManagementSystem.csproj` – project file with SQLite dependency
- `Program.cs` – entry point for the application
- `Database/DatabaseHelper.cs` – SQLite connection and CRUD logic
- `Forms/` – dashboard, plant, worker, activity, expense, and report forms
- `Models/` – model classes for database records

## Requirements

- Windows 10/11
- Visual Studio 2022
- .NET 8 SDK
- SQLite support via NuGet package

## Installation

1. Install Visual Studio 2022 with the following workloads:
   - .NET desktop development
2. Install .NET 8 SDK if not already installed.
3. Open the `PlantationManagementSystem.sln` solution in Visual Studio.
4. Restore NuGet packages.

## Build

From the project folder:

```bash
dotnet restore
dotnet build
```

## Run

From the project folder:

```bash
dotnet run --project PlantationManagementSystem.csproj
```

Or run the project directly in Visual Studio by pressing F5.

## Database

The application creates a SQLite database file named `plantation.db` automatically in the application folder when the program starts.

## Notes

This project is designed as a simple college mini-project and uses easy-to-understand C# code with standard Windows Forms controls.

---

Project title: Plantation Management System
Technology: C#, Windows Forms, SQLite, Visual Studio

