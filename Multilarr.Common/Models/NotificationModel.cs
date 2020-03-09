using SQLite;
using System;

namespace Multilarr.Common.Models
{
	public class NotificationModel
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		public string LogTitle { get; set; }
		public string LogMessage { get; set; }
        public DateTime LogDate { get; set; }
	}
}

