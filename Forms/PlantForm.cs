using PlantationManagementSystem.Database;
using PlantationManagementSystem.Models;

namespace PlantationManagementSystem.Forms;

public class PlantForm : Form
{
    private readonly DataGridView gridPlants = new();
    private readonly TextBox txtName = new();
    private readonly TextBox txtType = new();
    private readonly TextBox txtQuantity = new();
    private readonly TextBox txtLocation = new();
    private readonly DateTimePicker dtPlantingDate = new();
    private readonly ComboBox cboStatus = new();
    private readonly Button btnSave = new();
    private readonly Button btnDelete = new();
    private readonly Button btnClear = new();
    private int selectedPlantId = 0;

    public PlantForm()
    {
        InitializeForm();
        ConfigureControls();
        LoadPlants();
    }

    private void InitializeForm()
    {
        this.Text = "Plant Management";
        this.Size = new Size(1000, 700);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.FromArgb(236, 247, 235);
    }

    private void ConfigureControls()
    {
        var title = new Label
        {
            Text = "Plant Management",
            Font = new Font("Segoe UI", 20, FontStyle.Bold),
            ForeColor = Color.FromArgb(20, 90, 50),
            Location = new Point(30, 20),
            Size = new Size(400, 35)
        };
        this.Controls.Add(title);

        var lblName = new Label { Text = "Plant Name:", Location = new Point(30, 80), Size = new Size(100, 25) };
        txtName.Location = new Point(140, 80);
        txtName.Size = new Size(200, 30);
        this.Controls.Add(lblName);
        this.Controls.Add(txtName);

        var lblType = new Label { Text = "Type:", Location = new Point(30, 120), Size = new Size(100, 25) };
        txtType.Location = new Point(140, 120);
        txtType.Size = new Size(200, 30);
        this.Controls.Add(lblType);
        this.Controls.Add(txtType);

        var lblQuantity = new Label { Text = "Quantity:", Location = new Point(30, 160), Size = new Size(100, 25) };
        txtQuantity.Location = new Point(140, 160);
        txtQuantity.Size = new Size(200, 30);
        this.Controls.Add(lblQuantity);
        this.Controls.Add(txtQuantity);

        var lblLocation = new Label { Text = "Location:", Location = new Point(30, 200), Size = new Size(100, 25) };
        txtLocation.Location = new Point(140, 200);
        txtLocation.Size = new Size(200, 30);
        this.Controls.Add(lblLocation);
        this.Controls.Add(txtLocation);

        var lblDate = new Label { Text = "Planting Date:", Location = new Point(30, 240), Size = new Size(100, 25) };
        dtPlantingDate.Location = new Point(140, 240);
        dtPlantingDate.Size = new Size(200, 30);
        dtPlantingDate.Format = DateTimePickerFormat.Short;
        this.Controls.Add(lblDate);
        this.Controls.Add(dtPlantingDate);

        var lblStatus = new Label { Text = "Status:", Location = new Point(30, 280), Size = new Size(100, 25) };
        cboStatus.Location = new Point(140, 280);
        cboStatus.Size = new Size(200, 30);
        cboStatus.Items.AddRange(new object[] { "Healthy", "Needs Care", "Growing", "Harvest Ready", "Damaged" });
        cboStatus.SelectedIndex = 0;
        this.Controls.Add(lblStatus);
        this.Controls.Add(cboStatus);

        btnSave.Text = "Save Plant";
        btnSave.Location = new Point(30, 330);
        btnSave.Size = new Size(120, 35);
        btnSave.BackColor = Color.FromArgb(42, 128, 74);
        btnSave.ForeColor = Color.White;
        btnSave.Click += BtnSave_Click;
        this.Controls.Add(btnSave);

        btnDelete.Text = "Delete";
        btnDelete.Location = new Point(170, 330);
        btnDelete.Size = new Size(120, 35);
        btnDelete.BackColor = Color.FromArgb(204, 73, 61);
        btnDelete.ForeColor = Color.White;
        btnDelete.Click += BtnDelete_Click;
        this.Controls.Add(btnDelete);

        btnClear.Text = "Clear";
        btnClear.Location = new Point(310, 330);
        btnClear.Size = new Size(120, 35);
        btnClear.BackColor = Color.FromArgb(112, 128, 144);
        btnClear.ForeColor = Color.White;
        btnClear.Click += BtnClear_Click;
        this.Controls.Add(btnClear);

        gridPlants.Location = new Point(420, 80);
        gridPlants.Size = new Size(520, 500);
        gridPlants.ReadOnly = true;
        gridPlants.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        gridPlants.RowHeaderVisible = false;
        gridPlants.CellClick += GridPlants_CellClick;
        this.Controls.Add(gridPlants);
    }

    private void LoadPlants()
    {
        try
        {
            var plants = DatabaseHelper.GetPlants();
            gridPlants.DataSource = plants;
            gridPlants.Columns["Id"].Visible = false;
            gridPlants.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridPlants.Refresh();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading plants: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtType.Text) || string.IsNullOrWhiteSpace(txtLocation.Text))
            {
                MessageBox.Show("Please fill in all plant information.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Quantity must be a valid positive number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var plant = new PlantRecord
            {
                Id = selectedPlantId,
                Name = txtName.Text.Trim(),
                PlantType = txtType.Text.Trim(),
                Quantity = quantity,
                Location = txtLocation.Text.Trim(),
                PlantingDate = dtPlantingDate.Value,
                Status = cboStatus.SelectedItem?.ToString() ?? "Healthy"
            };

            if (selectedPlantId > 0)
            {
                DatabaseHelper.UpdatePlant(plant);
                MessageBox.Show("Plant updated successfully.", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                DatabaseHelper.AddPlant(plant);
                MessageBox.Show("Plant added successfully.", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            ClearForm();
            LoadPlants();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnDelete_Click(object sender, EventArgs e)
    {
        if (selectedPlantId <= 0)
        {
            MessageBox.Show("Please select a plant to delete.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var result = MessageBox.Show("Are you sure you want to delete this plant?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (result == DialogResult.Yes)
        {
            try
            {
                DatabaseHelper.DeletePlant(selectedPlantId);
                MessageBox.Show("Plant deleted successfully.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                LoadPlants();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Delete failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void BtnClear_Click(object sender, EventArgs e) => ClearForm();

    private void GridPlants_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        var plant = (PlantRecord)gridPlants.Rows[e.RowIndex].DataBoundItem;
        if (plant == null) return;

        selectedPlantId = plant.Id;
        txtName.Text = plant.Name;
        txtType.Text = plant.PlantType;
        txtQuantity.Text = plant.Quantity.ToString();
        txtLocation.Text = plant.Location;
        dtPlantingDate.Value = plant.PlantingDate;
        cboStatus.SelectedItem = plant.Status;
    }

    private void ClearForm()
    {
        selectedPlantId = 0;
        txtName.Clear();
        txtType.Clear();
        txtQuantity.Clear();
        txtLocation.Clear();
        dtPlantingDate.Value = DateTime.Today;
        cboStatus.SelectedIndex = 0;
    }
}
