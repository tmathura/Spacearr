using SQLite;
using System;

namespace Multilarr.Common.Models
{
	public class Log
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		public string LogMessage { get; set; }
		public Enumeration.LogType LogType { get; set; }
        public DateTime LogDate { get; set; }
	}
}

