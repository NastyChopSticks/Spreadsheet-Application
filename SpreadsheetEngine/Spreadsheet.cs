// <copyright file="Spreadsheet.cs" company="Kaden Metzger Id: 11817362">
// Copyright (c) Kaden Metzger Id: 11817362. All rights reserved.
// </copyright>

namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data.Common;
    using System.Diagnostics;
    using System.Linq;
    using System.Net.Http.Headers;
    using System.Net.NetworkInformation;
    using System.Reflection;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Xml;
    using SpreadsheetEngine.SpreadsheetCommands;

    /// <summary>
    /// Spreadsheet class, public. Used for creating a logical spreadsheet. This class deals with all logic and is separate from the UI.
    /// </summary>
    public class Spreadsheet
    {
        private Cell[,] cells;
        private int columnCount;
        private int rowCount;
        private UndoRedoManager undoRedoManager;
        private Dictionary<string, List<string>> dependents = new Dictionary<string, List<string>>();
        private Dictionary<string, int> exitInfiniteRecursion = new Dictionary<string, int>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Spreadsheet"/> class.
        /// Creates a spreadsheet based on an input of rows and columns.
        /// </summary>
        /// <param name="rows">Used to declare how many rows the user wants.</param>
        /// <param name="columns">Used to declare how many columns the user wants.</param>
        /// <exception cref="IndexOutOfRangeException">Thrown when input is out of range ie. > 50, > 26. < 0.</exception>
        public Spreadsheet(int rows, int columns)
        {
            if (columns > 26 || rows > 50 || columns <= 0 || rows <= 0)
            {
                throw new IndexOutOfRangeException();
            }

            // Initialize undoRedoManager
            this.undoRedoManager = new UndoRedoManager();

            // allocate the array of cells
            this.cells = new Cell[rows, columns];

            // update column and row count
            this.columnCount = columns;
            this.rowCount = rows;

            // populate the array of cells.
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    this.cells[i, j] = new SpreadsheetCell(i, j);
                    this.cells[i, j].PropertyChanged += this.CellPropertyChangedHandler;
                }
            }
        }

        /// <summary>
        /// Property Event Cell property changed. Used to notify when a cell property is changed.
        /// </summary>
        public event PropertyChangedEventHandler CellPropertyChanged = (sender, e) => { };

        /// <summary>
        /// Gets columnCount, read only property.
        /// </summary>
        public int ColumnCount
        {
            get
            {
                return this.columnCount;
            }
        }

        /// <summary>
        /// Gets rowCount, read only property.
        /// </summary>
        public int RowCount
        {
            get
            {
                return this.rowCount;
            }
        }

        /// <summary>
        /// Executes command.
        /// </summary>
        /// <param name="command">command input.</param>
        public void ExecuteCommand(ICommand command)
        {
            this.undoRedoManager.ExecuteCommand(command);
        }

        /// <summary>
        /// undoes command.
        /// </summary>
        public void Undo()
        {
            this.undoRedoManager.Undo();
        }

        /// <summary>
        /// Redoes command.
        /// </summary>
        public void Redo()
        {
            this.undoRedoManager.Redo();
        }

        /// <summary>
        /// Gets size of redo stack.
        /// </summary>
        /// <returns>returns size of redo stack.</returns>
        public int GetRedoSize()
        {
            return this.undoRedoManager.GetRedoSize();
        }

        /// <summary>
        /// Gets undo stack size.
        /// </summary>
        /// <returns>returns undo stack size.</returns>
        public int GetUndoSize()
        {
            return this.undoRedoManager.GetUndoSize();
        }

        /// <summary>
        /// Gets a cell based on its index.
        /// </summary>
        /// <param name="row">Index of cell row.</param>
        /// <param name="column">Index of cell column.</param>
        /// <returns>Returns the cell at row, column.</returns>
        /// <exception cref="IndexOutOfRangeException">Throws an exception when the accessed row or column is out of range, ie negative.</exception>
        public Cell GetCell(int row, int column)
        {
            // If row or column is negative throw exception.
            if (row < 0 || column < 0 || row > this.rowCount || column > this.columnCount)
            {
                throw new IndexOutOfRangeException();
            }

            // if the input is greater than the declared size return null.
            if (row > this.RowCount || column > this.ColumnCount)
            {
                throw new IndexOutOfRangeException();
            }

            // return the cell.
            return this.cells[row, column];
        }

        /// <summary>
        /// Update Value method that updates the value of a cell based on the input text.
        /// </summary>
        /// <param name="expression">Text that was inputted into the cell.</param>
        /// <param name="row">The row of the cell that is being updated.</param>
        /// <param name="column">The column of the cell that is being updated.</param>
        public void UpdateCell(string expression, int row, int column)
        {
            if (row < 0 || column < 0 || row > this.RowCount || column > this.columnCount)
            {
                throw new IndexOutOfRangeException();
            }

            // If the expression is empty, meaning the user deleted the cell values, swap back to NULL which is a default value.
            // We also remove it from our cell dependencies since it has been deleted.
            if (expression == string.Empty)
            {
                this.cells[row, column].Value = null;
                this.cells[row, column].Text = null;
                this.RemoveDependent(row, column);
                return;
            }

            // If the expression is null then we remove dependencies, this is for a special case. This code prevents crashing.
            if (expression == null)
            {
                this.RemoveDependent(row, column);
                return;
            }

            if (expression[0] != '=')
            {
                this.cells[row, column].Value = expression;
                this.cells[row, column].Text = expression;
            }
            else
            {
                this.EvaluateCell(expression, row, column);
            }

            // ... now we check if the current cell being updated is in fact apart of the dependencies.
            string curCell = this.ConvertToVariable((row, column));

            // The current cell is a dependency we then revaluate ALL expressions
            if (this.dependents.ContainsKey(curCell))
            {
                // expressionCell is the cell that contains the expression that needs to be updated.
                string expressionCell = string.Empty;
                ValueTuple<int, int> expressionCellCoordinates = (0, 0);
                for (int i = 0; i < this.dependents[curCell].Count; i++)
                {
                    // We need a way to check if there is infinite recursion. This kind of slip still happens because we need circular references to still be dependent cells.
                    // Meaning we can have A1->B1 and B1->A1 as valid dependencies so that the user can update either cell and see the changes reflected.
                    // In event of this case we have an infinite recursion checker. Since the tree will automatically evaluate correctly and handle exceptions we only need to worry about the dependencies chaining over and over
                    // Hence why we have another dictionary that keeps track of whenever a dependent cell is called. If the dependent cell is called more than once then we know that the evaluation of the current expression
                    // causes infinite recursion and we need to exit as soon as the we know infinite recursion has taken place.
                    expressionCell = this.dependents[curCell][i];
                    if (!this.exitInfiniteRecursion.ContainsKey(expressionCell))
                    {
                        this.exitInfiniteRecursion.Add(expressionCell, 1);
                    }
                    else
                    {
                        return;
                    }

                    // Convert to a tuple so we can get the expressionCell expression
                    expressionCellCoordinates = this.ConvertToTuple(expressionCell);
                    expression = this.GetCell(expressionCellCoordinates.Item1, expressionCellCoordinates.Item2).Text;
                    this.UpdateCell(expression, expressionCellCoordinates.Item1, expressionCellCoordinates.Item2);
                }

                // Important that we clear the infinite recursion dictionary since each cell's expression is unique.
                this.exitInfiniteRecursion = new Dictionary<string, int>();
            }
        }

        /// <summary>
        /// Updates the color of a cell via logic.
        /// </summary>
        /// <param name="color">takes a hex color.</param>
        /// <param name="row">the row we want to change.</param>
        /// <param name="column">the column we want to change.</param>
        public void UpdateCellColor(uint color, int row, int column)
        {
            this.cells[row, column].BGColor = color;
        }

        /// <summary>
        /// method for saving the spreadsheet as an xml.
        /// </summary>
        /// <param name="filePath">takes a file path as input.</param>
        public void SaveAsXML(string filePath)
        {
            using (XmlWriter writer = XmlWriter.Create(filePath, new XmlWriterSettings { Indent = true }))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");
                for (int i = 0; i < this.rowCount; i++)
                {
                    for (int j = 0; j < this.columnCount; j++)
                    {
                        if (this.GetCell(i, j).Text != null || this.GetCell(i, j).BGColor != 0xFFFFFFFF)
                        {
                            char letter = (char)(j + 65);
                            string cell = letter + (i + 1).ToString();
                            writer.WriteStartElement("cell");
                            writer.WriteAttributeString("name", cell);
                            writer.WriteStartElement("bgcolor");
                            writer.WriteString(this.cells[i, j].BGColor.ToString());
                            writer.WriteEndElement();
                            writer.WriteStartElement("text");
                            writer.WriteString(this.cells[i, j].Text);
                            writer.WriteEndElement();
                            writer.WriteEndElement();
                        }
                    }
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        /// <summary>
        /// Method for loading xml spreadsheet files.
        /// </summary>
        /// <param name="filePath">takes a file path as input.</param>
        public void LoadXML(string filePath)
        {
            // we need to clear our spreadsheet when loading
            this.ClearSpreadsheet();

            // declare some variables that will be used in the loading loop.
            string cell = string.Empty;
            ValueTuple<int, int> cords = (0, 0);
            uint color = 0;
            string text = string.Empty;

            using (XmlReader reader = XmlReader.Create(filePath))
            {
                while (reader.Read())
                {
                    // If we reach a cell element
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "cell")
                    {
                        // Get the cell's name attribute
                        cell = reader.GetAttribute("name");

                        // Convert the variable, ex: A1 to coordiantes
                        cords = this.ConvertToTuple(cell);

                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                // If we reach bg color element
                                if (reader.Name == "bgcolor")
                                {
                                    // get the color
                                    color = uint.Parse(reader.ReadElementContentAsString());
                                }
                                else if (reader.Name == "text")
                                {
                                    // We reach the text element store it
                                    text = reader.ReadElementContentAsString();
                                }
                            }
                            else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "cell")
                            {
                                break;
                            }
                        }

                        // update the spreadsheet based on the colors we got and the text
                        this.UpdateCell(text, cords.Item1, cords.Item2);
                        this.UpdateCellColor(color, cords.Item1, cords.Item2);
                    }
                }
            }
        }

        /// <summary>
        /// Removes dependent cells from dictionary.
        /// </summary>
        /// <param name="row">the cord of the row.</param>
        /// <param name="column">the cord of the column.</param>
        private void RemoveDependent(int row, int column)
        {
            string cell = this.ConvertToVariable((row, column));
            this.dependents.Remove(cell);
        }

        /// <summary>
        /// Clears the spreadsheet.
        /// </summary>
        private void ClearSpreadsheet()
        {
            this.undoRedoManager.ClearStacks();
            for (int i = 0; i < this.RowCount; i++)
            {
                for (int j = 0; j < this.columnCount; j++)
                {
                    this.cells[i, j].Value = null;
                    this.cells[i, j].Text = null;
                    this.cells[i, j].BGColor = 0xFFFFFFFF;
                }
            }
        }

        /// <summary>
        /// Refactoring our CopyCell method to make a new one that handles copying and evaluating.
        /// </summary>
        /// <param name="expression">the expression.</param>
        /// <param name="row">the row of the expression.</param>
        /// <param name="column">the column of the expression.</param>
        private void EvaluateCell(string expression, int row, int column)
        {
            // First we update the cell to the current expression. This is because we need to get the expression ASAP so it will be added to dependents.
            this.cells[row, column].Value = expression;
            this.cells[row, column].Text = expression;

            // The sender is the cell that contains the expression
            string sender = this.ConvertToVariable((row, column));

            // We need to test if the tree throws any exceptions, this means the cell reference was bad or that there was a circular or self reference.
            try
            {
                // Interestingly enough we need to ensure that even self references and circular references are added to dependents
                // Not adding them would not allow us to update the spreadsheet when updating the circular refernce cell
                ExpressionTree testForException = new ExpressionTree();
                testForException.PopulateVarList(expression);

                // We add dependents here
                this.AddDependents(testForException, row, column);

                // then call the constructor that finds exceptions
                testForException = new ExpressionTree(expression, sender, this.GetVariableDict());
            }

            // we catch self references here.
            catch (SelfReferenceException)
            {
                this.cells[row, column].Value = "!(self reference)";
                this.cells[row, column].Text = expression;
                return;
            }

            // circular references here
            catch (CircularReferenceException)
            {
                this.cells[row, column].Value = "!(circular reference)";
                this.cells[row, column].Text = expression;
                return;
            }

            // bad references here
            catch (ArgumentException)
            {
                this.cells[row, column].Value = "!(bad reference)";
                this.cells[row, column].Text = expression;
                return;
            }

            ExpressionTree cellTree = new ExpressionTree(expression, sender, this.GetVariableDict());
            this.cells[row, column].Value = cellTree.Evaluate().ToString();
            this.cells[row, column].Text = expression;
        }

        private void AddDependents(ExpressionTree tree, int row, int column)
        {
            // So this is the idea for handling dependent cells.
            // Basically, we will store every dependent cell in a list of cells. so if we have Cell A1 -> =B1-B2 we will store B1:A1 and B2:A1. If we have C1=B1*B2 we simply add C1 to the key. so B1:A1,C1 and B2:A1,C1
            // This will be done every time we have an expression, hence why this operation is done in the evaluate cell method.
            // Now, we will move to the UpdateCell method...

            // Get variable list, contains all variable nodes that were used in the expression that was just evaluated.
            List<string> varList = tree.GetVarList();

            // Convert the current cell to a string so we can store it.
            string curCell = this.ConvertToVariable((row, column));

            // For every variable we add its dependent cells.
            for (int i = 0; i < varList.Count; i++)
            {
                // If the key doesn't exist currently
                if (!this.dependents.ContainsKey(varList[i]))
                {
                    this.dependents[varList[i]] = new List<string>();
                    this.dependents[varList[i]].Add(curCell);
                }
                else if (!this.dependents[varList[i]].Contains(curCell))
                {
                    // If the key does exist we add the value.
                    this.dependents[varList[i]].Add(curCell);
                }
            }
        }

        /// <summary>
        /// Finds all cells that have values and stores in dictionary.
        /// </summary>
        /// <returns>returns the dictionary.</returns>
        private Dictionary<string, string> GetVariableDict()
        {
            Dictionary<string, string> varDict = new Dictionary<string, string>();
            double parsedValue;
            for (int i = 0; i < this.rowCount; i++)
            {
                for (int j = 0; j < this.columnCount; j++)
                {
                    if (this.cells[i, j].Value != null)
                    {
                        // Add the expression itself rather than the value of the cell, this is done so that we can reference cells that are circular or bad or self referenced or empty
                        varDict.Add((char)(j + 65) + (i + 1).ToString(), this.cells[i, j].Text);
                    }
                }
            }

            return varDict;
        }

        /// <summary>
        /// Converts a variable to a row and column.
        /// </summary>
        /// <param name="variable">Cell name.</param>
        /// <returns>returns the row and column in tuple form.</returns>
        private ValueTuple<int, int> ConvertToTuple(string variable)
        {
            int column = (int)variable[0] - 65;
            int row = int.Parse(variable.Substring(1)) - 1;
            return (row, column);
        }

        /// <summary>
        /// Converts a row and column into a variable/cell name.
        /// </summary>
        /// <param name="varCoordinates">a tuple of row and column indicies.</param>
        /// <returns>the converted variable in string form.</returns>
        private string ConvertToVariable(ValueTuple<int, int> varCoordinates)
        {
            string column = string.Empty;
            string row = string.Empty;
            column = ((char)(varCoordinates.Item2 + 65)).ToString();
            row = (varCoordinates.Item1 + 1).ToString();
            return column + row;
        }

        /// <summary>
        /// Used to invoke the property changed so we can access the specific property that was changed.
        /// </summary>
        /// <param name="sender">default param.</param>
        /// <param name="e">default param..</param>
        private void CellPropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            this.CellPropertyChanged?.Invoke(sender, e);
        }
    }
}
