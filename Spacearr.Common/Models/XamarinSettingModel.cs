﻿using SQLite;
using System;

namespace Spacearr.Common.Models
{
    [Table("XamarinSettingModel")]
    public class XamarinSettingModel
	{
		[PrimaryKey, AutoIncrement]

        [Column("Id")]
        public int Id { get; set; }

        [Column("DeviceId")]
        public Guid? DeviceId { get; set; }
        
        [Column("IsDarkTheme")]
        public bool IsDarkTheme { get; set; }
        
        [Column("CreatedDate")]
        public DateTime CreatedDate { get; set; }
        
        [Column("UpdatedDate")]
        public DateTime UpdatedDate { get; set; }
	}
}

