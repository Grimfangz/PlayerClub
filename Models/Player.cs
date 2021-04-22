using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlayerClub.Models
{
    public class Player
    {
        public int Id { get; set; }
        [Required]
        public String Name { get; set; }
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}")]
        public DateTime Birthday { get; set; }
        public String Height { get; set; }
        public String Weight { get; set; }
        public String PlaceOfBirth { get; set; }
        public Team Team { get; set; }
    }
}
