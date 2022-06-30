using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmtpD.Core.Commons.Exceptions;
using SmtpD.Core.Dtos.Emails.Responses;
using SmtpD.Core.Dtos.Generic;
using SmtpD.Core.Services.Domain;

namespace SmtpD.Web.Controllers;

[Route("api/emails")]
[ApiController]
public class EmailController : ControllerBase {

    private readonly IEmailService emailService = null;
    private readonly ILogger<EmailController> logger = null;


    public EmailController(IEmailService emailService, ILogger<EmailController> logger) {
        this.emailService = emailService;
        this.logger = logger;
    }


    /// <summary>
    /// Ottenimento di tutte le emails disponibili
    /// </summary>
    /// <returns>Lista di emails</returns>
    [HttpGet]
    public async Task<IActionResult> GetAll() {

        try {
            List<EmailDto> emails = await this.emailService.GetAllAsync();
            return Ok(emails);
        } catch(Exception ex) {
            this.logger.LogError(ex, ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, String.Empty);
        }
    }


    /// <summary>
    /// Flag email come letta
    /// </summary>
    /// <param name="id">ID dell'email</param>
    [HttpPut("{id}")]
    public async Task<IActionResult> FlagAsReaded([FromRoute(Name = "id")] long id) {

        try {
            await this.emailService.FlagAsReadedAsync(id);
            return Ok();
        } catch (NotFoundException ex) {
            return BadRequest(new ErrorDto(ex.Message));
        } catch (Exception ex) {
            this.logger.LogError(ex, ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, String.Empty);
        }
    }


    /// <summary>
    /// Eliminazione email tramite ID
    /// </summary>
    /// <param name="id">ID dell'email da eliminare</param>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteById([FromRoute(Name = "id")] long id) {

        try {
            await this.emailService.DeleteByIdAsync(id);
            return Ok();
        } catch (NotFoundException ex) {
            return BadRequest(new ErrorDto(ex.Message));
        } catch (Exception ex) {
            this.logger.LogError(ex, ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, String.Empty);
        }
    }


    /// <summary>
    /// Eliminazione di tutte le emails
    /// </summary>
    [HttpDelete]
    public async Task<IActionResult> DeleteAll() {

        try {
            await this.emailService.DeleteAllAsync();
            return Ok();
        } catch (Exception ex) {
            this.logger.LogError(ex, ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, String.Empty);
        }
    }

}
