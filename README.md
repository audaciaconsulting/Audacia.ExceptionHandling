## Audacia.ExceptionHandling
Fluent standardized exception configuration for ASP.NET Web APIs.

### .NET Core

After adding the `Audacia.ExceptionHandling.AspNetCore` package, the following can be added to your `Startup.cs` file:

```c#
app.ConfigureExceptions(e => { });
```

And we can now start adding some handlers like this:

```c#
app.ConfigureExceptions(e =>
{
    // Add the default handler for a KeyNotFoundException
    e.Handle.KeyNotFoundException();
    e.Handle.UnauthorizedAccessException();
    
    // Add a custom handler for an exception 
    e.Handle<ConfigurationErrorsException>(exception => new ErrorResult(HttpStatusCode.ServiceUnavailable, "The app is not configured properly."));
});
```

As can be seen, there are some default handlers that are included in the base library, and new handlers can be defined in-line.

A full list of default handlers and their usages are as follows:


| Exception                              | Package                                    | Response Code              |
|----------------------------------------|--------------------------------------------|----------------------------|
| KeyNotFoundException                   | Audacia.ExceptionHandling                  | 404 (Not Found)            |
| UnauthorizedAccessException            | Audacia.ExceptionHandling                  | 403 (Forbidden)            |
| ValidationException                    | Audacia.ExceptionHandling.Annotations      | 422 (Unprocessable Entity) |
| ValidationException (FluentValidation) | Audacia.ExceptionHandling.FluentValidation | 422 (Unprocessable Entity) |
| JsonReaderException (Json.NET)         | Audacia.ExceptionHandling.Json             | 400 (Bad Request)          |