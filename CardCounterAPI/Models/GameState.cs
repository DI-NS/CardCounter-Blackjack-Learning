namespace CardCounterAPI.Models;

/// <summary>
/// Guarda o estado atual do jogo
/// </summary>
public class GameState
{
    /// <summary>
    /// Carta atual mostrada ao jogador
    /// </summary>
    public Card? CurrentCard { get; set; }

    /// <summary>
    /// Contagem correta até agora
    /// </summary>
    public int CorrectCount { get; set; } = 0;

    /// <summary>
    /// Quantas cartas já foram mostradas
    /// </summary>
    public int CardsShown { get; set; } = 0;

    /// <summary>
    /// Quantas cartas restam no baralho
    /// </summary>
    public int CardsRemaining { get; set; } = 52;

    /// <summary>
    /// Pontuação do jogador (acertos)
    /// </summary>
    public int Score { get; set; } = 0;
}
