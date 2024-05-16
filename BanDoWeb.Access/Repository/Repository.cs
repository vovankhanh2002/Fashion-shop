using Microsoft.EntityFrameworkCore;
using Project.DataAccess.Repository.IRepository;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using BanDoWeb.Access.Dbcontext;

namespace Project.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbcontextBanDo _dbContext;
        internal DbSet<T> _dbSet { get; set; }
        public Repository(DbcontextBanDo dbContext) {

            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }
        public void Add(T entity)
        {
           _dbSet.Add(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public IEnumerable<T> GetAll(string? include = null)
        {
            IQueryable<T> query = _dbSet;
            if (include != null) {
                foreach (var item in include.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)) {
                    query = query.Include(item);
                }
                
            }
            return query.ToList();
        }

        public T GetById(Expression<Func<T, bool>> exception, string? include = null)
        {
            IQueryable<T> query = _dbSet;
            if (include != null)
            {
                foreach (var item in include.Split(new char[','], StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }

            } 
            query = query.Where(exception);
            return query.SingleOrDefault();
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public IEnumerable<T> GetAllWhere(Expression<Func<T, bool>> exception, string? include = null)
        {
            IQueryable<T> query = _dbSet;
            query = query.Where(exception);
            if (include != null)
            {
                foreach (var item in include.Split(new char[','], StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }

            }
            return query.ToList();
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _dbContext.RemoveRange(entities);
        }
    }
}
