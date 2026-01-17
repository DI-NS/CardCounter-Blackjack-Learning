-- ==========================================
-- CARDCOUNTER - SCHEMA DO BANCO DE DADOS
-- ==========================================

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

-- Tabela de Histórico de Jogadas (cada carta mostrada)
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

-- Índices para melhorar performance
CREATE INDEX IF NOT EXISTS idx_sessions_player ON game_sessions(player_id);
CREATE INDEX IF NOT EXISTS idx_sessions_active ON game_sessions(is_active);
CREATE INDEX IF NOT EXISTS idx_plays_session ON game_plays(session_id);

-- Criar um jogador padrão para testes
INSERT INTO players (username, email) 
VALUES ('guest', 'guest@cardcounter.com')
ON CONFLICT (username) DO NOTHING;
