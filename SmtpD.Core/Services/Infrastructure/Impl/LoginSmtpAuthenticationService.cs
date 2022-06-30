using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpD.Core.Services.Infrastructure.Impl;
public class LoginSmtpAuthenticationService : SmtpAuthenticationService {

    public LoginSmtpAuthenticationService(string username, string password) : base(username, password) { }


    public override void SetCredentials(string rawCredentials) {
        throw new NotImplementedException();
    }
    

    public override void SetPassword(string rawPassword) {
        byte[] passwordPlainText = Convert.FromBase64String(rawPassword);
        this.ReceivedPassword = Encoding.UTF8.GetString(passwordPlainText);
    }


    public override void SetUsername(string rawUsername) {
        byte[] usernamePlainText = Convert.FromBase64String(rawUsername);
        this.ReceivedUsername = Encoding.UTF8.GetString(usernamePlainText);
    }
}
