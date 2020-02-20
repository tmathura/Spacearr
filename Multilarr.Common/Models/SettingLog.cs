using SQLite;
using System;
using System.ComponentModel.DataAnnotations;

namespace Multilarr.Common.Models
{
	public class SettingLog
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

        [Required, MinLength(5)]
		public string ComputerName { get; set; }

        [Required]
		public string PusherAppId { get; set; }

        [Required]
		public string PusherKey { get; set; }

        [Required]
		public string PusherSecret { get; set; }

        [Required]
		public string PusherCluster { get; set; }

		public bool IsDefault { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }
	}
}

