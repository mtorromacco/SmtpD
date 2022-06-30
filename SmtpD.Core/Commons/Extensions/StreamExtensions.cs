using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SmtpD.Core.Commons.Extensions;
public static class StreamExtensions {

    /// <summary>
    /// Lettura stream
    /// </summary>
    /// <param name="stream">Stream da cui leggere</param>
    /// <returns>Contenuto letto dallo stream</returns>
    public static async Task<string> ReadAsync(this NetworkStream stream) {

        byte[] buffer = new byte[8192];
        StringBuilder messageStringBuilder = new();

        do {
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            messageStringBuilder.AppendFormat("{0}", Encoding.ASCII.GetString(buffer, 0, bytesRead));
        } while (stream.DataAvailable);

        string message = messageStringBuilder.ToString();

        if (message.EndsWith("\r\n"))
            message = message.Remove(message.LastIndexOf("\r\n"));

        return message;
    }


    /// <summary>
    /// Scrittura sullo stream
    /// </summary>
    /// <param name="stream">Stream sul quale scrivere</param>
    /// <param name="message">Messaggio da scrivere</param>
    public static async Task WriteAsync(this NetworkStream stream, string message) {

        message += "\r\n";
        byte[] messageByte = Encoding.UTF8.GetBytes(message);
        await stream.WriteAsync(messageByte, 0, messageByte.Length);
    }
}
