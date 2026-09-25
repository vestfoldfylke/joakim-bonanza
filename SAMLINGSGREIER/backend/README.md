# appsettings.Development.json
Opprett en fil i backend root som heter 'appsettings.Development.json' og legg inn følgene:
```
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

# appsettings.json
Opprett enda en fil i backend root som heter 'appsettings.json' og legg inn følgene:

```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}

```