using Microsoft.EntityFrameworkCore;
using SmtpD.Core.Repositories;
using SmtpD.Infrastructure.Sqlite.Contexts;
using System.Linq.Expressions;

namespace SmtpD.Infrastructure.Sqlite.Repositories;
public class BaseRepository<TEntity, TId> : IBaseRepository<TEntity, TId> where TEntity : class {

    protected ApplicationDbContext context = null;
    private readonly DbSet<TEntity> table = null;


    public BaseRepository(ApplicationDbContext context) {
        this.context = context;
        this.table = context.Set<TEntity>();
    }


    public void Create(TEntity entity) {
        this.table.Add(entity);
    }


    public void Delete(TEntity entity) {
        this.table.Remove(entity);
    }


    public async Task<TEntity> FindByIdAsync(TId id) {
        return await this.table.FindAsync(id);
    }


    public async Task<List<TEntity>> GetAllAsync() {
        return await this.table.ToListAsync();
    }


    public async Task<TEntity> QueryFirstAsync(Expression<Func<TEntity, bool>> where) {
        return await this.table.Where(where).FirstOrDefaultAsync();
    }


    public async Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> where) {
        return await this.table.Where(where).ToListAsync();
    }


    public async Task SaveChangesAsync() {
        await this.context.SaveChangesAsync();
    }


    public void Update(TEntity entity) {
        this.table.Attach(entity);
        this.context.Entry(entity).State = EntityState.Modified;
    }
}

