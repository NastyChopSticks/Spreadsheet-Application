// <copyright file="UpdateExpressionTreeTests.cs" company="Kaden Metzger Id: 11817362">
// Copyright (c) Kaden Metzger Id: 11817362. All rights reserved.
// </copyright>

namespace HW6Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using SpreadsheetEngine;

    internal class UpdateExpressionTreeTests
    {
        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// tests if computation is correct with parentheses
        /// </summary>
        [Test]
        public void NormalTestEvaluate()
        {
            ExpressionTree testTree = new ExpressionTree("(1+2)*3");
            Assert.That(testTree.Evaluate(), Is.EqualTo(9));
        }

        /// <summary>
        /// tests if computation is correct with parentheses
        /// </summary>
        [Test]
        public void NormalTestTwoEvaluate()
        {
            ExpressionTree testTree = new ExpressionTree("(1+2)*(4-2)");
            Assert.That(testTree.Evaluate(), Is.EqualTo(6));
        }

        /// <summary>
        /// tests if computation is correct with variables
        /// </summary>
        [Test]
        public void NormalTestThreeEvaluate()
        {
            ExpressionTree testTree = new ExpressionTree("(5-1)*3");
            Assert.That(testTree.Evaluate(), Is.EqualTo(12));
        }

        [Test]
        public void EdgeEvaluate()
        {
            ExpressionTree testTree = new ExpressionTree("(1+2)*(4-2)");
            Assert.That(testTree.Evaluate(), Is.EqualTo(6));
        }

        [Test]
        public void ExceptionEvaluate()
        {
            string overFlowString = new string('A', 5000);

            Assert.Throws<OverflowException>(() =>
            {
                ExpressionTree testTree = new ExpressionTree(overFlowString);
            });
        }
    }
}
