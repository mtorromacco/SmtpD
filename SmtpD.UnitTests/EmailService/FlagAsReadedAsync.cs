using Moq;
using SmtpD.Core.Commons.Exceptions;
using SmtpD.Core.Entities;
using SmtpD.Core.Repositories;
using SmtpD.Core.Services.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpD.UnitTests.EmailService;
public class FlagAsReadedAsync {

    [Fact]
    public async Task EmailNotFound() {

        Mock<IEmailRepository> mockEmailRepository = new();

        mockEmailRepository.Setup(instance => instance.FindByIdAsync(It.IsAny<long>())).Returns(Task.FromResult<Email>(null));

        IEmailService emailService = new Core.Services.Domain.Impl.EmailService(mockEmailRepository.Object);
        NotFoundException ex = await Assert.ThrowsAsync<NotFoundException>(() => emailService.FlagAsReadedAsync(1));

        Assert.Equal("Email non trovata con l'ID '1'", ex.Message);
    }


    [Fact]
    public async Task FlaggedAsReadedCorrectly() {

        Mock<IEmailRepository> mockEmailRepository = new();

        mockEmailRepository.Setup(instance => instance.FindByIdAsync(It.IsAny<long>())).ReturnsAsync(new Email() { Id = 1, From = "abc@example.com", To = "xyz@example.com", CreatedAt = DateTime.Now.AddDays(-2), Readed = false, Subject = "Subject test 1", Message = "Message test 1" });

        Email flaggedEmailInstance = null;
        mockEmailRepository.Setup(instance => instance.Update(It.IsAny<Email>()))
                            .Callback<Email>(email => flaggedEmailInstance = email);

        IEmailService emailService = new Core.Services.Domain.Impl.EmailService(mockEmailRepository.Object);
        await emailService.FlagAsReadedAsync(1);

        mockEmailRepository.Verify(instance => instance.Update(It.IsAny<Email>()), Times.Once());
        Assert.NotNull(flaggedEmailInstance);
        Assert.True(flaggedEmailInstance.Readed);
    }
}
