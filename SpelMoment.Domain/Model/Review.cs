namespace SpelMoment.Domain.Model;

public class Review
{
    public int Id { get; set; }

    public int Rating { get; set; }

    public string Text { get; set; } = string.Empty;

    public User Reviewer { get; set; } = null!;

    public Moment Moment { get; set; } = null!;
}