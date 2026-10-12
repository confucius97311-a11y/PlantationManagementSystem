using System;
using System.Drawing;
using System.Windows.Forms;

namespace PlantationManagementSystem
{
    public partial class MainForm : Form
    {
        private readonly DatabaseHelper _database = new DatabaseHelper();

        private Label lblTotalPlants = new();
        private Label lblTotalWorkers = new();
        private Label lblTotalAreas = new();
        private Label lblTotalExpenses = new();

        private TextBox txtPlantName = new();
        private TextBox txtPlantType = new();
        private NumericUpDown numPlantQuantity = new();
        private DateTimePicker dtPlantDate = new();
        private ComboBox cmbPlantStatus = new();
        private TextBox txtPlantArea = new();
        private Button btnPlantSave = new();
        private Button btnPlantDelete = new();
        private Button btnPlantClear = new();
        private DataGridView dgvPlants = new();
        private int plantIdToEdit;

        private TextBox txtWorkerName = new();
        private TextBox txtWorkerContact = new();
        private TextBox txtWorkerRole = new();
        private NumericUpDown numWorkerSalary = new();
        private Button btnWorkerSave = new();
        private Button btnWorkerDelete = new();
        private Button btnWorkerClear = new();
        private DataGridView dgvWorkers = new();
        private int workerIdToEdit;

        private DateTimePicker dtActivityDate = new();
        private TextBox txtActivityDescription = new();
        private ComboBox cmbWorker = new();
        private Button btnActivitySave = new();
        private Button btnActivityDelete = new();
        private Button btnActivityClear = new();
        private DataGridView dgvActivities = new();
        private int activityIdToEdit;

        private DateTimePicker dtExpenseDate = new();
        private ComboBox cmbExpenseCategory = new();
        private NumericUpDown numExpenseAmount = new();
        private TextBox txtExpenseDescription = new();
        private Button btnExpenseSave = new();
        private Button btnExpenseDelete = new();
        private Button btnExpenseClear = new();
        private DataGridView dgvExpenses = new();
        private int expenseIdToEdit;

        private DataGridView dgvReportPlants = new();
        private DataGridView dgvReportWorkers = new();
        private DataGridView dgvReportActivities = new();
        private DataGridView dgvReportExpenses = new();

        public MainForm()
        {
            InitializeComponent();
            LoadDashboard();
            LoadPlants();
            LoadWorkers();
            LoadActivities();
            LoadExpenses();
            LoadReports();
        }

        private void InitializeComponent()
        {
            this.Text = "Plantation Management System";
            this.Size = new Size(1200, 760);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 10F);
            this.BackColor = Color.FromArgb(240, 248, 239);

            var tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F)
            };

            tabControl.TabPages.Add(CreateDashboardTab());
            tabControl.TabPages.Add(CreatePlantsTab());
            tabControl.TabPages.Add(CreateWorkersTab());
            tabControl.TabPages.Add(CreateActivitiesTab());
            tabControl.TabPages.Add(CreateExpensesTab());
            tabControl.TabPages.Add(CreateReportsTab());

            this.Controls.Add(tabControl);
        }

        private TabPage CreateDashboardTab()
        {
            var page = new TabPage("Dashboard");
            var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20) };

            var title = new Label
            {
                Text = "Plantation Dashboard",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 20)
            };
            panel.Controls.Add(title);

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 180,
                ColumnCount = 4,
                RowCount = 1,
                Padding = new Padding(5),
                BackColor = Color.WhiteSmoke
            };

            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));

            var cards = new[]
            {
                new { Title = "Total Plants", Value = lblTotalPlants },
                new { Title = "Total Workers", Value = lblTotalWorkers },
                new { Title = "Plantation Areas", Value = lblTotalAreas },
                new { Title = "Total Expenses", Value = lblTotalExpenses }
            };

            foreach (var card in cards)
            {
                var group = new GroupBox
                {
                    Text = card.Title,
                    Dock = DockStyle.Fill,
                    Padding = new Padding(10),
                    ForeColor = Color.DarkGreen,
                    BackColor = Color.White,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold)
                };

                card.Value.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
                card.Value.ForeColor = Color.DarkGreen;
                card.Value.AutoSize = true;
                card.Value.Location = new Point(20, 35);
                group.Controls.Add(card.Value);
                layout.Controls.Add(group, Array.IndexOf(cards, card), 0);
            }

            panel.Controls.Add(layout);

            var info = new Label
            {
                Text = "Manage plantation plants, workers, activities, expenses, and reports from one place.",
                Font = new Font("Segoe UI", 12F),
                AutoSize = true,
                Location = new Point(20, 220)
            };
            panel.Controls.Add(info);

            page.Controls.Add(panel);
            return page;
        }

        private TabPage CreatePlantsTab()
        {
            var page = new TabPage("Plants");

            var topPanel = new Panel { Dock = DockStyle.Top, Height = 220, Padding = new Padding(10) };
            var formLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 3,
                Padding = new Padding(5)
            };

            formLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18F));
            formLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32F));
            formLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18F));
            formLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32F));
            formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));

            txtPlantName = new TextBox();
            txtPlantType = new TextBox();
            numPlantQuantity = new NumericUpDown { Minimum = 0, Maximum = 1000000 };
            dtPlantDate = new DateTimePicker { Format = DateTimePickerFormat.Short };
            cmbPlantStatus = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            txtPlantArea = new TextBox();
            cmbPlantStatus.Items.AddRange(new object[] { "Seedling", "Growing", "Healthy", "Mature", "Harvest Ready" });

            var entries = new[]
            {
                new { Label = "Plant Name", Control = txtPlantName },
                new { Label = "Plant Type", Control = txtPlantType },
                new { Label = "Quantity", Control = numPlantQuantity },
                new { Label = "Planting Date", Control = dtPlantDate },
                new { Label = "Growth Status", Control = cmbPlantStatus },
                new { Label = "Area Name", Control = txtPlantArea }
            };

            int row = 0;
            int col = 0;
            foreach (var entry in entries)
            {
                formLayout.Controls.Add(new Label { Text = entry.Label, AutoSize = true }, col, row);
                formLayout.Controls.Add(entry.Control, col + 1, row);
                col += 2;
                if (col >= 4)
                {
                    col = 0;
                    row++;
                }
            }

            var buttonRow = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                FlowDirection = FlowDirection.LeftToRight
            };

            btnPlantSave = new Button { Text = "Add Plant", Width = 130, BackColor = Color.DarkGreen, ForeColor = Color.White };
            btnPlantDelete = new Button { Text = "Delete", Width = 110, BackColor = Color.Firebrick, ForeColor = Color.White };
            btnPlantClear = new Button { Text = "Clear", Width = 110 };
            btnPlantSave.Click += BtnPlantSave_Click;
            btnPlantDelete.Click += BtnPlantDelete_Click;
            btnPlantClear.Click += BtnPlantClear_Click;
            buttonRow.Controls.Add(btnPlantSave);
            buttonRow.Controls.Add(btnPlantDelete);
            buttonRow.Controls.Add(btnPlantClear);

            topPanel.Controls.Add(formLayout);
            topPanel.Controls.Add(buttonRow);

            dgvPlants = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoGenerateColumns = true,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.Fixed3D
            };
            dgvPlants.SelectionChanged += DgvPlants_SelectionChanged;

            page.Controls.Add(topPanel);
            page.Controls.Add(dgvPlants);
            return page;
        }

        private TabPage CreateWorkersTab()
        {
            var page = new TabPage("Workers");

            var topPanel = new Panel { Dock = DockStyle.Top, Height = 220, Padding = new Padding(10) };
            var formLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 3,
                Padding = new Padding(5)
            };

            formLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18F));
            formLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32F));
            formLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18F));
            formLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32F));
            formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));

            txtWorkerName = new TextBox();
            txtWorkerContact = new TextBox();
            txtWorkerRole = new TextBox();
            numWorkerSalary = new NumericUpDown { Minimum = 0, Maximum = 1000000, DecimalPlaces = 2 };

            var entries = new[]
            {
                new { Label = "Worker Name", Control = txtWorkerName },
                new { Label = "Contact Number", Control = txtWorkerContact },
                new { Label = "Job Role", Control = txtWorkerRole },
                new { Label = "Salary", Control = numWorkerSalary }
            };

            int row = 0;
            int col = 0;
            foreach (var entry in entries)
            {
                formLayout.Controls.Add(new Label { Text = entry.Label, AutoSize = true }, col, row);
                formLayout.Controls.Add(entry.Control, col + 1, row);
                col += 2;
                if (col >= 4)
                {
                    col = 0;
                    row++;
                }
            }

            var buttonRow = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                FlowDirection = FlowDirection.LeftToRight
            };

            btnWorkerSave = new Button { Text = "Add Worker", Width = 130, BackColor = Color.DarkGreen, ForeColor = Color.White };
            btnWorkerDelete = new Button { Text = "Delete", Width = 110, BackColor = Color.Firebrick, ForeColor = Color.White };
            btnWorkerClear = new Button { Text = "Clear", Width = 110 };
            btnWorkerSave.Click += BtnWorkerSave_Click;
            btnWorkerDelete.Click += BtnWorkerDelete_Click;
            btnWorkerClear.Click += BtnWorkerClear_Click;
            buttonRow.Controls.Add(btnWorkerSave);
            buttonRow.Controls.Add(btnWorkerDelete);
            buttonRow.Controls.Add(btnWorkerClear);

            topPanel.Controls.Add(formLayout);
            topPanel.Controls.Add(buttonRow);

            dgvWorkers = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoGenerateColumns = true
            };
            dgvWorkers.SelectionChanged += DgvWorkers_SelectionChanged;

            page.Controls.Add(topPanel);
            page.Controls.Add(dgvWorkers);
            return page;
        }

        private TabPage CreateActivitiesTab()
        {
            var page = new TabPage("Activities");

            var topPanel = new Panel { Dock = DockStyle.Top, Height = 220, Padding = new Padding(10) };
            var formLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 3,
                Padding = new Padding(5)
            };

            formLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18F));
            formLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32F));
            formLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18F));
            formLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32F));
            formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));

            dtActivityDate = new DateTimePicker { Format = DateTimePickerFormat.Short };
            txtActivityDescription = new TextBox();
            cmbWorker = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };

            var entries = new[]
            {
                new { Label = "Activity Date", Control = dtActivityDate },
                new { Label = "Assigned Worker", Control = cmbWorker },
                new { Label = "Description", Control = txtActivityDescription }
            };

            int row = 0;
            int col = 0;
            foreach (var entry in entries)
            {
                formLayout.Controls.Add(new Label { Text = entry.Label, AutoSize = true }, col, row);
                formLayout.Controls.Add(entry.Control, col + 1, row);
                col += 2;
                if (col >= 4)
                {
                    col = 0;
                    row++;
                }
            }

            var buttonRow = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                FlowDirection = FlowDirection.LeftToRight
            };

            btnActivitySave = new Button { Text = "Save Activity", Width = 140, BackColor = Color.DarkGreen, ForeColor = Color.White };
            btnActivityDelete = new Button { Text = "Delete", Width = 110, BackColor = Color.Firebrick, ForeColor = Color.White };
            btnActivityClear = new Button { Text = "Clear", Width = 110 };
            btnActivitySave.Click += BtnActivitySave_Click;
            btnActivityDelete.Click += BtnActivityDelete_Click;
            btnActivityClear.Click += BtnActivityClear_Click;
            buttonRow.Controls.Add(btnActivitySave);
            buttonRow.Controls.Add(btnActivityDelete);
            buttonRow.Controls.Add(btnActivityClear);

            topPanel.Controls.Add(formLayout);
            topPanel.Controls.Add(buttonRow);

            dgvActivities = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoGenerateColumns = true
            };
            dgvActivities.SelectionChanged += DgvActivities_SelectionChanged;

            page.Controls.Add(topPanel);
            page.Controls.Add(dgvActivities);

            LoadWorkerCombo();
            return page;
        }

        private TabPage CreateExpensesTab()
        {
            var page = new TabPage("Expenses");

            var topPanel = new Panel { Dock = DockStyle.Top, Height = 220, Padding = new Padding(10) };
            var formLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 3,
                Padding = new Padding(5)
            };

            formLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18F));
            formLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32F));
            formLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18F));
            formLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32F));
            formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));

            dtExpenseDate = new DateTimePicker { Format = DateTimePickerFormat.Short };
            cmbExpenseCategory = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            cmbExpenseCategory.Items.AddRange(new object[] { "Seeds", "Fertilizer", "Equipment", "Wages", "Water", "Other" });
            numExpenseAmount = new NumericUpDown { Minimum = 0, Maximum = 1000000, DecimalPlaces = 2 };
            txtExpenseDescription = new TextBox();

            var entries = new[]
            {
                new { Label = "Expense Date", Control = dtExpenseDate },
                new { Label = "Category", Control = cmbExpenseCategory },
                new { Label = "Amount", Control = numExpenseAmount },
                new { Label = "Description", Control = txtExpenseDescription }
            };

            int row = 0;
            int col = 0;
            foreach (var entry in entries)
            {
                formLayout.Controls.Add(new Label { Text = entry.Label, AutoSize = true }, col, row);
                formLayout.Controls.Add(entry.Control, col + 1, row);
                col += 2;
                if (col >= 4)
                {
                    col = 0;
                    row++;
                }
            }

            var buttonRow = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                FlowDirection = FlowDirection.LeftToRight
            };

            btnExpenseSave = new Button { Text = "Save Expense", Width = 140, BackColor = Color.DarkGreen, ForeColor = Color.White };
            btnExpenseDelete = new Button { Text = "Delete", Width = 110, BackColor = Color.Firebrick, ForeColor = Color.White };
            btnExpenseClear = new Button { Text = "Clear", Width = 110 };
            btnExpenseSave.Click += BtnExpenseSave_Click;
            btnExpenseDelete.Click += BtnExpenseDelete_Click;
            btnExpenseClear.Click += BtnExpenseClear_Click;
            buttonRow.Controls.Add(btnExpenseSave);
            buttonRow.Controls.Add(btnExpenseDelete);
            buttonRow.Controls.Add(btnExpenseClear);

            topPanel.Controls.Add(formLayout);
            topPanel.Controls.Add(buttonRow);

            dgvExpenses = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoGenerateColumns = true
            };
            dgvExpenses.SelectionChanged += DgvExpenses_SelectionChanged;

            page.Controls.Add(topPanel);
            page.Controls.Add(dgvExpenses);
            return page;
        }

        private TabPage CreateReportsTab()
        {
            var page = new TabPage("Reports");
            var mainPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10) };

            var label = new Label
            {
                Text = "Reports",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(10, 10)
            };
            mainPanel.Controls.Add(label);

            var reportLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 2,
                Padding = new Padding(10)
            };

            reportLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            reportLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            reportLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            reportLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

            dgvReportPlants = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AutoGenerateColumns = true };
            dgvReportWorkers = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AutoGenerateColumns = true };
            dgvReportActivities = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AutoGenerateColumns = true };
            dgvReportExpenses = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AutoGenerateColumns = true };

            reportLayout.Controls.Add(dgvReportPlants, 0, 0);
            reportLayout.Controls.Add(dgvReportWorkers, 1, 0);
            reportLayout.Controls.Add(dgvReportActivities, 0, 1);
            reportLayout.Controls.Add(dgvReportExpenses, 1, 1);

            mainPanel.Controls.Add(reportLayout);
            page.Controls.Add(mainPanel);
            return page;
        }

        private void LoadDashboard()
        {
            var stats = _database.GetDashboardStats();
            lblTotalPlants.Text = stats.TotalPlants.ToString();
            lblTotalWorkers.Text = stats.TotalWorkers.ToString();
            lblTotalAreas.Text = stats.TotalAreas.ToString();
            lblTotalExpenses.Text = $"${stats.TotalExpenses:N2}";
        }

        private void LoadPlants()
        {
            var data = _database.GetPlants();
            dgvPlants.DataSource = data;
            if (dgvPlants.Columns.Contains("Id")) dgvPlants.Columns["Id"].HeaderText = "ID";
            if (dgvPlants.Columns.Contains("Name")) dgvPlants.Columns["Name"].HeaderText = "Plant Name";
            if (dgvPlants.Columns.Contains("Type")) dgvPlants.Columns["Type"].HeaderText = "Plant Type";
            if (dgvPlants.Columns.Contains("Quantity")) dgvPlants.Columns["Quantity"].HeaderText = "Quantity";
            if (dgvPlants.Columns.Contains("PlantingDate")) dgvPlants.Columns["PlantingDate"].HeaderText = "Planting Date";
            if (dgvPlants.Columns.Contains("GrowthStatus")) dgvPlants.Columns["GrowthStatus"].HeaderText = "Growth Status";
            if (dgvPlants.Columns.Contains("AreaName")) dgvPlants.Columns["AreaName"].HeaderText = "Area";
        }

        private void LoadWorkers()
        {
            var data = _database.GetWorkers();
            dgvWorkers.DataSource = data;
            if (dgvWorkers.Columns.Contains("Id")) dgvWorkers.Columns["Id"].HeaderText = "ID";
            if (dgvWorkers.Columns.Contains("Name")) dgvWorkers.Columns["Name"].HeaderText = "Worker Name";
            if (dgvWorkers.Columns.Contains("ContactNumber")) dgvWorkers.Columns["ContactNumber"].HeaderText = "Contact Number";
            if (dgvWorkers.Columns.Contains("JobRole")) dgvWorkers.Columns["JobRole"].HeaderText = "Job Role";
            if (dgvWorkers.Columns.Contains("Salary")) dgvWorkers.Columns["Salary"].HeaderText = "Salary";
        }

        private void LoadActivities()
        {
            var data = _database.GetActivities();
            dgvActivities.DataSource = data;
            if (dgvActivities.Columns.Contains("Id")) dgvActivities.Columns["Id"].HeaderText = "ID";
            if (dgvActivities.Columns.Contains("ActivityDate")) dgvActivities.Columns["ActivityDate"].HeaderText = "Date";
            if (dgvActivities.Columns.Contains("Description")) dgvActivities.Columns["Description"].HeaderText = "Description";
            if (dgvActivities.Columns.Contains("WorkerName")) dgvActivities.Columns["WorkerName"].HeaderText = "Assigned Worker";
        }

        private void LoadExpenses()
        {
            var data = _database.GetExpenses();
            dgvExpenses.DataSource = data;
            if (dgvExpenses.Columns.Contains("Id")) dgvExpenses.Columns["Id"].HeaderText = "ID";
            if (dgvExpenses.Columns.Contains("ExpenseDate")) dgvExpenses.Columns["ExpenseDate"].HeaderText = "Date";
            if (dgvExpenses.Columns.Contains("Category")) dgvExpenses.Columns["Category"].HeaderText = "Category";
            if (dgvExpenses.Columns.Contains("Amount")) dgvExpenses.Columns["Amount"].HeaderText = "Amount";
            if (dgvExpenses.Columns.Contains("Description")) dgvExpenses.Columns["Description"].HeaderText = "Description";
        }

        private void LoadReports()
        {
            dgvReportPlants.DataSource = _database.GetPlants();
            dgvReportWorkers.DataSource = _database.GetWorkers();
            dgvReportActivities.DataSource = _database.GetActivities();
            dgvReportExpenses.DataSource = _database.GetExpenses();
        }

        private void LoadWorkerCombo()
        {
            var data = _database.GetWorkerListForCombo();
            cmbWorker.DataSource = data;
            cmbWorker.DisplayMember = "Name";
            cmbWorker.ValueMember = "Id";
            if (cmbWorker.Items.Count > 0)
                cmbWorker.SelectedIndex = 0;
        }

        private void BtnPlantSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPlantName.Text))
            {
                MessageBox.Show("Plant name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPlantType.Text))
            {
                MessageBox.Show("Plant type is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var plant = new Plant
            {
                Id = plantIdToEdit,
                Name = txtPlantName.Text.Trim(),
                Type = txtPlantType.Text.Trim(),
                Quantity = (int)numPlantQuantity.Value,
                PlantingDate = dtPlantDate.Value,
                GrowthStatus = cmbPlantStatus.SelectedItem?.ToString() ?? "Growing",
                AreaName = txtPlantArea.Text.Trim()
            };

            if (plantIdToEdit > 0)
            {
                _database.UpdatePlant(plant);
                MessageBox.Show("Plant updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                _database.AddPlant(plant);
                MessageBox.Show("Plant added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            ClearPlantForm();
            LoadPlants();
            LoadDashboard();
            LoadReports();
        }

        private void BtnPlantDelete_Click(object sender, EventArgs e)
        {
            if (plantIdToEdit == 0)
            {
                MessageBox.Show("Select a plant to delete.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("Are you sure you want to delete this plant?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                _database.DeletePlant(plantIdToEdit);
                MessageBox.Show("Plant deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearPlantForm();
                LoadPlants();
                LoadDashboard();
                LoadReports();
            }
        }

        private void BtnPlantClear_Click(object sender, EventArgs e)
        {
            ClearPlantForm();
        }

        private void ClearPlantForm()
        {
            plantIdToEdit = 0;
            txtPlantName.Clear();
            txtPlantType.Clear();
            numPlantQuantity.Value = 0;
            dtPlantDate.Value = DateTime.Today;
            cmbPlantStatus.SelectedIndex = 0;
            txtPlantArea.Clear();
            btnPlantSave.Text = "Add Plant";
        }

        private void DgvPlants_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPlants.CurrentRow == null) return;

            var row = dgvPlants.CurrentRow;
            plantIdToEdit = Convert.ToInt32(row.Cells["Id"].Value);

            txtPlantName.Text = row.Cells["Name"].Value?.ToString() ?? string.Empty;
            txtPlantType.Text = row.Cells["Type"].Value?.ToString() ?? string.Empty;
            numPlantQuantity.Value = Convert.ToDecimal(row.Cells["Quantity"].Value ?? 0);
            dtPlantDate.Value = Convert.ToDateTime(row.Cells["PlantingDate"].Value ?? DateTime.Today);
            cmbPlantStatus.Text = row.Cells["GrowthStatus"].Value?.ToString() ?? "Growing";
            txtPlantArea.Text = row.Cells["AreaName"].Value?.ToString() ?? string.Empty;

            btnPlantSave.Text = "Update Plant";
        }

        private void BtnWorkerSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtWorkerName.Text))
            {
                MessageBox.Show("Worker name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtWorkerContact.Text))
            {
                MessageBox.Show("Contact number is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var worker = new Worker
            {
                Id = workerIdToEdit,
                Name = txtWorkerName.Text.Trim(),
                ContactNumber = txtWorkerContact.Text.Trim(),
                JobRole = txtWorkerRole.Text.Trim(),
                Salary = numWorkerSalary.Value
            };

            if (workerIdToEdit > 0)
            {
                _database.UpdateWorker(worker);
                MessageBox.Show("Worker updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                _database.AddWorker(worker);
                MessageBox.Show("Worker added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            ClearWorkerForm();
            LoadWorkers();
            LoadDashboard();
            LoadReports();
            LoadWorkerCombo();
        }

        private void BtnWorkerDelete_Click(object sender, EventArgs e)
        {
            if (workerIdToEdit == 0)
            {
                MessageBox.Show("Select a worker to delete.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("Are you sure you want to delete this worker?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                _database.DeleteWorker(workerIdToEdit);
                MessageBox.Show("Worker deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearWorkerForm();
                LoadWorkers();
                LoadDashboard();
                LoadReports();
                LoadWorkerCombo();
            }
        }

        private void BtnWorkerClear_Click(object sender, EventArgs e)
        {
            ClearWorkerForm();
        }

        private void ClearWorkerForm()
        {
            workerIdToEdit = 0;
            txtWorkerName.Clear();
            txtWorkerContact.Clear();
            txtWorkerRole.Clear();
            numWorkerSalary.Value = 0;
            btnWorkerSave.Text = "Add Worker";
        }

        private void DgvWorkers_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvWorkers.CurrentRow == null) return;

            var row = dgvWorkers.CurrentRow;
            workerIdToEdit = Convert.ToInt32(row.Cells["Id"].Value);

            txtWorkerName.Text = row.Cells["Name"].Value?.ToString() ?? string.Empty;
            txtWorkerContact.Text = row.Cells["ContactNumber"].Value?.ToString() ?? string.Empty;
            txtWorkerRole.Text = row.Cells["JobRole"].Value?.ToString() ?? string.Empty;
            numWorkerSalary.Value = Convert.ToDecimal(row.Cells["Salary"].Value ?? 0);

            btnWorkerSave.Text = "Update Worker";
        }

        private void BtnActivitySave_Click(object sender, EventArgs e)
        {
            if (cmbWorker.SelectedValue == null)
            {
                MessageBox.Show("Please assign a worker to the activity.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtActivityDescription.Text))
            {
                MessageBox.Show("Activity description is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var activity = new Activity
            {
                Id = activityIdToEdit,
                ActivityDate = dtActivityDate.Value,
                Description = txtActivityDescription.Text.Trim(),
                WorkerId = Convert.ToInt32(cmbWorker.SelectedValue)
            };

            if (activityIdToEdit > 0)
            {
                _database.UpdateActivity(activity);
                MessageBox.Show("Activity updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                _database.AddActivity(activity);
                MessageBox.Show("Activity added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            ClearActivityForm();
            LoadActivities();
            LoadReports();
        }

        private void BtnActivityDelete_Click(object sender, EventArgs e)
        {
            if (activityIdToEdit == 0)
            {
                MessageBox.Show("Select an activity to delete.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("Are you sure you want to delete this activity?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                _database.DeleteActivity(activityIdToEdit);
                MessageBox.Show("Activity deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearActivityForm();
                LoadActivities();
                LoadReports();
            }
        }

        private void BtnActivityClear_Click(object sender, EventArgs e)
        {
            ClearActivityForm();
        }

        private void ClearActivityForm()
        {
            activityIdToEdit = 0;
            dtActivityDate.Value = DateTime.Today;
            txtActivityDescription.Clear();
            if (cmbWorker.Items.Count > 0) cmbWorker.SelectedIndex = 0;
            btnActivitySave.Text = "Save Activity";
        }

        private void DgvActivities_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvActivities.CurrentRow == null) return;

            var row = dgvActivities.CurrentRow;
            activityIdToEdit = Convert.ToInt32(row.Cells["Id"].Value);

            dtActivityDate.Value = Convert.ToDateTime(row.Cells["ActivityDate"].Value ?? DateTime.Today);
            txtActivityDescription.Text = row.Cells["Description"].Value?.ToString() ?? string.Empty;
            var workerName = row.Cells["WorkerName"].Value?.ToString() ?? string.Empty;

            if (cmbWorker.Items.Count > 0)
            {
                for (int i = 0; i < cmbWorker.Items.Count; i++)
                {
                    var item = (DataRowView)cmbWorker.Items[i];
                    if (item["Name"].ToString() == workerName)
                    {
                        cmbWorker.SelectedIndex = i;
                        break;
                    }
                }
            }

            btnActivitySave.Text = "Update Activity";
        }

        private void BtnExpenseSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cmbExpenseCategory.Text))
            {
                MessageBox.Show("Expense category is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var expense = new Expense
            {
                Id = expenseIdToEdit,
                ExpenseDate = dtExpenseDate.Value,
                Category = cmbExpenseCategory.Text,
                Amount = numExpenseAmount.Value,
                Description = txtExpenseDescription.Text.Trim()
            };

            if (expenseIdToEdit > 0)
            {
                _database.UpdateExpense(expense);
                MessageBox.Show("Expense updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                _database.AddExpense(expense);
                MessageBox.Show("Expense added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            ClearExpenseForm();
            LoadExpenses();
            LoadDashboard();
            LoadReports();
        }

        private void BtnExpenseDelete_Click(object sender, EventArgs e)
        {
            if (expenseIdToEdit == 0)
            {
                MessageBox.Show("Select an expense to delete.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("Are you sure you want to delete this expense?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                _database.DeleteExpense(expenseIdToEdit);
                MessageBox.Show("Expense deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearExpenseForm();
                LoadExpenses();
                LoadDashboard();
                LoadReports();
            }
        }

        private void BtnExpenseClear_Click(object sender, EventArgs e)
        {
            ClearExpenseForm();
        }

        private void ClearExpenseForm()
        {
            expenseIdToEdit = 0;
            dtExpenseDate.Value = DateTime.Today;
            cmbExpenseCategory.SelectedIndex = 0;
            numExpenseAmount.Value = 0;
            txtExpenseDescription.Clear();
            btnExpenseSave.Text = "Save Expense";
        }

        private void DgvExpenses_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvExpenses.CurrentRow == null) return;

            var row = dgvExpenses.CurrentRow;
            expenseIdToEdit = Convert.ToInt32(row.Cells["Id"].Value);

            dtExpenseDate.Value = Convert.ToDateTime(row.Cells["ExpenseDate"].Value ?? DateTime.Today);
            cmbExpenseCategory.Text = row.Cells["Category"].Value?.ToString() ?? "Seeds";
            numExpenseAmount.Value = Convert.ToDecimal(row.Cells["Amount"].Value ?? 0);
            txtExpenseDescription.Text = row.Cells["Description"].Value?.ToString() ?? string.Empty;

            btnExpenseSave.Text = "Update Expense";
        }
    }
}
