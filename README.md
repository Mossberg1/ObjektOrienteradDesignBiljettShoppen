# BiljettShoppen

Slut projekt för kursen Objekt Orienterad Design och problemlösning på Högskolan Dalarna.
Diagram ligger under docs mappen.
Powerpoint presentation ligger under Presentation mappen.

## Kör projektet

### Clona projektet
```bash
git clone https://github.com/Mossberg1/ObjektOrienteradDesignBiljettShoppen.git
cd ObjektOrienteradDesignBiljettShoppen
```

### Starta databas

```bash
docker compose up -d
```

### Skapa objekt i databasen
```bash
dotnet ef database update --project BiljettShoppen/DataAccess
```
Detta kommando genererar ett felmedelande trotts att objekten skapats i databasen och att allt fungerar som det ska. Skulle ef core inte vara intsallerat på din dator går det att installera med: 
```bash
dotnet tool install --global dotnet-ef
```

### Kör projektet
```
dotnet run --project BiljettShoppen/Web
```