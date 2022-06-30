using Microsoft.EntityFrameworkCore;
using SmtpD.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpD.Infrastructure.Sqlite.Contexts;
public class ApplicationDbContext : DbContext {

    public virtual DbSet<Email> Emails { get; set; }
    public virtual DbSet<Config> Configs { get; set; }


    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {
    }

}
