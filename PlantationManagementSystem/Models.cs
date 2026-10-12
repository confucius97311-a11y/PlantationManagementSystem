using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PlantationManagementSystem
{
    public class Plant
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public DateTime PlantingDate { get; set; }
        public string GrowthStatus { get; set; } = string.Empty;
        public string AreaName { get; set; } = string.Empty;
    }

    public class Worker
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
        public string JobRole { get; set; } = string.Empty;
        public decimal Salary { get; set; }
    }

    public class Activity
    {
        public int Id { get; set; }
        public DateTime ActivityDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public int WorkerId { get; set; }
        public string WorkerName { get; set; } = string.Empty;
    }

    public class Expense
    {
        public int Id { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string Category { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    public class DashboardStats
    {
        public int TotalPlants { get; set; }
        public int TotalWorkers { get; set; }
        public int TotalAreas { get; set; }
        public decimal TotalExpenses { get; set; }
    }
}
