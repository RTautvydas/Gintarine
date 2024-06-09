# Gintarine Clients API

## Overview

Gintarine Clients API is a web application designed to manage client information and their postcodes by interacting with an external API. This project is built using ASP.NET Core and demonstrates the use of dependency injection, configuration, HTTP clients, and Entity Framework Core for data access.

## Prerequisites

- .NET 6.0 SDK or later
- SQL Server (local or remote)

## Getting Started

### 1. Configure the Database
Update the connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "GintarineConnectionString": "Your-SQL-Server-Connection-String"
  }
}
```
### 2. Set Up Post api settings

Update the `appsettings.json` to include your post API url and key:

```json
{
  "PostSettings": {
    "Url": "https://api.postit.lt/",
    "ApiKey": "postit.lt-examplekey"
  }
}
```
### 3. Run the Application
Use the following command to run the application from Gintarine.Api folder:
```bash
dotnet run
```

Swagger UI with endpoints examples on how to use them should be available here: http://localhost:5057/swagger/index.html
