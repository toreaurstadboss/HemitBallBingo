namespace Data.Models
{
    public class Prize
    {
        public required int Id { get; set; }
        public required string Description { get; set; }
        public byte[]? ImageData { get; set; }
        public required int LotteryDrawId { get; set; }
        public required LotteryDraw LotteryDraw { get; set; }
    }

}
