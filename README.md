# Audacia.ExceptionHandling

Fluent standardized exception configuration for ASP.NET Web APIs.

## Frameworks

### ASP.NET MVC Core

After adding the `Audacia.ExceptionHandling.AspNetCore` package, the following can be added to your `Startup.cs` file:

```csharp
public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
{
    app.ConfigureExceptions(e =>
    {
        ...
    }, loggerfactory);
}
```

This adds an exception filter that catches every exception that happens and sends a HTTP response of your choosing.

### ASP.NET Framework

For older .NET Framework projects this can be added into the `Global.asax.cs` file like so:

```csharp
private static ILoggerFactory LoggerFactory { get; private set; }

protected void Application_Start()
{
    // Logger factory will need to be configured as required on startup
    LoggerFactory = new LoggerFactory();
    GlobalConfiguration.Configuration.Filters.ConfigureExceptions(e =>
    {
        ...
    }, LoggerFactory);
}
```

## Customisation

### Adding Handlers

Both of the above methods accept actions to customise the handlers that deal with exceptions. An example of this below:

```csharp
app.ConfigureExceptions(e =>
{
    e.Handle((KeyNotFoundException ex) =>
    {
        return new ErrorModel(ErrorCodes.NotFound, ex.message);
    }, HttpStatusCode.NotFound);

    e.Handle((InvalidOperationException ex) =>
    {
        return new ErrorModel(ErrorCodes.InvalidOperation, ex.message);
    });

    e.Handle((DomainValidationException ex) => ex.Errors
        .GroupBy(v => v.MemberName)
        .Select(v => 
        {
            return new FieldValidationErrorModel(v.MemberName, v.ErrorMessage);
        })
    );

    e.Handle((DbUpdateException ex) => new ErrorModel(ErrorCodes.DatabaseUpdateFailure, ex.Message)
    {
        ExtraProperties =
        {
            { "StackTrace", ex.StackTrace.ToString() },
            {
                "Entries", 
                ex.Entries.Select(e => new
                {
                    EntityType = e.Entity.GetType().ToString(),
                    Entity = JsonConvert.SerializeObject(e.Entity)
                }).ToList()
            }
        }
    });
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
                .Select((entityValidation, index) => entityValidation.ValidationErrors
                    .Select(propertyValidation =>
                    {
                        var entityType = entityValidation.Entry.Entity.GetType().Name;
                        var propertyName = propertyValidation.PropertyName;
                        var message = propertyValidation.ErrorMessage;
                        return new
                        {
                            EntityType = entityType,
                            PropertyName = propertyName,
                            Message = message
                        }
                    })).SelectMany(c => c),
        HttpStatusCode.BadRequest);
```

##### FluentValidation

```csharp
e.Handle((ValidationException ex) => ex.Errors
                    .Select(member => new
                        {
                            Message = member.ErrorMessage,
                            PropertyName = member.PropertyName
                        }),
        HttpStatusCode.BadRequest);
```

##### NewtonsoftJSON

```csharp
e.Handle((JsonReaderException ex) => new
        {
            Message = ex.Message,
            Path = ex.Path,
            LineNumber = ex.LineNumber,
            LinePosition = ex.LinePosition
        },
    HttpStatusCode.BadRequest);
```

### Logging

It is possible for you to setup logging for exceptions as a whole, or different types of logging for each type of exception.

#### Default Logging

When setting a default logging method, this is done directly against the builder that you get access to in the `ConfigureExceptions` method.

```csharp
var logger = app.ApplicationServices.GetService<ILogger>();

app.ConfigureExceptions(e =>
{
    e.WithDefaultLogging(ex =>
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
}, ex =>
{
    Console.Error.Write("Argument exception encountered");
});
```

If you don't pass an action for logging a specific type of `Exception`, it will revert back to the default logging that was setup.
