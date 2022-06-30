<div id="top"></div>

[![Issues][issues-shield]][issues-url]
[![Release][release-shield]][release-url]
[![MIT License][license-shield]][license-url]
[![Tests][test-shield]][test-url]
[![LinkedIn][linkedin-shield]][linkedin-url]


<br />
<div align="center">
  <a href="https://github.com/mtorromacco/SmtpD">
    <img src="logo.png" alt="Logo" width="80" height="80">
  </a>

  <h3 align="center">SmtpD</h3>

  <p align="center">
    Server SMTP per sviluppo e tests
    <br />
    ·
    <a href="https://github.com/mtorromacco/SmtpD/issues">Segnala Bug</a>
    ·
    <a href="https://github.com/mtorromacco/SmtpD/issues">Richiedi Feature</a>
  </p>
</div>


<details>
  <summary>Indice</summary>
  <ol>
    <li>
      <a href="#descrizione">Descrizione</a>
      <ul>
        <li><a href="#tecnologie-utilizzate">Tecnologie utilizzate</a></li>
      </ul>
    </li>
    <li>
      <a href="#come-iniziare">Come iniziare</a>
      <ul>
        <li><a href="#prerequisiti">Prerequisiti</a></li>
        <li><a href="#installazione-source-code">Installazione (source code)</a></li>
        <li><a href="#installazione-release">Installazione (release)</a></li>
      </ul>
    </li>
    <li><a href="#utilizzo">Utilizzo</a></li>
    <li><a href="#struttura">Struttura</a></li>
    <li><a href="#contribuire">Contribuire</a></li>
    <li><a href="#licenza">Licenza</a></li>
  </ol>
</details>


## Descrizione

[![Product Name Screen Shot][product-screenshot]](https://github.com/mtorromacco/SmtpD)

Semplice server SMTP per Windows, Linux e Mac OS-X con interfaccia Web che ti permette di testare l'invio di email dalla tua applicazione senza necessità di un vero server.
Questo repository contiene solo la parte di back-end, se si fosse interessati anche al codice sorgente della parte front-end cliccare [QUI](https://github.com/mtorromacco/SmtpD-FE) per accedere al repository.

### Tecnologie utilizzate

* [Angular](https://angular.io/)
* [.NET](https://dotnet.microsoft.com/)
* [SQLite](https://www.sqlite.org/)


## Come iniziare

E' possibile clonare il codice sorgente e compilarlo oppure scaricare direttamente la release dalla sezione dedicata.

### Prerequisiti

Per poter compilare il codice sorgente del backend dell'applicazione è necessario aver installato .NET 6.

### Installazione (source code)

1. Clona la repo
   ```sh
   git clone https://github.com/mtorromacco/SmtpD.git
   ```
2. Esegui l'applicazione
   ```sh
   dotnet run
   ```

### Installazione (release)

1. Scarica l'ultima versione corrispondente al tuo sistema operativo dalla relativa pagina
2. Estrai l'archivio e spostalo in una cartella a te comoda
3. Esegui l'applicazione all'interno della cartella bin/ senza cambiarle percorso


## Utilizzo

Per lanciare l'applicazione basta semplicemente avviare l'eseguibile presente all'interno della cartella bin. 


## Struttura

* bin/ → Contiene l'eseguibile per avviare il server SMTP
* lib/ → Contiene la release dell'applicazione front-end
* data/ → Contiene il database SQLite con tutti i dati dell'applicazione
* logs/ → Contiene eventuali logs generati dall'applicazione


## Contribuire

I contributi sono ciò che rende la comunità open source un magnifico posto per imparare, ispirare e creare. Qualsiasi contributo sarà **molto apprezzato**.

Se hai un suggerimento per rendere l'applicazione migliore, per favore crea un fork del progetto e poi una pull request oppure apri una issue anche solo per un consiglio.

Processo:

1. Fork del progetto
2. Crea il tuo feature branch (`git checkout -b feature/MyFeatures`)
3. Commit delle modifiche (`git commit -m 'Aggiunte alcune features'`)
4. Push sul branch (`git push origin feature/MyFeatures`)
5. Apri una pull request


## Licenza

Distributed under the MIT License. See `LICENSE.txt` for more information.

<p align="right">(<a href="#top">Torna in alto</a>)</p>



[issues-shield]: https://img.shields.io/github/issues/mtorromacco/SmtpD.svg?style=for-the-badge
[issues-url]: https://github.com/mtorromacco/SmtpD/issues

[release-shield]: https://img.shields.io/github/v/release/mtorromacco/SmtpD.svg?display_name=tag&style=for-the-badge
[release-url]: https://github.com/mtorromacco/SmtpD/releases

[license-shield]: https://img.shields.io/badge/License-MIT-yellow.svg?style=for-the-badge
[license-url]: https://opensource.org/licenses/MIT
	
[test-shield]: https://img.shields.io/github/workflow/status/mtorromacco/SmtpD/Tests/main.svg?style=for-the-badge 
[test-url]: https://github.com/mtorromacco/SmtpD/actions/workflows/tests.yml

[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?logo=linkedin&colorB=555&style=for-the-badge
[linkedin-url]: https://linkedin.com/in/matteo-torromacco

[product-screenshot]: screenshot.png
