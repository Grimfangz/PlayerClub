using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayerClub.Models
{
    // used for 2 models in 1 view
    public class Sign
    {
        public Player Player { get; set;}
        public List<Team> Teams { get; set;}
    }
}
