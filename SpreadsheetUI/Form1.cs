// <copyright file="Form1.cs" company="Kaden Metzger Id: 11817362">
// Copyright (c) Kaden Metzger Id: 11817362. All rights reserved.
// </copyright>

namespace HW7
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using System.Xml;
    using System.Xml.Linq;
    using SpreadsheetEngine;
    using SpreadsheetEngine.SpreadsheetCommands;
    using static System.Windows.Forms.VisualStyles.VisualStyleElement;

    /// <summary>
    /// Inherits from Form class. Used for UI display.
    /// </summary>
    public partial class Form1 : Form
    {
        private Spreadsheet spreadsheet;

        /// <summary>
        /// Initializes a new instance of the <see cref="Form1"/> class.
        /// Form constructor, calls methods for UI.
        /// </summary>
        public Form1()
        {
            this.spreadsheet = new Spreadsheet(50, 26);
            this.InitializeComponent();
            this.InitializeDataGrid();
            this.InitializeMenuButtons();
            this.spreadsheet.CellPropertyChanged += this.SpreadsheetPropertyChanged;
        }

        /// <summary>
        /// Initializes the buttons as unusable when the program first starts.
        /// </summary>
        public void InitializeMenuButtons()
        {
            this.redoToolStripMenuItem.ForeColor = Color.LightGray;
            this.redoToolStripMenuItem.Enabled = false;
            this.undoToolStripMenuItem.ForeColor = Color.LightGray;
            this.undoToolStripMenuItem.Enabled = false;
        }

        /// <summary>
        /// Intializes Data grid.
        /// </summary>
        private void InitializeDataGrid()
        {
            // clear the existing columns
            this.dataGridView1.Columns.Clear();

            // Create 26 columns A-Z and 1-50 rows
            for (int i = 0; i < 26; i++)
            {
                char letter = (char)(i + 65);
                this.dataGridView1.Columns.Add(letter.ToString(), letter.ToString());
            }

            // Add 50 rows with a title
            for (int i = 0; i < 50; i++)
            {
                this.dataGridView1.Rows.Add();
                this.dataGridView1.Rows[i].HeaderCell.Value = (i + 1).ToString();
            }

            for (int i = 0; i < 50; i++)
            {
                for (int j = 0; j < 26; j++)
                {
                    this.dataGridView1.Rows[i].Cells[j].Value = this.spreadsheet.GetCell(i, j).Value;
                }
            }

            // Adjust width so that row title displays
            this.dataGridView1.RowHeadersWidth = 60;
        }

#pragma warning disable SA1625 // Element documentation should not be copied and pasted
        /// <summary>
        /// Will be executed anytime a spreadsheet's text is updated.
        /// </summary>
        /// <param name="sender">default param.</param>
        /// <param name="e">default param.</param>
        private void SpreadsheetPropertyChanged(object? sender, PropertyChangedEventArgs e)
#pragma warning restore SA1625 // Element documentation should not be copied and pasted
        {
            if (sender is Cell cell && e.PropertyName == nameof(Cell.Text))
            {
                this.dataGridView1.Rows[cell.RowIndex].Cells[cell.ColumnIndex].Value = cell.Value;
                Console.WriteLine("Row " + cell.RowIndex + " Column " + cell.ColumnIndex + " was updated to value: " + cell.Value);
            }

            if (sender is Cell cellValue && e.PropertyName == nameof(Cell.Value))
            {
                this.dataGridView1.Rows[cellValue.RowIndex].Cells[cellValue.ColumnIndex].Value = cellValue.Value;
                Console.WriteLine("Row " + cellValue.RowIndex + " Column " + cellValue.ColumnIndex + " was updated to value: " + cellValue.Value);
            }

            if (sender is Cell cellColor && e.PropertyName == nameof(Cell.BGColor))
            {
                var colorTuple = ColorMethods.UIntToARGB(cellColor.BGColor);
                this.dataGridView1.Rows[cellColor.RowIndex].Cells[cellColor.ColumnIndex].Style.BackColor = System.Drawing.Color.FromArgb(colorTuple.alpha, colorTuple.red, colorTuple.green, colorTuple.blue);
            }
        }

        /// <summary>
        /// This calls the demo button event.
        /// </summary>
        /// <param name="sender">Sender object for form object.</param>
        /// <param name="e">event argument for form object.</param>
        private void DemoButton_Click(object sender, EventArgs e)
        {
            this.DemoButtonClickEvent(sender, e);
        }

        /// <summary>
        /// This is the actual demo of how updating the cells changes the UI.
        /// </summary>
        /// <param name="sender">deafult param for form events.</param>
        /// <param name="e">default event param for form events.</param>
        private void DemoButtonClickEvent(object sender, EventArgs e)
        {
            // Generate 50 random index's and update them to a string
            for (int i = 0; i < 50; i++)
            {
                Random randNum = new Random();
                int randRow = randNum.Next(0, 50);
                int randColumn = randNum.Next(0, 26);
                this.spreadsheet.UpdateCell("Random Cell!", randRow, randColumn);
            }

            // Update every B cell to 'This is cell B#'
            for (int i = 0; i < 50; i++)
            {
                this.spreadsheet.UpdateCell("This is cell B" + (i + 1), i, 1);
            }

            // Update every A cell to "=B#", which essentially sets every A cell to whatever B is.
            for (int i = 0; i < 50; i++)
            {
                this.spreadsheet.UpdateCell("=B" + (i + 1), i, 0);
            }
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            // We need to display the TEXT not the value
            this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = this.spreadsheet.GetCell(e.RowIndex, e.ColumnIndex).Text;
            Console.WriteLine("Editing Cell at ({0}, {1}) Current Text {2}", e.RowIndex, e.ColumnIndex, this.spreadsheet.GetCell(e.RowIndex, e.ColumnIndex).Text);
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var editedValue = this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue;
            UpdateCellCommand updateCommand = new UpdateCellCommand(this.spreadsheet, editedValue.ToString(), e.RowIndex, e.ColumnIndex);
            this.spreadsheet.ExecuteCommand(updateCommand);
            if (this.spreadsheet.GetUndoSize() > 0)
            {
                this.undoToolStripMenuItem.Enabled = true;
                this.undoToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            }

            if (this.spreadsheet.GetRedoSize() > 0)
            {
                this.redoToolStripMenuItem.Enabled = true;
                this.redoToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            }

            // After editing is finished we display the value once again Index, e.ColumnIndex).Value;
            Console.WriteLine("Finished Editing Cell at ({0}, {1}) Current Value {2}", e.RowIndex, e.ColumnIndex, this.spreadsheet.GetCell(e.RowIndex, e.ColumnIndex).Value);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:Element should begin with upper-case letter", Justification = "this was created as a default, changing the name would cause assembly issues")]
        private void changeBackgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog myDialog = new ColorDialog();
            myDialog.AllowFullOpen = false;
            myDialog.ShowHelp = true;
            myDialog.Color = this.dataGridView1.ForeColor;
            List<(int row, int column)> selectedCells = new List<(int, int)>();

            if (myDialog.ShowDialog() == DialogResult.OK)
            {
                uint color = ColorMethods.RGBToHex(myDialog.Color.A, myDialog.Color.R, myDialog.Color.G, myDialog.Color.B);
                foreach (DataGridViewCell cell in this.dataGridView1.SelectedCells)
                {
                    selectedCells.Add((cell.RowIndex, cell.ColumnIndex));
                }

                UpdateColorCommand colorCommand = new UpdateColorCommand(this.spreadsheet, color, selectedCells);
                this.spreadsheet.ExecuteCommand(colorCommand);
                if (this.spreadsheet.GetUndoSize() > 0)
                {
                    this.undoToolStripMenuItem.Enabled = true;
                    this.undoToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
                }

                if (this.spreadsheet.GetRedoSize() > 0)
                {
                    this.redoToolStripMenuItem.Enabled = true;
                    this.redoToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
                }
            }
        }

        private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.spreadsheet.Undo();
            if (this.spreadsheet.GetUndoSize() > 0)
            {
                this.undoToolStripMenuItem.Enabled = true;
                this.undoToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            }

            if (this.spreadsheet.GetRedoSize() > 0)
            {
                this.redoToolStripMenuItem.Enabled = true;
                this.redoToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            }

            if (this.spreadsheet.GetUndoSize() == 0)
            {
                this.undoToolStripMenuItem.Enabled = false;
                this.undoToolStripMenuItem.ForeColor = System.Drawing.Color.LightGray;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:Element should begin with upper-case letter", Justification = "this was created as a default, changing the name would cause assembly issues")]
        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.spreadsheet.Redo();
            if (this.spreadsheet.GetUndoSize() > 0)
            {
                this.undoToolStripMenuItem.Enabled = true;
                this.undoToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            }

            if (this.spreadsheet.GetRedoSize() > 0)
            {
                this.redoToolStripMenuItem.Enabled = true;
                this.redoToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            }

            if (this.spreadsheet.GetRedoSize() == 0)
            {
                this.redoToolStripMenuItem.Enabled = false;
                this.redoToolStripMenuItem.ForeColor = System.Drawing.Color.LightGray;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:Element should begin with upper-case letter", Justification = "this was created as a default, changing the name would cause assembly issues")]
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "XML-File | *.xml";
            saveFileDialog1.Title = "Save your spreadsheet as an XML file";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.spreadsheet.SaveAsXML(saveFileDialog1.FileName);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:Element should begin with upper-case letter", Justification = "this was created as a default, changing the name would cause assembly issues")]
        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.InitializeDataGrid();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML-File | *.xml";
            openFileDialog.Title = "Open your spreadsheet as an XML file";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.spreadsheet.LoadXML(openFileDialog.FileName);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:Element should begin with upper-case letter", Justification = "this was created as a default, changing the name would cause assembly issues")]
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.spreadsheet = new Spreadsheet(50, 26);
            this.InitializeDataGrid();
            this.InitializeMenuButtons();
            this.spreadsheet.CellPropertyChanged += this.SpreadsheetPropertyChanged;
        }
    }
}
