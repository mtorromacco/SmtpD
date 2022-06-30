using SmtpD.Core.Dtos.Emails.Requests;
using SmtpD.Core.Dtos.Emails.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpD.Core.Services.Domain;
public interface IEmailService {

    /// <summary>
    /// Ottenimento tutte emails
    /// </summary>
    /// <returns>Lista di emails</returns>
    Task<List<EmailDto>> GetAllAsync();

    /// <summary>
    /// Eliminazione email tramite ID
    /// </summary>
    /// <param name="id">ID dell'email da eliminare</param>
    Task DeleteByIdAsync(long id);

    /// <summary>
    /// Flag email come letta
    /// </summary>
    /// <param name="id">ID dell'email impostare come letta</param>
    Task FlagAsReadedAsync(long id);

    /// <summary>
    /// Eliminazione di tutte le emails
    /// </summary>
    Task DeleteAllAsync();

    /// <summary>
    /// Creazione di una nuova email
    /// </summary>
    /// <param name="from">Mittente</param>
    /// <param name="to">Destinatario</param>
    /// <param name="subject">Oggettto</param>
    /// <param name="message">Messaggio</param>
    /// <returns>Nuova email</returns>
    Task<EmailDto> CreateAsync(string from, string to, string subject, string message);
}
