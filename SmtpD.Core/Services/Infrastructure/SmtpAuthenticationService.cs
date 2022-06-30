using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpD.Core.Services.Infrastructure;
public abstract class SmtpAuthenticationService {
    
    /// <summary>
    /// Username presente nel DB
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Password presente nel DB
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Username ricevuto dal client
    /// </summary>
    public string ReceivedUsername { get; set; }

    /// <summary>
    /// Password ricevuta dal client
    /// </summary>
    public string ReceivedPassword { get; set; }


    protected SmtpAuthenticationService(string username, string password) {
        this.Username = username;
        this.Password = password;
    }


    /// <summary>
    /// Impostazione username
    /// </summary>
    /// <param name="rawUsername">Username grezzo ricevuto</param>
    public abstract void SetUsername(string rawUsername);

    /// <summary>
    /// Impostazione password
    /// </summary>
    /// <param name="rawPassword">Password grezza ricevuta</param>
    public abstract void SetPassword(string rawPassword);

    /// <summary>
    /// Impostazione username e password
    /// </summary>
    /// <param name="rawCredentials">Username e password grezzi ricevuti</param>
    public abstract void SetCredentials(string rawCredentials);


    /// <summary>
    /// Verifica della validità dell'autenticazione
    /// </summary>
    /// <returns>true se il client è autenticato, altrimenti false</returns>
    public bool IsAuthenticated() {
        return this.Username == this.ReceivedUsername && this.Password == this.ReceivedPassword;
    }
}
