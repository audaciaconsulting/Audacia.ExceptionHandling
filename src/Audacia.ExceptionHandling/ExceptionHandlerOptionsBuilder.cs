using System;
using System.Net;
using Audacia.ExceptionHandling.Handlers;

namespace Audacia.ExceptionHandling
{
    public class ExceptionHandlerOptionsBuilder
    {
        private ExceptionHandlerOptions _options = new ExceptionHandlerOptions();

        public ExceptionHandlerOptionsBuilder Handle<TException, TResult>(
            Func<TException, TResult> handlerAction,
            Action<TException>? logAction = null)
            where TException : Exception
        {
            _options.HandlerCollection.Add(handlerAction, logAction);
            return this;
        }

        public ExceptionHandlerOptionsBuilder Handle<TException, TResult>(
            Func<TException, TResult> handlerAction,
            HttpStatusCode statusCode,
            Action<TException>? logAction = null)
            where TException : Exception
        {
            _options.HandlerCollection.Add(handlerAction, statusCode, logAction);
            return this;
        }

        public ExceptionHandlerOptionsBuilder AddHandler<TException, TResult>(
            ExceptionHandler<TException, TResult> handler)
            where TException : Exception
        {
            _options.HandlerCollection.Add<TException>(handler);
            return this;
        }

        public ExceptionHandlerOptionsBuilder WithDefaultLogging(Action<Exception> loggingAction)
        {
            _options.Logging = loggingAction;
            return this;
        }

        public ExceptionHandlerOptions Build()
        {
            return _options;
        }
    }
}