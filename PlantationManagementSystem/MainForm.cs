using Microsoft.Data.Sqlite;
using System;
using System.Data;
using System.IO;

namespace PlantationManagementSystem
{
    public class DatabaseHelper
    {
        private readonly string _databasePath;

        public DatabaseHelper()
        {
            _databasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PlantationDB.sqlite");
            InitializeDatabase();
        }

        public string ConnectionString => $"Data Source={_databasePath}";

        public void InitializeDatabase()
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Plants (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Type TEXT NOT NULL,
                    Quantity INTEGER NOT NULL,
                    PlantingDate TEXT NOT NULL,
                    GrowthStatus TEXT NOT NULL,
                    AreaName TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS Workers (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    ContactNumber TEXT NOT NULL,
                    JobRole TEXT NOT NULL,
                    Salary REAL NOT NULL
                );

                CREATE TABLE IF NOT EXISTS Activities (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    ActivityDate TEXT NOT NULL,
                    Description TEXT NOT NULL,
                    WorkerId INTEGER NOT NULL
                );

                CREATE TABLE IF NOT EXISTS Expenses (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    ExpenseDate TEXT NOT NULL,
                    Category TEXT NOT NULL,
                    Amount REAL NOT NULL,
                    Description TEXT NOT NULL
                );";

            command.ExecuteNonQuery();
        }

        public int AddPlant(Plant plant)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Plants (Name, Type, Quantity, PlantingDate, GrowthStatus, AreaName)
                VALUES (@Name, @Type, @Quantity, @PlantingDate, @GrowthStatus, @AreaName)";

            command.Parameters.AddWithValue("@Name", plant.Name);
            command.Parameters.AddWithValue("@Type", plant.Type);
            command.Parameters.AddWithValue("@Quantity", plant.Quantity);
            command.Parameters.AddWithValue("@PlantingDate", plant.PlantingDate.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@GrowthStatus", plant.GrowthStatus);
            command.Parameters.AddWithValue("@AreaName", plant.AreaName);

            command.ExecuteNonQuery();
            return (int)connection.LastInsertRowId;
        }

        public void UpdatePlant(Plant plant)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                UPDATE Plants
                SET Name = @Name,
                    Type = @Type,
                    Quantity = @Quantity,
                    PlantingDate = @PlantingDate,
                    GrowthStatus = @GrowthStatus,
                    AreaName = @AreaName
                WHERE Id = @Id";

            command.Parameters.AddWithValue("@Id", plant.Id);
            command.Parameters.AddWithValue("@Name", plant.Name);
            command.Parameters.AddWithValue("@Type", plant.Type);
            command.Parameters.AddWithValue("@Quantity", plant.Quantity);
            command.Parameters.AddWithValue("@PlantingDate", plant.PlantingDate.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@GrowthStatus", plant.GrowthStatus);
            command.Parameters.AddWithValue("@AreaName", plant.AreaName);

            command.ExecuteNonQuery();
        }

        public void DeletePlant(int id)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Plants WHERE Id = @Id";
            command.Parameters.AddWithValue("@Id", id);
            command.ExecuteNonQuery();
        }

        public DataTable GetPlants()
        {
            var table = new DataTable();
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT Id, Name, Type, Quantity, PlantingDate, GrowthStatus, AreaName
                FROM Plants
                ORDER BY Id DESC";

            using var reader = command.ExecuteReader();
            table.Load(reader);
            return table;
        }

        public int AddWorker(Worker worker)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Workers (Name, ContactNumber, JobRole, Salary)
                VALUES (@Name, @ContactNumber, @JobRole, @Salary)";

            command.Parameters.AddWithValue("@Name", worker.Name);
            command.Parameters.AddWithValue("@ContactNumber", worker.ContactNumber);
            command.Parameters.AddWithValue("@JobRole", worker.JobRole);
            command.Parameters.AddWithValue("@Salary", worker.Salary);

            command.ExecuteNonQuery();
            return (int)connection.LastInsertRowId;
        }

        public void UpdateWorker(Worker worker)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                UPDATE Workers
                SET Name = @Name,
                    ContactNumber = @ContactNumber,
                    JobRole = @JobRole,
                    Salary = @Salary
                WHERE Id = @Id";

            command.Parameters.AddWithValue("@Id", worker.Id);
            command.Parameters.AddWithValue("@Name", worker.Name);
            command.Parameters.AddWithValue("@ContactNumber", worker.ContactNumber);
            command.Parameters.AddWithValue("@JobRole", worker.JobRole);
            command.Parameters.AddWithValue("@Salary", worker.Salary);

            command.ExecuteNonQuery();
        }

        public void DeleteWorker(int id)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Workers WHERE Id = @Id";
            command.Parameters.AddWithValue("@Id", id);
            command.ExecuteNonQuery();
        }

        public DataTable GetWorkers()
        {
            var table = new DataTable();
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT Id, Name, ContactNumber, JobRole, Salary
                FROM Workers
                ORDER BY Id DESC";

            using var reader = command.ExecuteReader();
            table.Load(reader);
            return table;
        }

        public int AddActivity(Activity activity)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Activities (ActivityDate, Description, WorkerId)
                VALUES (@ActivityDate, @Description, @WorkerId)";

            command.Parameters.AddWithValue("@ActivityDate", activity.ActivityDate.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@Description", activity.Description);
            command.Parameters.AddWithValue("@WorkerId", activity.WorkerId);

            command.ExecuteNonQuery();
            return (int)connection.LastInsertRowId;
        }

        public void UpdateActivity(Activity activity)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                UPDATE Activities
                SET ActivityDate = @ActivityDate,
                    Description = @Description,
                    WorkerId = @WorkerId
                WHERE Id = @Id";

            command.Parameters.AddWithValue("@Id", activity.Id);
            command.Parameters.AddWithValue("@ActivityDate", activity.ActivityDate.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@Description", activity.Description);
            command.Parameters.AddWithValue("@WorkerId", activity.WorkerId);

            command.ExecuteNonQuery();
        }

        public void DeleteActivity(int id)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Activities WHERE Id = @Id";
            command.Parameters.AddWithValue("@Id", id);
            command.ExecuteNonQuery();
        }

        public DataTable GetActivities()
        {
            var table = new DataTable();
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT a.Id, a.ActivityDate, a.Description, w.Name AS WorkerName
                FROM Activities a
                LEFT JOIN Workers w ON a.WorkerId = w.Id
                ORDER BY a.Id DESC";

            using var reader = command.ExecuteReader();
            table.Load(reader);
            return table;
        }

        public int AddExpense(Expense expense)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Expenses (ExpenseDate, Category, Amount, Description)
                VALUES (@ExpenseDate, @Category, @Amount, @Description)";

            command.Parameters.AddWithValue("@ExpenseDate", expense.ExpenseDate.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@Category", expense.Category);
            command.Parameters.AddWithValue("@Amount", expense.Amount);
            command.Parameters.AddWithValue("@Description", expense.Description);

            command.ExecuteNonQuery();
            return (int)connection.LastInsertRowId;
        }

        public void UpdateExpense(Expense expense)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                UPDATE Expenses
                SET ExpenseDate = @ExpenseDate,
                    Category = @Category,
                    Amount = @Amount,
                    Description = @Description
                WHERE Id = @Id";

            command.Parameters.AddWithValue("@Id", expense.Id);
            command.Parameters.AddWithValue("@ExpenseDate", expense.ExpenseDate.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@Category", expense.Category);
            command.Parameters.AddWithValue("@Amount", expense.Amount);
            command.Parameters.AddWithValue("@Description", expense.Description);

            command.ExecuteNonQuery();
        }

        public void DeleteExpense(int id)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Expenses WHERE Id = @Id";
            command.Parameters.AddWithValue("@Id", id);
            command.ExecuteNonQuery();
        }

        public DataTable GetExpenses()
        {
            var table = new DataTable();
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT Id, ExpenseDate, Category, Amount, Description
                FROM Expenses
                ORDER BY Id DESC";

            using var reader = command.ExecuteReader();
            table.Load(reader);
            return table;
        }

        public DashboardStats GetDashboardStats()
        {
            var stats = new DashboardStats();
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT
                    (SELECT COUNT(*) FROM Plants) AS TotalPlants,
                    (SELECT COUNT(*) FROM Workers) AS TotalWorkers,
                    (SELECT COUNT(DISTINCT AreaName) FROM Plants) AS TotalAreas,
                    (SELECT COALESCE(SUM(Amount), 0) FROM Expenses) AS TotalExpenses";

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                stats.TotalPlants = Convert.ToInt32(reader["TotalPlants"]);
                stats.TotalWorkers = Convert.ToInt32(reader["TotalWorkers"]);
                stats.TotalAreas = Convert.ToInt32(reader["TotalAreas"]);
                stats.TotalExpenses = Convert.ToDecimal(reader["TotalExpenses"]);
            }

            return stats;
        }

        public DataTable GetWorkerListForCombo()
        {
            var table = new DataTable();
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, Name FROM Workers ORDER BY Name ASC";

            using var reader = command.ExecuteReader();
            table.Load(reader);
            return table;
        }
    }
}
