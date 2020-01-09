using System;
using Multilarr.Common;
using SQLite;

namespace Multilarr.Models
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

