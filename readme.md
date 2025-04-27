<h1 align="left"><img src="https://raw.githubusercontent.com/AdamTovatt/pg-dump/master/Icon/Icon.png" alt="PgDump Logo" width="48" height="48" style="vertical-align: middle; margin-left: 10px;" /> PgDump</h1>

[![NuGet Version](https://img.shields.io/nuget/v/PgDump.svg)](https://www.nuget.org/packages/PgDump/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/PgDump.svg)](https://www.nuget.org/packages/PgDump/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

**Do you want to easily use `pg_dump` from C# but still have great control over how the output is handled?**

*Look no further!*
  
This library lets you use one of the existing output providers or define your own to handle the output stream in your preferred way - file, memory, network, or anything else you want.

Simple to get started ([see Quick Start](#quick-start)). Powerful when needed.

## What is pg_dump?

> "pg_dump is a utility for backing up a PostgreSQL database."  
> — [PostgreSQL Official Documentation](https://www.postgresql.org/docs/current/app-pgdump.html)

`pg_dump` is the standard PostgreSQL command-line tool used to create backups of a database.  
It exports the contents of a database to a file — either as plain SQL text or as a binary archive (tar, custom, or directory formats).  
These files can later be used to restore the database exactly as it was.

This library makes it easy to use `pg_dump` directly from your C# applications, without manually handling command-line arguments, processes, or stream output.

---

## Core Components of this library

- `PgClient` — main class to perform database dumps or list databases.
- `DumpAsync()` — dump a database to any `IOutputProvider`.
- `ListDatabasesAsync()` — list all databases on the server. Returns a simple `List<string>` with the names.
- `IOutputProvider` — interface for handling output (write to file, memory, your own type).

---

## Installation
**NuGet** package available here:
https://www.nuget.org/packages/PgDump/
*(uses .NET 8)*

## Quick Start

### Dump a database to a file (super simple)

```csharp
ConnectionOptions options = new ConnectionOptions("localhost", 5432, "postgres", "your_password", "your_database");
PgClient client = new PgClient(options);

FileOutputProvider outputProvider = new FileOutputProvider("dump.tar");

await client.DumpAsync(outputProvider, timeout: TimeSpan.FromMinutes(1));
```

---

### Dump a database into a memory stream

```csharp
ConnectionOptions options = new ConnectionOptions("localhost", 5432, "postgres", "your_password", "your_database");
PgClient client = new PgClient(options);

using MemoryStream memoryStream = new MemoryStream();
StreamOutputProvider outputProvider = new StreamOutputProvider(memoryStream);

await client.DumpAsync(outputProvider, timeout: TimeSpan.FromMinutes(1));

// You now have the dump data in memoryStream
```

---

### Create your own custom output provider

Implement `IOutputProvider`:

```csharp
public class MyCustomOutputProvider : IOutputProvider
{
    public async Task WriteAsync(Stream inputStream, CancellationToken cancellationToken)
    {
        // Example: read and process the dump data however you want
        using MemoryStream buffer = new MemoryStream();
        await inputStream.CopyToAsync(buffer, cancellationToken);

        // Do something with buffer...
    }
}
```

Use it:

```csharp
MyCustomOutputProvider outputProvider = new MyCustomOutputProvider();
await client.DumpAsync(outputProvider, timeout: TimeSpan.FromMinutes(1));
```

---

## Listing databases

You can also list all databases on the server easily:

```csharp
List<string> databases = await client.ListDatabasesAsync(TimeSpan.FromSeconds(30));

foreach (string database in databases)
{
    Console.WriteLine(database);
}
```
---
# Getting into details:
The following sections explain the parameters and features of the library in greater detail.
## The Parameters of `DumpAsync`

**Method signature:**

```csharp
Task DumpAsync(IOutputProvider outputProvider, TimeSpan timeout, DumpFormat format = DumpFormat.Tar, CancellationToken cancellationToken = default)
```

### Parameters:

- `outputProvider`  
  The output provider that will receive the `pg_dump` stream.  
  You can use built-in ones like `FileOutputProvider` and `StreamOutputProvider`, or create your own.

- `timeout`  
  The maximum allowed time for the dump operation.  
  If the operation exceeds this time, it will automatically cancel and throw a `TimeoutException`.  
  *(Timeout is enforced even if the provided `CancellationToken` does not cancel manually.)*

- `format`  
  The desired output format for the dump.  
  One of:  
  - `DumpFormat.Plain` (plain SQL text)  
  - `DumpFormat.Tar` (tar archive)  
  - `DumpFormat.Custom` (PostgreSQL custom binary format) (not tested in the unit tests!)
  - `DumpFormat.Directory` (directory with separate files) (not tested in the unit tests!)

  Default is `DumpFormat.Tar`.

- `cancellationToken`  
  An optional external `CancellationToken` that you can pass if you want manual control over cancellation.  
  If canceled, the operation will throw an `OperationCanceledException`.  
  *(This is combined with the timeout internally.)*

---

### ⚡ How timeout and cancellationToken work together

- **Either** timeout expiration **or** `cancellationToken` cancellation will immediately cancel the operation.
- If timeout happens first → you get a `TimeoutException`.
- If cancellation token cancels first → you get an `OperationCanceledException`.
- Safe and reliable in both cases.

---

## Detailed Features

- **DumpAsync** — runs `pg_dump` and writes output to any stream (file, memory, network, etc.).
- **ListDatabasesAsync** — runs `psql` and lists all database names cleanly.
- **Flexible output handling** — built-in file and memory output providers, and you can easily create your own.
- **Proper cancellation and timeout** — no stuck processes or infinite hangs.
- **Safe environment handling** — password is passed securely through environment variables.
- **No assumptions** — fully configurable connection options.
- **Strong nullability** — supports C# nullable reference types properly.

---

## Testing

This project comes with full **unit tests** and **real integration tests**.

- Providers are tested (file and stream).
- Client behavior is tested (timeouts, errors, success).
- Real integration tests run `pg_dump` and `psql` against a real database.

---

## Nuget
**NuGet package available here:**  
https://www.nuget.org/packages/PgDump/

[![NuGet Version](https://img.shields.io/nuget/v/PgDump.svg)](https://www.nuget.org/packages/PgDump/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/PgDump.svg)](https://www.nuget.org/packages/PgDump/)

---

## License

MIT License — do whatever you want with it, but no warranty.

## Tags

PostgreSQL · pg_dump · psql · database · backup · dump · export · C# · async · command-line wrapper · streaming · output-provider · .net 
