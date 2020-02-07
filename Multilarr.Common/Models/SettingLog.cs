using SQLite;
using System;

namespace Multilarr.Common.Models
{
	public class SettingLog
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
        public string ComputerName { get; set; }
		public string PusherAppId { get; set; }
		public string PusherKey { get; set; }
		public string PusherSecret { get; set; }
		public string PusherCluster { get; set; }
		public bool IsDefault { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
	}
}

