// <copyright file="UpdateColorCommand.cs" company="Kaden Metzger Id: 11817362">
// Copyright (c) Kaden Metzger Id: 11817362. All rights reserved.
// </copyright>

namespace SpreadsheetEngine.SpreadsheetCommands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Update color command.
    /// </summary>
    public class UpdateColorCommand : ICommand
    {
        private Spreadsheet spreadsheet;
        private List<(int row, int column, uint oldColor, uint newColor)> cellChanges;
        private string type = "Text";

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateColorCommand"/> class.
        /// </summary>
        /// <param name="spreadsheet">Takes a spreadsheet.</param>
        /// <param name="newColor">A new color input.</param>
        /// <param name="selectedCells">and a list of selceted cells that are to be updated.</param>
        public UpdateColorCommand(Spreadsheet spreadsheet, uint newColor, List<(int row, int column)> selectedCells)
        {
            this.spreadsheet = spreadsheet;
            this.cellChanges = new List<(int, int, uint, uint)>();

            // For every cell that is selected we add it to the list of cell changes that stores the data for each cell.
            foreach (var cell in selectedCells)
            {
                uint oldColor = this.spreadsheet.GetCell(cell.row, cell.column).BGColor;
                this.cellChanges.Add((cell.row, cell.column, oldColor, newColor));
            }
        }

        /// <summary>
        /// Execute command.
        /// </summary>
        public void Execute()
        {
            // For every cell in the selcted cells we update its color to the new color.
            foreach (var (row, column, oldColor, newColor) in this.cellChanges)
            {
                this.spreadsheet.UpdateCellColor(newColor, row, column);
            }
        }

        /// <summary>
        /// Undo command.
        /// </summary>
        public void Undo()
        {
            // For every selcted cell we update to its old color.
            foreach (var (row, column, oldColor, newColor) in this.cellChanges)
            {
                this.spreadsheet.UpdateCellColor(oldColor, row, column);
            }
        }
    }
}
