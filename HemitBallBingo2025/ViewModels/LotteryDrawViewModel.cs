using System.ComponentModel.DataAnnotations;

namespace HemitBallBingo2025.ViewModels
{

    public class LotteryDrawCreateViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime Created { get; set; }
    }

}
