using Moq;
using SmtpD.Core.Entities;
using SmtpD.Core.Repositories;
using SmtpD.Core.Services.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpD.UnitTests.EmailService;
public class DeleteAllAsync {

    [Fact]
    public async Task NoEmails() {

        Mock<IEmailRepository> mockEmailRepository = new();

        mockEmailRepository.Setup(instance => instance.GetAllAsync()).ReturnsAsync(new List<Email>());

        IEmailService emailService = new Core.Services.Domain.Impl.EmailService(mockEmailRepository.Object);
        await emailService.DeleteAllAsync();

        mockEmailRepository.Verify(instance => instance.Delete(It.IsAny<Email>()), Times.Never());
    }


    [Fact]
    public async Task DeletedAllCorrectly() {

        List<Email> emailsFromDb = new List<Email>() {
            new() {
                Id = 1,
                From = "abc@example.com",
                To = "xyz@example.com",
                CreatedAt = DateTime.Now.AddDays(-2),
                Readed = true,
                Subject = "Subject test 1",
                Message = "Message test 1"
            },
            new() {
                Id = 2,
                From = "xyz@example.com",
                To = "abc@example.com",
                CreatedAt = DateTime.Now.AddDays(-1),
                Readed = false,
                Subject = "Subject test 2",
                Message = "Message test 2"
            }
        };

        Mock<IEmailRepository> mockEmailRepository = new();

        mockEmailRepository.Setup(instance => instance.GetAllAsync()).ReturnsAsync(emailsFromDb);

        IEmailService emailService = new Core.Services.Domain.Impl.EmailService(mockEmailRepository.Object);
        await emailService.DeleteAllAsync();

        mockEmailRepository.Verify(instance => instance.Delete(It.IsAny<Email>()), Times.Exactly(2));
    }
}
