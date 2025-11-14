using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class HemitBallbingoContext : DbContext
    {
        public HemitBallbingoContext(DbContextOptions<HemitBallbingoContext> options) : base(options) { }

        public DbSet<LotteryDraw> LotteryDraws { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Prize> Prizes { get; set; }
    }
}