// <copyright file="Cell.cs" company="Kaden Metzger Id: 11817362">
// Copyright (c) Kaden Metzger Id: 11817362. All rights reserved.
// </copyright>

namespace SpreadsheetEngine
{
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Formats.Asn1;
    using System.Threading.Tasks.Sources;

    /// <summary>
    /// Abstract class cell. Contains cell information.
    /// </summary>
    public abstract class Cell : INotifyPropertyChanged
    {
        /// <summary>
        /// Protected string text. Is the actual text that is entered inside of a cell.
        /// </summary>
#pragma warning disable SA1401 // Fields should be private

        // Disabled since this must be a protected field per the instructions requirement.
        protected string text;
#pragma warning restore SA1401 // Fields should be private

        /// <summary>
        /// Protected string value. It is the evaluated string of the text. Used when there is a formula entered.
        /// </summary>
#pragma warning disable SA1401 // Fields should be private

        // Disabled since this must be a protected field per the instructions requirement.
        protected string cellValue;
#pragma warning restore SA1401 // Fields should be private

        private int rowIndex;
        private int columnIndex;
        private uint color;

        /// <summary>
        /// Initializes a new instance of the <see cref="Cell"/> class.
        /// Cell constructor which is declared with a row and column index.
        /// </summary>
        /// <param name="row">Row index of cell.</param>
        /// <param name="column">Column index of cell.</param>
        public Cell(int row, int column)
        {
            this.color = 0xFFFFFFFF;
            this.rowIndex = row;
            this.columnIndex = column;
        }

        /// <summary>
        /// PropertyChanged used to notify when a property of the cell is being changed.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged = (sender, e) => { };

        /// <summary>
        /// Gets rowIndex, read-only.
        /// </summary>
        public int RowIndex
        {
            get
            {
                return this.rowIndex;
            }
        }

        /// <summary>
        /// Gets columnIndex, read-only.
        /// </summary>
        public int ColumnIndex
        {
            get
            {
                return this.columnIndex;
            }
        }

        /// <summary>
        /// Gets or sets and Sets BGColor property. Also notifies when property is changed.
        /// </summary>
        public uint BGColor
        {
            get
            {
                return this.color;
            }

            set
            {
                this.color = value;
                Console.WriteLine($"PropertyChanged triggered for {this} with new color: {value}");
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.BGColor)));
            }
        }

        /// <summary>
        /// Gets or sets text field.
        /// </summary>
        public string Text
        {
            get => this.text;
            set
            {
                if (this.text == value)
                {
                    return;
                }
                else
                {
                    // add the new property that was just changed
                    this.text = value;
                    Console.WriteLine($"PropertyChanged triggered for {this} with new text: {value}");

                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Text)));
                }
            }
        }

        /// <summary>
        /// Gets cellValue. Setter is internal, meaning only the engine classes can access it.
        /// </summary>
        public string Value
        {
            get => this.cellValue;
            internal set
            {
                this.cellValue = value;
                Console.WriteLine($"PropertyChanged triggered for {this} with new value: {value}");

                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Value)));
            }
        }
    }
}
