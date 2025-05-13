// <copyright file="ColorMethodTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HW8_Tests
{
    using SpreadsheetEngine;

    /// <summary>
    /// Tests color methods.
    /// </summary>
    public class ColorMethodTests
    {
        /// <summary>
        /// Tests converting RGB to Hex.
        /// </summary>
        [Test]
        public void NormalTestConvertToHex()
        {
            Assert.That(ColorMethods.RGBToHex(255, 255, 0, 0), Is.EqualTo(0xFFFF0000));
        }

        /// <summary>
        /// Normal test for convert to hex.
        /// </summary>
        [Test]
        public void NormalTestTwoConvertToHex()
        {
            Assert.That(ColorMethods.RGBToHex(102, 179, 150, 23), Is.EqualTo(0x66B39617));
        }

        /// <summary>
        /// Third normal test for convert to hex.
        /// </summary>
        [Test]
        public void NormalTestThreeConvertToHex()
        {
            Assert.That(ColorMethods.RGBToHex(153, 40, 200, 64), Is.EqualTo(0x9928C840));
        }

        /// <summary>
        /// Fourth test for convert to hex.
        /// </summary>
        [Test]
        public void NormalTestFourConvertToHex()
        {
            Assert.That(ColorMethods.RGBToHex(255, 255, 255, 255), Is.EqualTo(0xFFFFFFFF));
        }

        /// <summary>
        /// Convert hex to argb test.
        /// </summary>
        [Test]
        public void NormalTestConvertUIntToARGB()
        {
            Assert.That(ColorMethods.UIntToARGB(0xFFFFFFFF), Is.EqualTo((255, 255, 255, 255)));
        }

        /// <summary>
        /// Convert to argb test two.
        /// </summary>
        [Test]
        public void NormalTestTwoConvertUIntToARGB()
        {
            Assert.That(ColorMethods.UIntToARGB(0x66B39617), Is.EqualTo((102, 179, 150, 23)));
        }

        /// <summary>
        /// Convert to argb test three.
        /// </summary>
        [Test]
        public void NormalTestThreeConvertUIntToARGB()
        {
            Assert.That(ColorMethods.UIntToARGB(0x9928C840), Is.EqualTo((153, 40, 200, 64)));
        }

        /// <summary>
        /// Convert to argb test four.
        /// </summary>
        [Test]
        public void NormalTesFourConvertUIntToARGB()
        {
            Assert.That(ColorMethods.UIntToARGB(0xFFFF0000), Is.EqualTo((255, 255, 0, 0)));
        }
    }
}