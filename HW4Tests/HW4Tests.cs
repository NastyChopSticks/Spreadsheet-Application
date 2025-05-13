// <copyright file="HW4Tests.cs" company="Kaden Metzger Id: 11817362">
// Copyright (c) Kaden Metzger Id: 11817362. All rights reserved.
// </copyright>

namespace HW4Tests
{
    using System.Reflection;
    using SpreadsheetEngine;

    /// <summary>
    /// Test class which tests all logic methods.
    /// </summary>
    public class HW4Tests
    {
        private Spreadsheet spreadSheetObj = new Spreadsheet(50, 26);

        /// <summary>
        /// Tests the GetCell() method. Create a spreadsheet, check if the getcell function returns the correct cell. If so then that exact cell's index will match the get cell call.
        /// </summary>
        [Test]
        public void NormalTestGetCell()
        {
            Spreadsheet spreadsheet = new Spreadsheet(5, 5);
            Assert.That(spreadsheet.GetCell(3, 2).RowIndex, Is.EqualTo(3));
            Assert.That(spreadsheet.GetCell(3, 2).ColumnIndex, Is.EqualTo(2));
        }

        /// <summary>
        /// Exception test. If we call get cell out of memory bounds we should throw an exception.
        /// </summary>
        [Test]
        public void ExceptionTestGetCell()
        {
            Spreadsheet spreadsheet = new Spreadsheet(5, 5);
            Assert.Throws<IndexOutOfRangeException>(() => spreadsheet.GetCell(-1, 1));
            Assert.Throws<IndexOutOfRangeException>(() => spreadsheet.GetCell(1, -1));
        }

        /// <summary>
        /// Tests the spreadsheet constructor. Normal test. Test checks if when we create the spreadsheet if there exist x,y amount of rows and columns.
        /// </summary>
        [Test]
        public void NormalTestSpreadsheetCon()
        {
            Spreadsheet testSpread = new Spreadsheet(5, 6);
            Assert.That(testSpread.RowCount, Is.EqualTo(5));
            Assert.That(testSpread.RowCount, Is.EqualTo(5));
        }

        /// <summary>
        /// Exception case for constructor. If we attempt to create a spreadsheet with any negative input we should throw an out of range exception.
        /// </summary>
        [Test]
        public void ExceptionTestSpreadsheetCon()
        {
            Assert.Throws<IndexOutOfRangeException>(() => new Spreadsheet(-1, 5));
        }

        /// <summary>
        /// Normal Test case for Update Cell. Tests if the text is updated correctly when the method is called.
        /// </summary>
        [Test]
        public void NormalTestUpdateCell()
        {
            Spreadsheet testSpread = new Spreadsheet(5, 6);
            testSpread.UpdateCell("Test", 1, 1);
            Assert.That(testSpread.GetCell(1, 1).Text, Is.EqualTo("Test"));
        }

        /// <summary>
        /// Tests the edge case for update cell. Out of bounds call.
        /// </summary>
        [Test]
        public void EdgeTestUpdateCell()
        {
            Spreadsheet testSpread = new Spreadsheet(5, 6);

            Assert.Throws<IndexOutOfRangeException>(() => testSpread.UpdateCell("Test", 7, 1));
        }

        /// <summary>
        /// Tests the exception case for UpdateCell, negative indexing.
        /// </summary>
        [Test]
        public void ExceptionTestUpdateCell()
        {
            Spreadsheet testSpread = new Spreadsheet(5, 6);
            Assert.Throws<IndexOutOfRangeException>(() => testSpread.UpdateCell("Test", -1, 5));
        }

        /// <summary>
        /// GetMethod, method. Used to get private and internal methods.
        /// </summary>
        /// <param name="methodName">The name of the method.</param>
        /// <returns>The method.</returns>
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