using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SmtpD.Core.Services.Infrastructure.Impl;
public class CramMd5SmtpAuthenticationService : SmtpAuthenticationService {

    private readonly string challenge = null;


    public CramMd5SmtpAuthenticationService(string username, string password, string challenge) : base(username, password) {
        this.challenge = challenge;
    }


    public override void SetCredentials(string rawCredentials) {

        byte[] credentialsPlainText = Convert.FromBase64String(rawCredentials);
        string receivedCredentials = Encoding.UTF8.GetString(credentialsPlainText);

        this.ReceivedUsername = receivedCredentials.Split(" ").First();
        this.ReceivedPassword = receivedCredentials.Split(" ").Last().ToLower();

        using HMACMD5 hmacsha256 = new(Encoding.UTF8.GetBytes(this.Password));
        
        byte[] hash = hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(this.challenge));
        string password = Convert.ToHexString(hash).ToLower();
        this.Password = password;
    }


    public override void SetPassword(string rawPassword) {
        throw new NotImplementedException();
    }


    public override void SetUsername(string rawUsername) {
        throw new NotImplementedException();
    }
}
