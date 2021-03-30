using Lpp.Dns.DTO.Security;
using Lpp.Utilities.Security;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Lpp.Dns.Data.Query
{

    public class GetDataMartsQuery : Query<DataContext, ApiIdentity, IQueryable<DataMart>>
	{
		public GetDataMartsQuery(DataContext db) : base(db) { }

		public override IQueryable<DataMart> Execute(ApiIdentity param)
		{
            return _db.Secure<DataMart>(param, PermissionIdentifiers.DataMartInProject.SeeRequests);
		}

        public override Task<IQueryable<DataMart>> ExecuteAsync(ApiIdentity param)
        {
            throw new NotImplementedException();
        }
    }
}
