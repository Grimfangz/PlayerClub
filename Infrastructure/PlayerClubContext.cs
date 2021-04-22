using Microsoft.EntityFrameworkCore;
using PlayerClub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayerClub.Infrastructure
{
    public class PlayerClubContext : DbContext
    {
        public PlayerClubContext(DbContextOptions<PlayerClubContext> options)
            : base(options)
        { 
        }
        public DbSet<Player> Player { get; set; }
        public DbSet<Team> Team { get; set; }
    }
}
