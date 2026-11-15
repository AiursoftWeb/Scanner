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

See [the usage guide](https://github.com/AiursoftWeb/scanner/blob/master/docs/usage.md) for configuration and additional examples.

## Configuration

No additional configuration is required.

<a name="advanced-usage"></a>

Select service lifetimes through the marker interfaces. Pass the abstractions your application uses to `AddScannedDependencies(typeof(IMyService))`; see [abstract-service registration](https://github.com/AiursoftWeb/scanner/blob/master/docs/usage.md#advanced-usage) for examples.

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
