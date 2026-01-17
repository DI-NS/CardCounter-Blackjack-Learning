namespace CardCounterAPI.Models;

/// <summary>
/// Representa uma jogada individual (carta mostrada e palpite)
/// </summary>
public class GamePlay
{
    public int Id { get; set; }
    public int SessionId { get; set; }
    public string CardName { get; set; } = "";
    public string CardSuit { get; set; } = "";
    public int CardCountValue { get; set; }
    public int CorrectCountAtMoment { get; set; }
    public int? PlayerGuess { get; set; }
    public bool? WasCorrect { get; set; }
    public DateTime PlayedAt { get; set; }
}
