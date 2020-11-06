# Audacia.ExceptionHandling

Fluent standardized exception configuration for ASP.NET Web APIs.

## Frameworks

### ASP.NET MVC Core

After adding the `Audacia.ExceptionHandling.AspNetCore` package, the following can be added to your `Startup.cs` file:

```c#
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    app.ConfigureExceptions(e => { });
}
```

This adds an exception filter that catches every exception that happens and sends a HTTP response of your choosing.

### ASP.NET Framework

For older .NET Framework projects this can be added into the `Global.asax.cs` file like so:

```c#
protected void Application_Start()
{
    GlobalConfiguration.Configuration.Filters.ConfigureExceptions(e => { });
}
```

## Customisation

### Adding Handlers

Both of the above methods accept actions to customise the handlers that deal with exceptions. An example of this below:

```c#
app.ConfigureExceptions(e =>
{
    e.Handle((KeyNotFoundException ex) => new
    {
        Result = ex.Message
    }, HttpStatusCode.NotFound);

    e.Handle((InvalidOperationException ex) => new
    {
        Result = ex.Message
    });

    e.Handle((ArgumentException ex) => new
    {
        Result = ex.Message
    });
});
```

Both of the above methods state the type of exception being handled as well state what result to return as the response body. The `Add` method accepts two generic arguments, one for the type of `Exception` and one for the type of result, both of these can be insinuated if you define the action to perform with the `Exception` as above.

As you can see, there is a difference between the two method calls. One of them accepts a `HttpStatusCode`, which tells the exception filter what Status Code to return the response as. If you don't specify the Status Code will default to `400`.

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
e.Logging(ex =>
{
    Console.Error.Write(ex);
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
