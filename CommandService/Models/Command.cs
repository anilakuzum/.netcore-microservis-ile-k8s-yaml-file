using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CommandService.Models
{
    public class Command
    {
        [Key]
        [Required]
        public int Id { get; set; }
        
        [Required]
        public string HowTo { get; set; }

        [Required]
        public string CommandLiene { get; set; }

        [Required]
        public int PlatformId { get; set; }
        public Platform Platform { get; set; }

    }
}
