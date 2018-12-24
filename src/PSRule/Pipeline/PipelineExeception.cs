﻿using System;

namespace PSRule.Pipeline
{
    /// <summary>
    /// A base class for all pipeline exceptions.
    /// </summary>
    public abstract class PipelineExeception : Exception
    {
        /// <summary>
        /// Creates a pipeline exception.
        /// </summary>
        public PipelineExeception()
        {
        }

        /// <summary>
        /// Creates a pipeline exception.
        /// </summary>
        /// <param name="message">The detail of the exception.</param>
        public PipelineExeception(string message) : base(message)
        {
        }

        /// <summary>
        /// Creates a pipeline exception.
        /// </summary>
        /// <param name="message">The detail of the exception.</param>
        /// <param name="innerException">A nested exception that caused the issue.</param>
        public PipelineExeception(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
