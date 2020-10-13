using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Lpp.Dns.Data.Query
{
	public class LastUsedPasswordCheckParams
	{
		public User User { get; set; }
		public int NumberOfEntries { get; set; }
		public DateTimeOffset DateRange { get; set; }
		public string Hash { get; set; }
	}

	public class LastUsedPasswordCheckQuery : Query<DataContext, LastUsedPasswordCheckParams, bool>
	{
		public LastUsedPasswordCheckQuery(DataContext db) : base(db){}

		public override bool Execute(LastUsedPasswordCheckParams param)
		{
			return (from logs in _db.LogsUserPasswordChange
					let userID = param.User.ID
					let hash = param.Hash
					let anyOverallPrevious = _db.LogsUserPasswordChange.Where(x => x.UserChangedID == userID).OrderBy(x => x.TimeStamp).Take(param.NumberOfEntries)
					let timePreviousUsage = _db.LogsUserPasswordChange.Where(x => x.UserChangedID == userID && x.TimeStamp > param.DateRange)
					where anyOverallPrevious.Any(x => x.OriginalPassword == hash) || timePreviousUsage.Any(x => x.OriginalPassword == hash)
					select logs.TimeStamp).Any();
		}

		public override async Task<bool> ExecuteAsync(LastUsedPasswordCheckParams param)
		{
			return await (from logs in _db.LogsUserPasswordChange
						 let userID = param.User.ID
						 let hash = param.Hash
						 let anyOverallPrevious = _db.LogsUserPasswordChange.Where(x => x.UserChangedID == userID).OrderBy(x => x.TimeStamp).Take(param.NumberOfEntries)
						 let timePreviousUsage = _db.LogsUserPasswordChange.Where(x => x.UserChangedID == userID && x.TimeStamp > param.DateRange)
						 where anyOverallPrevious.Any(x => x.OriginalPassword == hash) || timePreviousUsage.Any(x => x.OriginalPassword == hash)
						 select logs.TimeStamp).AnyAsync();
		}
	}
}
