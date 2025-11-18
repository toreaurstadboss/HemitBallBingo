namespace Data.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public required int TicketNumber { get; set; }
        public required string OwnerName { get; set; }
        public int LotteryDrawId { get; set; }
        public LotteryDraw LotteryDraw { get; set; }

        public bool IsDrawn { get; set; }

        /// <summary>
        /// Prize number won by this ticket (1 for first prize, 2 for second, 3 for third and so on if any more prizes), null if no prize won.
        /// </summary>
        public int? PrizeNumber { get; set; }

    }
}