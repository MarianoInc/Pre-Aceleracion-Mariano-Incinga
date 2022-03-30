using System.Collections.Generic;

namespace AlkemyChallenge.API.Interfaces
{
    public interface IRepository<T> where T : class
    {
        public List<T> GetAllEntities();
        public T Get(int id);
        public T Post(T Entity);
        public T Update(T Entity);
        public T Delete(int? id);
    }
}
