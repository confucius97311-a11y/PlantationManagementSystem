using PlantationManagementSystem.Database;

namespace PlantationManagementSystem.Forms;

public class ReportsForm : Form
{
    private readonly TabControl tabControl = new();

    public ReportsForm()
    {
        InitializeForm();
        BuildTabs();
        LoadReports();
    }

    private void InitializeForm()
    {
        this.Text = "Reports";
        this.Size = new Size(1100, 700);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.FromArgb(236, 247, 235);
    }

    private void BuildTabs()
    {
        tabControl.Dock = DockStyle.Fill;
        tabControl.Font = new Font("Segoe UI", 11, FontStyle.Regular);
        this.Controls.Add(tabControl);

        var plantsTab = new TabPage("Plants");
        var workersTab = new TabPage("Workers");
        var activitiesTab = new TabPage("Activities");
        var expensesTab = new TabPage("Expenses");

        tabControl.TabPages.Add(plantsTab);
        tabControl.TabPages.Add(workersTab);
        tabControl.TabPages.Add(activitiesTab);
        tabControl.TabPages.Add(expensesTab);

        var plantsGrid = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
        var workersGrid = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
        var activitiesGrid = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
        var expensesGrid = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };

        plantsTab.Controls.Add(plantsGrid);
        workersTab.Controls.Add(workersGrid);
        activitiesTab.Controls.Add(activitiesGrid);
        expensesTab.Controls.Add(expensesGrid);

        plantsTab.Tag = plantsGrid;
        workersTab.Tag = workersGrid;
        activitiesTab.Tag = activitiesGrid;
        expensesTab.Tag = expensesGrid;
    }

    private void LoadReports()
    {
        try
        {
            var plantsGrid = (DataGridView)tabControl.TabPages[0].Tag;
            var workersGrid = (DataGridView)tabControl.TabPages[1].Tag;
            var activitiesGrid = (DataGridView)tabControl.TabPages[2].Tag;
            var expensesGrid = (DataGridView)tabControl.TabPages[3].Tag;

            plantsGrid.DataSource = DatabaseHelper.GetPlants();
            workersGrid.DataSource = DatabaseHelper.GetWorkers();
            activitiesGrid.DataSource = DatabaseHelper.GetActivities();
            expensesGrid.DataSource = DatabaseHelper.GetExpenses();

            foreach (DataGridView grid in new[] { plantsGrid, workersGrid, activitiesGrid, expensesGrid })
            {
                grid.RowHeadersVisible = false;
                grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                grid.ReadOnly = true;
                grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }

            if (plantsGrid.Columns.Contains("Id")) plantsGrid.Columns["Id"].Visible = false;
            if (workersGrid.Columns.Contains("Id")) workersGrid.Columns["Id"].Visible = false;
            if (activitiesGrid.Columns.Contains("Id")) activitiesGrid.Columns["Id"].Visible = false;
            if (activitiesGrid.Columns.Contains("PlantId")) activitiesGrid.Columns["PlantId"].Visible = false;
            if (activitiesGrid.Columns.Contains("WorkerId")) activitiesGrid.Columns["WorkerId"].Visible = false;
            if (expensesGrid.Columns.Contains("Id")) expensesGrid.Columns["Id"].Visible = false;

            var totalLabel = new Label
            {
                Text = $"Total Expenses: {DatabaseHelper.GetDashboardSummary().TotalExpenses:C}",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(20, 90, 50),
                Dock = DockStyle.Top,
                Height = 30,
                Margin = new Padding(10)
            };

            var expensesTab = tabControl.TabPages[3];
            expensesTab.Controls.Add(totalLabel);
            totalLabel.BringToFront();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading reports: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
