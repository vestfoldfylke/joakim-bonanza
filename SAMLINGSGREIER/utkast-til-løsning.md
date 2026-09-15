## Poenget med denne oppgaven
- Bli kjent med en vanlig tech-stach av ulike komponenter som sikkert dukker opp (konseptuelt) på fagprøve

### Konseptene du skal bli kjent med / bruke
- Frontend
  - Hva som skjer og synes i browseren
  - Velg node-basert frontend - enten SvelteKit (med null ting på server hvis du velger noe annet som API)
- Backend/API
  - Hva som skjer på serveren
  - Velg enten dotnet Minimal API, eller sett opp API-ruter i SvelteKit server side
  - Hvis du velger Sveltekit prøv først å holde deg unna +page.server-load funksjonene, prøv heller å sette opp API-ruter for GET/PUT/POST osv, og påkall disse fra frontend for å enkelt skjønne konseptene
- Enkel database
  - Lagring av data
  - Du kan også ha et interface for databasen (sjekk ut litt youtube på interface), for da kan du også evt bare lagre til filsystem - viktigste er konseptene, men lurt å prøve en db også :)
  - Sjekk ut sqlite i første omgang
    - Hvis dotnet - sjekk ut EFCore
    - Hvis Sveltekit - prøv først sql-spørringene for hånd, evt sleng på Drizzle etterhvert
- Pålogging / autentisering + autorisering
  Begynn med mock først, så setter vi opp en entra appreg der du kan kjøre / skrive manuell OIDC-flyt helt selv (for læring)
  - Rollestyrt tilgang etterhvert


### Ekstra ting som må også på plass
- Sikkerhetsaspektene
  - OWASP top 10
  - Ulike angrep som kan skje
  - Hvordan beskytter man seg mot angrepene
- Testing av koden (unit test, end-to-end osv)
- Git (branch, pr, pull, osv)
- Ci / cd - faktisk få ting ut i produksjon


### Skisse til oppgave
Denne bestemmer du egt selv siden du er brukeren
- Oversikt over samlingsgreiene til Joakim, men oppgaven kan du endre på selv når du vil, eller hvis andre har morsomme ideer. Poenget er at du skal være i stand til å skissere en web-app løsning (full-stack), og lage en prototype av det som kjører lokalt.

### Første steg
- Velg teknologier du har lyst til å utforske (dotnet/node)
- Sett opp kjørende rammeverk
- Prøv å koble på en database-fil (sqlite) eller filsystemet (men husk interface dersom filsystem, egt uansett)
- Utvid herfra - men del opp i bug-fikser og i nye features
  - En bugfix = en branch
  - En ny funksjonalitet = en branch
  - Men selv oppsettet i første omgang kan du ha i en eller flere branch/pr, det velger du - f. eks i branch "project scaffolding"

### Forslag til første funksjonalitet
- Mulighet for å åpne appen, liste opp hvilke gjenstander som finnes i samlingen, og kunne registrere en ny gjenstand. (men du kan velge annet)

### Mens R og J er på konferanse
Bare herje løs! En god lærdom er at det kan ofte lønne seg å starte helt på nytt i blant, så om det blir en PR med masse kommentarer, kan det hende det er like greit å gå tilbake til tegnebrettet.
