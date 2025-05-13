namespace HW9_Tests
{
    using SpreadsheetEngine;
    public class Tests
    {
        /// <summary>
        /// Tests if the save xml file method creates the file.
        /// </summary>
        [Test]
        public void TestSaveXML()
        {
            Spreadsheet newSpreadsheet = new Spreadsheet(5, 5);
            newSpreadsheet.UpdateCell("8*5", 0, 0);
            newSpreadsheet.SaveAsXML("test.xml");
            Assert.That(File.Exists("test.xml"), Is.EqualTo(true));
            Console.WriteLine(Path.GetFullPath("test.xml"));
        }

        /// <summary>
        /// Tests if the file is properly saved. I created a file that serves as a checker for this test.
        /// </summary>
        [Test]
        public void TestTwoSaveXML()
        {
            Spreadsheet newSpreadsheet = new Spreadsheet(5, 5);
            newSpreadsheet.UpdateCell("10", 0, 0);
            newSpreadsheet.UpdateCell("=A1*5", 0, 1);
            newSpreadsheet.UpdateCellColor(0x66B39617, 0, 0);
            newSpreadsheet.UpdateCellColor(0x9928C840, 0, 1);
            newSpreadsheet.SaveAsXML("testsave.xml");

            // Compares if the saved file matches the test file
            Assert.That(File.ReadAllText("testsave.xml"), Is.EquivalentTo(File.ReadAllText("testTwo.xml")));
        }

        /// <summary>
        /// Test for loading xml files. Simply uses the file from TestSaveTwo and checks if the spreadsheet is properly updated.
        /// </summary>
        [Test]
        public void TestLoadXML()
        {
            Spreadsheet newSpreadsheet = new Spreadsheet(5, 5);
            newSpreadsheet.LoadXML("testTwo.xml");
            Assert.That(newSpreadsheet.GetCell(0, 0).Value, Is.EqualTo(10.ToString()) );
            Assert.That(newSpreadsheet.GetCell(1, 0).Value, Is.EqualTo(50.ToString()) );
        }

        /// <summary>
        /// This tests the load xml method whilst using a more complex file. This file contains a formula, a cell color, and random tags that aren't to be read. This means if the XML file has random tags that don't belong it will still load properly.
        /// </summary>
        [Test]
        public void TestTwoLoadXML()
        {
            Spreadsheet newSpreadsheet = new Spreadsheet(5, 5);
            newSpreadsheet.LoadXML("testLoading.xml");
            Assert.That(newSpreadsheet.GetCell(0,0).Value, Is.EqualTo(40.ToString()) );
            Assert.That(newSpreadsheet.GetCell(0, 0).BGColor, Is.EqualTo(0x66B39617));
        }
    }
}