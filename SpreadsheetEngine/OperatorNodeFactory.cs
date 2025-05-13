// <copyright file="OperatorNodeFactory.cs" company="Kaden Metzger Id: 11817362">
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
    /// Operator Node Factory Class responsible for creating Operator Nodes.
    /// </summary>
    internal static class OperatorNodeFactory
    {
        /// <summary>
        /// Create Operator Node Method which creates instances of the Operator Nodes based on their operator.
        /// </summary>
        /// <param name="op">The operator that is being used in the expression.</param>
        /// <returns>The operator node that corresponds to the operator used.</returns>
        public static OperatorNode CreateOperatorNode(char op)
        {
            if (op == '+')
            {
                AdditionOperatorNode addNode = new AdditionOperatorNode();
                return addNode;
            }

            if (op == '-')
            {
                SubtractionOperatorNode subNode = new SubtractionOperatorNode();
                return subNode;
            }

            if (op == '*')
            {
                MultiplicationOperatorNode multNode = new MultiplicationOperatorNode();
                return multNode;
            }

            if (op == '/')
            {
                DivisionOperatorNode divNode = new DivisionOperatorNode();
                return divNode;
            }

            return null;
        }
    }
}
