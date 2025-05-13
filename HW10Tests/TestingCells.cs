namespace HW10Tests
{
    using SpreadsheetEngine;
    /// <summary>
    /// Class for testing cell updates.
    /// </summary>
    public class TestingCells
    {
        private Spreadsheet testSpreadsheet = new Spreadsheet(5, 5);

        /// <summary>
        /// Tests when we reference an empty cell.
        /// </summary>
        [Test]
        public void TestEmptyReference()
        {
            this.testSpreadsheet = new Spreadsheet(5, 5);
            this.testSpreadsheet.UpdateCell("=B1", 0, 0);
            Assert.That(this.testSpreadsheet.GetCell(0,0).Value, Is.EqualTo(0.ToString()));
            Assert.That(this.testSpreadsheet.GetCell(0, 0).Text, Is.EqualTo("=B1"));
        }

        /// <summary>
        /// Tests when we self reference a cell.
        /// </summary>
        [Test]
        public void TestSelfReference()
        {
            this.testSpreadsheet = new Spreadsheet(5, 5);
            this.testSpreadsheet.UpdateCell("=A1", 0, 0);
            Assert.That(this.testSpreadsheet.GetCell(0, 0).Value, Is.EqualTo("!(self reference)"));
            Assert.That(this.testSpreadsheet.GetCell(0, 0).Text, Is.EqualTo("=A1"));
        }

        /// <summary>
        /// Tests when we reference a cell that is a self reference.
        /// </summary>
        [Test]
        public void TestReferencingASelfReference()
        {
            this.testSpreadsheet = new Spreadsheet(5, 5);
            this.testSpreadsheet.UpdateCell("=A1", 0, 0);
            this.testSpreadsheet.UpdateCell("=A1", 0, 1);
            Assert.That(this.testSpreadsheet.GetCell(0, 1).Value, Is.EqualTo("!(self reference)"));
            Assert.That(this.testSpreadsheet.GetCell(0, 1).Text, Is.EqualTo("=A1"));
        }

        /// <summary>
        /// Tests when there is a circular reference, if we update one of the cells are the changes reflected properly.
        /// </summary>
        [Test]
        public void TestBug2()
        {
            this.testSpreadsheet = new Spreadsheet(5, 5);
            this.testSpreadsheet.UpdateCell("=B1", 0, 0);
            this.testSpreadsheet.UpdateCell("=A1", 0, 1);
            this.testSpreadsheet.UpdateCell("10", 0, 0);
            Assert.That(this.testSpreadsheet.GetCell(0, 1).Value, Is.EqualTo(10.ToString()));
        }

        /// <summary>
        /// Tests circular references across many cells.
        /// </summary>
        [Test]
        public void TestCircularReference()
        {
            this.testSpreadsheet = new Spreadsheet(5, 5);
            this.testSpreadsheet.UpdateCell("=B1*2", 0, 0);
            this.testSpreadsheet.UpdateCell("=B2*3", 0, 1);
            this.testSpreadsheet.UpdateCell("=A2*4", 1, 1);
            this.testSpreadsheet.UpdateCell("=A1*5", 1, 0);
            Assert.That(this.testSpreadsheet.GetCell(0,0).Value, Is.EqualTo("!(circular reference)"));
            Assert.That(this.testSpreadsheet.GetCell(0, 1).Value, Is.EqualTo("!(circular reference)"));
            Assert.That(this.testSpreadsheet.GetCell(1, 1).Value, Is.EqualTo("!(circular reference)"));
            Assert.That(this.testSpreadsheet.GetCell(1, 0).Value, Is.EqualTo("!(circular reference)"));
        }

        /// <summary>
        /// Tests if we reference a bad cell.
        /// </summary>
        [Test]
        public void TestBadReference()
        {
            this.testSpreadsheet = new Spreadsheet(5, 5);
            this.testSpreadsheet.UpdateCell("=6+Cell*27", 0, 0);
            Assert.That(this.testSpreadsheet.GetCell(0, 0).Value, Is.EqualTo("!(bad reference)"));
        }

        /// <summary>
        /// Tests if we properly update empty cells. That is if A1=B1 and B1 is empty. Then we update B1 does A1 change?
        /// </summary>
        [Test]
        public void UpdatingEmptyCellsTests()
        {
            this.testSpreadsheet = new Spreadsheet(5, 5);
            this.testSpreadsheet.UpdateCell("=B1", 0, 0);
            this.testSpreadsheet.UpdateCell("=B2", 0, 1);
            Assert.That(this.testSpreadsheet.GetCell(0, 0).Value, Is.EqualTo(0.ToString()));
            this.testSpreadsheet.UpdateCell("12", 1, 1);
            Assert.That(this.testSpreadsheet.GetCell(0, 0).Value, Is.EqualTo(12.ToString()));
            Assert.That(this.testSpreadsheet.GetCell(0, 1).Value, Is.EqualTo(12.ToString()));
        }
    }
}