using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpD.Core.Dtos.Configs.Responses;
public record ConfigDto(string Username, string Password, string Host, int Port);
