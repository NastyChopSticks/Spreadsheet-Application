// <copyright file="ColorMethods.cs" company="Kaden Metzger Id: 11817362">
// Copyright (c) Kaden Metzger Id: 11817362. All rights reserved.
// </copyright>

namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Methods for converting argb values to hexadecimal.
    /// </summary>
    public static class ColorMethods
    {
        /// <summary>
        /// Converts argb values to hexadecimal/uint since that is the type we are using.
        /// </summary>
        /// <param name="alpha">alpha value.</param>
        /// <param name="red">red value.</param>
        /// <param name="green">green value.</param>
        /// <param name="blue">blue value.</param>
        /// <returns>hexadecimal/uint value of the argb.</returns>
        public static uint RGBToHex(int alpha, int red, int green, int blue)
        {
            // So a hex color is represented by 8 bits following 0x. the first two bits is for alpha, next two for red, etc.
            // So we shift alpha by 24 to get the first two bits followed by 6 zeros. Example: 255 << 24 = ff000000
            // We continue shifting bits and use bitwise OR to concatenate each 2 bits together
            return (uint)(alpha << 24 | red << 16 | green << 8 | blue);
        }

        /// <summary>
        /// Takes a hexadecimal value and converts it into argb values.
        /// </summary>
        /// <param name="color">Hexadecimal value.</param>
        /// <returns>returns argb values.</returns>
        public static (int alpha, int red, int green, int blue) UIntToARGB(uint color)
        {
            int alpha = (int)color >> 24 & 0xFF;
            int red = (int)color >> 16 & 0xFF;
            int green = (int)color >> 8 & 0xFF;
            int blue = (int)color & 0xFF;
            return (alpha, red, green, blue);
        }
    }
}
