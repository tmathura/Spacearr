using SQLite;
using System;

namespace Spacearr.Common.Models
{
    [Table("FirebasePushNotificationDeviceModel")]
    public class FirebasePushNotificationDeviceModel
    {
		[PrimaryKey, AutoIncrement]
        [Column("Id")]
        public int Id { get; set; }

        [Column("DeviceId")]
		public Guid DeviceId { get; set; }

        [Column("Token")]
		public string Token { get; set; }

        [Column("CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [Column("UpdatedDate")]
        public DateTime UpdatedDate { get; set; }
	}
}

