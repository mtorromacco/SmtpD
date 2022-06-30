using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SmtpD.UnitTests.SmtpAuthenticationService;
public class CramMd5SmtpAuthenticationService {

    [Fact]
    public void CheckValidCramMd5Auth() {

        string challenge = Guid.NewGuid().ToString();

        Core.Services.Infrastructure.SmtpAuthenticationService authService = new Core.Services.Infrastructure.Impl.CramMd5SmtpAuthenticationService("abc", "xyz", challenge);

        using HMACMD5 hmacsha256 = new(Encoding.UTF8.GetBytes("xyz"));

        byte[] hash = hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(challenge));
        string password = Convert.ToHexString(hash).ToLower();

        string credentials = $"abc {password}";

        byte[] bytesCredentials = Encoding.UTF8.GetBytes(credentials);
        string base64Credentials = Convert.ToBase64String(bytesCredentials);

        authService.SetCredentials(base64Credentials);

        Assert.True(authService.IsAuthenticated());
    }


    [Fact]
    public void CheckInvalidUsernameCramMd5Auth() {

        string challenge = Guid.NewGuid().ToString();

        Core.Services.Infrastructure.SmtpAuthenticationService authService = new Core.Services.Infrastructure.Impl.CramMd5SmtpAuthenticationService("abc", "xyz", challenge);

        using HMACMD5 hmacsha256 = new(Encoding.UTF8.GetBytes("xyz"));

        byte[] hash = hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(challenge));
        string password = Convert.ToHexString(hash).ToLower();

        string credentials = $"abcd {password}";

        byte[] bytesCredentials = Encoding.UTF8.GetBytes(credentials);
        string base64Credentials = Convert.ToBase64String(bytesCredentials);

        authService.SetCredentials(base64Credentials);

        Assert.False(authService.IsAuthenticated());
    }


    [Fact]
    public void CheckInvalidPasswordCramMd5Auth() {

        string challenge = Guid.NewGuid().ToString();

        Core.Services.Infrastructure.SmtpAuthenticationService authService = new Core.Services.Infrastructure.Impl.CramMd5SmtpAuthenticationService("abc", "xyz", challenge);

        using HMACMD5 hmacsha256 = new(Encoding.UTF8.GetBytes("wxyz"));

        byte[] hash = hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(challenge));
        string password = Convert.ToHexString(hash).ToLower();

        string credentials = $"abc {password}";

        byte[] bytesCredentials = Encoding.UTF8.GetBytes(credentials);
        string base64Credentials = Convert.ToBase64String(bytesCredentials);

        authService.SetCredentials(base64Credentials);

        Assert.False(authService.IsAuthenticated());
    }


    [Fact]
    public void CheckInvalidUsernamePasswordCramMd5Auth() {

        string challenge = Guid.NewGuid().ToString();

        Core.Services.Infrastructure.SmtpAuthenticationService authService = new Core.Services.Infrastructure.Impl.CramMd5SmtpAuthenticationService("abc", "xyz", challenge);

        using HMACMD5 hmacsha256 = new(Encoding.UTF8.GetBytes("wxyz"));

        byte[] hash = hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(challenge));
        string password = Convert.ToHexString(hash).ToLower();

        string credentials = $"abcd {password}";

        byte[] bytesCredentials = Encoding.UTF8.GetBytes(credentials);
        string base64Credentials = Convert.ToBase64String(bytesCredentials);

        authService.SetCredentials(base64Credentials);

        Assert.False(authService.IsAuthenticated());
    }

}
