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

Fra `SAMLINGSGREIER/backend`:

1. Installer dotnet-verktøyene som prosjektet bruker (f.eks. `dotnet-ef`) med `dotnet tool restore`

2. Hent NuGet pakker med `dotnet restore`

3. Bygg databasen ved å kjøre migrations med `dotnet ef database update`

4. Start serveren / backend med `dotnet run`
