using Microsoft.EntityFrameworkCore;
using TutorialProjectAPI.Contexts;

namespace TutorialProjectAPI.Repositories
{
    public class GenericRepository<T> where T : class
    {
        protected readonly MainContext _context;
        protected readonly DbSet<T> entities;
        public GenericRepository(MainContext context)
        {
            _context = context;
            entities = context.Set<T>();
        }

        public IQueryable<T> GetAll()
        {
            return entities.AsQueryable();
        }

        public void Insert(T entity)
        {
            if (entity == null) throw new ArgumentNullException("Entity does not exist.");

            entities.Add(entity);
        }

        public void Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException("Entity does not exist.");

            entities.Update(entity);
        }

        public void Delete(T entity)
        {
            if (entity == null) throw new ArgumentNullException("Entity does not exist.");

            entities.Remove(entity);
        }

        public IQueryable<T> GetAllAsReadOnly()
        {
            return entities.AsNoTracking();
        }

        public void AddRange(IEnumerable<T> entity)
        {
            entities.AddRange(entity);
        }

        public void DeleteRange(IEnumerable<T> entity)
        {
            entities.RemoveRange(entity);
        }

        public void UpdateRange(IEnumerable<T> entity)
        {
            entities.UpdateRange(entity);
        }
    }

}
