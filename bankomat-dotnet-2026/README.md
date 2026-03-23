# bankomat-dotnet-2026 / ATM

Det här är ett litet C# konsolprojek som simulerar en enkel bankomat.
Användaren kan mata in kort, ange PIN-kod, se saldo, ta ut pengar, sätta in pengar och mata ut kortet.

## Funktioner

- Menydriven bankomat i terminalen
- Enkel inloggning med PIN-kod
- Visa kontosaldo
- Uttag och insättning
- Utmatning av kort och avslut

## Hur programmet fungerar

Vid start skapas:

- ett konto med startsaldo `5000`
- ett kort med nummer `1234-5678`
- PIN-kod `1234`
- en bankomat med kontantkassa `11000`

Programmet går sedan igenom tre lägen:

1. **Välkomstmeny** – mata in kort eller avsluta
2. **PIN-meny** – verifiera PIN-kod
3. **Huvudmeny** – saldo, uttag, insättning, mata ut kort

## Projektstruktur (kort)

- `Program.cs` – startpunkt och initiering av demo-data
- `ConsoleRunner.cs` – all menylogik och användarflöden
- `AtmService.cs` – bankomatens tillstånd och operationer
- `Card.cs` – kortinformation och PIN-kontroll
- `Account.cs` – kontots saldo, uttag och insättning

## Köra projektet

Krav: .NET SDK med stöd för `net10.0`.

```bash
dotnet run
```




