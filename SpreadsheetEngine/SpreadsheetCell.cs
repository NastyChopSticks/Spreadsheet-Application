// <copyright file="SpreadsheetCell.cs" company="Kaden Metzger Id: 11817362">
// Copyright (c) Kaden Metzger Id: 11817362. All rights reserved.
// </copyright>

namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection.Metadata;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Internal class Spreadsheet cell. Can only be accessed by spreadsheet class. It is used for creating an array of cells. Since we inherit, we can simply downcast to cells.
    /// </summary>
    internal class SpreadsheetCell : Cell
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpreadsheetCell"/> class.
        /// Simple constructor which is exactly the same as the cell class. Only difference is this one is non abstract.
        /// </summary>
        /// <param name="row">Takes an input of the index of the row.</param>
        /// <param name="column">Takes an input of the index of the column.</param>
        public SpreadsheetCell(int row, int column)
            : base(row, column)
        {
        }
    }
}
