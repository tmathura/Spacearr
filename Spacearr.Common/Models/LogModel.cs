using Spacearr.Common.Enums;
using SQLite;
using System;

namespace Spacearr.Common.Models
{
    [Table("LogModel")]
	public class LogModel
	{
		[PrimaryKey, AutoIncrement]
        [Column("Id")]
		public int Id { get; set; }

        [Column("LogMessage")]
		public string LogMessage { get; set; }

        [Column("LogStackTrace")]
		public string LogStackTrace { get; set; }

        [Column("LogType")]
		public LogType LogType { get; set; }

        [Column("LogDate")]
		public DateTime LogDate { get; set; }
	}
}

