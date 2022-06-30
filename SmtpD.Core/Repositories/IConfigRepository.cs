using SmtpD.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpD.Core.Repositories;
public interface IConfigRepository : IBaseRepository<Config, int> {
}
