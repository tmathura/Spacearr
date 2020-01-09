using System;
using SQLite;

namespace Multilarr.Common.Logger
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

