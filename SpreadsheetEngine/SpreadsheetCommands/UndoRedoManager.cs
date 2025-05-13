// <copyright file="UndoRedoManager.cs" company="Kaden Metzger Id: 11817362">
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
    /// UndoRedo Manager. Used for managing redo and undo commands.
    /// </summary>
    public class UndoRedoManager
    {
        // declare our stacks
        private Stack<ICommand> undoStack = new Stack<ICommand>();
        private Stack<ICommand> redoStack = new Stack<ICommand>();

        /// <summary>
        /// Execute command method. simply runs ICommand.Execute().
        /// </summary>
        /// <param name="command">Takes a command input.</param>
        public void ExecuteCommand(ICommand command)
        {
            command.Execute();
            this.undoStack.Push(command);
        }

        /// <summary>
        /// Undo method. simply calls the undo method.
        /// </summary>
        public void Undo()
        {
            // IF the stack is not empty we pop the command and push it to the redo stack.
            if (this.undoStack.Count > 0)
            {
                ICommand command = this.undoStack.Pop();
                command.Undo();
                this.redoStack.Push(command);
            }
        }

        /// <summary>
        /// Redo command method. calls the execute command from the redo stack.
        /// </summary>
        public void Redo()
        {
            // If the redo stack is not empty
            if (this.redoStack.Count > 0)
            {
                // pop the redo stack
                ICommand command = this.redoStack.Pop();

                // execute
                command.Execute();

                // push to the undo stack
                this.undoStack.Push(command);
            }
        }

        /// <summary>
        /// Get method.
        /// </summary>
        /// <returns>Returns the size of the redo stack.</returns>
        public int GetRedoSize()
        {
            return this.redoStack.Count;
        }

        /// <summary>
        /// Get method.
        /// </summary>
        /// <returns>Returns the size of the undo stack.</returns>
        public int GetUndoSize()
        {
            return this.undoStack.Count;
        }

        /// <summary>
        /// Clears the undo and redo stacks.
        /// </summary>
        public void ClearStacks()
        {
            this.undoStack.Clear();
            this.redoStack.Clear();
        }
    }
}
