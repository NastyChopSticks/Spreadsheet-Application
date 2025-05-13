// <copyright file="UpdateCellCommand.cs" company="Kaden Metzger Id: 11817362">
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
    /// Update cell command class. inherits from ICommand interface.
    /// </summary>
    public class UpdateCellCommand : ICommand
    {
        private Spreadsheet spreadsheet;
        private string expression;
        private string oldExpression;
        private int row;
        private int column;
        private string type = "Text";

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCellCommand"/> class.
        /// </summary>
        /// <param name="spreadsheet">The spreadsheet we want to update.</param>
        /// <param name="expression">the expression we want to update a cell to.</param>
        /// <param name="row">the row of the cell.</param>
        /// <param name="column">the column of the cell.</param>
        public UpdateCellCommand(Spreadsheet spreadsheet, string expression, int row, int column)
        {
            this.spreadsheet = spreadsheet;
            this.expression = expression;
            this.row = row;
            this.column = column;
            this.oldExpression = spreadsheet.GetCell(row, column).Text;
        }

        /// <summary>
        /// Update cell command execute.
        /// </summary>
        public void Execute()
        {
            // call the update cell method.
            this.spreadsheet.UpdateCell(this.expression, this.row, this.column);
        }

        /// <summary>
        /// Undo command method.
        /// </summary>
        public void Undo()
        {
            if (this.oldExpression == null)
            {
                // check if the old expression is an empty cell
                this.spreadsheet.UpdateCell(string.Empty, this.row, this.column);
            }
            else
            {
                // else just simply update the cell to the previous expression
                this.spreadsheet.UpdateCell(this.oldExpression, this.row, this.column);
            }
        }
    }
}
