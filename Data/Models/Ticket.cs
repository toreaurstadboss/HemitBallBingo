namespace Data.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public required int TicketNumber { get; set; }
        public required string OwnerName { get; set; }
        public int LotteryDrawId { get; set; }
        public LotteryDraw LotteryDraw { get; set; }
    }
}