# CommanderGQL

A basic GraphQL API built with ASP.NET Core and Hot Chocolate. It supports querying, mutating, and subscribing to `Platform` and `Command` resources using an underlying relational database.

## 🚀 Getting Started

Before running the project, make sure you have the following:

- [.NET 8 SDK](https://dotnet.microsoft.com/download) installed
- SQL Server up and running
- `DB_PASSWORD` environment variable set

## 🔧 Environment Setup

This project expects a `DB_PASSWORD` environment variable to be set **before starting** the application.

### Windows (PowerShell)

```powershell
$env:DB_PASSWORD="your_db_password_here"
```

### Linux or Mac

```bash
export DB_PASSWORD="your_db_password_here"
```
