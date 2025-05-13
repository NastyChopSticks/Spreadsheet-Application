// <copyright file="Parser.cs" company="Kaden Metzger Id: 11817362">
// Copyright (c) Kaden Metzger Id: 11817362. All rights reserved.
// </copyright>

namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Parser class that helps us parse the expression. Helps us build the tree much easier.
    /// </summary>
    public static class Parser
    {
        /// <summary>
        /// CleanParentheses method. Cleans useless parentheses from the expression.
        /// </summary>
        /// <param name="expression">Takes an expression input.</param>
        /// <returns>The cleaned expression.</returns>
        /// <exception cref="OverflowException">If the input if larger than size 5000 we throw an overflow.</exception>
        internal static string CleanParentheses(string expression)
        {
            // Check if the string is too large
            if (expression.Length >= 5000)
            {
                throw new OverflowException();
            }

            // While there exists a '(' and it's matching brace is at the end, essentially meaning they are useless parentheses, we parse them.
            while (FindMatchingBrace(expression, 0) == expression.Length - 1 && expression[0] == '(')
            {
                // Find the index we want to parse
                int braceIndex = FindMatchingBrace(expression, 0);

                // Parse
                expression = expression.Remove(0, 1);
                expression = expression.Remove(braceIndex - 1, 1);
            }

            // Return cleaned expression
            return expression;
        }

        /// <summary>
        /// Helper method that finds a matching brace's index. Useful for parsing.
        /// </summary>
        /// <param name="expression">Takes an expression input.</param>
        /// <param name="startingIndex">Takes the index of the left parentheses. A starting point for our algorithm (saves us time).</param>
        /// <returns>The index of the matching brace.</returns>
        /// <exception cref="OverflowException">If the input size is larger than 5000 we throw an overflow.</exception>
        internal static int FindMatchingBrace(string expression, int startingIndex)
        {
            // Check for overflow
            if (expression.Length >= 5000)
            {
                throw new OverflowException();
            }

            // Since we pass the index of the left brace, we know that we should look at least one space over. Hence j = startingIndex + 1
            // We loop through the expression and count the parentheses in order to keep track of which parentheses is the matching one.
            int countLeft = 0;
            int countRight = 0;
            for (int j = startingIndex + 1; j < expression.Length; j++)
            {
                // If we find a right brace and our rightCount is 0 then we know we have found our matching brace
                if (expression[j] == ')' && countRight == 0)
                {
                    return j;
                }

                // If we find a left brace we count one left brace and know for a fact that there also must exist a right brace (which we assume since invalid expression checking will be done else where)
                if (expression[j] == '(')
                {
                    countLeft++;
                    countRight++;
                }

                // If we encounter a right then we decrement our counters to signify a matching pair
                if (expression[j] == ')' && countRight != 0)
                {
                    countRight--;
                    countLeft--;
                }
            }

            return -1;
        }

        /// <summary>
        /// Finds lowest precedence operator in an expression. Handles parentheses cases as well.
        /// </summary>
        /// <param name="expression">Takes an expression input.</param>
        /// <returns>The index of the lowest precedence operator.</returns>
        /// <exception cref="OverflowException">If the input is larger than 5000 we throw an overflow.</exception>
        internal static int FindLowestPrecedenceOp(string expression)
        {
            if (expression.Length >= 5000)
            {
                throw new OverflowException();
            }

            // We can assume that parentheses have been cleaned since this method is run after we clean. I'd argue its bad design and a waste of time to implement a useless parentheses version since they will be removed anyways in the tree.
            // One way we can do this is by implementing a quick dictionary of the operators and their precedence levels. + and - will be equal and * and / will be greater than + and - but will be equal.
            Dictionary<char, int> opDict = new Dictionary<char, int>();
            opDict.Add('-', 1);
            opDict.Add('+', 1);
            opDict.Add('*', 2);
            opDict.Add('/', 2);

            // Now that we have defined precedence we must actually write the code to find the lowest precedence.
            // But First we must consider parentheses
            // If there exists parentheses then we know that we must ignore the operators inside of those parentheses
            // Since we already developed a way to find matching braces we will reuse that method here
            // Also there are two cases for parentheses. First: If the matching brace is the last index of the string. Second: If the matching brace is not the last index (which means there is more to the expression to the right)
            int operatorIndex = -1;
            int minOperatorValue = 0;
            for (int i = 0; i < expression.Length; i++)
            {
                if (expression[i] == '(')
                {
                    int rightBraceIndex = FindMatchingBrace(expression, i);

                    // Case 1: The matching brace is the last index, then we have already found our lowest precedence operator (this is because we assume parentheses are cleaned already!!! which they should be!!!)
                    if (rightBraceIndex == expression.Length - 1)
                    {
                        return operatorIndex;
                    }

                    // Case 2: if the matching brace isnt the last index then we know for sure there is more to the expression that must be read in order to find the lowest precedence operator.
                    // We also skip the pointer to the position right of the matching brace index. Which saves us time and also avoids any incorrect logic.
                    else
                    {
                        i = rightBraceIndex + 1;
                    }
                }

                // Now to actually find the operator, first we look for any operators.
                if (opDict.ContainsKey(expression[i]))
                {
                    // If an operator is found, we check its precedence level. If it is less than or equal to the current min then we update our index and our current minimumValue.
                    // Else we simply continue in the algorithm
                    if (opDict[expression[i]] <= minOperatorValue || minOperatorValue == 0)
                    {
                        operatorIndex = i;
                        minOperatorValue = opDict[expression[i]];
                    }
                }
            }

            return operatorIndex;
        }

        /// <summary>
        /// Method to find middle operator of expression. (old method for building tree, no longer used).
        /// </summary>
        /// <param name="expression">Takes an expression and finds middle.</param>
        /// <returns>The middle operator index.</returns>
        internal static int FindMiddleOperatorIndex(string expression)
        {
            // This is step 1 of the expression tree algorithm
            // For this method the first step is to count how many operators are being used
            // Once find how many we find the middle of that amount
            // then we simply return the middle
            double count = 0;

            bool[] operatorDictionary = new bool[128];
            operatorDictionary['-'] = true;
            operatorDictionary['+'] = true;
            operatorDictionary['*'] = true;
            operatorDictionary['/'] = true;
            for (int i = 0; i < expression.Length; i++)
            {
                // If the current character exists in our dictionary then we know that character is an operator.
                if (operatorDictionary[expression[i]] == true)
                {
                    count++;
                }
            }

            // This checks if there even exists a middle operator. With recursion this will be apart of our base case. This also covers invalid string inputs that lack any operators.
            if (count == 0)
            {
                return -1;
            }

            // Out of n operators, find the middle operator
            int ithOperator = (int)Math.Floor(count / 2) + 1;
            int middleOperatorIndex = 0;
            count = 0;

            // We count again, if our count matches the ith operator then we return the index
            for (int i = 0; i < expression.Length; i++)
            {
                if (operatorDictionary[expression[i]] == true)
                {
                    count++;
                }

                if (count == ithOperator)
                {
                    middleOperatorIndex = i;
                    break;
                }
            }

            return middleOperatorIndex;
        }

        /// <summary>
        /// Converts an expression to a postfix stack.
        /// </summary>
        /// <param name="expression">String expression input.</param>
        /// <returns>Returns postfix queue.</returns>
        internal static Queue<string> ConvertToPostFix(string expression)
        {
            // The alg:
            // if operand enqueue
            // if operator push to stack
            // continue this pattern, now if an operator encountered has a lower or equal precedence we pop the operator stack and queue the operator.
            // if a left parentheses has been encountered. push to stack. and continue as before pushing any operators to the stack and operands to the queue
            // when the right parentheses is encountered pop the operator and enqueue it as well as popping the parentheses
            // continue until we reach the end of the string and the stack is empty
            Dictionary<char, int> opDict = new Dictionary<char, int>();
            opDict.Add('-', 1);
            opDict.Add('+', 1);
            opDict.Add('*', 2);
            opDict.Add('/', 2);
            string token = string.Empty;

            // problem, we need a way to identify variable nodes.
            Queue<string> postFixQueue = new Queue<string>();
            Stack<string> postFixStack = new Stack<string>();
            for (int i = 0; i < expression.Length; i++)
            {
                // If character is a left parentheses
                if (expression[i] == '(')
                {
                    postFixStack.Push(expression[i].ToString());
                }

                if (expression[i] == ')')
                {
                    postFixQueue.Enqueue(token);
                    token = string.Empty;
                    while (postFixStack.Peek() != "(")
                    {
                        // If our stack ever runs empty before encountering a ), we know that the parentheses input is wrong.
                        if (postFixStack.Count == 0)
                        {
                            throw new ArgumentException();
                        }

                        postFixQueue.Enqueue(postFixStack.Pop());
                    }

                    // Pop the (
                    postFixStack.Pop();
                }

                // While the current character is not an operator we create a token
                if (!opDict.ContainsKey(expression[i]) && expression[i] != '(' && expression[i] != ')' && expression[i] != ' ')
                {
                    token += expression[i];
                }

                // If we encounter an operator
                if (opDict.ContainsKey(expression[i]))
                {
                    if (postFixStack.Count == 0)
                    {
                        postFixStack.Push(expression[i].ToString());
                    }
                    else
                    {
                        string op = postFixStack.Peek();

                        // If the operator being read has a higher precedence we push the operator
                        if (op == "(")
                        {
                            postFixStack.Push(expression[i].ToString());
                        }
                        else if (opDict[expression[i]] > opDict[op[0]])
                        {
                            postFixStack.Push(expression[i].ToString());
                        }
                        else
                        {
                            postFixQueue.Enqueue(token);

                            // Pop the last operator and push it to the queue
                            postFixQueue.Enqueue(postFixStack.Pop());

                            // Push the current operator
                            postFixStack.Push(expression[i].ToString());
                            token = string.Empty;
                        }
                    }

                    if (token != string.Empty)
                    {
                        postFixQueue.Enqueue(token);
                        token = string.Empty;
                    }
                }

                // If we reach the end of the expression we push the last token.
                if (i == expression.Length - 1)
                {
                    if (token != string.Empty)
                    {
                        postFixQueue.Enqueue(token);
                    }

                    while (postFixStack.Count != 0)
                    {
                        // If a matching brace was never found throw exception.
                        if (postFixStack.Peek() == "(")
                        {
                            throw new ArgumentException();
                        }

                        postFixQueue.Enqueue(postFixStack.Pop());
                    }
                }
            }

            return postFixQueue;
        }
    }
}