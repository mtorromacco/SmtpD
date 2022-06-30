using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpD.UnitTests.SmtpAuthenticationService;
public class PlainSmtpAuthenticationService {

    [Fact]
    public void CheckValidPlainAuth() {

        Core.Services.Infrastructure.SmtpAuthenticationService authService = new Core.Services.Infrastructure.Impl.PlainSmtpAuthenticationService("abc", "xyz");

        string credentials = "abc\0xyz";

        byte[] bytesCredentials = Encoding.UTF8.GetBytes(credentials);
        string base64Credentials = Convert.ToBase64String(bytesCredentials);

        authService.SetCredentials(base64Credentials);

        Assert.True(authService.IsAuthenticated());
    }


    [Fact]
    public void CheckInvalidUsernamePlainAuth() {

        Core.Services.Infrastructure.SmtpAuthenticationService authService = new Core.Services.Infrastructure.Impl.PlainSmtpAuthenticationService("abc", "xyz");

        string credentials = "abcd\0xyz";

        byte[] bytesCredentials = Encoding.UTF8.GetBytes(credentials);
        string base64Credentials = Convert.ToBase64String(bytesCredentials);

        authService.SetCredentials(base64Credentials);

        Assert.False(authService.IsAuthenticated());
    }


    [Fact]
    public void CheckInvalidPasswordPlainAuth() {

        Core.Services.Infrastructure.SmtpAuthenticationService authService = new Core.Services.Infrastructure.Impl.PlainSmtpAuthenticationService("abc", "xyz");

        string credentials = "abc\0wxyz";

        byte[] bytesCredentials = Encoding.UTF8.GetBytes(credentials);
        string base64Credentials = Convert.ToBase64String(bytesCredentials);

        authService.SetCredentials(base64Credentials);

        Assert.False(authService.IsAuthenticated());
    }


    [Fact]
    public void CheckInvalidUsernamePasswordPlainAuth() {

        Core.Services.Infrastructure.SmtpAuthenticationService authService = new Core.Services.Infrastructure.Impl.PlainSmtpAuthenticationService("abc", "xyz");

        string credentials = "abcd\0wxyz";

        byte[] bytesCredentials = Encoding.UTF8.GetBytes(credentials);
        string base64Credentials = Convert.ToBase64String(bytesCredentials);

        authService.SetCredentials(base64Credentials);

        Assert.False(authService.IsAuthenticated());
    }

}
