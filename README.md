## Audacia.ExceptionHandling
Fluent standardized exception configuration for ASP.NET Web APIs.

### ASP.NET Core

After adding the `Audacia.ExceptionHandling.AspNetCore` package, the following can be added to your `Startup.cs` file:

```c#
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    app.ConfigureExceptions(e => { });
}
```

### ASP.NET Framework

For older .NET Framework projects this can be added into the `Global.asax.cs` file like so:

```c#
protected void Application_Start()
{
    GlobalConfiguration.Configuration.Filters.ConfigureExceptions(e => { });
}
```

### Adding Handlers

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


| Exception                                                                                 | Package                                     | Response Code              |
|-------------------------------------------------------------------------------------------|---------------------------------------------|----------------------------|
| KeyNotFoundException                                                                      | Audacia.ExceptionHandling                   | 404 (Not Found)            |
| UnauthorizedAccessException                                                               | Audacia.ExceptionHandling                   | 403 (Forbidden)            |
| ValidationException                                                                       | Audacia.ExceptionHandling.Annotations       | 422 (Unprocessable Entity) |
| ValidationException [FluentValidation](https://github.com/JeremySkinner/FluentValidation) | Audacia.ExceptionHandling.FluentValidation  | 422 (Unprocessable Entity) |
| JsonReaderException [Json.NET](https://github.com/JamesNK/Newtonsoft.Json)                | Audacia.ExceptionHandling.Json              | 400 (Bad Request)          |