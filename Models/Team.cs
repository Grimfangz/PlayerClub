using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlayerClub.Models
{
    public class Team
    {
        [Required]
        [Key]
        public String Name { get; set; }
        public String Ground { get; set; }
        public String Coach { get; set; }
        public String Region { get; set; }
        public List<Player> Players { get; set; }

    }
}
