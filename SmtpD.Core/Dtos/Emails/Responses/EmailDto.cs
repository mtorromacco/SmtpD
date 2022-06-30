using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpD.Core.Dtos.Emails.Responses;
public record EmailDto(long Id, string From, string To, string Subject, string Message, string CreatedAt, bool Readed);
