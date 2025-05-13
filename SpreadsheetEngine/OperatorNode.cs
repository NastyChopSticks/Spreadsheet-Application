// <copyright file="OperatorNode.cs" company="Kaden Metzger Id: 11817362">
// Copyright (c) Kaden Metzger Id: 11817362. All rights reserved.
// </copyright>

namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Operator Node class which is used for building the expression tree.
    /// </summary>
    internal class OperatorNode : Node
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OperatorNode"/> class.
        /// </summary>
        public OperatorNode()
        {
        }

        /// <summary>
        /// Gets or Sets the Data property for the OperatorNode.
        /// </summary>
        internal char Op { get; set; }

        /// <summary>
        /// Gets or Sets Left child Node for tree structure.
        /// </summary>
        internal Node LeftChild { get; set; }

        /// <summary>
        /// Gets or Sets Right child Node for tree structre.
        /// </summary>
        internal Node RightChild { get; set; }

        /// <summary>
        /// Returns the left child's expression, the node operator, and the right expression as a single string.
        /// </summary>
        /// <returns>Returns the full expression.</returns>
        internal override string GetExpression()
        {
            char nodeOperator = '0';

            nodeOperator = this.Op;
            return this.LeftChild.GetExpression() + nodeOperator + this.RightChild.GetExpression();
        }

        /// <summary>
        /// Returns a default value, since this is a base class for other operator nodes, it doesnt actually do anything.
        /// </summary>
        /// <returns>Default value.</returns>
        internal override double Evaluate()
        {
            return 0.0;
        }
    }

    /// <summary>
    /// Addition Node class.
    /// </summary>
    internal class AdditionOperatorNode : OperatorNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdditionOperatorNode"/> class.
        /// </summary>
        public AdditionOperatorNode()
        {
            this.Op = '+';
        }

        /// <summary>
        /// Override evaluate function which adds left and right sub trees.
        /// </summary>
        /// <returns>The value of the right and left tree's added.</returns>
        internal override double Evaluate()
        {
            return this.LeftChild.Evaluate() + this.RightChild.Evaluate();
        }
    }

    /// <summary>
    /// Subtraction Node class.
    /// </summary>
    internal class SubtractionOperatorNode : OperatorNode
    {
        private int precedence = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubtractionOperatorNode"/> class.
        /// </summary>
        public SubtractionOperatorNode()
        {
            this.Op = '-';
        }

        /// <summary>
        /// Overrides the evaluate method for subtraction.
        /// </summary>
        /// <returns>Computed value.</returns>
        internal override double Evaluate()
        {
            return this.LeftChild.Evaluate() - this.RightChild.Evaluate();
        }
    }

    /// <summary>
    /// Multiplication Operator Class.
    /// </summary>
    internal class MultiplicationOperatorNode : OperatorNode
    {
        private int precedence = 2;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiplicationOperatorNode"/> class.
        /// </summary>
        public MultiplicationOperatorNode()
        {
            this.Op = '*';
        }

        /// <summary>
        /// Overrides the evaluate method for multiplication.
        /// </summary>
        /// <returns>Computed value for multiplication.</returns>
        internal override double Evaluate()
        {
            return this.LeftChild.Evaluate() * this.RightChild.Evaluate();
        }
    }

    /// <summary>
    /// Division Operator Class.
    /// </summary>
    internal class DivisionOperatorNode : OperatorNode
    {
        private int precedence = 2;

        /// <summary>
        /// Initializes a new instance of the <see cref="DivisionOperatorNode"/> class.
        /// </summary>
        public DivisionOperatorNode()
        {
            this.Op = '/';
        }

        /// <summary>
        /// Overrides the evaluate method for division.
        /// </summary>
        /// <returns>Computed value for division.</returns>
        internal override double Evaluate()
        {
            return this.LeftChild.Evaluate() / this.RightChild.Evaluate();
        }
    }
}
