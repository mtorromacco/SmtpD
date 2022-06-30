using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmtpD.Core.Dtos.Configs.Requests;
using SmtpD.Core.Dtos.Configs.Responses;
using SmtpD.Core.Services.Domain;

namespace SmtpD.Web.Controllers;

[Route("api/configs")]
[ApiController]
public class ConfigController : ControllerBase {

    private readonly IConfigService configService = null;
    private readonly ILogger<ConfigController> logger = null;


    public ConfigController(IConfigService configService, ILogger<ConfigController> logger) {
        this.configService = configService;
        this.logger = logger;
    }


    /// <summary>
    /// Ottenimento configurazione
    /// </summary>
    /// <returns>Configurazione</returns>
    [HttpGet]
    public async Task<IActionResult> GetConfigs() {

        try {
            ConfigDto config = await this.configService.GetConfigAsync();
            return Ok(config);
        } catch(Exception ex) {
            this.logger.LogError(ex, ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, String.Empty);
        }
    }


    /// <summary>
    /// Aggiornamento credenziali
    /// </summary>
    /// <param name="dto">Nuovo username e/o password</param>
    /// <returns>Configurazione aggiornata</returns>
    [HttpPut("credentials")]
    public async Task<IActionResult> UpdateCredentials([FromBody] UpdateCredentialsDto dto) {

        try {
            ConfigDto config = await this.configService.UpdateCredentialsAsync(dto.Username, dto.Password);
            return Ok(config);
        } catch (Exception ex) {
            this.logger.LogError(ex, ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, String.Empty);
        }
    }


    /// <summary>
    /// Aggiornamento porta server SMTP
    /// </summary>
    /// <param name="dto">Nuova porta</param>
    /// <returns>Configurazione aggiornata</returns>
    [HttpPut("port")]
    public async Task<IActionResult> UpdatePort([FromBody] UpdatePortDto dto) {

        if (!ModelState.IsValid) {
            List<string> errorMessages = ModelState.Values.SelectMany(value => value.Errors.Select(error => error.ErrorMessage.ToString())).ToList();
            return BadRequest(new { Message = errorMessages.First() });
        }

        try {
            ConfigDto config = await this.configService.UpdatePortAsync(dto.Port);
            return Ok(config);
        } catch (Exception ex) {
            this.logger.LogError(ex, ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, String.Empty);
        }
    }
}
