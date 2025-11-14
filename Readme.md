# HEMIT-Ballbingo 🎱

En ASP.NET Core MVC-app for bingo-lotteri.

## Funksjoner
- Registrer lodd (1–opptil 300 per trekning)
- Flere trekninger med historikk
- Trekning med 3 vinnere og visning av tapere
- Legg til premier med navn på premie og bilde (lagres i DB). Bilde opplisting her er opsjonelt.
- Viser totalpotten (50 kr per lodd) som inngår i trekningen.
- Bootstrap 5 og stor font for enkel opplesning

## Oppsett
1. Klon repo
2. Sett connection string i `appsettings.json`
3. Hvis man trenger å installere EF Core CLI verktøy :
   ```powershell
   dotnet tool install --global dotnet-ef --version 8.*
4. Ved behov for å lage ny migrasjon - Kjør:
   ```powershell
   dotnet ef migrations add Initial --project Data
5. Oppdater til nyeste migreringen for database slik:
   ```powershell
   dotnet ef database update --project Data
   
   

