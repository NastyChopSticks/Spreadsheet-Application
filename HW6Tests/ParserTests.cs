// <copyright file="ParserTests.cs" company="Kaden Metzger Id: 11817362">
// Copyright (c) Kaden Metzger Id: 11817362. All rights reserved.
// </copyright>

namespace ParserTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using SpreadsheetEngine;

    /// <summary>
    /// Tests for parser class.
    /// </summary>
    internal class ParserTests
    {
        private Type parserObject = typeof(Parser);

        /// <summary>
        /// Set up function for tests.
        /// </summary>
        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// Test cleaning parentheses.
        /// </summary>
        [Test]
        public void TestNormalCleanParentheses()
        {
            MethodInfo cleanParenMethod = this.parserObject.GetMethod("CleanParentheses", BindingFlags.NonPublic | BindingFlags.Static);
            object parsedString = cleanParenMethod.Invoke(null, new object[] { "(1+2)" });
            Assert.That(parsedString, Is.EqualTo("1+2"));
        }

        /// <summary>
        /// Edge test for clean parentheses.
        /// </summary>
        [Test]
        public void TestEdgeCleanParentheses()
        {
            MethodInfo cleanParenMethod = this.parserObject.GetMethod("CleanParentheses", BindingFlags.NonPublic | BindingFlags.Static);
            object parsedString = cleanParenMethod.Invoke(null, new object[] { "((((1+2))))" });
            Assert.That(parsedString, Is.EqualTo("1+2"));
        }

        /// <summary>
        /// Edge test two for cleaning parentheses.
        /// </summary>
        [Test]
        public void TestTwoEdgeCleanParentheses()
        {
            MethodInfo cleanParenMethod = this.parserObject.GetMethod("CleanParentheses", BindingFlags.NonPublic | BindingFlags.Static);
            object parsedString = cleanParenMethod.Invoke(null, new object[] { "(1+2)*(3+2)" });
            Assert.That(parsedString, Is.EqualTo("(1+2)*(3+2)"));
        }

        /// <summary>
        /// Exception test for cleaning parentheses.
        /// </summary>
        [Test]
        public void ExceptionCleanParentheses()
        {
            MethodInfo cleanParenMethod = this.parserObject.GetMethod("CleanParentheses", BindingFlags.NonPublic | BindingFlags.Static);
            string overFlowString = new string('A', 5000);
            Assert.Throws<TargetInvocationException>(() =>
            {
                cleanParenMethod.Invoke(null, new object[] { overFlowString });
            });
        }

        /// <summary>
        /// Tests Private get Middle method, normal test.
        /// </summary>
        [Test]
        public void NormalTestGetMiddle()
        {
            MethodInfo findMidMethod = this.parserObject.GetMethod("FindMiddleOperatorIndex", BindingFlags.NonPublic | BindingFlags.Static);
            object midOp = findMidMethod.Invoke(this.parserObject, new object[] { "A1-A2" });
            Assert.That(midOp, Is.EqualTo(2));
        }

        /// <summary>
        /// Tests get middle index, edge case (no operator in string).
        /// </summary>
        [Test]
        public void EdgeTestGetMiddle()
        {
            MethodInfo findMidMethod = this.parserObject.GetMethod("FindMiddleOperatorIndex", BindingFlags.NonPublic | BindingFlags.Static);
            object midOp = findMidMethod.Invoke(this.parserObject, new object[] { "A1" });
            Assert.That(midOp, Is.EqualTo(-1));
        }

        /// <summary>
        /// Normal test for finding matching brace.
        /// </summary>
        [Test]
        public void NormalFindMatchingBrace()
        {
            MethodInfo findMatchingBraceMethod = this.parserObject.GetMethod("FindMatchingBrace", BindingFlags.NonPublic | BindingFlags.Static);
            object matchingBraceIndex = findMatchingBraceMethod.Invoke(this.parserObject, new object[] { "()", 0 });
            Assert.That(matchingBraceIndex, Is.EqualTo(1));
        }

        /// <summary>
        /// Normal test two for finding matching brace.
        /// </summary>
        [Test]
        public void NormalTwoFindMatchingBrace()
        {
            MethodInfo findMatchingBraceMethod = this.parserObject.GetMethod("FindMatchingBrace", BindingFlags.NonPublic | BindingFlags.Static);
            object matchingBraceIndex = findMatchingBraceMethod.Invoke(this.parserObject, new object[] { "012345(012345)", 6 });
            Assert.That(matchingBraceIndex, Is.EqualTo(13));
        }

        /// <summary>
        /// Edge case for finidng matching brace.
        /// </summary>
        [Test]
        public void EdgeFindMatchingBrace()
        {
            MethodInfo findMatchingBraceMethod = this.parserObject.GetMethod("FindMatchingBrace", BindingFlags.NonPublic | BindingFlags.Static);
            object matchingBraceIndex = findMatchingBraceMethod.Invoke(this.parserObject, new object[] { "12345", 0 });
            Assert.That(matchingBraceIndex, Is.EqualTo(-1));
        }

        /// <summary>
        /// Exceptional test case for finding matching brace.
        /// </summary>
        [Test]
        public void ExceptionFindMatchingBrace()
        {
            MethodInfo findMatchingBraceMethod = this.parserObject.GetMethod("FindMatchingBrace", BindingFlags.NonPublic | BindingFlags.Static);
            string overFlowString = new string('A', 5000);

            Assert.Throws<TargetInvocationException>(() =>
            {
                findMatchingBraceMethod.Invoke(this.parserObject, new object[] { overFlowString, 0 });
            });
        }

        /// <summary>
        /// Normal test for finding lowest precedence operator.
        /// </summary>
        [Test]
        public void NormalFindLowestPrecedence()
        {
            MethodInfo findMatchingBraceMethod = this.parserObject.GetMethod("FindLowestPrecedenceOp", BindingFlags.NonPublic | BindingFlags.Static);
            object opIndex = findMatchingBraceMethod.Invoke(this.parserObject, new object[] { "1*2+3-4*6-5+7" });
            Assert.That(opIndex, Is.EqualTo(11));
        }

        /// <summary>
        /// Edge case for finding lowest precedence operator.
        /// </summary>
        [Test]
        public void EdgeFindLowestPrecedence()
        {
            MethodInfo findMatchingBraceMethod = this.parserObject.GetMethod("FindLowestPrecedenceOp", BindingFlags.NonPublic | BindingFlags.Static);
            object opIndex = findMatchingBraceMethod.Invoke(this.parserObject, new object[] { "(1+2)*5" });
            Assert.That(opIndex, Is.EqualTo(5));
        }

        /// <summary>
        /// Edge test for finding lowest precedence operator.
        /// </summary>
        [Test]
        public void EdgeTestTwoFindLowestPrecedence()
        {
            MethodInfo findMatchingBraceMethod = this.parserObject.GetMethod("FindLowestPrecedenceOp", BindingFlags.NonPublic | BindingFlags.Static);
            object opIndex = findMatchingBraceMethod.Invoke(this.parserObject, new object[] { "(1+2)*(1+2)" });
            Assert.That(opIndex, Is.EqualTo(5));
        }

        /// <summary>
        /// Exceptional case for lowest precedence.
        /// </summary>
        [Test]
        public void ExceptionFindLowestPrecedence()
        {
            MethodInfo findMatchingBraceMethod = this.parserObject.GetMethod("FindLowestPrecedenceOp", BindingFlags.NonPublic | BindingFlags.Static);
            string overFlowString = new string('A', 5000);
            Assert.Throws<TargetInvocationException>(() =>
            {
                findMatchingBraceMethod.Invoke(this.parserObject, new object[] { overFlowString });
            });
        }

        private MethodInfo GetMethod(string methodName)
        {
            if (string.IsNullOrWhiteSpace(methodName))
            {
                Assert.Fail("methodName cannot be null or whitespace");
            }

            var method = this.parserObject.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            if (method == null)
            {
                Assert.Fail(string.Format("{0} method not found", methodName));
            }

            return method;
        }
    }
}
