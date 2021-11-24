😩 Errors, Exceptions...
I don't want to deal with it.
Let me just throw an Exception and let a point to catch all Exceptions and return me a standard  http response and model.
Ok then, just use this line in your MVC project.
```csharp
app.UseMiddleware<ErrorManagement.Mvc.ErrorHandlingMiddleware>();
```

It's that much easy 😏
