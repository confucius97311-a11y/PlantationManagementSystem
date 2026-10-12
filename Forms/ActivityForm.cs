using PlantationManagementSystem.Database;
using PlantationManagementSystem.Models;

namespace PlantationManagementSystem.Forms;

public class ActivityForm : Form
{
    private readonly DataGridView gridActivities = new();
    private readonly ComboBox cboActivityType = new();
    private readonly TextBox txtDetails = new();
    private readonly DateTimePicker dtActivityDate = new();
    private readonly ComboBox cboPlant = new();
    private readonly ComboBox cboWorker = new();
    private readonly TextBox txtDuration = new();
    private readonly TextBox txtNotes = new();
    private readonly Button btnSave = new();
    private readonly Button btnDelete = new();
    private readonly Button btnClear = new();
    private int selectedActivityId = 0;

    public ActivityForm()
    {
        InitializeForm();
        ConfigureControls();
        LoadActivityLists();
        LoadActivities();
    }

    private void InitializeForm()
    {
        this.Text = "Activity Management";
        this.Size = new Size(1050, 730);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.FromArgb(236, 247, 235);
    }

    private void ConfigureControls()
    {
        var title = new Label
        {
            Text = "Activity Management",
            Font = new Font("Segoe UI", 20, FontStyle.Bold),
            ForeColor = Color.FromArgb(20, 90, 50),
            Location = new Point(30, 20),
            Size = new Size(400, 35)
        };
        this.Controls.Add(title);

        var lblType = new Label { Text = "Activity Type:", Location = new Point(30, 80), Size = new Size(120, 25) };
        cboActivityType.Location = new Point(150, 80);
        cboActivityType.Size = new Size(200, 30);
        cboActivityType.Items.AddRange(new object[] { "Watering", "Fertilizing", "Pruning", "Harvesting" });
        cboActivityType.SelectedIndex = 0;
        this.Controls.Add(lblType);
        this.Controls.Add(cboActivityType);

        var lblDetails = new Label { Text = "Details:", Location = new Point(30, 120), Size = new Size(120, 25) };
        txtDetails.Location = new Point(150, 120);
        txtDetails.Size = new Size(200, 30);
        this.Controls.Add(lblDetails);
        this.Controls.Add(txtDetails);

        var lblDate = new Label { Text = "Date:", Location = new Point(30, 160), Size = new Size(120, 25) };
        dtActivityDate.Location = new Point(150, 160);
        dtActivityDate.Size = new Size(200, 30);
        dtActivityDate.Format = DateTimePickerFormat.Short;
        this.Controls.Add(lblDate);
        this.Controls.Add(dtActivityDate);

        var lblPlant = new Label { Text = "Plant:", Location = new Point(30, 200), Size = new Size(120, 25) };
        cboPlant.Location = new Point(150, 200);
        cboPlant.Size = new Size(200, 30);
        cboPlant.DropDownStyle = ComboBoxStyle.DropDownList;
        this.Controls.Add(lblPlant);
        this.Controls.Add(cboPlant);

        var lblWorker = new Label { Text = "Worker:", Location = new Point(30, 240), Size = new Size(120, 25) };
        cboWorker.Location = new Point(150, 240);
        cboWorker.Size = new Size(200, 30);
        cboWorker.DropDownStyle = ComboBoxStyle.DropDownList;
        this.Controls.Add(lblWorker);
        this.Controls.Add(cboWorker);

        var lblDuration = new Label { Text = "Duration (min):", Location = new Point(30, 280), Size = new Size(120, 25) };
        txtDuration.Location = new Point(150, 280);
        txtDuration.Size = new Size(200, 30);
        this.Controls.Add(lblDuration);
        this.Controls.Add(txtDuration);

        var lblNotes = new Label { Text = "Notes:", Location = new Point(30, 320), Size = new Size(120, 25) };
        txtNotes.Location = new Point(150, 320);
        txtNotes.Size = new Size(200, 80);
        this.Controls.Add(lblNotes);
        this.Controls.Add(txtNotes);

        btnSave.Text = "Save Activity";
        btnSave.Location = new Point(30, 420);
        btnSave.Size = new Size(140, 35);
        btnSave.BackColor = Color.FromArgb(42, 128, 74);
        btnSave.ForeColor = Color.White;
        btnSave.Click += BtnSave_Click;
        this.Controls.Add(btnSave);

        btnDelete.Text = "Delete";
        btnDelete.Location = new Point(190, 420);
        btnDelete.Size = new Size(120, 35);
        btnDelete.BackColor = Color.FromArgb(204, 73, 61);
        btnDelete.ForeColor = Color.White;
        btnDelete.Click += BtnDelete_Click;
        this.Controls.Add(btnDelete);

        btnClear.Text = "Clear";
        btnClear.Location = new Point(330, 420);
        btnClear.Size = new Size(120, 35);
        btnClear.BackColor = Color.FromArgb(112, 128, 144);
        btnClear.ForeColor = Color.White;
        btnClear.Click += BtnClear_Click;
        this.Controls.Add(btnClear);

        gridActivities.Location = new Point(430, 80);
        gridActivities.Size = new Size(570, 500);
        gridActivities.ReadOnly = true;
        gridActivities.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        gridActivities.RowHeaderVisible = false;
        gridActivities.CellClick += GridActivities_CellClick;
        this.Controls.Add(gridActivities);
    }

    private void LoadActivityLists()
    {
        cboPlant.Items.Clear();
        cboWorker.Items.Clear();

        cboPlant.Items.Add(new ComboItem { Text = "No Plant", Value = 0 });
        cboWorker.Items.Add(new ComboItem { Text = "No Worker", Value = 0 });

        foreach (var plant in DatabaseHelper.GetPlants())
        {
            cboPlant.Items.Add(new ComboItem { Text = plant.Name, Value = plant.Id });
        }

        foreach (var worker in DatabaseHelper.GetWorkers())
        {
            cboWorker.Items.Add(new ComboItem { Text = worker.Name, Value = worker.Id });
        }

        if (cboPlant.Items.Count > 0) cboPlant.SelectedIndex = 0;
        if (cboWorker.Items.Count > 0) cboWorker.SelectedIndex = 0;
    }

    private void LoadActivities()
    {
        try
        {
            var activities = DatabaseHelper.GetActivities();
            gridActivities.DataSource = activities;
            gridActivities.Columns["Id"].Visible = false;
            gridActivities.Columns["PlantId"].Visible = false;
            gridActivities.Columns["WorkerId"].Visible = false;
            gridActivities.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridActivities.Refresh();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading activities: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txtDetails.Text))
            {
                MessageBox.Show("Please enter activity details.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtDuration.Text, out int duration) || duration <= 0)
            {
                MessageBox.Show("Duration must be a valid positive number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var plantValue = (cboPlant.SelectedItem as ComboItem)?.Value ?? 0;
            var workerValue = (cboWorker.SelectedItem as ComboItem)?.Value ?? 0;

            var activity = new ActivityRecord
            {
                Id = selectedActivityId,
                ActivityType = cboActivityType.SelectedItem?.ToString() ?? "Watering",
                Details = txtDetails.Text.Trim(),
                ActivityDate = dtActivityDate.Value,
                PlantId = plantValue == 0 ? null : plantValue,
                WorkerId = workerValue == 0 ? null : workerValue,
                DurationMinutes = duration,
                Notes = txtNotes.Text.Trim()
            };

            if (selectedActivityId > 0)
            {
                DatabaseHelper.UpdateActivity(activity);
                MessageBox.Show("Activity updated successfully.", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                DatabaseHelper.AddActivity(activity);
                MessageBox.Show("Activity recorded successfully.", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            ClearForm();
            LoadActivityLists();
            LoadActivities();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnDelete_Click(object sender, EventArgs e)
    {
        if (selectedActivityId <= 0)
        {
            MessageBox.Show("Please select an activity to delete.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var result = MessageBox.Show("Are you sure you want to delete this activity?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (result == DialogResult.Yes)
        {
            try
            {
                DatabaseHelper.DeleteActivity(selectedActivityId);
                MessageBox.Show("Activity deleted successfully.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                LoadActivities();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Delete failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void BtnClear_Click(object sender, EventArgs e) => ClearForm();

    private void GridActivities_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        var activity = (ActivityRecord)gridActivities.Rows[e.RowIndex].DataBoundItem;
        if (activity == null) return;

        selectedActivityId = activity.Id;
        cboActivityType.SelectedItem = activity.ActivityType;
        txtDetails.Text = activity.Details;
        dtActivityDate.Value = activity.ActivityDate;
        txtDuration.Text = activity.DurationMinutes.ToString();
        txtNotes.Text = activity.Notes;

        if (activity.PlantId.HasValue)
        {
            foreach (ComboItem item in cboPlant.Items)
            {
                if (item.Value == activity.PlantId.Value)
                {
                    cboPlant.SelectedItem = item;
                    break;
                }
            }
        }
        else
        {
            cboPlant.SelectedIndex = 0;
        }

        if (activity.WorkerId.HasValue)
        {
            foreach (ComboItem item in cboWorker.Items)
            {
                if (item.Value == activity.WorkerId.Value)
                {
                    cboWorker.SelectedItem = item;
                    break;
                }
            }
        }
        else
        {
            cboWorker.SelectedIndex = 0;
        }
    }

    private void ClearForm()
    {
        selectedActivityId = 0;
        cboActivityType.SelectedIndex = 0;
        txtDetails.Clear();
        txtDuration.Clear();
        txtNotes.Clear();
        dtActivityDate.Value = DateTime.Today;
        if (cboPlant.Items.Count > 0) cboPlant.SelectedIndex = 0;
        if (cboWorker.Items.Count > 0) cboWorker.SelectedIndex = 0;
    }
}

public class ComboItem
{
    public string Text { get; set; } = string.Empty;
    public int Value { get; set; }

    public override string ToString() => Text;
}
