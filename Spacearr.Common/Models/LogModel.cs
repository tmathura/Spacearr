﻿using SQLite;
using System;

namespace Spacearr.Common.Models
{
	public class LogModel
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		public string LogMessage { get; set; }
		public string LogStackTrace { get; set; }
		public Enumeration.LogType LogType { get; set; }
        public DateTime LogDate { get; set; }
	}
}
