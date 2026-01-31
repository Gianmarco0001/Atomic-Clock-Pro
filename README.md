# Atomic Clock Pro 

**Atomic Clock Pro** è un widget leggero e ad alta precisione per Windows, progettato specificamente per chi ha bisogno di un controllo millimetrico del tempo durante i **ClickDay**, i bandi online o i lanci di prodotti limitati.

Sviluppato in C# e WPF, questo orologio atomico si posiziona sopra la taskbar di Windows e offre una sincronizzazione in tempo reale con i server NTP mondiali, compensando automaticamente la latenza di rete.

---

![Registrazione2026-01-31211652-ezgif com-video-to-gif-converter](https://github.com/user-attachments/assets/478b4c6b-f3b0-42f2-847f-47fcdda99270)

---

## Caratteristiche Principali

* **Sincronizzazione Atomica ad Alta Precisione**: Interroga i server NTP (Google, Cloudflare, INRIM) e calcola il Round Trip Time (RTT) per eliminare l'errore di latenza.
* **Display al Millisecondo**: Visualizzazione fluida sincronizzata con il refresh rate del monitor tramite `CompositionTarget.Rendering`.
* **Modalità ClickDay (Target Countdown)**: Imposta un orario obiettivo specifico e monitora il countdown in tempo reale.
* **Feedback Visivo "Semaforo"**: 
    * 🟢 **Verde**: Operativo.
    * 🟠 **Arancione**: -30 secondi al target.
    * 🔴 **Rosso**: -5 secondi (momento del click).
* **Segnali Acustici**: Alert sonori negli ultimi 3 secondi per un tempismo perfetto anche senza guardare lo schermo.
* **Modalità "Ghost" (Click-Through)**: Utilizzo delle API Win32 (`SetWindowLong`) per rendere l'orologio trasparente ai click, evitando interferenze con la taskbar.
* **Controllo Opacità Dinamico**: Regola la trasparenza al volo usando la rotellina del mouse.
* **Persistence**: Sistema di salvataggio custom (`SettingsManager`) per ricordare posizione, opacità e server scelto.

---

## Scorciatoie da Tastiera (Hotkeys)

* **ALT + K**: Blocca/Sblocca l'orologio. Quando è sbloccato (bordo verde) puoi trascinarlo; quando è bloccato (bordo grigio) i click passano attraverso il widget.
* **Rotellina del Mouse**: Regola l'opacità (dal 10% al 100%) posizionando il puntatore sul widget.

---

## Struttura del Progetto (Source Code)

Per chi desidera compilare o studiare il codice sorgente, i file principali sono organizzati come segue:

| File | Descrizione |
| :--- | :--- |
| **atomic_clock.slnx** | File di soluzione principale per aprire il progetto in Visual Studio. |
| **atomic_clock.csproj** | File di configurazione del progetto .NET e delle dipendenze. |
| **MainWindow.xaml / .cs** | Cuore del widget: interfaccia grafica e logica di sincronizzazione NTP. |
| **SetTargetWindow.xaml / .cs** | Interfaccia per l'inserimento manuale dell'orario obiettivo. |
| **AboutWindow.xaml / .cs** | Finestra informativa sui crediti del progetto. |
| **SettingsManager.cs** | Gestore personalizzato per la serializzazione JSON delle impostazioni. |
| **Settings.settings** | Definizione delle proprietà utente e valori predefiniti. |
| **atomicicona.ico** | Asset grafico originale |

---

## Requisiti

* Windows 10 o 11 (64-bit consigliato).
* .NET 8.0 Runtime.

---

## Installazione e Uso

1.  Scarica il pacchetto e avvia l'eseguibile `atomic_clock.exe` dalla sezione **Releases**.
2.  Posiziona l'orologio dove preferisci sulla tua taskbar o vicino ai pulsanti che dovrai cliccare.
3.  Clicca con il tasto destro per accedere al menu:
    * Scegli il server NTP più vicino a te (es. INRIM per l'Italia).
    * Imposta l'orario del tuo ClickDay.
    * Seleziona "Blocca sulla Taskbar" per attivare la modalità Ghost.

---

## Autore

Progetto ideato e sviluppato da:
**Gianmarco Benedetti | GBytez**

---

> [!IMPORTANT]
> *Nota: Questo software è fornito a scopo informativo per assistere l'utente nel monitoraggio del tempo. L'autore non si assume responsabilità per l'esito dei clickday o per eventuali problemi di rete dell'utente.*
