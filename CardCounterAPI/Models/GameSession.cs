namespace CardCounterAPI.Models;

/// <summary>
/// Representa uma sessão de jogo salva no banco
/// </summary>
public class GameSession
{
    public int Id { get; set; }
    public int? PlayerId { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public int TotalCardsShown { get; set; }
    public int TotalCorrectGuesses { get; set; }
    public int TotalWrongGuesses { get; set; }
    public int FinalScore { get; set; }
    public bool IsActive { get; set; }
}
