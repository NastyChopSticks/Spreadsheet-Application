// <copyright file="VariableNode.cs" company="Kaden Metzger Id: 11817362">
// Copyright (c) Kaden Metzger Id: 11817362. All rights reserved.
// </copyright>

namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Variable node class used to create variable nodes for the expression tree.
    /// </summary>
    internal class VariableNode : Node
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VariableNode"/> class.
        /// </summary>
        /// <param name="expression">Takes a variable input string.</param>
        internal VariableNode(string expression)
        {
            this.Data = expression;
        }

        /// <summary>
        /// Gets or Sets the string Data property.
        /// </summary>
        internal string Data { get; set; }

        /// <summary>
        /// Gets or Sets the value of the variable.
        /// </summary>
        protected internal double Value { get; set; }

        /// <summary>
        /// Overrides GetExpression for Variable Nodes.
        /// </summary>
        /// <returns>The variable stored in the node.</returns>
        internal override string GetExpression()
        {
            return this.Data;
        }

        /// <summary>
        /// Overrides the evaluate method.
        /// </summary>
        /// <returns>Returns the value associated with the variable.</returns>
        internal override double Evaluate()
        {
            return this.Value;
        }
    }
}
