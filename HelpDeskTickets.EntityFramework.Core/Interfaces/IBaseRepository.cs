using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskTickets.EntityFramework.Core.Interfaces
{
	public interface IBaseRepository<T> where T : class
	{
		IEnumerable<T> All();

		Task<IEnumerable<T>> AllAsync();

		Task<T> FindAsync(dynamic id);

		IQueryable<T> Where(Expression<Func<T, bool>> predicate);

		Task<IEnumerable<T>> WhereAsync(Expression<Func<T, bool>> predicate);

		T Add(T item);

		IEnumerable<T> AddAll(IEnumerable<T> items);

		void Update(T item);

		Task<long> CountAsync();

		Task<long> CountAsync(Expression<Func<T, bool>> predicate);

		Task<int> SaveAsync();

		IEnumerable<T> Paginate<TKey>(int pageSize, int pageNumber, Func<T, TKey> keySelector);

		void DeleteAll(ISet<T> items);

		void Delete(T item);
	}
}
