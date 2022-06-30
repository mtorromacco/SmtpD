using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpD.UnitTests.SmtpAuthenticationService;
public class LoginSmtpAuthenticationService {

    [Fact]
    public void CheckValidLoginAuth() {

        Core.Services.Infrastructure.SmtpAuthenticationService authService = new Core.Services.Infrastructure.Impl.LoginSmtpAuthenticationService("abc", "xyz");

        byte[] bytesUsername = Encoding.UTF8.GetBytes("abc");
        string base64Username = Convert.ToBase64String(bytesUsername);

        authService.SetUsername(base64Username);

        byte[] bytesPassword = Encoding.UTF8.GetBytes("xyz");
        string base64Password = Convert.ToBase64String(bytesPassword);

        authService.SetPassword(base64Password);

        Assert.True(authService.IsAuthenticated());
    }


    [Fact]
    public void CheckInvalidUsernameLoginAuth() {

        Core.Services.Infrastructure.SmtpAuthenticationService authService = new Core.Services.Infrastructure.Impl.LoginSmtpAuthenticationService("abc", "xyz");

        byte[] bytesUsername = Encoding.UTF8.GetBytes("abcd");
        string base64Username = Convert.ToBase64String(bytesUsername);

        authService.SetUsername(base64Username);

        byte[] bytesPassword = Encoding.UTF8.GetBytes("xyz");
        string base64Password = Convert.ToBase64String(bytesPassword);

        authService.SetPassword(base64Password);

        Assert.False(authService.IsAuthenticated());
    }


    [Fact]
    public void CheckInvalidPasswordLoginAuth() {

        Core.Services.Infrastructure.SmtpAuthenticationService authService = new Core.Services.Infrastructure.Impl.LoginSmtpAuthenticationService("abc", "xyz");

        byte[] bytesUsername = Encoding.UTF8.GetBytes("abc");
        string base64Username = Convert.ToBase64String(bytesUsername);

        authService.SetUsername(base64Username);

        byte[] bytesPassword = Encoding.UTF8.GetBytes("wxyz");
        string base64Password = Convert.ToBase64String(bytesPassword);

        authService.SetPassword(base64Password);

        Assert.False(authService.IsAuthenticated());
    }


    [Fact]
    public void CheckInvalidUsernamePasswordLoginAuth() {

        Core.Services.Infrastructure.SmtpAuthenticationService authService = new Core.Services.Infrastructure.Impl.LoginSmtpAuthenticationService("abc", "xyz");

        byte[] bytesUsername = Encoding.UTF8.GetBytes("abcd");
        string base64Username = Convert.ToBase64String(bytesUsername);

        authService.SetUsername(base64Username);

        byte[] bytesPassword = Encoding.UTF8.GetBytes("wxyz");
        string base64Password = Convert.ToBase64String(bytesPassword);

        authService.SetPassword(base64Password);

        Assert.False(authService.IsAuthenticated());
    }

}
