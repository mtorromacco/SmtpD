using SmtpD.Core.Entities;
using SmtpD.Core.Repositories;
using SmtpD.Infrastructure.Sqlite.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpD.Infrastructure.Sqlite.Repositories;
public class EmailRepository : BaseRepository<Email, long>, IEmailRepository {
    public EmailRepository(ApplicationDbContext context) : base(context) {
    }
}
