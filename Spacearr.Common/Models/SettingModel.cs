using SQLite;
using System;
using System.ComponentModel.DataAnnotations;

namespace Spacearr.Common.Models
{
    [Table("SettingModel")]
	public class SettingModel
	{
		[PrimaryKey, AutoIncrement]
        [Column("Id")]
		public int Id { get; set; }

        [Display(Name = "Computer Name")]
		[Required(ErrorMessage = "Please specify the {0}.")]
		[StringLength(50, MinimumLength = 5, ErrorMessage = "{0}'s length should be between {2} and {1}.")]
        [Column("ComputerName")]
		public string ComputerName { get; set; }

        [Display(Name = "Pusher App Id")]
        [Required]
        [Column("PusherAppId")]
        public string PusherAppId { get; set; }

        [Display(Name = "Pusher Key")]
        [Required]
        [Column("PusherKey")]
        public string PusherKey { get; set; }

        [Display(Name = "Pusher Secret")]
        [Required]
        [Column("PusherSecret")]
        public string PusherSecret { get; set; }

        [Display(Name = "Pusher Cluster")]
        [Required]
        [Column("PusherCluster")]
        public string PusherCluster { get; set; }

		[Column("CreatedDate")]
		public DateTime CreatedDate { get; set; }

        [Column("UpdatedDate")]
		public DateTime UpdatedDate { get; set; }

        [Ignore]
        public string Version { get; set; }
    }
}

