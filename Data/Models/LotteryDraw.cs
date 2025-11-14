namespace Data.Models
{

    public class LotteryDraw
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
        public ICollection<Prize> Prizes { get; set; } = new List<Prize>();
    }

}
