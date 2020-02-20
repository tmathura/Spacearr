using SQLite;
using System;
using System.ComponentModel.DataAnnotations;

namespace Multilarr.Common.Models
{
	public class SettingLog
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

        [Display(Name = "Computer Name")]
		[Required(ErrorMessage = "Please specify the {0}.")]
		[StringLength(50, MinimumLength = 5, ErrorMessage = "{0}'s length should be between {2} and {1}.")]
		public string ComputerName { get; set; }

        [Display(Name = "Pusher App Id")]
		[Required]
		public string PusherAppId { get; set; }

        [Display(Name = "Pusher Key")]
		[Required]
		public string PusherKey { get; set; }

        [Display(Name = "Pusher Secret")]
		[Required]
		public string PusherSecret { get; set; }

        [Display(Name = "Pusher Cluster")]
		[Required]
		public string PusherCluster { get; set; }

		public bool IsDefault { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }
	}
}

