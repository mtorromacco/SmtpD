using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpD.Core.Dtos.Configs.Requests;
public record UpdateCredentialsDto(string Username, string Password);