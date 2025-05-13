// <copyright file="HW5Tests.cs" company="Kaden Metzger Id: 11817362">
// Copyright (c) Kaden Metzger Id: 11817362. All rights reserved.
// </copyright>

namespace HW5_Tests
{
    using System.Reflection;
    using SpreadsheetEngine;

    /// <summary>
    /// Test class used to test functionality.
    /// </summary>
    public class HW5Tests
    {
        private ExpressionTree expressionTreeObject = new ExpressionTree("A1-A2");

        /// <summary>
        /// Set up function for tests.
        /// </summary>
        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// tests if computation is correct for small amounts of inputs.
        /// </summary>
        [Test]
        public void NormalTestOneEvaluate()
        {
            ExpressionTree testTree = new ExpressionTree("50-12");
            Assert.That(testTree.Evaluate(), Is.EqualTo(38));
        }

        /// <summary>
        /// Normal Test for evaluating an expression. Checks if division works properly.
        /// </summary>
        [Test]
        public void NormalTestTwoEvaluate()
        {
            ExpressionTree testTree = new ExpressionTree("50/2");
            Assert.That(testTree.Evaluate(), Is.EqualTo(25));
        }

        /// <summary>
        /// Checks if multiplication works properly.
        /// </summary>
        [Test]
        public void NormalTestThreeEvaluate()
        {
            ExpressionTree testTree = new ExpressionTree("50*2");
            Assert.That(testTree.Evaluate(), Is.EqualTo(100));
        }

        /// <summary>
        /// Checks if addition works properly.
        /// </summary>
        [Test]
        public void NormalTestFourEvaluate()
        {
            ExpressionTree testTree = new ExpressionTree("50+2");
            Assert.That(testTree.Evaluate(), Is.EqualTo(52));
        }

        /// <summary>
        /// Checks if computation is correct for many inputs.
        /// </summary>
        [Test]
        public void EdgeTestEvaluate()
        {
            ExpressionTree testTree = new ExpressionTree("5+10+15+20+25+30+35+40+45+50");
            Assert.That(testTree.Evaluate(), Is.EqualTo(275));
        }

        /// <summary>
        /// Normal test case which checks if values are being updated properly.
        /// </summary>
        [Test]
        public void NormalTestSetVariable()
        {
            Spreadsheet spreadsheetObj = new Spreadsheet(5, 5);
            spreadsheetObj.UpdateCell("1", 0, 0);
            spreadsheetObj.UpdateCell("1", 0, 1);

            this.expressionTreeObject = new ExpressionTree("A1-B1");
            MethodInfo getSetVar = this.GetMethod("SetVariable");
            MethodInfo getRoot = this.GetMethod("GetRootNode");
            object root = getRoot.Invoke(this.expressionTreeObject, new object[] { });
            getSetVar.Invoke(this.expressionTreeObject, new object[] { "A1", 12, root });
            getSetVar.Invoke(this.expressionTreeObject, new object[] { "B1", 2, root });
            Assert.That(this.expressionTreeObject.Evaluate(), Is.EqualTo(10));
        }

        /// <summary>
        /// Test if given an expression that doesn't exist that it does not change any computation present.
        /// </summary>
        [Test]
        public void EdgeTestSetVariable()
        {
            Spreadsheet spreadsheetObj = new Spreadsheet(5, 5);
            spreadsheetObj.UpdateCell("1", 0, 0);
            spreadsheetObj.UpdateCell("1", 0, 1);
            spreadsheetObj.UpdateCell("1", 0, 2);

            this.expressionTreeObject = new ExpressionTree("(A1-B1-C1)*2");
            MethodInfo getSetVar = this.GetMethod("SetVariable");
            MethodInfo getRoot = this.GetMethod("GetRootNode");
            object root = getRoot.Invoke(this.expressionTreeObject, new object[] { });
            getSetVar.Invoke(this.expressionTreeObject, new object[] { "A1", 12, root });
            getSetVar.Invoke(this.expressionTreeObject, new object[] { "B1", 2, root });
            getSetVar.Invoke(this.expressionTreeObject, new object[] { "C1", 5, root });
            Assert.That(this.expressionTreeObject.Evaluate(), Is.EqualTo(10));
        }

        /// <summary>
        /// Checks if the default values exist for tree expression. Default should be 0.
        /// </summary>
        [Test]
        public void EdgeTwoTestSetVariable()
        {
            ExpressionTree testTree = new ExpressionTree("A1-B1");
            Assert.That(testTree.Evaluate(), Is.EqualTo(0));
        }

        /// <summary>
        /// Tests the Get Expression method, normal case.
        /// </summary>
        [Test]
        public void NormalTestGetExpression()
        {
            ExpressionTree testTree = new ExpressionTree("A1-B1");
            Assert.That(testTree.GetExpression(), Is.EqualTo("A1-B1"));
        }

        /// <summary>
        /// Gets a method based on its name.
        /// </summary>
        /// <param name="methodName">Takes the method name as input.</param>
        /// <returns>Returns the method.</returns>
        private MethodInfo GetMethod(string methodName)
        {
            if (string.IsNullOrWhiteSpace(methodName))
            {
                Assert.Fail("methodName cannot be null or whitespace");
            }

            var method = this.expressionTreeObject.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            if (method == null)
            {
                Assert.Fail(string.Format("{0} method not found", methodName));
            }

            return method;
        }
    }
}