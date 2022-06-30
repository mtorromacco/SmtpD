using Moq;
using SmtpD.Core.Entities;
using SmtpD.Core.Repositories;
using SmtpD.Core.Services.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpD.UnitTests.ConfigService;
public class InitializeAsync {

    [Fact]
    public async Task ConfigNotFound() {

        Mock<IConfigRepository> mockConfigRepository = new();

        mockConfigRepository.Setup(instance => instance.GetAllAsync()).ReturnsAsync(new List<Config>());

        Config newConfigInstance = null;
        mockConfigRepository.Setup(instance => instance.Create(It.IsAny<Config>()))
                            .Callback<Config>(config => newConfigInstance = config);

        IConfigService configService = new Core.Services.Domain.Impl.ConfigService(mockConfigRepository.Object);
        await configService.InitializeAsync();

        mockConfigRepository.Verify(instance => instance.Create(It.IsAny<Config>()), Times.Once());
        Assert.NotNull(newConfigInstance);
        Assert.Equal(25, newConfigInstance.Port);
    }


    [Fact]
    public async Task AppConfigured() {

        Mock<IConfigRepository> mockConfigRepository = new();

        mockConfigRepository.Setup(instance => instance.GetAllAsync()).ReturnsAsync(new List<Config>() { new() { Id = 1, Username = String.Empty, Password = String.Empty, Port = 25 } });

        IConfigService configService = new Core.Services.Domain.Impl.ConfigService(mockConfigRepository.Object);
        await configService.InitializeAsync();

        mockConfigRepository.Verify(instance => instance.Create(It.IsAny<Config>()), Times.Never());
    }
}
