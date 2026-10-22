using System.ComponentModel.DataAnnotations;

namespace SpelMoment.ViewModels;

public class CreateReviewViewModel
{
    public int MomentId { get; set; }

    [Range(1, 5, ErrorMessage = "Geef een score tussen 1 en 5.")]
    public int Rating { get; set; }

    [Required(ErrorMessage = "Vul een reviewtekst in.")]
    public string Text { get; set; } = string.Empty;
}