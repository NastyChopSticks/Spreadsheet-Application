// <copyright file="SelfReferenceException.cs" company="Kaden Metzger Id: 11817362">
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
    /// Used for calling self reference exceptions.
    /// </summary>
    public class SelfReferenceException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelfReferenceException"/> class.
        /// </summary>
        public SelfReferenceException()
            : base("Self reference")
        {
        }
    }
}
