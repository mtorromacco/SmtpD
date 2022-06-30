using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpD.Core.Dtos.Configs.Requests;
public class UpdatePortDto {

    [Range(1, 65535, ErrorMessage = "Port out of range 1-65535")]
    public int Port { get; set; }
}