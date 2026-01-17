using Dapper;
using CardCounterAPI.Models;
using CardCounterAPI.Services;

namespace CardCounterAPI.Repositories;

/// <summary>
/// Repository para operações de jogo no banco de dados
/// </summary>
public class GameRepository
{
    private readonly DatabaseService _databaseService;

    public GameRepository(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    /// <summary>
    /// Cria uma nova sessão de jogo
    /// </summary>
    public async Task<int> CreateSessionAsync(int? playerId = null)
    {
        using var connection = _databaseService.CreateConnection();
        
        var sql = @"
            INSERT INTO game_sessions (player_id, is_active)
            VALUES (@PlayerId, true)
            RETURNING id";
        
        return await connection.ExecuteScalarAsync<int>(sql, new { PlayerId = playerId });
    }

    /// <summary>
    /// Atualiza uma sessão de jogo
    /// </summary>
    public async Task UpdateSessionAsync(GameSession session)
    {
        using var connection = _databaseService.CreateConnection();
        
        var sql = @"
            UPDATE game_sessions 
            SET total_cards_shown = @TotalCardsShown,
                total_correct_guesses = @TotalCorrectGuesses,
                total_wrong_guesses = @TotalWrongGuesses,
                final_score = @FinalScore,
                is_active = @IsActive,
                ended_at = @EndedAt
            WHERE id = @Id";
        
        await connection.ExecuteAsync(sql, session);
    }

    /// <summary>
    /// Busca uma sessão por ID
    /// </summary>
    public async Task<GameSession?> GetSessionAsync(int sessionId)
    {
        using var connection = _databaseService.CreateConnection();
        
        var sql = "SELECT * FROM game_sessions WHERE id = @SessionId";
        
        return await connection.QueryFirstOrDefaultAsync<GameSession>(sql, new { SessionId = sessionId });
    }

    /// <summary>
    /// Registra uma jogada (carta mostrada)
    /// </summary>
    public async Task<int> SavePlayAsync(GamePlay play)
    {
        using var connection = _databaseService.CreateConnection();
        
        var sql = @"
            INSERT INTO game_plays 
            (session_id, card_name, card_suit, card_count_value, correct_count_at_moment, player_guess, was_correct)
            VALUES 
            (@SessionId, @CardName, @CardSuit, @CardCountValue, @CorrectCountAtMoment, @PlayerGuess, @WasCorrect)
            RETURNING id";
        
        return await connection.ExecuteScalarAsync<int>(sql, play);
    }

    /// <summary>
    /// Busca todas as jogadas de uma sessão
    /// </summary>
    public async Task<IEnumerable<GamePlay>> GetPlaysAsync(int sessionId)
    {
        using var connection = _databaseService.CreateConnection();
        
        var sql = "SELECT * FROM game_plays WHERE session_id = @SessionId ORDER BY played_at";
        
        return await connection.QueryAsync<GamePlay>(sql, new { SessionId = sessionId });
    }

    /// <summary>
    /// Busca o ID do jogador guest
    /// </summary>
    public async Task<int> GetGuestPlayerIdAsync()
    {
        using var connection = _databaseService.CreateConnection();
        
        var sql = "SELECT id FROM players WHERE username = 'guest' LIMIT 1";
        
        return await connection.ExecuteScalarAsync<int>(sql);
    }

    /// <summary>
    /// Busca as últimas sessões
    /// </summary>
    public async Task<IEnumerable<GameSession>> GetRecentSessionsAsync(int limit = 10)
    {
        using var connection = _databaseService.CreateConnection();
        
        var sql = @"
            SELECT * FROM game_sessions 
            ORDER BY started_at DESC 
            LIMIT @Limit";
        
        return await connection.QueryAsync<GameSession>(sql, new { Limit = limit });
    }
}
