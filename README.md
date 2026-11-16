# Aiursoft Scanner

[![MIT licensed](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/AiursoftWeb/scanner/blob/master/LICENSE)
[![Pipeline stat](https://badge-proxy.aiursoft.com/aiursoft/scanner/badges/master/pipeline.svg)](https://gitlab.aiursoft.com/aiursoft/scanner/-/pipelines)
[![Test Coverage](https://badge-proxy.aiursoft.com/aiursoft/scanner/badges/master/coverage.svg)](https://gitlab.aiursoft.com/aiursoft/scanner/-/pipelines)
[![S360 compliance](https://s360.aiursoft.com/badge.svg?project=aiursoft%2Fscanner)](https://s360.aiursoft.com/SupplyChain?tab=projects&project=aiursoft%2Fscanner&scope=baseline&ecosystem=all)
[![NuGet version (Aiursoft.Scanner)](https://img.shields.io/nuget/v/Aiursoft.Scanner.svg)](https://www.nuget.org/packages/Aiursoft.Scanner/)
[![Man hours](https://manhours.aiursoft.com/r/github.com/aiursoftweb/Scanner.svg)](https://manhours.aiursoft.com/r/github.com/aiursoftweb/Scanner.html)

Scanner registers .NET dependency-injection services by scanning classes with singleton, scoped or transient marker interfaces.

## Overview

<a name="why-this-project"></a>

Replace repeated service registrations with `ISingletonDependency`, `IScopedDependency` and `ITransientDependency` markers. Register implementations by their concrete types, or supply interfaces and abstract classes to register those abstractions as well. The abstractions are available in the separate `Aiursoft.Scanner.Abstractions` package.

The traditional way to add dependencies is:

```csharp
service.AddScoped<MyScopedDependency>();
```

Which means that you have to manually inject all dependencies. When you have too many of them, it is possible to make a mistake.

## Installation

Add the package to your .NET project:

```bash
dotnet add package Aiursoft.Scanner
```

Requires .NET 10 or later.

## Usage

In an ASP.NET Core project's `Program.cs`, register marked services before building the application:

```csharp
using Aiursoft.Scanner;
using Aiursoft.Scanner.Abstractions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScannedDependencies();

var app = builder.Build();
app.MapGet("/", (GreetingService greeting) => greeting.GetMessage());
app.Run();

public sealed class GreetingService : IScopedDependency
{
    public string GetMessage() => "Hello from Scanner";
}
```

A request to `/` resolves `GreetingService` with the scoped lifetime and returns `Hello from Scanner`.

### Additional examples

First, install `Aiursoft.Scanner` to your ASP.NET Core project from nuget.org:

```bash
dotnet add package Aiursoft.Scanner
```

Add the interface to your class like this:

```csharp
using Aiursoft.Scanner.Abstractions;

public class MySingletonService : ISingletonDependency
{

}

public class MyScopedService : IScopedDependency
{

}

public class MyTransientService : ITransientDependency
{

}
```

And just call this in your `StartUp.cs`:

```csharp
using Aiursoft.Scanner;

services.AddScannedDependencies();
```

That's all! All your dependencies are registered. Just use it like previous before:

```csharp
public class MyController : Controller
{
    private readonly MyScopedService _service;
    public MyController(MyScopedService service)
    {
        _service = service;
    }
}
```

### Advanced usage

When you want to register a dependency that implements an abstract, your previous way is:

```csharp
public class MyClass : IAbstract
{

}
```

```csharp
service.AddScoped<IAbstract, MyClass>();
```

That's fine. But now we want to register this automatically.

Add the dependency interface to your service like this:

```csharp
public class MyClass : IAbstract, IScopedDependency
{

}
```

When you are registering all dependencies in your `StartUp.cs`, tell us that your project supports your abstract.

```csharp
services.AddScannedDependencies(typeof(IAbstract));
```

And you can call it with multiple abstracts:

```csharp
services.AddScannedDependencies(typeof(IAbstract1), typeof(IAbstract2), typeof(IAbstract3));
```

That's all! Enjoy!

See [the usage guide](https://github.com/AiursoftWeb/scanner/blob/master/docs/usage.md) for configuration and additional examples.

## Configuration

No configuration file or external service is required. Service registration is configured in startup code as shown below.

### Registration and options

| Marker | Lifetime |
| --- | --- |
| `ISingletonDependency` | One instance for the service provider |
| `IScopedDependency` | One instance per dependency-injection scope |
| `ITransientDependency` | A new instance on each resolution |

`AddScannedDependencies()` scans classes accessible from the entry assembly, excluding system and Microsoft assemblies. `AddLibraryDependencies()` scans the calling assembly. `AddAssemblyDependencies(assembly, abstracts)` selects an assembly explicitly. Pass supported interfaces or abstract classes to register implementations under those abstractions, as shown in Advanced usage. The marker interfaces are supplied by `Aiursoft.Scanner.Abstractions`, which is a dependency of `Aiursoft.Scanner`.

## Development

Install the [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0).

From the repository root, restore, build and test:

```bash
dotnet restore Aiursoft.Scanner.sln
dotnet build Aiursoft.Scanner.sln --no-restore
dotnet test Aiursoft.Scanner.sln --no-restore
```

Run `./lint.sh` for the existing static checks.

## Documentation

- <a name="how-to-install"></a>[Dependency markers and abstract-service registration](https://github.com/AiursoftWeb/scanner/blob/master/docs/usage.md)
- [Scanning boundaries](https://github.com/AiursoftWeb/scanner/blob/master/docs/usage.md#scanning-boundaries)
- [Build and test](#development)

## How to contribute

Report bugs and feature requests through the [issue tracker](https://github.com/AiursoftWeb/scanner/issues). Create a feature branch or fork and submit a merge request or pull request for review.

Follow the [development guide](#development) to build and run the existing checks before submitting changes.

## License

Licensed under the [MIT License](https://github.com/AiursoftWeb/scanner/blob/master/LICENSE).
