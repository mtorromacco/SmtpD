using SmtpD.Core.Entities;
using SmtpD.Core.Repositories;
using SmtpD.Infrastructure.Sqlite.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpD.Infrastructure.Sqlite.Repositories;
public class ConfigRepository : BaseRepository<Config, int>, IConfigRepository {
    public ConfigRepository(ApplicationDbContext context) : base(context) {
    }
}
