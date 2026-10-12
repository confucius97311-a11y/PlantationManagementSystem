using System.Globalization;
using PlantationManagementSystem.Database;
using PlantationManagementSystem.Models;

namespace PlantationManagementSystem.Forms;

public class MainDashboardForm : Form
{
    private readonly Label lblTitle = new();
    private readonly FlowLayoutPanel panelNavigation = new();
    private readonly Button btnPlants = new();
    private readonly Button btnWorkers = new();
    private readonly Button btnActivities = new();
    private readonly Button btnExpenses = new();
    private readonly Button btnReports = new();

    private readonly GroupBox groupSummary = new();
    private readonly Label lblPlantsValue = new();
    private readonly Label lblWorkersValue = new();
    private readonly Label lblActivitiesValue = new();
    private readonly Label lblExpensesValue = new();

    public MainDashboardForm()
    {
        InitializeForm();
        ConfigureControls();
        RefreshDashboard();
    }

    private void InitializeForm()
    {
        this.Text = "Plantation Management System";
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Size = new Size(1000, 720);
        this.BackColor = Color.FromArgb(236, 247, 235);
        this.Icon = null;
    }

    private void ConfigureControls()
    {
        lblTitle.Text = "Plantation Management System";
        lblTitle.Font = new Font("Segoe UI", 24, FontStyle.Bold);
        lblTitle.ForeColor = Color.FromArgb(20, 90, 50);
        lblTitle.Location = new Point(40, 25);
        lblTitle.Size = new Size(700, 40);
        this.Controls.Add(lblTitle);

        panelNavigation.Location = new Point(40, 80);
        panelNavigation.Size = new Size(900, 60);
        panelNavigation.FlowDirection = FlowDirection.LeftToRight;
        panelNavigation.WrapContents = false;
        panelNavigation.AutoSize = true;
        this.Controls.Add(panelNavigation);

        AddNavigationButton(btnPlants, "Plants", Color.FromArgb(42, 128, 74));
        AddNavigationButton(btnWorkers, "Workers", Color.FromArgb(65, 150, 88));
        AddNavigationButton(btnActivities, "Activities", Color.FromArgb(87, 176, 103));
        AddNavigationButton(btnExpenses, "Expenses", Color.FromArgb(109, 201, 116));
        AddNavigationButton(btnReports, "Reports", Color.FromArgb(31, 112, 61));

        groupSummary.Text = "Dashboard Summary";
        groupSummary.Location = new Point(40, 170);
        groupSummary.Size = new Size(900, 430);
        groupSummary.Font = new Font("Segoe UI", 14, FontStyle.Bold);
        groupSummary.ForeColor = Color.FromArgb(20, 90, 50);
        groupSummary.BackColor = Color.FromArgb(240, 250, 242);
        this.Controls.Add(groupSummary);

        CreateSummaryCard(0, 40, "Total Plants", lblPlantsValue, Color.FromArgb(196, 230, 202));
        CreateSummaryCard(230, 40, "Total Workers", lblWorkersValue, Color.FromArgb(179, 220, 182));
        CreateSummaryCard(460, 40, "Total Activities", lblActivitiesValue, Color.FromArgb(159, 207, 169));
        CreateSummaryCard(690, 40, "Total Expenses", lblExpensesValue, Color.FromArgb(140, 196, 150));

        var infoLabel = new Label
        {
            Text = "Welcome! Use the buttons above to manage your plantation records and generate reports.",
            Font = new Font("Segoe UI", 12, FontStyle.Regular),
            ForeColor = Color.FromArgb(40, 70, 50),
            Location = new Point(40, 250),
            Size = new Size(820, 120),
            BackColor = Color.Transparent
        };
        groupSummary.Controls.Add(infoLabel);

        btnPlants.Click += (s, e) => OpenForm(new PlantForm());
        btnWorkers.Click += (s, e) => OpenForm(new WorkerForm());
        btnActivities.Click += (s, e) => OpenForm(new ActivityForm());
        btnExpenses.Click += (s, e) => OpenForm(new ExpenseForm());
        btnReports.Click += (s, e) => OpenForm(new ReportsForm());
    }

    private void AddNavigationButton(Button button, string text, Color color)
    {
        button.Text = text;
        button.Size = new Size(150, 40);
        button.BackColor = color;
        button.ForeColor = Color.White;
        button.Font = new Font("Segoe UI", 11, FontStyle.Bold);
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderSize = 0;
        button.Cursor = Cursors.Hand;
        panelNavigation.Controls.Add(button);
    }

    private void CreateSummaryCard(int x, int y, string title, Label valueLabel, Color color)
    {
        var card = new Panel
        {
            Location = new Point(x, y),
            Size = new Size(180, 200),
            BackColor = color,
            BorderStyle = BorderStyle.FixedSingle
        };

        var titleLabel = new Label
        {
            Text = title,
            Font = new Font("Segoe UI", 12, FontStyle.Bold),
            ForeColor = Color.FromArgb(20, 90, 50),
            Location = new Point(15, 20),
            Size = new Size(150, 30)
        };

        valueLabel.Text = "0";
        valueLabel.Font = new Font("Segoe UI", 26, FontStyle.Bold);
        valueLabel.ForeColor = Color.FromArgb(20, 90, 50);
        valueLabel.Location = new Point(15, 90);
        valueLabel.Size = new Size(140, 42);
        valueLabel.TextAlign = ContentAlignment.MiddleLeft;

        card.Controls.Add(titleLabel);
        card.Controls.Add(valueLabel);
        groupSummary.Controls.Add(card);

        if (title == "Total Expenses")
        {
            var expensesText = new Label
            {
                Text = "(Total cost)",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(30, 70, 40),
                Location = new Point(15, 150),
                Size = new Size(140, 20)
            };
            card.Controls.Add(expensesText);
        }
    }

    private void RefreshDashboard()
    {
        var summary = DatabaseHelper.GetDashboardSummary();
        lblPlantsValue.Text = summary.TotalPlants.ToString();
        lblWorkersValue.Text = summary.TotalWorkers.ToString();
        lblActivitiesValue.Text = summary.TotalActivities.ToString();
        lblExpensesValue.Text = summary.TotalExpenses.ToString("C", CultureInfo.GetCultureInfo("en-US"));
    }

    private void OpenForm(Form form)
    {
        form.Show();
        form.Activate();
    }
}
