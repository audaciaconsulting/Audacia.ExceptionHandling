using System;
using System.Collections.Generic;

namespace Audacia.ExceptionHandling
{
    /// <summary>
    /// A wrapper around how to handle different exception types
    /// </summary>
    /// <typeparam name="TException">The type of exception being handled.</typeparam>
    /// <typeparam name="TResult">The result type that is returned.</typeparam>
    public class ExceptionHandler<TException, TResult> where TException : Exception
    {
        /// <summary>Initializes a new instance of <see cref="ExceptionHandler{TException, TResult}"/></summary>
        public ExceptionHandler(Func<TException, TResult> action)
        {
            Action = action;
        }

        /// <summary>The type of Exception this handler handles.</summary>
        public Type ExceptionType => typeof(TException);

        /// <summary>The type of Result this handler outputs.</summary>
        public Type ResultType => typeof(TResult);

        /// <summary>The function which transforms the exception into <see cref="TResult"/>.</summary>
        public Func<TException, TResult> Action { get; }
    }
}