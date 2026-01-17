using Microsoft.AspNetCore.Mvc;
using CardCounterAPI.Models;
using CardCounterAPI.Services;
using CardCounterAPI.Repositories;

namespace CardCounterAPI.Controllers;

/// <summary>
/// Controller que gerencia as rotas do jogo
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class GameController : ControllerBase
{
    // Serviço do baralho (injetado)
    private readonly DeckService _deckService;

    // Repository para salvar no banco
    private readonly GameRepository _gameRepository;

    // Estado atual do jogo (em memória)
    private static GameState _gameState = new GameState();

    // ID da sessão atual
    private static int? _currentSessionId = null;

    /// <summary>
    /// Construtor - recebe dependências por injeção
    /// </summary>
    public GameController(DeckService deckService, GameRepository gameRepository)
    {
        _deckService = deckService;
        _gameRepository = gameRepository;
    }

    /// <summary>
    /// GET /api/game/start
    /// Inicia um novo jogo
    /// </summary>
    [HttpGet("start")]
    public async Task<ActionResult<GameState>> StartGame()
    {
        // Cria um novo baralho
        _deckService.Reset();

        // Reseta o estado do jogo
        _gameState = new GameState
        {
            CurrentCard = null,
            CorrectCount = 0,
            CardsShown = 0,
            CardsRemaining = 52,
            Score = 0
        };

        // Cria uma nova sessão no banco de dados
        try
        {
            var guestId = await _gameRepository.GetGuestPlayerIdAsync();
            _currentSessionId = await _gameRepository.CreateSessionAsync(guestId);
            Console.WriteLine($"🎮 Nova sessão criada: {_currentSessionId}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️ Erro ao criar sessão no banco: {ex.Message}");
            // Continua o jogo mesmo se falhar no banco
        }

        return Ok(_gameState);
    }

    /// <summary>
    /// GET /api/game/card
    /// Pega a próxima carta do baralho
    /// </summary>
    [HttpGet("card")]
    public async Task<ActionResult<GameState>> GetNextCard()
    {
        // Tenta pegar uma carta
        Card? card = _deckService.DrawCard();

        // Se não tem mais cartas
        if (card == null)
        {
            return BadRequest("O baralho acabou! Inicie um novo jogo.");
        }

        // Atualiza o estado do jogo
        _gameState.CurrentCard = card;
        _gameState.CorrectCount += card.CountValue;
        _gameState.CardsShown++;
        _gameState.CardsRemaining = _deckService.GetRemainingCards();

        // Salva a jogada no banco de dados
        if (_currentSessionId.HasValue)
        {
            try
            {
                var play = new GamePlay
                {
                    SessionId = _currentSessionId.Value,
                    CardName = card.Name,
                    CardSuit = card.Suit,
                    CardCountValue = card.CountValue,
                    CorrectCountAtMoment = _gameState.CorrectCount
                };

                await _gameRepository.SavePlayAsync(play);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Erro ao salvar jogada: {ex.Message}");
            }
        }

        return Ok(_gameState);
    }

    /// <summary>
    /// POST /api/game/guess
    /// Jogador envia seu palpite da contagem
    /// </summary>
    [HttpPost("guess")]
    public async Task<ActionResult<object>> SubmitGuess([FromBody] int playerGuess)
    {
        // Verifica se o palpite está correto
        bool isCorrect = playerGuess == _gameState.CorrectCount;

        // Se acertou, aumenta a pontuação
        if (isCorrect)
        {
            _gameState.Score++;
        }

        // Atualiza a sessão no banco de dados
        if (_currentSessionId.HasValue)
        {
            try
            {
                var session = await _gameRepository.GetSessionAsync(_currentSessionId.Value);
                if (session != null)
                {
                    session.TotalCardsShown = _gameState.CardsShown;
                    session.TotalCorrectGuesses += isCorrect ? 1 : 0;
                    session.TotalWrongGuesses += isCorrect ? 0 : 1;
                    session.FinalScore = _gameState.Score;

                    await _gameRepository.UpdateSessionAsync(session);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Erro ao atualizar sessão: {ex.Message}");
            }
        }

        // Retorna o resultado
        return Ok(new
        {
            IsCorrect = isCorrect,
            CorrectAnswer = _gameState.CorrectCount,
            PlayerGuess = playerGuess,
            Score = _gameState.Score,
            Message = isCorrect ? "Muito bem!" : "Errou! Tente novamente."
        });
    }

    /// <summary>
    /// GET /api/game/state
    /// Retorna o estado atual do jogo
    /// </summary>
    [HttpGet("state")]
    public ActionResult<GameState> GetState()
    {
        return Ok(_gameState);
    }

    /// <summary>
    /// GET /api/game/history
    /// Retorna o histórico de sessões recentes
    /// </summary>
    [HttpGet("history")]
    public async Task<ActionResult<object>> GetHistory()
    {
        try
        {
            var sessions = await _gameRepository.GetRecentSessionsAsync(10);
            return Ok(new
            {
                TotalSessions = sessions.Count(),
                Sessions = sessions.Select(s => new
                {
                    s.Id,
                    s.StartedAt,
                    s.EndedAt,
                    s.TotalCardsShown,
                    s.TotalCorrectGuesses,
                    s.TotalWrongGuesses,
                    s.FinalScore,
                    AccuracyPercentage = s.TotalCardsShown > 0
                        ? (double)s.TotalCorrectGuesses / s.TotalCardsShown * 100
                        : 0
                })
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = "Erro ao buscar histórico", Message = ex.Message });
        }
    }
}
