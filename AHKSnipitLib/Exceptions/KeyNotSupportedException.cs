﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Exceptions
{
    /// <summary>
    /// Exception for Hotkey Format Errors
    /// </summary>
    [Serializable]
    public class KeyNotSupportedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the KeyNotSupportedException class.
        /// </summary>
        public KeyNotSupportedException() { }
        /// <summary>
        /// Initializes a new instance of the KeyNotSupportedException class with a specified error
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public KeyNotSupportedException(string message) : base(message) { }
        /// <summary>
        /// Initializes a new instance of the KeyNotSupportedException class with a specified error
        /// message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">
        /// The exception that is the cause of the current exception, or a null reference
        /// (Nothing in Visual Basic) if no inner exception is specified.
        /// </param>
        public KeyNotSupportedException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Initializes a new instance of the KeyNotSupportedException class with serialized data.
        /// </summary>
        /// <param name="info">
        /// The System.Runtime.Serialization.SerializationInfo that holds the serialized
        /// object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The System.Runtime.Serialization.StreamingContext that contains contextual information
        /// about the source or destination.
        /// </param>
        /// <exception cref="System.ArgumentNullException">The info parameter is null.</exception>
        /// <exception cref="System.Runtime.Serialization.SerializationException">The class name is null or System.Exception.HResult is zero (0).</exception>
        protected KeyNotSupportedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
    }
}
