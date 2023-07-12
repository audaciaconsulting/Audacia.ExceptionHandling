# Audacia.ExceptionHandling

Fluent standardized exception configuration for ASP.NET Web APIs.Automatically generates and logs customer reference numbers for handled exceptions.

**Please Note**: `IncludeScopes` MUST be enabled for your logging provider (i.e. Application Insights) to allow for customer reference numbers to be attached to error logs.

For information on how to handle an `ErrorResponse` from the UI, please see the [template project](https://dev.azure.com/audacia/Audacia/_wiki/wikis/Audacia.Template/1789/Validation?anchor=handling-validation-responses-from-the-server) documentation on configuring the [HttpInterceptor](https://dev.azure.com/audacia/Audacia/_wiki/wikis/Audacia.Angular.HttpInterceptor/2457/README).

## Frameworks

### ASP.NET Core

After adding the `Audacia.ExceptionHandling.AspNetCore` package, the following can be added to your `Startup.cs` file:

```csharp
public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
{
    app.ConfigureExceptions(loggerfactory, e =>
    {
        ...
    });
}
```

This adds an exception filter that catches every exception that happens and sends a HTTP response of your choosing.

Starting from version 5, the `IResponseSerializer` interface must be implemented to allow the exception handler to serialize error responses (prior to version 5, this was done using `Newtonsoft.Json`, which may not always be appropriate and introduced an unwanted dependency in `Audacia.ExceptionHandling.AspNetCore`).

An example implementation in an app that _is_ using `Newtonsoft.Json` would be:
```csharp
public class JsonResponseSerializer : IResponseSerializer
{
    public string Serialize(object response)
    {
        return JsonConvert.SerializeObject(response);
    }
}
```

The `IResponseSerializer` implementation must be registered in the app's DI container. For example:
```csharp
// Note: choose the appropriate lifetime here, e.g. Transient or Singleton
services.AddSingleton<IResponseSerializer, JsonResponseSerializer>();
```

### ASP.NET Framework

For older .NET Framework projects this can be added into the `Global.asax.cs` file like so:

```csharp
private static ILoggerFactory LoggerFactory { get; private set; }

protected void Application_Start()
{
    // Logger factory will need to be configured as required on startup
    LoggerFactory = new LoggerFactory();
    GlobalConfiguration.Configuration.Filters.ConfigureExceptions(loggerfactory, e =>
    {
        ...
    });
}
```

## Customisation

### Adding Handlers

Both of the above methods accept actions to customise the handlers that deal with exceptions. An example of this below:

```csharp
app.ConfigureExceptions(loggerfactory, e =>
{
    e.Handle((KeyNotFoundException ex) =>
    {
        return new ErrorResult(ErrorCodes.NotFound, ex.message);
    }, HttpStatusCode.NotFound);

    e.Handle((InvalidOperationException ex) =>
        new ErrorResult(ErrorCodes.InvalidOperation, ex.message));

    e.Handle((ValidationException ex) =>
    {
        return ex.Errors
            .GroupBy(ve => ve.PropertyName, ve => ve.ErrorMessage)
            .Select(group => new ErrorResult(group.Key, group));
    }
    statusCode: HttpStatusCode.BadRequest,
    responseType: ExceptionResponseTypes.Validation);

    e.Handle((Exception ex) => ErrorResult.FromException(ex));
});
```

Both of the above methods state the type of exception being handled as well state what result to return as the response body. The `Add` method accepts two generic arguments, one for the type of `Exception` and one for the type of result, both of these can be insinuated if you define the action to perform with the `Exception` as above.

As you can see, there is a difference between the two method calls. One of them accepts a `HttpStatusCode`, which tells the exception filter what Status Code to return the response as. If you don't specify the Status Code will default to `400`.

### Creating your own handlers

There are multiple classes that are made available to the user of this library to write your own handlers:

1. `IExceptionHandler`
2. `IHttpExceptionHandler`
3. `ExceptionHandler`
4. `HttpExceptionHandler`

If you write your own handler there is an add method on the options builder to just add a handler to te options that accepts `IExceptionHandler`.

#### IExceptionHandler

This is an interface that a handler _needs_ to implement in order to be used. It forces the implementation of a method that takes in an exception and returns an object result.

It also allows the optional implementation of a logging method for that specific type of exception.

#### IHttpException Handler

This is an optional interface that a handler can implement. It inherits from `IExceptionHandler`, but also has a `StatusCode` that is used to change the `StatusCode` of the error response that comes from APIs.

#### ExceptionHandler

This is the default implementation of `IExceptionHandler`, you can inherit from this if you want to have some work already done for you.

#### HttpExceptionHandler

This is the default implementation of `IHttpExceptionHandler`, you can inherit from this if you want to have some work already done for you.

#### Examples

##### Entity Framework 6

```csharp
e.Handle((DbEntityValidationException ex) => ex.EntityValidationErrors
    .SelectMany((entityValidation, index) =>
    {
        var entityType = entityValidation.Entry.Entity.GetType().Name;
        return entityValidation.ValidationErrors
            .Select(propertyValidation =>
            {
                var propertyName = propertyValidation.PropertyName;
                var message = propertyValidation.ErrorMessage;
                return new ErrorResult($"{entityType}.{propertyName}", message);
            });
    }),
    statusCode: HttpStatusCode.BadRequest);
```

##### FluentValidation

```csharp
return builder.Handle(
    (ValidationException exception) =>
    {
        return exception.Errors
        .GroupBy(error => error.PropertyName, error => error.ErrorMessage)
        .Select(group => new ErrorResult(group.Key, group));
    },
    statusCode: HttpStatusCode.BadRequest,
    responseType: ExceptionResponseTypes.Validation);
```

### Logging

It is possible for you to setup logging for exceptions as a whole, or different types of logging for each type of exception.

#### Default Logging

When setting a default logging method, this is done directly against the builder that you get access to in the `ConfigureExceptions` method.

If you do not configure a default logging action, then the library will automatically log the exception message.

```csharp
app.ConfigureExceptions(loggerfactory, e =>
{
    e.WithDefaultLogging((logger, ex) =>
    {
        logger.LogError(ex, ex.Message);
        Console.Error.Write(ex);
    });
});
```

#### Logging per Exception Type

When setting the logging action for an individual handler it is just passed in as argument to the `Handle` method.

```csharp
e.Handle((ArgumentException ex) => new
{
    Result = ex.Message
}, (logger, ex) =>
{
    logger.LogTrace("Argument exception encountered");
});
```

If you don't pass an action for logging a specific type of `Exception`, it will revert back to the default logging that was setup.

# Contributing
We welcome contributions! Please feel free to check our [Contribution Guidlines](https://github.com/audaciaconsulting/.github/blob/main/CONTRIBUTING.md) for feature requests, issue reporting and guidelines.