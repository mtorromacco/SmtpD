using SmtpD.Core.Dtos.Configs.Responses;
using SmtpD.Core.Entities;
using SmtpD.Core.Repositories;
using SmtpD.Core.Services.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpD.Core.Services.Domain.Impl;
public class ConfigService : IConfigService {

    private readonly IConfigRepository configRepository = null;


    public ConfigService(IConfigRepository configRepository) {
        this.configRepository = configRepository;
    }


    public async Task InitializeAsync() {

        List<Config> configs = await this.configRepository.GetAllAsync();

        if (configs is not null and { Count: > 0 })
            return;

        Config config = new() {
            Port = 25
        };

        this.configRepository.Create(config);
        await this.configRepository.SaveChangesAsync();
    }


    public async Task<ConfigDto> GetConfigAsync() {

        List<Config> configs = await this.configRepository.GetAllAsync();

        if (configs is null or { Count: 0 })
            throw new Exception("Configurazione non trovata");

        Config config = configs.First();

        ConfigDto dto = new(config.Username ?? String.Empty, config.Password ?? String.Empty, "localhost", Convert.ToInt32(config.Port));
        return dto;
    }


    public async Task<ConfigDto> UpdateCredentialsAsync(string username, string password) {

        List<Config> configs = await this.configRepository.GetAllAsync();

        if (configs is null or { Count: 0 })
            throw new Exception("Configurazione non trovata");

        Config config = configs.First();

        config.Username = username?.Trim();
        config.Password = password?.Trim();

        this.configRepository.Update(config);
        await this.configRepository.SaveChangesAsync();

        ConfigDto dto = new(config.Username ?? String.Empty, config.Password ?? String.Empty, "localhost", Convert.ToInt32(config.Port));
        return dto;
    }


    public async Task<ConfigDto> UpdatePortAsync(int port) {

        List<Config> configs = await this.configRepository.GetAllAsync();

        if (configs is null or { Count: 0 })
            throw new Exception("Configurazione non trovata");

        Config config = configs.First();

        config.Port = port;

        this.configRepository.Update(config);
        await this.configRepository.SaveChangesAsync();

        ConfigDto dto = new(config.Username ?? String.Empty, config.Password ?? String.Empty, "localhost", Convert.ToInt32(config.Port));
        return dto;
    }
}
