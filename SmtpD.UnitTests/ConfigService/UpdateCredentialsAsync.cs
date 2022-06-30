using Moq;
using SmtpD.Core.Dtos.Configs.Responses;
using SmtpD.Core.Entities;
using SmtpD.Core.Repositories;
using SmtpD.Core.Services.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpD.UnitTests.ConfigService;
public class UpdateCredentialsAsync {

    [Fact]
    public async Task ConfigNotFound() {

        Mock<IConfigRepository> mockConfigRepository = new();

        mockConfigRepository.Setup(instance => instance.GetAllAsync()).ReturnsAsync(new List<Config>());

        IConfigService configService = new Core.Services.Domain.Impl.ConfigService(mockConfigRepository.Object);
        Exception ex = await Assert.ThrowsAsync<Exception>(() => configService.UpdateCredentialsAsync("abc", "xyz"));

        Assert.Equal("Configurazione non trovata", ex.Message);
    }


    [Fact]
    public async Task UpdateCorrectly() {

        Mock<IConfigRepository> mockConfigRepository = new();

        mockConfigRepository.Setup(instance => instance.GetAllAsync()).ReturnsAsync(new List<Config>() { new() { Id = 1, Username = String.Empty, Password = String.Empty, Port = 25 } });

        IConfigService configService = new Core.Services.Domain.Impl.ConfigService(mockConfigRepository.Object);
        ConfigDto dto = await configService.UpdateCredentialsAsync("abc", "xyz");

        Assert.NotNull(dto);
        Assert.Equal("abc", dto.Username);
        Assert.Equal("xyz", dto.Password);
    }


    [Fact]
    public async Task UpdateWithNullValues() {

        Mock<IConfigRepository> mockConfigRepository = new();

        mockConfigRepository.Setup(instance => instance.GetAllAsync()).ReturnsAsync(new List<Config>() { new() { Id = 1, Username = String.Empty, Password = String.Empty, Port = 25 } });

        IConfigService configService = new Core.Services.Domain.Impl.ConfigService(mockConfigRepository.Object);
        ConfigDto dto = await configService.UpdateCredentialsAsync(null, null);

        Assert.NotNull(dto);
        Assert.Equal(dto.Username, dto.Username);
        Assert.Equal(dto.Username, dto.Password);
    }

}
