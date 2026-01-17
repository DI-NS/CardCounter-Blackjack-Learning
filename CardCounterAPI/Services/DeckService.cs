using CardCounterAPI.Models;

namespace CardCounterAPI.Services;

/// <summary>
/// Serviço responsável por gerenciar o baralho
/// </summary>
public class DeckService
{
    // Lista que guarda todas as cartas do baralho
    private List<Card> _deck = new List<Card>();

    // Índice da carta atual
    private int _currentIndex = 0;

    // Gerador de números aleatórios
    private Random _random = new Random();

    /// <summary>
    /// Cria um novo baralho com 52 cartas
    /// </summary>
    public void CreateDeck()
    {
        // Limpa o baralho anterior
        _deck.Clear();
        _currentIndex = 0;

        // Define os naipes
        string[] suits = { "hearts", "diamonds", "clubs", "spades" };

        // Define as cartas e seus valores de contagem
        var cardInfos = new (string name, int countValue)[]
        {
            ("A", -1),
            ("2", +1),
            ("3", +1),
            ("4", +1),
            ("5", +1),
            ("6", +1),
            ("7", 0),
            ("8", 0),
            ("9", 0),
            ("10", -1),
            ("J", -1),
            ("Q", -1),
            ("K", -1)
        };

        // Para cada naipe...
        foreach (string suit in suits)
        {
            // Para cada carta...
            foreach (var info in cardInfos)
            {
                // Cria e adiciona a carta ao baralho
                Card card = new Card
                {
                    Name = info.name,
                    Suit = suit,
                    CountValue = info.countValue,
                    ImageUrl = $"/cards/{info.name}_{suit}.png"
                };

                _deck.Add(card);
            }
        }

        // Embaralha o baralho
        Shuffle();
    }

    /// <summary>
    /// Embaralha o baralho (Algoritmo Fisher-Yates)
    /// </summary>
    private void Shuffle()
    {
        // Percorre o baralho de trás para frente
        for (int i = _deck.Count - 1; i > 0; i--)
        {
            // Escolhe uma posição aleatória
            int j = _random.Next(i + 1);

            // Troca as cartas de posição
            Card temp = _deck[i];
            _deck[i] = _deck[j];
            _deck[j] = temp;
        }
    }

    /// <summary>
    /// Pega a próxima carta do baralho
    /// </summary>
    /// <returns>A próxima carta ou null se acabou</returns>
    public Card? DrawCard()
    {
        // Verifica se ainda tem cartas
        if (_currentIndex >= _deck.Count)
        {
            return null; // Acabou o baralho!
        }

        // Pega a carta atual e avança o índice
        Card card = _deck[_currentIndex];
        _currentIndex++;

        return card;
    }

    /// <summary>
    /// Retorna quantas cartas ainda restam
    /// </summary>
    public int GetRemainingCards()
    {
        return _deck.Count - _currentIndex;
    }

    /// <summary>
    /// Reinicia o baralho (cria e embaralha novamente)
    /// </summary>
    public void Reset()
    {
        CreateDeck();
    }
}
