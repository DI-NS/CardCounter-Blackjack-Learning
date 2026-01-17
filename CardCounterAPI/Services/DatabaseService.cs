using Npgsql;
using Dapper;

namespace CardCounterAPI.Services;

/// <summary>
/// Serviço para gerenciar a conexão e operações no banco de dados
/// </summary>
public class DatabaseService
{
    private readonly string _connectionString;

    public DatabaseService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("SupabaseConnection")
            ?? throw new InvalidOperationException("Connection string não encontrada!");
    }

    /// <summary>
    /// Cria uma conexão com o banco de dados
    /// </summary>
    public NpgsqlConnection CreateConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }

    /// <summary>
    /// Inicializa as tabelas do banco de dados
    /// </summary>
    public async Task InitializeDatabaseAsync()
    {
        using var connection = CreateConnection();
        await connection.OpenAsync();

        // Script SQL para criar as tabelas
        var sql = @"
            -- Tabela de Jogadores
            CREATE TABLE IF NOT EXISTS players (
                id SERIAL PRIMARY KEY,
                username VARCHAR(50) UNIQUE NOT NULL,
                email VARCHAR(100) UNIQUE,
                created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
            );

            -- Tabela de Sessões de Jogo
            CREATE TABLE IF NOT EXISTS game_sessions (
                id SERIAL PRIMARY KEY,
                player_id INTEGER REFERENCES players(id),
                started_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                ended_at TIMESTAMP,
                total_cards_shown INTEGER DEFAULT 0,
                total_correct_guesses INTEGER DEFAULT 0,
                total_wrong_guesses INTEGER DEFAULT 0,
                final_score INTEGER DEFAULT 0,
                is_active BOOLEAN DEFAULT true
            );

            -- Tabela de Histórico de Jogadas
            CREATE TABLE IF NOT EXISTS game_plays (
                id SERIAL PRIMARY KEY,
                session_id INTEGER REFERENCES game_sessions(id) ON DELETE CASCADE,
                card_name VARCHAR(3) NOT NULL,
                card_suit VARCHAR(10) NOT NULL,
                card_count_value INTEGER NOT NULL,
                correct_count_at_moment INTEGER NOT NULL,
                player_guess INTEGER,
                was_correct BOOLEAN,
                played_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
            );

            -- Índices
            CREATE INDEX IF NOT EXISTS idx_sessions_player ON game_sessions(player_id);
            CREATE INDEX IF NOT EXISTS idx_sessions_active ON game_sessions(is_active);
            CREATE INDEX IF NOT EXISTS idx_plays_session ON game_plays(session_id);

            -- Jogador padrão
            INSERT INTO players (username, email) 
            VALUES ('guest', 'guest@cardcounter.com')
            ON CONFLICT (username) DO NOTHING;
        ";

        await connection.ExecuteAsync(sql);
    }

    /// <summary>
    /// Testa a conexão com o banco
    /// </summary>
    public async Task<bool> TestConnectionAsync()
    {
        try
        {
            using var connection = CreateConnection();
            await connection.OpenAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
