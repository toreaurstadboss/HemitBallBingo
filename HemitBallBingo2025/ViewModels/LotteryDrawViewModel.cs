using System.ComponentModel.DataAnnotations;

namespace HemitBallBingo2025.ViewModels
{

    public class LotteryDrawCreateViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Created { get; set; }
    }

}
