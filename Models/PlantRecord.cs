namespace PlantationManagementSystem.Models;

public class PlantRecord
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string PlantType { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string Location { get; set; } = string.Empty;
    public DateTime PlantingDate { get; set; }
    public string Status { get; set; } = "Healthy";
}

public class WorkerRecord
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public decimal Salary { get; set; }
}

public class ActivityRecord
{
    public int Id { get; set; }
    public string ActivityType { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public DateTime ActivityDate { get; set; }
    public int? PlantId { get; set; }
    public string? PlantName { get; set; }
    public int? WorkerId { get; set; }
    public string? WorkerName { get; set; }
    public int DurationMinutes { get; set; }
    public string Notes { get; set; } = string.Empty;
}

public class ExpenseRecord
{
    public int Id { get; set; }
    public string Category { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime ExpenseDate { get; set; }
    public string Description { get; set; } = string.Empty;
}

public class DashboardSummary
{
    public int TotalPlants { get; set; }
    public int TotalWorkers { get; set; }
    public int TotalActivities { get; set; }
    public decimal TotalExpenses { get; set; }
}
