# Oppgave: Hva spilles på P3 Musikk?

## Bakgrunn

Jørgen lurer ofte på hvilken låt og artist som spilles akkurat nå på **P3 Musikk**. I stedet for å sjekke [radio.nrk.no/direkte/p3musikk](https://radio.nrk.no/direkte/p3musikk) manuelt, skal du lage en enkel nettside som viser dette automatisk – og som oppdaterer seg selv når låten byttes.

## Mål

Når du er ferdig skal Jørgen kunne åpne en HTML-fil (eventuelt via en lokal server) og se:

- Hvilken **låt** som spilles akkurat nå
- Hvilken **artist** som spilles
- Informasjonen skal **oppdatere seg selv** når NRK bytter til neste låt (du velger selv hvordan – f.eks. med jevne mellomrom, eller når du oppdager at noe har endret seg)

## Rammer for løsningen

- **Bruk Svelte og TypeScript.** Du har jobbet med Express og React fra før – dette er en fin anledning til å bli kjent med et nytt rammeverk og med typet JavaScript.
- Sett opp prosjektet med [SvelteKit](https://kit.svelte.dev/) (eller et annet standard Svelte+TS-oppsett du finner naturlig), som en SPA. Du trenger ikke servere noe fra en backend for denne oppgaven.
- **Ingen eksterne pakker/biblioteker** utover Svelte/SvelteKit-oppsettet, med mindre det er helt nødvendig for å løse oppgaven. Kan du løse det med innebygde nettleser-API-er (f.eks. `fetch`), skal du gjøre det.
- Skriv koden i `.ts`-filer og typede `.svelte`-komponenter (`<script lang="ts">`). Del opp logikken i egne moduler/filer der det gir mening (f.eks. én modul for å hente data fra NRK-API-et, egne typer/interfaces for dataene du henter).
- Definer **typer/interfaces** for dataene du henter fra API-et, i stedet for å bruke `any`.
- **Skriv koden selv.** Du kan bruke KI (som Claude) som sparringspartner for å forstå konsepter, diskutere løsninger eller feilsøke – men ikke be KI skrive koden for deg, og ikke kopier kode direkte fra andre kilder, skriv den sjæl.
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

1. **Sett opp et Svelte + TypeScript-prosjekt** (f.eks. med SvelteKit).
2. **Finn endepunktet** ved hjelp av Network-fanen, slik som beskrevet over.
3. **Definer typer** for dataene du forventer å få tilbake fra API-et (låtnavn, artist, ev. mer).
4. **Hent data med `fetch`** i en egen TS-modul, og skriv resultatet til konsollen (`console.log`) for å bekrefte at du får riktige, korrekt typede data.
5. **Vis data i en Svelte-komponent** – bruk Sveltes reaktivitet (`let`/`$:` eller stores, avhengig av hva du synes gir mest mening) til å vise låtnavn og artist i stedet for å bare logge til konsollen.
6. **Style siden** litt med CSS, slik at det ser noenlunde presentabelt ut.
7. **Få siden til å oppdatere seg selv**, f.eks. ved å hente nye data med jevne mellomrom (`setInterval`) og oppdatere state i komponenten kun når noe faktisk har endret seg.
8. **Rydd opp i koden**: fornuftig mappestruktur, gode variabel- og typenavn, logikk delt inn i komponenter/moduler etter ansvar (f.eks. én modul for å hente data, én komponent for visning).

## Git

- Opprett et git-repo for prosjektet fra starten av.
- Gjør committer underveis i naturlige steg (ikke bare én stor commit til slutt) – f.eks. ett steg fra delmål-listen over per commit.
- Skriv korte, beskrivende commit-meldinger som forklarer *hva* som er gjort og gjerne *hvorfor*.
- Legg til en `.gitignore` om det er relevant (f.eks. hvis du bruker en lokal server med avhengigheter).

## Læringsmål

- Bli kjent med **Svelte** som rammeverk – hvordan det skiller seg fra React (komponenter, reaktivitet, mindre boilerplate)
- Grunnleggende ferdigheter i **TypeScript**: typer, interfaces, og å typete data fra et eksternt API
- Forstå og bruke **`fetch`** til å hente data fra et eksternt API
- Jobbe med **TS-moduler** (`import`/`export`) og komponent-basert struktur
- Lese og tolke **JSON**-responser og lage typer som matcher dem
- Bruke **utviklerverktøy** i nettleseren til å utforske nettverkstrafikk
- Praktisk bruk av **Git**: committe i fornuftige steg med gode meldinger

## Definition of done

- [ ] Siden viser låt og artist som spilles nå på P3 Musikk
- [ ] Informasjonen oppdateres automatisk uten at man må laste siden på nytt manuelt
- [ ] Prosjektet er satt opp med Svelte og TypeScript
- [ ] Dataene fra API-et er typet (ikke `any`)
- [ ] Logikk og visning er delt inn i moduler/komponenter etter ansvar
- [ ] Ingen unødvendige eksterne pakker er brukt
- [ ] Koden er skrevet av deg selv (KI kun brukt som sparringspartner)
- [ ] Prosjektet ligger i et git-repo med flere, meningsfulle commits

## Videre arbeid (senere)

Dette er en start – vi bygger videre og gjør prosjektet mer komplekst etter hvert. Ikke bekymre deg for å "fremtidssikre" koden for mye ennå; fokuser på å få noe som fungerer, og gjør det ryddig underveis.

Lykke til! 🎧
