using SmtpD.Core.Commons.Exceptions;
using SmtpD.Core.Dtos.Emails.Requests;
using SmtpD.Core.Dtos.Emails.Responses;
using SmtpD.Core.Entities;
using SmtpD.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpD.Core.Services.Domain.Impl;
public class EmailService : IEmailService {

    private readonly IEmailRepository emailRepository = null;


    public EmailService(IEmailRepository emailRepository) {
        this.emailRepository = emailRepository;
    }


    public async Task<List<EmailDto>> GetAllAsync() {

        List<Email> emails = await this.emailRepository.GetAllAsync();

        if (emails is null)
            emails = new();

        List<EmailDto> dtos = emails.OrderByDescending(email => email.CreatedAt).Select(email => new EmailDto(email.Id, email.From, email.To, email.Subject, email.Message, ((DateTime)email.CreatedAt).ToString("dd/MM/yyyy HH:mm"), email.Readed)).ToList();

        return dtos;
    }


    public async Task DeleteByIdAsync(long id) {

        Email email = await this.emailRepository.FindByIdAsync(id);

        if (email == null)
            throw new NotFoundException($"Email non trovata con l'ID '{id}'");

        this.emailRepository.Delete(email);
        await this.emailRepository.SaveChangesAsync();
    }


    public async Task FlagAsReadedAsync(long id) {

        Email email = await this.emailRepository.FindByIdAsync(id);

        if (email == null)
            throw new NotFoundException($"Email non trovata con l'ID '{id}'");

        email.Readed = true;

        this.emailRepository.Update(email);
        await this.emailRepository.SaveChangesAsync();
    }


    public async Task DeleteAllAsync() {

        List<Email> emails = await this.emailRepository.GetAllAsync();

        if (emails is null or { Count: 0 })
            return;

        emails.ForEach(email => this.emailRepository.Delete(email));
        await this.emailRepository.SaveChangesAsync();
    }


    public async Task<EmailDto> CreateAsync(string from, string to, string subject, string message) {

        Email email = new() {
            From = from,
            To = to,
            Subject = subject,
            Message = message,
            CreatedAt = DateTime.Now,
            Readed = false
        };

        this.emailRepository.Create(email);
        await this.emailRepository.SaveChangesAsync();

        EmailDto dto = new(email.Id, email.From, email.To, email.Subject, email.Message, ((DateTime)email.CreatedAt).ToString("dd/MM/yyyy HH:mm"), email.Readed);
        return dto;
    }
}
