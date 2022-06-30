using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpD.Core.Services.Infrastructure.Impl;
public class PlainSmtpAuthenticationService : SmtpAuthenticationService {

    public PlainSmtpAuthenticationService(string username, string password) : base(username, password) { }


    public override void SetCredentials(string rawCredentials) {

        byte[] credentialsPlain = Convert.FromBase64String(rawCredentials);
        string credentials = Encoding.UTF8.GetString(credentialsPlain);

        this.ReceivedPassword = credentials.Split("\0").Last();
        this.ReceivedUsername = credentials.Split("\0").Reverse().Skip(1).First();
    }


    public override void SetPassword(string rawPassword) {
        throw new NotImplementedException();
    }


    public override void SetUsername(string rawUsername) {
        throw new NotImplementedException();
    }
}
