﻿using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;

namespace Audacia.ExceptionHandling
{
    /// <summary>Represents an API response which contains the details of an error.</summary>
    public class ErrorResult : Dictionary<string, object>
    {
        /// <summary>The message which summarises the error.</summary>
        public string Message => this["Message"].ToString();

        /// <summary>The property on the model which caused the error, if applicable.</summary>
        public string Property => this["Property"].ToString();

        /// <summary>Creates a new instance of <see cref="ErrorResult"/>.</summary>
        public ErrorResult() { }

        /// <summary>Create an <see cref="ErrorResult"/> with the specified message.</summary>
        public ErrorResult(string message)
        {
            this["Message"] = message;
        }

        /// <summary>Create an error result with the specified message for the specified property.</summary>
        public ErrorResult(string message, string property)
        {
            this["Message"] = message;
            this["Property"] = property;
        }
    }
}