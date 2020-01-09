using Multilarr.WorkerService.Windows.Common;
using SQLite;
using System;

namespace Multilarr.WorkerService.Windows.Models
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

