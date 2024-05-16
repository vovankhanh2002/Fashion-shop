using System.Linq.Expressions;

namespace Project.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class // T là lớp đại diện cho cái class trong Db
    {
        //Lấy all class T
        IEnumerable<T> GetAll( string? include = null);
        //Add ? kiểu T
        void Add(T entity);
        //Tìm id 
        T GetById(Expression <Func<T,bool>> exception, string? include = null);
        void Update(T entity);
        void Delete(T entity);
        IEnumerable<T> GetAllWhere(Expression<Func<T, bool>> exception, string? include = null);
        void DeleteRange(IEnumerable<T> entities);
    }
}
