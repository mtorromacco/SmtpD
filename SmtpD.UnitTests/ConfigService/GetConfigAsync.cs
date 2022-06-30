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
public class GetConfigAsync {

    [Fact]
    public async Task ConfigNotExists() {

        Mock<IConfigRepository> mockConfigRepository = new();

        mockConfigRepository.Setup(instance => instance.GetAllAsync()).ReturnsAsync(new List<Config>());

        IConfigService configService = new Core.Services.Domain.Impl.ConfigService(mockConfigRepository.Object);
        Exception ex = await Assert.ThrowsAsync<Exception>(() => configService.GetConfigAsync());

        Assert.Equal("Configurazione non trovata", ex.Message);
    }


    [Fact]
    public async Task ConfigExists() {

        Mock<IConfigRepository> mockConfigRepository = new();

        mockConfigRepository.Setup(instance => instance.GetAllAsync()).ReturnsAsync(new List<Config>() { new() { Id = 1, Username = String.Empty, Password = String.Empty, Port = 25 } });

        IConfigService configService = new Core.Services.Domain.Impl.ConfigService(mockConfigRepository.Object);
        ConfigDto dto = await configService.GetConfigAsync();

        Assert.NotNull(dto);
        Assert.Equal(25, dto.Port);
        Assert.Equal("localhost", dto.Host);
        Assert.Equal(String.Empty, dto.Username);
        Assert.Equal(String.Empty, dto.Password);
    }
}
