// <copyright file="HW7Tests.cs" company="Kaden Metzger Id: 11817362">
// Copyright (c) Kaden Metzger Id: 11817362. All rights reserved.
// </copyright>

namespace HW7Tests
{
    using System.Reflection;
    using Microsoft.VisualStudio.TestPlatform.ObjectModel;
    using SpreadsheetEngine;

    /// <summary>
    /// Tests for HW7 functionality.
    /// </summary>
    public class HW7Tests
    {
        private Spreadsheet spreadSheetObj = new Spreadsheet(50, 26);

        /// <summary>
        /// Normal test for evaluating cells with = sign.
        /// </summary>
        [Test]
        public void NormalTestEvaluateCell()
        {
            // Create a test formula
            this.spreadSheetObj.UpdateCell("=(10+2)*2", 0, 0);

            // Set the value of B1 = A1 which A1 should = 24 is logic is correct
            this.spreadSheetObj.UpdateCell("=A1", 0, 1);

            Assert.That(this.spreadSheetObj.GetCell(0, 1).Value, Is.EqualTo("24"));
        }

        /// <summary>
        /// Tests if convert to tuple method works as intended.
        /// </summary>
        [Test]
        public void NormalTestConvertToTuple()
        {
            MethodInfo convertToTupleMethod = this.GetMethod("ConvertToTuple");
            Assert.That(convertToTupleMethod.Invoke(this.spreadSheetObj, new object[] { "A1" }), Is.EqualTo((0, 0)));
        }

        /// <summary>
        /// Tests cell dependency, so if a dependent cell is updated we test if it works.
        /// </summary>
        [Test]
        public void NormalTestDependentCell()
        {
            this.spreadSheetObj = new Spreadsheet(50, 26);
            this.spreadSheetObj.UpdateCell("5", 0, 1);
            this.spreadSheetObj.UpdateCell("2", 1, 1);
            this.spreadSheetObj.UpdateCell("=B1*B2", 0, 0);
            Assert.That(this.spreadSheetObj.GetCell(0, 0).Value, Is.EqualTo(10.ToString()));
            this.spreadSheetObj.UpdateCell("10", 0, 1);
            Assert.That(this.spreadSheetObj.GetCell(0, 0).Value, Is.EqualTo(20.ToString()));
        }

        /// <summary>
        /// Edge test version of dependent cell testing.Basically adds more than one dependency.
        /// </summary>
        [Test]
        public void EdgeTestDependentCell()
        {
            this.spreadSheetObj = new Spreadsheet(50, 26);
            this.spreadSheetObj.UpdateCell("5", 0, 1);
            this.spreadSheetObj.UpdateCell("2", 1, 1);
            this.spreadSheetObj.UpdateCell("=B1*B2", 0, 0);
            Assert.That(this.spreadSheetObj.GetCell(0, 0).Value, Is.EqualTo(10.ToString()));
            this.spreadSheetObj.UpdateCell("=B1+B2", 0, 2);
            Assert.That(this.spreadSheetObj.GetCell(0, 2).Value, Is.EqualTo(7.ToString()));
            this.spreadSheetObj.UpdateCell("10", 0, 1);
            Assert.That(this.spreadSheetObj.GetCell(0, 2).Value, Is.EqualTo(12.ToString()));
            Assert.That(this.spreadSheetObj.GetCell(0, 0).Value, Is.EqualTo(20.ToString()));
        }

        /// <summary>
        /// Tests if conver to variable method words as intended.
        /// </summary>
        [Test]
        public void NormalTestConvertToVariable()
        {
            MethodInfo convertToTupleMethod = this.GetMethod("ConvertToVariable");
            Assert.That(convertToTupleMethod.Invoke(this.spreadSheetObj, new object[] { (0, 1) }), Is.EqualTo("B1"));
        }

        /// <summary>
        /// Get method, method. Gets private or internal methods.
        /// </summary>
        /// <param name="methodName">name of method.</param>
        /// <returns>Returns method.</returns>
        private MethodInfo GetMethod(string methodName)
        {
            if (string.IsNullOrWhiteSpace(methodName))
            {
                Assert.Fail("methodName cannot be null or whitespace");
            }

            var method = this.spreadSheetObj.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            if (method == null)
            {
                Assert.Fail(string.Format("{0} method not found", methodName));
            }

            return method;
        }
    }
}