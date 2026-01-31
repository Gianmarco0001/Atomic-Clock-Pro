# Atomic Clock Pro

Atomic Clock Pro è un widget leggero e ad alta precisione per Windows, progettato specificamente per chi ha bisogno di un controllo millimetrico del tempo durante i **ClickDay**, i bandi online o i lanci di prodotti limitati.

Sviluppato in C# e WPF, questo orologio atomico si posiziona sopra la taskbar di Windows e offre una sincronizzazione in tempo reale con i server NTP mondiali, compensando automaticamente la latenza di rete.

## Caratteristiche Principali

- **Sincronizzazione Atomica ad Alta Precisione**: Interroga i server NTP (Google, Cloudflare, INRIM) e calcola il Round Trip Time (RTT) per eliminare l'errore di latenza.
- **Display al Millisecondo**: Visualizzazione fluida sincronizzata con il refresh rate del monitor.
- **Modalità ClickDay (Target Countdown)**: Imposta un orario obiettivo e monitora il countdown.
- **Feedback Visivo "Semaforo"**: 
  - 🟢 **Verde**: Operativo.
  - 🟠 **Arancione**: -30 secondi al target.
  - 🔴 **Rosso**: -5 secondi (momento del click).
- **Segnali Acustici**: Alert sonori negli ultimi 3 secondi per un tempismo perfetto anche senza guardare lo schermo.
- **Modalità "Ghost" (Click-Through)**: Una volta posizionato, l'orologio può diventare trasparente ai click del mouse per non interferire con l'uso della taskbar o del browser.
- **Controllo Opacità Dinamico**: Regola la trasparenza al volo usando la rotellina del mouse.
- **Persistence**: Ricorda automaticamente l'ultima posizione, l'opacità e il server scelto.

## Scorciatoie da Tastiera (Hotkeys)

- **ALT + K**: Blocca/Sblocca l'orologio. Quando è sbloccato (bordo verde) puoi trascinarlo; quando è bloccato (bordo grigio) i click passano attraverso il widget.
- **Rotellina del Mouse**: Regola l'opacità (dal 10% al 100%) posizionando il puntatore sul widget.

## Requisiti

- Windows 10 o 11 (64-bit consigliato)
- .NET 8.0 Runtime

## Installazione e Uso

1. Scarica l'eseguibile `atomic_clock.exe`.
2. Posiziona l'orologio dove preferisci sulla tua taskbar o vicino ai pulsanti che dovrai cliccare.
3. Clicca con il tasto destro per accedere al menu:
   - Scegli il server NTP più vicino a te (es. INRIM per l'Italia).
   - Imposta l'orario del tuo ClickDay.
   - Seleziona "Blocca sulla Taskbar" per attivare la modalità Ghost.

## Autore

Progetto ideato e sviluppato da:
**Gianmarco Benedetti | GBytez**

---

*Nota: Questo software è fornito a scopo informativo per assistere l'utente nel monitoraggio del tempo. L'autore non si assume responsabilità per l'esito dei clickday o per eventuali problemi di rete dell'utente.*
