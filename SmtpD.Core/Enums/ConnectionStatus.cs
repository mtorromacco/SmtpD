using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpD.Core.Enums;
public enum ConnectionStatus {
    EHLO,
    AUTH,
    AUTH_LOGIN_USERNAME,
    AUTH_LOGIN_PASSWORD,
    AUTH_PLAIN,
    AUTH_PLAIN_CREDENTIALS,
    AUTH_CRAM_MD5,
    FROM,
    TO,
    DATA,
    HEADER,
    BODY,
    DATA_ENDING,
    QUIT,
    CLOSE
}
