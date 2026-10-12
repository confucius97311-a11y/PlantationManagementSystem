using PlantationManagementSystem.Database;
using PlantationManagementSystem.Models;

namespace PlantationManagementSystem.Forms;

public class WorkerForm : Form
{
    private readonly DataGridView gridWorkers = new();
    private readonly TextBox txtName = new();
    private readonly TextBox txtPhone = new();
    private readonly TextBox txtRole = new();
    private readonly TextBox txtSalary = new();
    private readonly Button btnSave = new();
    private readonly Button btnDelete = new();
    private readonly Button btnClear = new();
    private int selectedWorkerId = 0;

    public WorkerForm()
    {
        InitializeForm();
        ConfigureControls();
        LoadWorkers();
    }

    private void InitializeForm()
    {
        this.Text = "Worker Management";
        this.Size = new Size(1000, 700);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.FromArgb(236, 247, 235);
    }

    private void ConfigureControls()
    {
        var title = new Label
        {
            Text = "Worker Management",
            Font = new Font("Segoe UI", 20, FontStyle.Bold),
            ForeColor = Color.FromArgb(20, 90, 50),
            Location = new Point(30, 20),
            Size = new Size(400, 35)
        };
        this.Controls.Add(title);

        var lblName = new Label { Text = "Name:", Location = new Point(30, 80), Size = new Size(100, 25) };
        txtName.Location = new Point(140, 80);
        txtName.Size = new Size(200, 30);
        this.Controls.Add(lblName);
        this.Controls.Add(txtName);

        var lblPhone = new Label { Text = "Phone:", Location = new Point(30, 120), Size = new Size(100, 25) };
        txtPhone.Location = new Point(140, 120);
        txtPhone.Size = new Size(200, 30);
        this.Controls.Add(lblPhone);
        this.Controls.Add(txtPhone);

        var lblRole = new Label { Text = "Job Role:", Location = new Point(30, 160), Size = new Size(100, 25) };
        txtRole.Location = new Point(140, 160);
        txtRole.Size = new Size(200, 30);
        this.Controls.Add(lblRole);
        this.Controls.Add(txtRole);

        var lblSalary = new Label { Text = "Salary:", Location = new Point(30, 200), Size = new Size(100, 25) };
        txtSalary.Location = new Point(140, 200);
        txtSalary.Size = new Size(200, 30);
        this.Controls.Add(lblSalary);
        this.Controls.Add(txtSalary);

        btnSave.Text = "Save Worker";
        btnSave.Location = new Point(30, 260);
        btnSave.Size = new Size(120, 35);
        btnSave.BackColor = Color.FromArgb(42, 128, 74);
        btnSave.ForeColor = Color.White;
        btnSave.Click += BtnSave_Click;
        this.Controls.Add(btnSave);

        btnDelete.Text = "Delete";
        btnDelete.Location = new Point(170, 260);
        btnDelete.Size = new Size(120, 35);
        btnDelete.BackColor = Color.FromArgb(204, 73, 61);
        btnDelete.ForeColor = Color.White;
        btnDelete.Click += BtnDelete_Click;
        this.Controls.Add(btnDelete);

        btnClear.Text = "Clear";
        btnClear.Location = new Point(310, 260);
        btnClear.Size = new Size(120, 35);
        btnClear.BackColor = Color.FromArgb(112, 128, 144);
        btnClear.ForeColor = Color.White;
        btnClear.Click += BtnClear_Click;
        this.Controls.Add(btnClear);

        gridWorkers.Location = new Point(420, 80);
        gridWorkers.Size = new Size(520, 500);
        gridWorkers.ReadOnly = true;
        gridWorkers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        gridWorkers.RowHeaderVisible = false;
        gridWorkers.CellClick += GridWorkers_CellClick;
        this.Controls.Add(gridWorkers);
    }

    private void LoadWorkers()
    {
        try
        {
            var workers = DatabaseHelper.GetWorkers();
            gridWorkers.DataSource = workers;
            gridWorkers.Columns["Id"].Visible = false;
            gridWorkers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridWorkers.Refresh();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading workers: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtPhone.Text) || string.IsNullOrWhiteSpace(txtRole.Text))
            {
                MessageBox.Show("Please fill in all worker fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtSalary.Text, out decimal salary) || salary <= 0)
            {
                MessageBox.Show("Salary must be a valid positive value.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var worker = new WorkerRecord
            {
                Id = selectedWorkerId,
                Name = txtName.Text.Trim(),
                Phone = txtPhone.Text.Trim(),
                Role = txtRole.Text.Trim(),
                Salary = salary
            };

            if (selectedWorkerId > 0)
            {
                DatabaseHelper.UpdateWorker(worker);
                MessageBox.Show("Worker updated successfully.", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                DatabaseHelper.AddWorker(worker);
                MessageBox.Show("Worker added successfully.", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            ClearForm();
            LoadWorkers();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnDelete_Click(object sender, EventArgs e)
    {
        if (selectedWorkerId <= 0)
        {
            MessageBox.Show("Please select a worker to delete.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var result = MessageBox.Show("Are you sure you want to delete this worker?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (result == DialogResult.Yes)
        {
            try
            {
                DatabaseHelper.DeleteWorker(selectedWorkerId);
                MessageBox.Show("Worker deleted successfully.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                LoadWorkers();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Delete failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void BtnClear_Click(object sender, EventArgs e) => ClearForm();

    private void GridWorkers_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        var worker = (WorkerRecord)gridWorkers.Rows[e.RowIndex].DataBoundItem;
        if (worker == null) return;

        selectedWorkerId = worker.Id;
        txtName.Text = worker.Name;
        txtPhone.Text = worker.Phone;
        txtRole.Text = worker.Role;
        txtSalary.Text = worker.Salary.ToString();
    }

    private void ClearForm()
    {
        selectedWorkerId = 0;
        txtName.Clear();
        txtPhone.Clear();
        txtRole.Clear();
        txtSalary.Clear();
    }
}
