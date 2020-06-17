using SQLite;
using System;

namespace Spacearr.Common.Models
{
    public class XamarinSettingModel
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		public bool IsDarkTheme { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }
	}
}

