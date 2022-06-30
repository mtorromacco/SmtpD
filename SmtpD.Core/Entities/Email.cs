using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpD.Core.Entities;

[Table("emails")]
public class Email {

    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]
    [Column("from")]
    public string From { get; set; }

    [Required]
    [Column("to")]
    public string To { get; set; }

    [Required]
    [Column("subject")]
    public string Subject { get; set; }

    [Required]
    [Column("message")]
    public string Message { get; set; }

    [Required]
    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("readed")]
    public bool Readed { get; set; }
}
