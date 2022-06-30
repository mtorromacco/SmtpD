using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmtpD.Core.Commons.Extensions;
using SmtpD.Core.Dtos.Configs.Responses;
using SmtpD.Core.Dtos.Emails.Requests;
using SmtpD.Core.Dtos.Emails.Responses;
using SmtpD.Core.Enums;
using SmtpD.Core.Services.Domain;
using SmtpD.Core.Services.Infrastructure;
using SmtpD.Core.Services.Infrastructure.Impl;
using SmtpD.Web.Hubs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SmtpD.Web.Smtp;
public class SmtpServer : BackgroundService {

    private readonly IServiceProvider serviceProvider = null;
    private readonly ILogger<SmtpServer> logger = null;


    public SmtpServer(IServiceProvider serviceProvider, ILogger<SmtpServer> logger) {
        this.serviceProvider = serviceProvider;
        this.logger = logger;
    }


    /// <summary>
    /// Job in background che crea il server SMTP
    /// </summary>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {

        await Task.Yield();

        this.logger.LogInformation("Avvio del server SMTP..");

        IConfigService configService = this.serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IConfigService>();

        ConfigDto dto = await configService.GetConfigAsync();

        if (dto.Port is < 1 or > 65535)
            throw new Exception("Porta fuori dal range 1-65535");

        if (dto.Port == 55555)
            throw new Exception("Porta 55555 utilizzata dall'applicazione");

        IPAddress host = IPAddress.Parse("127.0.0.1");
        TcpListener smtp = new(host, dto.Port);

        smtp.Start();

        this.logger.LogInformation($"Server SMTP in ascolto sulla porta {dto.Port}");
        this.logger.LogInformation($"Dashboard disponibile sulla porta 55555");

        while (!stoppingToken.IsCancellationRequested) {

            try {
                TcpClient client = await smtp.AcceptTcpClientAsync();
                _ = Task.Run(() => this.HandleClient(client, dto.Username, dto.Password));
            } catch (Exception ex) {
                this.logger.LogError(ex, ex.Message);
            }
        }
    }


    /// <summary>
    /// Gestione del client connesso
    /// </summary>
    /// <param name="client">Connessione TCP</param>
    /// <param name="username">Username per l'utenticazione presente nel DB</param>
    /// <param name="password">Password per l'autenticazione presente nel DB</param>
    private async Task HandleClient(TcpClient client, string username, string password) {

        client.NoDelay = true;

        bool authenticationRequired = !String.IsNullOrEmpty(username) && !String.IsNullOrEmpty(password);

        IEmailService emailService = this.serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IEmailService>();
        IHubContext<EmailHub> emailHub = this.serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IHubContext<EmailHub>>();

        NetworkStream stream = client.GetStream();

        await stream.WriteAsync("220 Ready");

        ConnectionStatus status = ConnectionStatus.EHLO;

        NewEmailDto newEmail = null;

        SmtpAuthenticationService smtpAuthenticationService = null;

        while (status != ConnectionStatus.CLOSE) {

            string message = await stream.ReadAsync();

            switch (status) {
                case ConnectionStatus.EHLO when message.ToLower().StartsWith("ehlo"):

                    if (authenticationRequired) {
                        status = ConnectionStatus.AUTH;
                        await stream.WriteAsync("250 Auth LOGIN PLAIN CRAM-MD5");
                    } else {
                        status = ConnectionStatus.FROM;
                        await stream.WriteAsync("250 Ok");
                    }
                    
                    break;
                case ConnectionStatus.AUTH when message.ToLower().EndsWith("login"):
                    status = ConnectionStatus.AUTH_LOGIN_USERNAME;
                    smtpAuthenticationService = new LoginSmtpAuthenticationService(username, password);
                    await stream.WriteAsync("334 VXNlcm5hbWU6");
                    break;
                case ConnectionStatus.AUTH when message.ToLower().EndsWith("plain"):
                    status = ConnectionStatus.AUTH_PLAIN_CREDENTIALS;
                    smtpAuthenticationService = new PlainSmtpAuthenticationService(username, password);
                    await stream.WriteAsync("334");
                    break;
                case ConnectionStatus.AUTH when message.ToLower().Contains("plain"):

                    string credentials = message.Split(" ").Last();

                    smtpAuthenticationService = new PlainSmtpAuthenticationService(username, password);
                    smtpAuthenticationService.SetCredentials(credentials);

                    if (smtpAuthenticationService.IsAuthenticated()) {
                        status = ConnectionStatus.FROM;
                        await stream.WriteAsync("235 Authentication successful");
                    } else {
                        status = ConnectionStatus.CLOSE;
                        await stream.WriteAsync("535 Authentication Failed");
                    }

                    break;
                case ConnectionStatus.AUTH when message.ToLower().EndsWith("cram-md5"):
                    status = ConnectionStatus.AUTH_CRAM_MD5;
                    (string challenge, string encodingChallenge) = this.GenerateChallenge();
                    smtpAuthenticationService = new CramMd5SmtpAuthenticationService(username, password, challenge);
                    await stream.WriteAsync($"334 {encodingChallenge}");
                    break;
                case ConnectionStatus.AUTH_LOGIN_USERNAME:
                    status = ConnectionStatus.AUTH_LOGIN_PASSWORD;
                    smtpAuthenticationService.SetUsername(message);
                    await stream.WriteAsync("334 UGFzc3dvcmQ6");
                    break;
                case ConnectionStatus.AUTH_LOGIN_PASSWORD:

                    smtpAuthenticationService.SetPassword(message);

                    if (smtpAuthenticationService.IsAuthenticated()) {
                        status = ConnectionStatus.FROM;
                        await stream.WriteAsync("235 Authentication successful");
                    } else {
                        status = ConnectionStatus.CLOSE;
                        await stream.WriteAsync("535 Authentication Failed");
                    }

                    break;
                case ConnectionStatus.AUTH_PLAIN_CREDENTIALS:
                case ConnectionStatus.AUTH_CRAM_MD5:

                    smtpAuthenticationService.SetCredentials(message);

                    if (smtpAuthenticationService.IsAuthenticated()) {
                        status = ConnectionStatus.FROM;
                        await stream.WriteAsync("235 Authentication successful");
                    } else {
                        status = ConnectionStatus.CLOSE;
                        await stream.WriteAsync("535 Authentication Failed");
                    }

                    break;
                
                case ConnectionStatus.FROM or ConnectionStatus.QUIT when message.ToLower().StartsWith("mail from"):

                    if (newEmail == null)
                        newEmail = new();
                    else {
                        EmailDto createdEmail = await emailService.CreateAsync(newEmail.From, newEmail.To, newEmail.Subject, newEmail.Message);
                        await emailHub.Clients.All.SendAsync("newEmail", createdEmail);
                        newEmail = new();
                    }

                    status = ConnectionStatus.TO;
                    newEmail.SetFrom(message);
                    await stream.WriteAsync("250 Ok");
                    break;
                case ConnectionStatus.TO when message.ToLower().StartsWith("rcpt to"):
                    status = ConnectionStatus.DATA;
                    newEmail.SetTo(message);
                    await stream.WriteAsync("250 Ok");
                    break;
                case ConnectionStatus.DATA when message.ToLower() == "data":
                    status = ConnectionStatus.HEADER;
                    await stream.WriteAsync("354 Send message content");
                    break;
                case ConnectionStatus.HEADER when message.ToLower().Contains("subject") && message.ToLower().Contains("date") && message.ToLower().Contains("\r\n\r\n"):
                    newEmail.SetSubjectAndMessage(message);
                    status = ConnectionStatus.QUIT;
                    await stream.WriteAsync("250 Ok");
                    break;
                case ConnectionStatus.HEADER when message.ToLower().StartsWith("subject"):
                    newEmail.SetSubject(message);
                    break;
                case ConnectionStatus.HEADER when message == "":
                    status = ConnectionStatus.BODY;
                    break;
                case ConnectionStatus.HEADER when message.ToLower().StartsWith("date"):
                case ConnectionStatus.HEADER:
                    break;
                case ConnectionStatus.BODY:
                    status = ConnectionStatus.DATA_ENDING;
                    newEmail.SetMessage(message);
                    break;
                case ConnectionStatus.DATA_ENDING:
                    status = ConnectionStatus.QUIT;
                    await stream.WriteAsync("250 Ok");
                    break;
                case ConnectionStatus.QUIT:
                    EmailDto email = await emailService.CreateAsync(newEmail.From, newEmail.To, newEmail.Subject, newEmail.Message);
                    await emailHub.Clients.All.SendAsync("newEmail", email);
                    status = ConnectionStatus.CLOSE;
                    break;
                default:
                    throw new Exception("Invalid TCP message");
            }
        }

        await stream.WriteAsync("221 Bye");
    }


    /// <summary>
    /// Generazione della challenge per l'autenticazione CRAM-MD5
    /// </summary>
    /// <returns></returns>
    private (string, string) GenerateChallenge() {
        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");
        string challenge = $"<{new Random().Next(1000, 5000)}.{timestamp}@localhost>";
        byte[] challengeBytes = Encoding.UTF8.GetBytes(challenge);
        string encodingChallenge = Convert.ToBase64String(challengeBytes);
        return (challenge, encodingChallenge);
    }

}
