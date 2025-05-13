// <copyright file="ICommand.cs" company="Kaden Metzger Id: 11817362">
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
    /// Interface for commands.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Execute method.
        /// </summary>
        public void Execute();

        /// <summary>
        /// Undo method.
        /// </summary>
        public void Undo();
    }
}
