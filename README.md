# BiljettShoppen

Slut projekt för kursen Objekt Orienterad Design och problemlösning på Högskolan Dalarna.

## Bygg projektet

### Starta databas
```bash
docker compose up -d
```

### Skapa objekt i databasen
```bash
dotnet ef database update --project BiljettShoppen/DataAccess
```

### Kör projektet
```
dotnet run --project BiljettShoppen/Web
```