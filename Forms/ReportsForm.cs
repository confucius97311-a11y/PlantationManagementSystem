using PlantationManagementSystem.Database;
using PlantationManagementSystem.Models;

namespace PlantationManagementSystem.Forms;

public class ExpenseForm : Form
{
    private readonly DataGridView gridExpenses = new();
    private readonly TextBox txtCategory = new();
    private readonly TextBox txtAmount = new();
    private readonly DateTimePicker dtExpenseDate = new();
    private readonly TextBox txtDescription = new();
    private readonly Button btnSave = new();
    private readonly Button btnDelete = new();
    private readonly Button btnClear = new();
    private int selectedExpenseId = 0;

    public ExpenseForm()
    {
        InitializeForm();
        ConfigureControls();
        LoadExpenses();
    }

    private void InitializeForm()
    {
        this.Text = "Expense Management";
        this.Size = new Size(1000, 700);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.FromArgb(236, 247, 235);
    }

    private void ConfigureControls()
    {
        var title = new Label
        {
            Text = "Expense Management",
            Font = new Font("Segoe UI", 20, FontStyle.Bold),
            ForeColor = Color.FromArgb(20, 90, 50),
            Location = new Point(30, 20),
            Size = new Size(400, 35)
        };
        this.Controls.Add(title);

        var lblCategory = new Label { Text = "Category:", Location = new Point(30, 80), Size = new Size(110, 25) };
        txtCategory.Location = new Point(150, 80);
        txtCategory.Size = new Size(200, 30);
        this.Controls.Add(lblCategory);
        this.Controls.Add(txtCategory);

        var lblAmount = new Label { Text = "Amount:", Location = new Point(30, 120), Size = new Size(110, 25) };
        txtAmount.Location = new Point(150, 120);
        txtAmount.Size = new Size(200, 30);
        this.Controls.Add(lblAmount);
        this.Controls.Add(txtAmount);

        var lblDate = new Label { Text = "Expense Date:", Location = new Point(30, 160), Size = new Size(110, 25) };
        dtExpenseDate.Location = new Point(150, 160);
        dtExpenseDate.Size = new Size(200, 30);
        dtExpenseDate.Format = DateTimePickerFormat.Short;
        this.Controls.Add(lblDate);
        this.Controls.Add(dtExpenseDate);

        var lblDescription = new Label { Text = "Description:", Location = new Point(30, 200), Size = new Size(110, 25) };
        txtDescription.Location = new Point(150, 200);
        txtDescription.Size = new Size(200, 80);
        this.Controls.Add(lblDescription);
        this.Controls.Add(txtDescription);

        btnSave.Text = "Save Expense";
        btnSave.Location = new Point(30, 310);
        btnSave.Size = new Size(120, 35);
        btnSave.BackColor = Color.FromArgb(42, 128, 74);
        btnSave.ForeColor = Color.White;
        btnSave.Click += BtnSave_Click;
        this.Controls.Add(btnSave);

        btnDelete.Text = "Delete";
        btnDelete.Location = new Point(170, 310);
        btnDelete.Size = new Size(120, 35);
        btnDelete.BackColor = Color.FromArgb(204, 73, 61);
        btnDelete.ForeColor = Color.White;
        btnDelete.Click += BtnDelete_Click;
        this.Controls.Add(btnDelete);

        btnClear.Text = "Clear";
        btnClear.Location = new Point(310, 310);
        btnClear.Size = new Size(120, 35);
        btnClear.BackColor = Color.FromArgb(112, 128, 144);
        btnClear.ForeColor = Color.White;
        btnClear.Click += BtnClear_Click;
        this.Controls.Add(btnClear);

        gridExpenses.Location = new Point(420, 80);
        gridExpenses.Size = new Size(520, 500);
        gridExpenses.ReadOnly = true;
        gridExpenses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        gridExpenses.RowHeaderVisible = false;
        gridExpenses.CellClick += GridExpenses_CellClick;
        this.Controls.Add(gridExpenses);
    }

    private void LoadExpenses()
    {
        try
        {
            var expenses = DatabaseHelper.GetExpenses();
            gridExpenses.DataSource = expenses;
            gridExpenses.Columns["Id"].Visible = false;
            gridExpenses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridExpenses.Refresh();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading expenses: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txtCategory.Text) || string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                MessageBox.Show("Please fill in the category and description.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtAmount.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Amount must be a valid positive value.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var expense = new ExpenseRecord
            {
                Id = selectedExpenseId,
                Category = txtCategory.Text.Trim(),
                Amount = amount,
                ExpenseDate = dtExpenseDate.Value,
                Description = txtDescription.Text.Trim()
            };

            if (selectedExpenseId > 0)
            {
                DatabaseHelper.UpdateExpense(expense);
                MessageBox.Show("Expense updated successfully.", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                DatabaseHelper.AddExpense(expense);
                MessageBox.Show("Expense added successfully.", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            ClearForm();
            LoadExpenses();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnDelete_Click(object sender, EventArgs e)
    {
        if (selectedExpenseId <= 0)
        {
            MessageBox.Show("Please select an expense to delete.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var result = MessageBox.Show("Are you sure you want to delete this expense?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (result == DialogResult.Yes)
        {
            try
            {
                DatabaseHelper.DeleteExpense(selectedExpenseId);
                MessageBox.Show("Expense deleted successfully.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                LoadExpenses();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Delete failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void BtnClear_Click(object sender, EventArgs e) => ClearForm();

    private void GridExpenses_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        var expense = (ExpenseRecord)gridExpenses.Rows[e.RowIndex].DataBoundItem;
        if (expense == null) return;

        selectedExpenseId = expense.Id;
        txtCategory.Text = expense.Category;
        txtAmount.Text = expense.Amount.ToString();
        dtExpenseDate.Value = expense.ExpenseDate;
        txtDescription.Text = expense.Description;
    }

    private void ClearForm()
    {
        selectedExpenseId = 0;
        txtCategory.Clear();
        txtAmount.Clear();
        txtDescription.Clear();
        dtExpenseDate.Value = DateTime.Today;
    }
}
