// <copyright file="Node.cs" company="Kaden Metzger Id: 11817362">
// Copyright (c) Kaden Metzger Id: 11817362. All rights reserved.
// </copyright>

namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices.Marshalling;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Base abstract class for nodes. Contains nothing and is simply used so we can create abstract types of nodes.
    /// </summary>
    public abstract class Node
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Node"/> class.
        /// </summary>
        public Node()
        {
        }

        /// <summary>
        /// Abstract method for getting the current expression.
        /// </summary>
        /// <returns>Returns the expression as a string.</returns>
        internal abstract string GetExpression();

        /// <summary>
        /// Evaluates the current expression.
        /// </summary>
        /// <returns>Returns a double of the computed value of the expression.</returns>
        internal abstract double Evaluate();
    }
}
