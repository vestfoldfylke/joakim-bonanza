# Oppgave: Hva spilles på P3 Musikk?

## Bakgrunn

Jørgen lurer ofte på hvilken låt og artist som spilles akkurat nå på **P3 Musikk**. I stedet for å sjekke [radio.nrk.no/direkte/p3musikk](https://radio.nrk.no/direkte/p3musikk) manuelt, skal du lage en enkel nettside som viser dette automatisk – og som oppdaterer seg selv når låten byttes.

## Mål

Når du er ferdig skal Jørgen kunne åpne en HTML-fil (eventuelt via en lokal server) og se:

- Hvilken **låt** som spilles akkurat nå
- Hvilken **artist** som spilles
- Informasjonen skal **oppdatere seg selv** når NRK bytter til neste låt (du velger selv hvordan – f.eks. med jevne mellomrom, eller når du oppdager at noe har endret seg)

## Rammer for løsningen

- **Kun plain HTML, CSS og JavaScript.** Ingen rammeverk (React, Vue, osv.).
- **Ingen eksterne pakker/biblioteker** med mindre det er helt nødvendig for å løse oppgaven. Kan du løse det med innebygde nettleser-API-er (f.eks. `fetch`), skal du gjøre det.
- **JavaScript skal ligge i egne moduler** (`.js`-filer med `import`/`export`), ikke som inline `<script>`-kode i HTML-filen. Husk at `<script type="module">` trengs i HTML-en for å bruke moduler.
- **Skriv koden selv.** Du kan bruke KI (som Claude) som sparringspartner for å forstå konsepter, diskutere løsninger eller feilsøke – men ikke be KI skrive koden for deg, og ikke kopier kode direkte, hvis du skal kopiere, skriv den faktisk "for hånd" for mengdetreningen.
   - Bruk gjerne KI til å sparre om kodestruktur, valg av funksjonsnavn, filplasseringer osv. Evt spørre om noe kan gjøres på en annen/bedre måte, bare for å lære.
- Design/utseende står du fritt til å velge selv. Hold det gjerne enkelt til å begynne med – funksjonalitet er viktigere enn utseende i denne omgangen.

## Fremgangsmåte – finne API-et

NRK har ikke et offentlig dokumentert API du bare kan slå opp, men siden radio.nrk.no/direkte/p3musikk viser denne informasjonen i nettleseren, må dataene hentes fra et sted. Slik kan du finne det:

1. Åpne [radio.nrk.no/direkte/p3musikk](https://radio.nrk.no/direkte/p3musikk) i nettleseren.
2. Åpne **utviklerverktøyene** (F12 eller høyreklikk → Inspiser).
3. Gå til fanen **Network** (Nettverk).
4. Filtrer gjerne på **Fetch/XHR** for å slippe unna bilder, CSS osv.
5. Last siden på nytt (F5) og se etter forespørsler som returnerer JSON-data med informasjon om låt/artist/program.
6. Klikk på en aktuell forespørsel og se på:
   - **URL-en** (dette er endepunktet du skal bruke)
   - **Response**-fanen (dette er dataformatet du må parse i JS)
7. Test gjerne endepunktet direkte i nettleseren eller med et verktøy som `curl`/Postman, for å bli kjent med strukturen på svaret før du begynner å kode.

**Tips:**
- Se etter ord som `playback`, `metadata`, `channel`, `playlist` eller lignende i URL-ene.
- Noen NRK-API-er kan ha begrensninger på hvilke domener som får kalle dem (CORS). Dukker det opp feilmeldinger om dette i konsollen, er det noe å diskutere/undersøke – ikke noe å gi opp på.
- Dataene kan være strukturert med informasjon om "nå-spilles" og gjerne også "forrige/neste". Utforsk responsen grundig.

## Delmål / foreslått rekkefølge

1. **Finn endepunktet** ved hjelp av Network-fanen, slik som beskrevet over.
2. **Hent data med `fetch`** fra en JS-modul, og skriv resultatet til konsollen (`console.log`) for å bekrefte at du får riktige data.
3. **Vis data i HTML-en** – oppdater DOM-en med låtnavn og artist i stedet for å bare logge til konsollen.
4. **Style siden** litt med CSS, slik at det ser noenlunde presentabelt ut.
5. **Få siden til å oppdatere seg selv**, f.eks. ved å hente nye data med jevne mellomrom (`setInterval`) og oppdatere DOM-en på nytt kun når noe faktisk har endret seg.
6. **Rydd opp i koden**: fornuftig mappestruktur, gode variabelnavn, moduler delt inn etter ansvar (f.eks. én modul for å hente data, én for å oppdatere DOM-en).

## Git

- Opprett et git-repo for prosjektet fra starten av.
- Gjør committer underveis i naturlige steg (ikke bare én stor commit til slutt) – f.eks. ett steg fra delmål-listen over per commit.
- Skriv korte, beskrivende commit-meldinger som forklarer *hva* som er gjort og gjerne *hvorfor*.
- Legg til en `.gitignore` om det er relevant (f.eks. hvis du bruker en lokal server med avhengigheter).

## Læringsmål

- Grunnleggende ferdigheter i **HTML, CSS og JavaScript**
- Forstå og bruke **`fetch`** til å hente data fra et eksternt API
- Jobbe med **JS-moduler** (`import`/`export`)
- Lese og tolke **JSON**-responser
- Bruke **utviklerverktøy** i nettleseren til å utforske nettverkstrafikk
- Praktisk bruk av **Git**: committe i fornuftige steg med gode meldinger

## Definition of done

- [ ] Siden viser låt og artist som spilles nå på P3 Musikk
- [ ] Informasjonen oppdateres automatisk uten at man må laste siden på nytt manuelt
- [ ] All JS ligger i moduler, ikke inline i HTML
- [ ] Ingen unødvendige eksterne pakker er brukt
- [ ] Koden er skrevet av deg selv (KI kun brukt som sparringspartner)
- [ ] Prosjektet ligger i et git-repo med flere, meningsfulle commits

## Videre arbeid (senere)

Dette er en start – vi bygger videre og gjør prosjektet mer komplekst etter hvert. Ikke bekymre deg for å "fremtidssikre" koden for mye ennå; fokuser på å få noe som fungerer, og gjør det ryddig underveis.

Lykke til! 🎧
