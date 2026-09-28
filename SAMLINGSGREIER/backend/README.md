# appsettings.Development.json
Opprett en fil i backend root som heter 'appsettings.Development.json' og legg inn følgene:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },

  "Cors": {
    "AllowedOrigins": ["<url til frontend>"]
  }
}
```
Note: `WithOrigins([])` krever eksakt origin (f.eks `http://localhost:5173` og ikke `http://localhost:5173/`)

# appsettings.json
Opprett enda en fil i backend root som heter 'appsettings.json' og legg inn følgene:

```json
{
  "ConnectionStrings": {
    "Samling": "Data Source=samling.db"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}

```

# How to run

To initiate tools, type `dotnet tool restore`

To get the database, type `dotnet ef database update` in a terminal from the /backend folder (cd SAMLINGSGREIER/backend)

in the same terminal, type `dotnet run` to run the backend from the /backend folder.
