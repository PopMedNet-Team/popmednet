using System.Data.Entity;
using System.Threading.Tasks;

namespace Lpp.Dns.Data.Query
{
	public abstract class Query<TDataContext, TExecuteParam, TReturnType>
		where TDataContext : DbContext
		where TExecuteParam : class
	{
		protected readonly TDataContext _db;
		public Query(TDataContext db){
			_db = db;
		}

		public abstract TReturnType Execute(TExecuteParam param);
		public abstract Task<TReturnType> ExecuteAsync(TExecuteParam param);
	}
}
