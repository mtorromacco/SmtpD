using Moq;
using SmtpD.Core.Dtos.Emails.Responses;
using SmtpD.Core.Repositories;
using SmtpD.Core.Services.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpD.UnitTests.EmailService;
public class CreateAsync {

    [Fact]
    public async Task CreateCorrectly() {

        Mock<IEmailRepository> mockEmailRepository = new();

        IEmailService emailService = new Core.Services.Domain.Impl.EmailService(mockEmailRepository.Object);

        EmailDto newEmail = await emailService.CreateAsync("abc@example.com", "xyz@example.com", "Subject test", "Message test");

        Assert.NotNull(newEmail);
        Assert.Equal("abc@example.com", newEmail.From);
        Assert.Equal("xyz@example.com", newEmail.To);
        Assert.Equal("Subject test", newEmail.Subject);
        Assert.Equal("Message test", newEmail.Message);
    }

}
