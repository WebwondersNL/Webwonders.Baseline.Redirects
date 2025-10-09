# Webwonders.Baseline.Redirects

A simple ASP.NET Core library for managing URL redirects in Umbraco-based web applications.

---

## Features

- Permanent redirects based on configuration
- Umbraco Cloud domain normalization
- Customizable redirect rules via `appsettings.json`
- HTTPS redirection
- Exclusion rules for development, staging, and specific paths

---

## Installation

Add the project to your solution and reference it in your ASP.NET Core application.

---

## Usage

### Configure redirects in `Program.cs`:

```csharp
using Webwonders.Baseline.Redirects.Configuration;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.ConfigureRedirects(app.Environment, builder.Configuration);

app.Run();
```

