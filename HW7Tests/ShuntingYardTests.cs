// <copyright file="ShuntingYardTests.cs" company="Kaden Metzger Id: 11817362">
// Copyright (c) Kaden Metzger Id: 11817362. All rights reserved.
// </copyright>

namespace HW7Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using SpreadsheetEngine;

    /// <summary>
    /// Test class for shunting yard alg.
    /// </summary>
    internal class ShuntingYardTests
    {
        private Type parserObject = typeof(Parser);

        /// <summary>
        /// Normal Test for ConvertingToPostFix.
        /// </summary>
        [Test]
        public void NormalTestConvertToPostFix()
        {
            MethodInfo convertToPostFixMethod = this.parserObject.GetMethod("ConvertToPostFix", BindingFlags.NonPublic | BindingFlags.Static);
            object postFix = convertToPostFixMethod.Invoke(this.parserObject, new object[] { "1 - 2" });
            Queue<string> postFixQueue = (Queue<string>)postFix;
            string postFixString = string.Empty;
            while (postFixQueue.Count != 0)
            {
                postFixString += postFixQueue.Peek();
                postFixQueue.Dequeue();
            }

            Assert.That(postFixString, Is.EqualTo("12-"));
        }

        /// <summary>
        /// Normal test for converting to post fix using variables.
        /// </summary>
        [Test]
        public void NormalTesTwoConvertToPostFix()
        {
            MethodInfo convertToPostFixMethod = this.parserObject.GetMethod("ConvertToPostFix", BindingFlags.NonPublic | BindingFlags.Static);

            object postFix = convertToPostFixMethod.Invoke(this.parserObject, new object[] { "A1-B2" });
            Queue<string> postFixQueue = (Queue<string>)postFix;
            string postFixString = string.Empty;
            while (postFixQueue.Count != 0)
            {
                postFixString += postFixQueue.Peek();
                postFixQueue.Dequeue();
            }

            Assert.That(postFixString, Is.EqualTo("A1B2-"));
        }

        /// <summary>
        /// Edge case test, tests if postFix works for inputs with parentheses.
        /// </summary>
        [Test]
        public void EdgeTestConvertToPostFix()
        {
            MethodInfo convertToPostFixMethod = this.parserObject.GetMethod("ConvertToPostFix", BindingFlags.NonPublic | BindingFlags.Static);

            object postFix = convertToPostFixMethod.Invoke(this.parserObject, new object[] { "3+4*2/(1-5)" });
            Queue<string> postFixQueue = (Queue<string>)postFix;
            string postFixString = string.Empty;
            while (postFixQueue.Count != 0)
            {
                postFixString += postFixQueue.Peek();
                postFixQueue.Dequeue();
            }

            Assert.That(postFixString, Is.EqualTo("342*15-/+"));
        }

        /// <summary>
        /// Exception Tests, tests if exception is thrown for inputs with unmatched parentheses.
        /// </summary>
        [Test]
        public void ExceptionTestConvertToPostFix()
        {
            MethodInfo convertToPostFixMethod = this.parserObject.GetMethod("ConvertToPostFix", BindingFlags.NonPublic | BindingFlags.Static);

            Assert.Throws<TargetInvocationException>(() =>
            {
                convertToPostFixMethod.Invoke(this.parserObject, new object[] { "(1 + 2" });
            });
        }

        private MethodInfo GetParserMethod(string methodName)
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
