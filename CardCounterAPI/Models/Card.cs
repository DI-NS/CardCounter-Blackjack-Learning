namespace CardCounterAPI.Models;

/// <summary>
/// Representa uma carta do baralho
/// </summary>
public class Card
{
    /// <summary>
    /// Nome da carta (ex: "A", "2", "K")
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// Naipe da carta (ex: "hearts", "spades")
    /// </summary>
    public string Suit { get; set; } = "";

    /// <summary>
    /// Valor para contagem Hi-Lo
    /// +1 para cartas 2-6
    ///  0 para cartas 7-9
    /// -1 para cartas 10, J, Q, K, A
    /// </summary>
    public int CountValue { get; set; }

    /// <summary>
    /// Caminho da imagem da carta
    /// </summary>
    public string ImageUrl { get; set; } = "";
}
