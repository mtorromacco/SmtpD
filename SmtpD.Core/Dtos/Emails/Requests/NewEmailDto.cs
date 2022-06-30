using MimeKit.Encodings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SmtpD.Core.Dtos.Emails.Requests;
public class NewEmailDto {
    public string From { get; set; }
    public string To { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }


    public void SetFrom(string message) {
        this.From = message.Replace("MAIL FROM:", "", StringComparison.InvariantCultureIgnoreCase).Replace("<", "").Replace(">", "");
    }


    public void SetTo(string message) {
        this.To = message.Replace("RCPT TO:", "", StringComparison.InvariantCultureIgnoreCase).Replace("<", "").Replace(">", "");
    }


    public void SetSubject(string subject) {

        subject = this.HandleEncodeData(subject);
        this.Subject = subject.Replace("Subject:", "", StringComparison.InvariantCultureIgnoreCase);
    }


    public void SetMessage(string message) {

        message = this.HandleEncodeData(message);
        this.Message = message;
    }


    public void SetSubjectAndMessage(string message) {

        message = this.HandleEncodeData(message);
        string[] chunks = message.Split("\r\n").ToArray();
        string subjectChunk = chunks.First(chunk => chunk.ToLower().StartsWith("subject"));
        string bodyChunk = chunks.SkipWhile(chunk => chunk != "").Skip(1).First();
        this.Subject = subjectChunk.Replace("Subject:", "", StringComparison.InvariantCultureIgnoreCase);
        this.Message = bodyChunk;
    }


    private string HandleEncodeData(string message) {

        if (message.ToLower().Contains("content-transfer-encoding: quoted-printable"))
            message = this.QuotedPrintableDecoder(message);

        while (message.IndexOf("=?") != -1) {

            int encodingStartOccurences = message.IndexOf("=?");
            int encodingEndOccurences = message.IndexOf("?=");

            int charsToTake = (encodingEndOccurences - encodingStartOccurences) + 2;

            string encodingString = message.Substring(encodingStartOccurences, charsToTake);

            string decodedString = this.MimeDecoder(encodingString);

            string startedString = message.Substring(0, encodingStartOccurences);
            string endedString = message.Substring(encodingEndOccurences + 2);

            if (endedString.Replace(" ", "").StartsWith("\r=?") || endedString.Replace(" ", "").StartsWith("\n=?") || endedString.Replace(" ", "").StartsWith("\r\n=?") || endedString.Replace(" ", "").StartsWith("\n\r=?"))
                endedString = new string(endedString.SkipWhile(str => str != '=').ToArray());

            message = startedString + decodedString + endedString;
        }

        return message;
    }


    private string MimeDecoder(string message) {

        MatchCollection regex = Regex.Matches(message,@"(?:=\?)([^\?]+)(?:\?q\?)([^\?]*)(?:\?=)");

        string charset = regex.First().Groups[1].Value;
        bool qEncoding = regex.First().Groups[0].Value.Contains("?q?");

        string data = "";

        regex.ToList().ForEach(row => data += row.Groups[2].Value);

        if (!qEncoding) {
            byte[] bytesData = Convert.FromBase64String(data);
            data = Encoding.GetEncoding(charset).GetString(bytesData);
        }

        return data;
    }


    private string QuotedPrintableDecoder(string message) {

        QuotedPrintableDecoder decoder = new();
        byte[] buffer = Encoding.ASCII.GetBytes(message);
        byte[] output = new byte[decoder.EstimateOutputLength(buffer.Length)];
        int used = decoder.Decode(buffer, 0, buffer.Length, output);
        Encoding encoding = Encoding.GetEncoding("utf-8");
        return encoding.GetString(output, 0, used);
    }
}
