using Microsoft.Data.Sqlite;
using PlantationManagementSystem.Models;

namespace PlantationManagementSystem.Database;

public static class DatabaseHelper
{
    private static readonly string DatabaseFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plantation.db");

    public static string ConnectionString => new SqliteConnectionStringBuilder
    {
        DataSource = DatabaseFile
    }.ToString();

    public static void Initialize()
    {
        try
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            var createPlants = @"
                CREATE TABLE IF NOT EXISTS Plants (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    PlantType TEXT NOT NULL,
                    Quantity INTEGER NOT NULL,
                    Location TEXT NOT NULL,
                    PlantingDate TEXT NOT NULL,
                    Status TEXT NOT NULL
                );";

            var createWorkers = @"
                CREATE TABLE IF NOT EXISTS Workers (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Phone TEXT NOT NULL,
                    Role TEXT NOT NULL,
                    Salary REAL NOT NULL
                );";

            var createActivities = @"
                CREATE TABLE IF NOT EXISTS Activities (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    ActivityType TEXT NOT NULL,
                    Details TEXT NOT NULL,
                    ActivityDate TEXT NOT NULL,
                    PlantId INTEGER,
                    WorkerId INTEGER,
                    DurationMinutes INTEGER NOT NULL,
                    Notes TEXT
                );";

            var createExpenses = @"
                CREATE TABLE IF NOT EXISTS Expenses (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Category TEXT NOT NULL,
                    Amount REAL NOT NULL,
                    ExpenseDate TEXT NOT NULL,
                    Description TEXT NOT NULL
                );";

            using (var cmd = new SqliteCommand(createPlants, connection))
            {
                cmd.ExecuteNonQuery();
            }

            using (var cmd = new SqliteCommand(createWorkers, connection))
            {
                cmd.ExecuteNonQuery();
            }

            using (var cmd = new SqliteCommand(createActivities, connection))
            {
                cmd.ExecuteNonQuery();
            }

            using (var cmd = new SqliteCommand(createExpenses, connection))
            {
                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Database initialization failed: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            throw;
        }
    }

    public static DashboardSummary GetDashboardSummary()
    {
        var summary = new DashboardSummary();

        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        using (var cmd = new SqliteCommand("SELECT COUNT(*) FROM Plants;", connection))
        {
            var value = cmd.ExecuteScalar();
            summary.TotalPlants = value == DBNull.Value || value == null ? 0 : Convert.ToInt32(value);
        }

        using (var cmd = new SqliteCommand("SELECT COUNT(*) FROM Workers;", connection))
        {
            var value = cmd.ExecuteScalar();
            summary.TotalWorkers = value == DBNull.Value || value == null ? 0 : Convert.ToInt32(value);
        }

        using (var cmd = new SqliteCommand("SELECT COUNT(*) FROM Activities;", connection))
        {
            var value = cmd.ExecuteScalar();
            summary.TotalActivities = value == DBNull.Value || value == null ? 0 : Convert.ToInt32(value);
        }

        using (var cmd = new SqliteCommand("SELECT COALESCE(SUM(Amount), 0) FROM Expenses;", connection))
        {
            var value = cmd.ExecuteScalar();
            summary.TotalExpenses = value == DBNull.Value || value == null ? 0 : Convert.ToDecimal(value);
        }

        return summary;
    }

    public static List<PlantRecord> GetPlants()
    {
        var list = new List<PlantRecord>();
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        using var cmd = new SqliteCommand("SELECT Id, Name, PlantType, Quantity, Location, PlantingDate, Status FROM Plants ORDER BY Id DESC;", connection);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(new PlantRecord
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                PlantType = reader.GetString(2),
                Quantity = reader.GetInt32(3),
                Location = reader.GetString(4),
                PlantingDate = DateTime.Parse(reader.GetString(5)),
                Status = reader.GetString(6)
            });
        }

        return list;
    }

    public static void AddPlant(PlantRecord plant)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        using var cmd = new SqliteCommand(@"INSERT INTO Plants (Name, PlantType, Quantity, Location, PlantingDate, Status) VALUES (@Name, @PlantType, @Quantity, @Location, @PlantingDate, @Status);", connection);
        cmd.Parameters.AddWithValue("@Name", plant.Name);
        cmd.Parameters.AddWithValue("@PlantType", plant.PlantType);
        cmd.Parameters.AddWithValue("@Quantity", plant.Quantity);
        cmd.Parameters.AddWithValue("@Location", plant.Location);
        cmd.Parameters.AddWithValue("@PlantingDate", plant.PlantingDate.ToString("yyyy-MM-dd"));
        cmd.Parameters.AddWithValue("@Status", plant.Status);
        cmd.ExecuteNonQuery();
    }

    public static void UpdatePlant(PlantRecord plant)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        using var cmd = new SqliteCommand(@"UPDATE Plants SET Name=@Name, PlantType=@PlantType, Quantity=@Quantity, Location=@Location, PlantingDate=@PlantingDate, Status=@Status WHERE Id=@Id;", connection);
        cmd.Parameters.AddWithValue("@Id", plant.Id);
        cmd.Parameters.AddWithValue("@Name", plant.Name);
        cmd.Parameters.AddWithValue("@PlantType", plant.PlantType);
        cmd.Parameters.AddWithValue("@Quantity", plant.Quantity);
        cmd.Parameters.AddWithValue("@Location", plant.Location);
        cmd.Parameters.AddWithValue("@PlantingDate", plant.PlantingDate.ToString("yyyy-MM-dd"));
        cmd.Parameters.AddWithValue("@Status", plant.Status);
        cmd.ExecuteNonQuery();
    }

    public static void DeletePlant(int id)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        using var cmd = new SqliteCommand("DELETE FROM Plants WHERE Id=@Id;", connection);
        cmd.Parameters.AddWithValue("@Id", id);
        cmd.ExecuteNonQuery();
    }

    public static List<WorkerRecord> GetWorkers()
    {
        var list = new List<WorkerRecord>();
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        using var cmd = new SqliteCommand("SELECT Id, Name, Phone, Role, Salary FROM Workers ORDER BY Id DESC;", connection);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(new WorkerRecord
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Phone = reader.GetString(2),
                Role = reader.GetString(3),
                Salary = reader.GetDecimal(4)
            });
        }

        return list;
    }

    public static void AddWorker(WorkerRecord worker)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        using var cmd = new SqliteCommand(@"INSERT INTO Workers (Name, Phone, Role, Salary) VALUES (@Name, @Phone, @Role, @Salary);", connection);
        cmd.Parameters.AddWithValue("@Name", worker.Name);
        cmd.Parameters.AddWithValue("@Phone", worker.Phone);
        cmd.Parameters.AddWithValue("@Role", worker.Role);
        cmd.Parameters.AddWithValue("@Salary", worker.Salary);
        cmd.ExecuteNonQuery();
    }

    public static void UpdateWorker(WorkerRecord worker)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        using var cmd = new SqliteCommand(@"UPDATE Workers SET Name=@Name, Phone=@Phone, Role=@Role, Salary=@Salary WHERE Id=@Id;", connection);
        cmd.Parameters.AddWithValue("@Id", worker.Id);
        cmd.Parameters.AddWithValue("@Name", worker.Name);
        cmd.Parameters.AddWithValue("@Phone", worker.Phone);
        cmd.Parameters.AddWithValue("@Role", worker.Role);
        cmd.Parameters.AddWithValue("@Salary", worker.Salary);
        cmd.ExecuteNonQuery();
    }

    public static void DeleteWorker(int id)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        using var cmd = new SqliteCommand("DELETE FROM Workers WHERE Id=@Id;", connection);
        cmd.Parameters.AddWithValue("@Id", id);
        cmd.ExecuteNonQuery();
    }

    public static List<ActivityRecord> GetActivities()
    {
        var list = new List<ActivityRecord>();
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        var query = @"
            SELECT a.Id, a.ActivityType, a.Details, a.ActivityDate, a.PlantId, p.Name,
                   a.WorkerId, w.Name, a.DurationMinutes, a.Notes
            FROM Activities a
            LEFT JOIN Plants p ON a.PlantId = p.Id
            LEFT JOIN Workers w ON a.WorkerId = w.Id
            ORDER BY a.Id DESC;";

        using var cmd = new SqliteCommand(query, connection);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(new ActivityRecord
            {
                Id = reader.GetInt32(0),
                ActivityType = reader.GetString(1),
                Details = reader.GetString(2),
                ActivityDate = DateTime.Parse(reader.GetString(3)),
                PlantId = reader.IsDBNull(4) ? null : reader.GetInt32(4),
                PlantName = reader.IsDBNull(5) ? null : reader.GetString(5),
                WorkerId = reader.IsDBNull(6) ? null : reader.GetInt32(6),
                WorkerName = reader.IsDBNull(7) ? null : reader.GetString(7),
                DurationMinutes = reader.GetInt32(8),
                Notes = reader.IsDBNull(9) ? string.Empty : reader.GetString(9)
            });
        }

        return list;
    }

    public static void AddActivity(ActivityRecord activity)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        using var cmd = new SqliteCommand(@"INSERT INTO Activities (ActivityType, Details, ActivityDate, PlantId, WorkerId, DurationMinutes, Notes) VALUES (@ActivityType, @Details, @ActivityDate, @PlantId, @WorkerId, @DurationMinutes, @Notes);", connection);
        cmd.Parameters.AddWithValue("@ActivityType", activity.ActivityType);
        cmd.Parameters.AddWithValue("@Details", activity.Details);
        cmd.Parameters.AddWithValue("@ActivityDate", activity.ActivityDate.ToString("yyyy-MM-dd"));
        cmd.Parameters.AddWithValue("@PlantId", activity.PlantId ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@WorkerId", activity.WorkerId ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@DurationMinutes", activity.DurationMinutes);
        cmd.Parameters.AddWithValue("@Notes", activity.Notes ?? string.Empty);
        cmd.ExecuteNonQuery();
    }

    public static void UpdateActivity(ActivityRecord activity)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        using var cmd = new SqliteCommand(@"UPDATE Activities SET ActivityType=@ActivityType, Details=@Details, ActivityDate=@ActivityDate, PlantId=@PlantId, WorkerId=@WorkerId, DurationMinutes=@DurationMinutes, Notes=@Notes WHERE Id=@Id;", connection);
        cmd.Parameters.AddWithValue("@Id", activity.Id);
        cmd.Parameters.AddWithValue("@ActivityType", activity.ActivityType);
        cmd.Parameters.AddWithValue("@Details", activity.Details);
        cmd.Parameters.AddWithValue("@ActivityDate", activity.ActivityDate.ToString("yyyy-MM-dd"));
        cmd.Parameters.AddWithValue("@PlantId", activity.PlantId ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@WorkerId", activity.WorkerId ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@DurationMinutes", activity.DurationMinutes);
        cmd.Parameters.AddWithValue("@Notes", activity.Notes ?? string.Empty);
        cmd.ExecuteNonQuery();
    }

    public static void DeleteActivity(int id)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        using var cmd = new SqliteCommand("DELETE FROM Activities WHERE Id=@Id;", connection);
        cmd.Parameters.AddWithValue("@Id", id);
        cmd.ExecuteNonQuery();
    }

    public static List<ExpenseRecord> GetExpenses()
    {
        var list = new List<ExpenseRecord>();
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        using var cmd = new SqliteCommand("SELECT Id, Category, Amount, ExpenseDate, Description FROM Expenses ORDER BY Id DESC;", connection);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(new ExpenseRecord
            {
                Id = reader.GetInt32(0),
                Category = reader.GetString(1),
                Amount = reader.GetDecimal(2),
                ExpenseDate = DateTime.Parse(reader.GetString(3)),
                Description = reader.GetString(4)
            });
        }

        return list;
    }

    public static void AddExpense(ExpenseRecord expense)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        using var cmd = new SqliteCommand(@"INSERT INTO Expenses (Category, Amount, ExpenseDate, Description) VALUES (@Category, @Amount, @ExpenseDate, @Description);", connection);
        cmd.Parameters.AddWithValue("@Category", expense.Category);
        cmd.Parameters.AddWithValue("@Amount", expense.Amount);
        cmd.Parameters.AddWithValue("@ExpenseDate", expense.ExpenseDate.ToString("yyyy-MM-dd"));
        cmd.Parameters.AddWithValue("@Description", expense.Description);
        cmd.ExecuteNonQuery();
    }

    public static void UpdateExpense(ExpenseRecord expense)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        using var cmd = new SqliteCommand(@"UPDATE Expenses SET Category=@Category, Amount=@Amount, ExpenseDate=@ExpenseDate, Description=@Description WHERE Id=@Id;", connection);
        cmd.Parameters.AddWithValue("@Id", expense.Id);
        cmd.Parameters.AddWithValue("@Category", expense.Category);
        cmd.Parameters.AddWithValue("@Amount", expense.Amount);
        cmd.Parameters.AddWithValue("@ExpenseDate", expense.ExpenseDate.ToString("yyyy-MM-dd"));
        cmd.Parameters.AddWithValue("@Description", expense.Description);
        cmd.ExecuteNonQuery();
    }

    public static void DeleteExpense(int id)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        using var cmd = new SqliteCommand("DELETE FROM Expenses WHERE Id=@Id;", connection);
        cmd.Parameters.AddWithValue("@Id", id);
        cmd.ExecuteNonQuery();
    }
}
