// <copyright file="ConstantNode.cs" company="Kaden Metzger Id: 11817362">
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
    /// Constant Node that inherits from a base node class.
    /// </summary>
    internal class ConstantNode : Node
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConstantNode"/> class.
        /// ConstantNode constructor, takes an int as input.
        /// </summary>
        /// <param name="constant">An integer that will be stored in the node.</param>
        public ConstantNode(int constant)
        {
            this.Data = constant;
        }

        /// <summary>
        /// Gets or sets Data property that is used to represent the value stored in the node.
        /// </summary>
        internal int Data { get; set; }

        /// <summary>
        /// Overrides the GetExpression Method for Constant nodes.
        /// </summary>
        /// <returns>Returns the data in string form.</returns>
        internal override string GetExpression()
        {
            return this.Data.ToString();
        }

        /// <summary>
        /// Overrides the evaluate function for ConstantNodes.
        /// </summary>
        /// <returns>Returns the value stored in the node.</returns>
        internal override double Evaluate()
        {
            return this.Data;
        }
    }
}
