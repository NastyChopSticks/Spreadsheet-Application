// <copyright file="ExpressionTree.cs" company="Kaden Metzger Id: 11817362">
// Copyright (c) Kaden Metzger Id: 11817362. All rights reserved.
// </copyright>

namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Reflection.Metadata.Ecma335;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    /// <summary>
    /// Expression Tree class which handles all expression tree functionality logic such as construction, setting values, evaluating tree.
    /// </summary>
    public class ExpressionTree
    {
        private Dictionary<string, string> varDict;
        private List<string> varList = new List<string>();
        private Dictionary<string, double> deprecatedVarDict;
        private Dictionary<string, int> nodeChain = new Dictionary<string, int>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionTree"/> class.
        /// Constructor that takes a string input and parses it into a tree.
        /// </summary>
        /// <param name="expression">The string that will be parsed.</param>
        public ExpressionTree(string expression)
        {
            // The constructor will build the entire tree
            // Algorithm for creating tree
            // 1) Find middle operator.
            // 2) Split into two substrings, left and right
            // 3) run recursion until there is no middle operator found in the substring/expression
            if (expression.Length >= 5000)
            {
                throw new OverflowException();
            }

            this.RootNode = this.BuildExpressionTree(expression, this.RootNode);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionTree"/> class.
        /// Spreadsheet version of constructor.
        /// </summary>
        /// <param name="expression">expression.</param>
        /// <param name="variableDict">All current variables stored in spreadsheet.</param>
        /// <param name="sender">The cell that contains the expression.</param>
        public ExpressionTree(string expression, string sender, Dictionary<string, string> variableDict)
        {
            if (expression.Length >= 5000)
            {
                throw new OverflowException();
            }

            this.varDict = variableDict;
            this.RootNode = this.BuildExpressionTree(expression, sender, variableDict, this.RootNode);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionTree"/> class.
        /// Note this constructor is used so we can access certain methods without invoking any exceptions.
        /// </summary>
        public ExpressionTree()
        {
        }

        // Note that the root node is always an operator in an expression tree.
        private Node RootNode { get; set; }

        /// <summary>
        /// Gets the expression from the Expression Tree.
        /// </summary>
        /// <returns>Returns a string.</returns>
        public string GetExpression()
        {
            return this.RootNode.GetExpression();
        }

        /// <summary>
        /// Public method calls private.
        /// </summary>
        /// <returns>Returns a double of the evaluated equation.</returns>
        public double Evaluate()
        {
            return this.RootNode.Evaluate();
        }

        /// <summary>
        /// Method used to find dependent cells of an expression. stores all variables used in a list.
        /// </summary>
        /// <returns>Returns the list of variables that were used in the expression.</returns>
        public List<string> GetVarList()
        {
            return this.varList;
        }

        /// <summary>
        /// public set variable, calls private.
        /// </summary>
        /// <param name="expression">string expression input.</param>
        /// <param name="variableValue">the value we want the variable to be set to.</param>
        public void SetVariable(string expression, double variableValue)
        {
            this.SetVariable(expression, variableValue, this.RootNode);
        }

        /// <summary>
        /// This method is used to populate the varList list that tells us which cells are used for dependencies.
        /// </summary>
        /// <param name="expression">Takes an expression input.</param>
        public void PopulateVarList(string expression)
        {
            string variablePattern = @"^[A-Z](?:[1-9]|[1-4][0-9]|50)$";
            if (expression[0] == '=')
            {
                expression = expression.Substring(1);
            }

            // This is the same as the build expression tree method exception it only reads variable nodes and adds them to the list.
            Queue<string> postFix = Parser.ConvertToPostFix(expression);
            Stack<Node> stackTree = new Stack<Node>();
            while (postFix.Count != 0)
            {
                string node = postFix.Dequeue();
                if (Regex.IsMatch(node, variablePattern))
                {
                    this.varList.Add(node);
                }
            }
        }

        /// <summary>
        /// Set variable method which recursively searches the tree for the expression. If not found then nothing is updated. If found the value is updated.
        /// </summary>
        /// <param name="expression">Expression string input that will be searched for.</param>
        /// <param name="variableValue">The value the expression is to be set to.</param>
        /// <param name="root">The root node of the tree used for traversal.</param>
        private void SetVariable(string expression, double variableValue, Node root)
        {
            if (root is OperatorNode opNode)
            {
                this.SetVariable(expression, variableValue, opNode.LeftChild);
                this.SetVariable(expression, variableValue, opNode.RightChild);
            }

            if (root is VariableNode varNode && varNode.Data == expression)
            {
                varNode.Value = variableValue;
            }
        }

        /// <summary>
        /// Update build expression tree that uses shunting yard algorithm.
        /// </summary>
        /// <param name="expression">takes a string expression.</param>
        /// <param name="root">takes root node of tree.</param>
        /// <returns>returns built expression tree.</returns>
        private Node BuildExpressionTree(string expression, string sender, Dictionary<string, string> varDict, Node root)
        {
            // The new algorithm will be as follows.
            // 1. convert expression to postfix
            // 2. create tree by using the postfix queue and a tree stack
            // 2.1 for every number/varaible/operand in the queue create a tree and push that tree to a stack
            // 2.2 when a operator is encountered in the postfix queue pop 2 operands.
            // 2.2 make the operator the root node and leftchild is the first popped element and the right the last popped element. now we push the tree back to the stack
            // 2.3 continue this process until no more symbols are left in the queue or the stack contains only one tree
            string variablePattern = @"^[A-Z](?:[1-9]|[1-4][0-9]|50)$";
            string constantPattern = @"^[0-9]+$";
            string operatorPattern = @"^[\+\-\*/]$";

            if (expression[0] == '=')
            {
                expression = expression.Substring(1);
            }

            // Convert to post fix
            Queue<string> postFix = Parser.ConvertToPostFix(expression);
            Stack<Node> stackTree = new Stack<Node>();

            // While the post fix queue is not empty
            while (postFix.Count != 0)
            {
                // Gather a node from the queue
                string node = postFix.Dequeue();

                // If the node is a variable node
                if (Regex.IsMatch(node, variablePattern))
                {
                    // Check if the reference is a self reference
                    if (sender == node)
                    {
                        throw new SelfReferenceException();
                    }

                    // If the variable dictionary contains the node, we know that the reference is not empty
                    if (this.varDict.ContainsKey(node))
                    {
                        // Since our variable dictionary takes the full expression rather than the computed value of the expression we need to check if its value is something like "=A1"
                        if (this.varDict[node][0] == '=')
                        {
                            // Here we use recursion and a dictionary to keep track if there is a circular reference.
                            sender = node;
                            string subExpr = this.varDict[node].Substring(1);

                            // Basically we have this node chain which keeps track which nodes have already been referenced. If a cell is referenced more than once in a chain of expressions then we know there is a circular reference present
                            if (!this.nodeChain.ContainsKey(subExpr))
                            {
                                this.nodeChain.Add(subExpr, 1);
                            }
                            else
                            {
                                throw new CircularReferenceException();
                            }

                            // Here we use recursion to evaluate the sub expression that was attached to the variable dictionary
                            // The reason why we are taking the full expression that is associated with the sender node is that we need to dynamically check of any of the references have been updated to a bad, self, circular, or empty reference
                            double value = this.BuildExpressionTree(subExpr, sender, varDict, root).Evaluate();
                            VariableNode varNode = new VariableNode(node);
                            varNode.Value = value;
                            this.varList.Add(varNode.Data);
                            stackTree.Push(varNode);
                        }
                        else
                        {
                            VariableNode varNode = new VariableNode(node);
                            varNode.Value = double.Parse(varDict[node]);
                            this.varList.Add(varNode.Data);
                            stackTree.Push(varNode);
                        }
                    }
                    else
                    {
                        // If the variable doesnt exist in the dictionary then its an empty reference so we set the value to 0.
                        VariableNode varNode = new VariableNode(node);
                        varNode.Value = 0;
                        this.varList.Add(varNode.Data);
                        stackTree.Push(varNode);
                    }
                }

                // If its a constant
                else if (Regex.IsMatch(node, constantPattern))
                {
                    ConstantNode constNode = new ConstantNode(int.Parse(node));
                    stackTree.Push(constNode);
                }

                // If its an operator
                else if (Regex.IsMatch(node, operatorPattern))
                {
                    OperatorNode opNode = OperatorNodeFactory.CreateOperatorNode(node[0]);
                    opNode.RightChild = stackTree.Pop();
                    opNode.LeftChild = stackTree.Pop();
                    stackTree.Push(opNode);
                }

                // Else, then its a bad reference or the input contained characters that cant be read by the tree.
                else
                {
                    throw new ArgumentException();
                }
            }

            root = stackTree.Pop();
            return root;
        }

        /// <summary>
        /// This expression tree is used for creating trees with default values of 0. This is separate from how the spreadsheet tree is constructed and HARDLY VAREIES IN CODE at all.
        /// The ONLY DIFFERENCE is that we set default values for this kind of tree since it wont be reading from a spreadsheet.
        /// This is important so that old tests will still pass.
        /// </summary>
        /// <param name="expression">expression input.</param>
        /// <param name="root">root node of expression tree.</param>
        /// <returns>returns built expression tree.</returns>
        private Node BuildExpressionTree(string expression, Node root)
        {
            string variablePattern = @"^[a-zA-z][a-zA-z0-9]*$";
            string constantPattern = @"^[0-9]+$";
            string operatorPattern = @"^[\+\-\*/]$";

            if (expression[0] == '=')
            {
                expression = expression.Substring(1);
            }

            Queue<string> postFix = Parser.ConvertToPostFix(expression);
            Stack<Node> stackTree = new Stack<Node>();
            while (postFix.Count != 0)
            {
                string node = postFix.Dequeue();
                if (Regex.IsMatch(node, variablePattern))
                {
                    VariableNode varNode = new VariableNode(node);
                    varNode.Value = 0;
                    stackTree.Push(varNode);
                }
                else if (Regex.IsMatch(node, constantPattern))
                {
                    ConstantNode constNode = new ConstantNode(int.Parse(node));
                    stackTree.Push(constNode);
                }
                else if (Regex.IsMatch(node, operatorPattern))
                {
                    OperatorNode opNode = OperatorNodeFactory.CreateOperatorNode(node[0]);
                    opNode.RightChild = stackTree.Pop();
                    opNode.LeftChild = stackTree.Pop();
                    stackTree.Push(opNode);
                }
                else
                {
                    throw new ArgumentException();
                }
            }

            root = stackTree.Pop();
            return root;
        }

        /// <summary>
        /// Old Method that builds the entire expression tree, no longer used.
        /// </summary>
        /// <param name="expression">Takes an expression string input.</param>
        /// <param name="root">Takes the root node of the tree for traversal.</param>
        /// <returns>Returns the head node of the built tree.</returns>
        private Node DeprecatedBuildExpressionTree(string expression, Node root)
        {
            // Algorithm for creating tree
            // 1) Clean parentheses
            // 2) Find lowest precedence operator
            // 3) Split into two substrings, left and right
            // 4) run recursion until there is no middle operator found in the substring/expression

            // Clean '=' from expression
            if (expression[0] == '=')
            {
                expression = expression.Substring(1);
            }

            // Clean parentheses
            expression = Parser.CleanParentheses(expression);

            // Find middle operator
            int operatorIndex = Parser.FindLowestPrecedenceOp(expression);

            // This means that we have hit a base case. The expression is either a constant or a variable
            // we will use regex to check if its a variable or valid constant
            if (operatorIndex == -1)
            {
                string pattern = @"^[a-zA-z][a-zA-z0-9]*$";
                string constantPattern = @"^[0-9]+$";

                if (Regex.IsMatch(expression, pattern))
                {
                    root = new VariableNode(expression);
                    if (this.varDict.ContainsKey(expression))
                    {
                        VariableNode varNode = root as VariableNode;
                        varNode.Value = this.deprecatedVarDict[expression];
                        this.varList.Add(varNode.Data);
                        return varNode;
                    }
                    else
                    {
                        VariableNode varNode = root as VariableNode;
                        varNode.Value = 0;
                        this.varList.Add(varNode.Data);
                        return varNode;
                    }
                }
                else if (Regex.IsMatch(expression, constantPattern))
                {
                    root = new ConstantNode(int.Parse(expression));
                }

                return root;
            }

            // If tree is empty then make head node the lowest precedence operator
            if (root == null)
            {
                root = OperatorNodeFactory.CreateOperatorNode(expression[operatorIndex]);
            }

            // Not empty then we traverse left and right recursively
            if (root is OperatorNode opNode)
            {
                string leftSubString = expression.Substring(0, operatorIndex);
                string rightSubString = expression.Substring(operatorIndex + 1);
                opNode.LeftChild = this.BuildExpressionTree(leftSubString, opNode.LeftChild);
                opNode.RightChild = this.BuildExpressionTree(rightSubString, opNode.RightChild);
            }

            // return root so we can set this.RootNode = root;
            return root;
        }

        /// <summary>
        /// Returns root node.
        /// </summary>
        /// <returns>Root node of tree.</returns>
        private Node GetRootNode()
        {
            return this.RootNode;
        }
    }
}
