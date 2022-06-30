using System.Linq.Expressions;

namespace SmtpD.Core.Repositories;
public interface IBaseRepository<TEntity, TId> {

    /// <summary>
    /// Ottenimento di tutte le entities
    /// </summary>
    /// <returns>Lista delle entities</returns>
    Task<List<TEntity>> GetAllAsync();

    /// <summary>
    /// Ottenimento entity tramite ID
    /// </summary>
    /// <param name="id">ID da ricercare nel DB</param>
    /// <returns>Entity con l'ID specificato</returns>
    Task<TEntity> FindByIdAsync(TId id);

    /// <summary>
    /// Creazione dell'entity del DB
    /// </summary>
    /// <param name="entity">Entity da creare</param>
    void Create(TEntity entity);

    /// <summary>
    /// Eliminazione di un'entity dal DB
    /// </summary>
    /// <param name="entity">Entity da eliminare</param>
    void Delete(TEntity entity);

    /// <summary>
    /// Aggiornamento di un'entity nel DB
    /// </summary>
    /// <param name="entity">Entity da aggiornare</param>
    void Update(TEntity entity);

    /// <summary>
    /// Salvataggio di tutte le modifiche apportate al DB
    /// </summary>
    Task SaveChangesAsync();

    /// <summary>
    /// Esecuzione di una query custom che ritorna il primo risultato utile
    /// </summary>
    /// <param name="where">Condizione</param>
    /// <returns>Prima entity che rispetta la condizione</returns>
    Task<TEntity> QueryFirstAsync(Expression<Func<TEntity, bool>> where);

    /// <summary>
    /// Esecuzione di una query custom che ritorna il primo risultato utile
    /// </summary>
    /// <param name="where">Condizione</param>
    /// <returns>Prima entity che rispetta la condizione</returns>
    Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> where);
}

