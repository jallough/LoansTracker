using Domain.Entities;
using Microsoft.Extensions.Logging;
using Persistence.DataContext;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Persistence.SQL
{
    public abstract class SQLRepository<T>(LoansDbContext dbContext, ILogger<SQLRepository<T>> logger) : IRepository<T> where T : BaseEntity
    {
        public async Task AddAsync(T entity)
        {
            try {
                dbContext.ChangeTracker.Clear();
                dbContext.Set<T>().Add(entity);
                await dbContext.SaveChangesAsync();
                logger.LogInformation("Added entity of type {EntityType} with ID {EntityId}", typeof(T).Name, entity.Id);
            } 
            catch (Exception ex)
            {
                logger.LogError(ex, "Error adding entity of type {EntityType}", typeof(T).Name);
                logger.LogError(ex, ex.Message);
                throw;
            }
            
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbContext.Set<T>().ToListAsync();      
        }

        public Task<T?> GetByIdAsync(long id)
        {
            return dbContext.Set<T>().FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task RemoveAsync(long id)
        {
            try { 
                var entity = await dbContext.Set<T>().FirstOrDefaultAsync(e => e.Id == id);
                if (entity == null)
                {
                    logger.LogWarning("Entity of type {EntityType} with ID {EntityId} not found for removal", typeof(T).Name, id);
                    return;
                }
                dbContext.Set<T>().Remove(entity);
                await dbContext.SaveChangesAsync();
                logger.LogInformation("Removed entity of type {EntityType} with ID {EntityId}", typeof(T).Name, id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error removing entity of type {EntityType} with ID {EntityId}", typeof(T).Name, id);
                logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateAsync(T entity)
        {
            try
            {
                dbContext.Set<T>().Update(entity);
                await dbContext.SaveChangesAsync();
                logger.LogInformation("Updated entity of type {EntityType} with ID {EntityId}", typeof(T).Name, entity.Id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error updating entity of type {EntityType} with ID {EntityId}", typeof(T).Name, entity.Id);
                logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
