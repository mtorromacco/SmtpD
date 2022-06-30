using SmtpD.Core.Dtos.Configs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpD.Core.Services.Domain;
public interface IConfigService {

    /// <summary>
    /// Inizializzazione configurazione tramite la creazione del record di config se non già presente
    /// </summary>
    Task InitializeAsync();

    /// <summary>
    /// Ottenimento configurazione
    /// </summary>
    /// <returns>Configurazione</returns>
    Task<ConfigDto> GetConfigAsync();

    /// <summary>
    /// Aggiornamento credenziali
    /// </summary>
    /// <param name="username">Nuovo username</param>
    /// <param name="password">Nuova password</param>
    /// <returns>Configurazione aggiornata</returns>
    Task<ConfigDto> UpdateCredentialsAsync(string username, string password);

    /// <summary>
    /// Aggiornamento porta
    /// </summary>
    /// <param name="port">Nuova porta</param>
    /// <returns>Configurazione aggiornata</returns>
    Task<ConfigDto> UpdatePortAsync(int port);
}
