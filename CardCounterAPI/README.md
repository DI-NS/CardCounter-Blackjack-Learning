# 🎮 CardCounter API - Backend

<div align="center">

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![C#](https://img.shields.io/badge/C%23-12-239120?logo=c-sharp)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-Supabase-4169E1?logo=postgresql)

**API REST em ASP.NET Core para o jogo CardCounter**

</div>

---

## 📋 Índice

- [Sobre](#-sobre)
- [Arquitetura](#-arquitetura)
- [Estrutura de Pastas](#-estrutura-de-pastas)
- [Camadas e Responsabilidades](#-camadas-e-responsabilidades)
- [Modelos de Dados](#-modelos-de-dados)
- [Endpoints](#-endpoints)
- [Banco de Dados](#-banco-de-dados)
- [Configuração](#-configuração)
- [Como Rodar](#-como-rodar)
- [Testes](#-testes)

---

## 🎯 Sobre

Esta API é o **backend** do projeto CardCounter. Ela é responsável por:

✅ **Gerenciar a lógica do jogo** (embaralhamento, contagem, validações)  
✅ **Expor endpoints REST** para o frontend consumir  
✅ **Persistir dados** no PostgreSQL (Supabase)  
✅ **Validar regras de negócio** (contagem Hi-Lo, pontuação)  
✅ **Documentar automaticamente** com Swagger/OpenAPI  

---

## 🏗️ Arquitetura

A API segue uma arquitetura em camadas baseada em **Clean Architecture** e **Domain-Driven Design (DDD)**:

```
┌──────────────────────────────────────────────┐
│              Presentation Layer              │
│         (Controllers, DTOs, Filters)         │
└────────────────┬─────────────────────────────┘
                 │
                 ↓
┌──────────────────────────────────────────────┐
│              Application Layer               │
│          (Services, Business Logic)          │
└────────────────┬─────────────────────────────┘
                 │
                 ↓
┌──────────────────────────────────────────────┐
│            Infrastructure Layer              │
│       (Repositories, Database Access)        │
└────────────────┬─────────────────────────────┘
                 │
                 ↓
┌──────────────────────────────────────────────┐
│                 Database                     │
│         (PostgreSQL via Supabase)            │
└──────────────────────────────────────────────┘
```

### Padrões Utilizados

| Padrão | Descrição | Onde é usado |
|--------|-----------|--------------|
| **Repository Pattern** | Abstrai o acesso a dados | `GameRepository.cs` |
| **Dependency Injection** | Inversão de controle | `Program.cs` |
| **Service Layer** | Lógica de negócio isolada | `DeckService.cs`, `DatabaseService.cs` |
| **DTO (Data Transfer Objects)** | Modelos de transferência | `GameState.cs` |
| **Singleton Pattern** | Instância única compartilhada | `DeckService`, `DatabaseService` |

---

## 📁 Estrutura de Pastas

```
CardCounterAPI/
│
├── Controllers/              ← Presentation Layer
│   └── GameController.cs     → Endpoints REST
│
├── Models/                   ← Domain Layer
│   ├── Card.cs               → Entidade Carta
│   ├── GameState.cs          → DTO Estado do Jogo (em memória)
│   ├── GameSession.cs        → Entidade Sessão (banco)
│   └── GamePlay.cs           → Entidade Jogada (banco)
│
├── Services/                 ← Application Layer
│   ├── DeckService.cs        → Lógica do baralho
│   └── DatabaseService.cs    → Gerenciamento do banco
│
├── Repositories/             ← Infrastructure Layer
│   └── GameRepository.cs     → Acesso ao PostgreSQL
│
├── Program.cs                → Configuração da aplicação
├── appsettings.json          → Configurações públicas
├── appsettings.Development.json → Senhas (NÃO COMMITAR!)
└── database_schema.sql       → Schema SQL de referência
```

---

## 🎯 Camadas e Responsabilidades

### 1️⃣ **Controllers** (Presentation Layer)

**Responsabilidade:** Receber requisições HTTP e retornar respostas.

**O que FAZ:**
- ✅ Recebe requisições do cliente (frontend)
- ✅ Valida parâmetros de entrada
- ✅ Chama os **Services** para executar a lógica
- ✅ Retorna respostas HTTP (200 OK, 400 Bad Request, etc.)

**O que NÃO FAZ:**
- ❌ Lógica de negócio (isso é responsabilidade dos Services)
- ❌ Acesso direto ao banco (isso é responsabilidade dos Repositories)

**Exemplo:**
```csharp
[HttpGet("start")]
public async Task<ActionResult<GameState>> StartGame()
{
    // 1. Chama o service para resetar o baralho
    _deckService.Reset();
    
    // 2. Chama o repository para criar sessão no banco
    var sessionId = await _gameRepository.CreateSessionAsync(guestId);
    
    // 3. Retorna o estado inicial
    return Ok(_gameState);
}
```

---

### 2️⃣ **Services** (Application Layer)

**Responsabilidade:** Implementar as **regras de negócio** do jogo.

**O que FAZ:**
- ✅ Cria e embaralha o baralho (52 cartas)
- ✅ Implementa o algoritmo Fisher-Yates
- ✅ Distribui cartas uma por vez
- ✅ Calcula a contagem Hi-Lo
- ✅ Inicializa e testa conexão com banco

**O que NÃO FAZ:**
- ❌ Lidar com requisições HTTP (isso é dos Controllers)
- ❌ Executar queries SQL diretamente (isso é dos Repositories)

**Exemplo - DeckService:**
```csharp
public void CreateDeck()
{
    _deck.Clear();
    
    // Cria 52 cartas (4 naipes × 13 cartas)
    foreach (string suit in suits)
    {
        foreach (var info in cardInfos)
        {
            var card = new Card
            {
                Name = info.name,
                Suit = suit,
                CountValue = info.countValue  // Hi-Lo: +1, 0, -1
            };
            _deck.Add(card);
        }
    }
    
    Shuffle();  // Algoritmo Fisher-Yates
}
```

---

### 3️⃣ **Repositories** (Infrastructure Layer)

**Responsabilidade:** Abstrair o **acesso ao banco de dados**.

**O que FAZ:**
- ✅ Executa queries SQL (INSERT, SELECT, UPDATE)
- ✅ Mapeia objetos C# para tabelas do banco
- ✅ Usa Dapper para queries eficientes
- ✅ Retorna entidades mapeadas

**O que NÃO FAZ:**
- ❌ Lógica de negócio (isso é dos Services)
- ❌ Validações de regras do jogo (isso é dos Services)

**Exemplo - GameRepository:**
```csharp
public async Task<int> CreateSessionAsync(int? playerId = null)
{
    using var connection = _databaseService.CreateConnection();
    
    var sql = @"
        INSERT INTO game_sessions (player_id, is_active)
        VALUES (@PlayerId, true)
        RETURNING id";
    
    return await connection.ExecuteScalarAsync<int>(sql, new { PlayerId = playerId });
}
```

---

### 4️⃣ **Models** (Domain Layer)

**Responsabilidade:** Definir a **estrutura dos dados**.

**Tipos de Models:**

| Model | Tipo | Propósito |
|-------|------|-----------|
| `Card` | Entity | Representa uma carta do baralho |
| `GameState` | DTO | Estado em memória (não persiste) |
| `GameSession` | Entity | Sessão de jogo (persiste no banco) |
| `GamePlay` | Entity | Jogada individual (persiste no banco) |

**Exemplo:**
```csharp
public class Card
{
    public string Name { get; set; } = "";        // "A", "2", "K"
    public string Suit { get; set; } = "";        // "hearts", "spades"
    public int CountValue { get; set; }           // +1, 0, -1
    public string ImageUrl { get; set; } = "";    // "/cards/A_hearts.png"
}
```

---

## 📊 Modelos de Dados

### **Card** (Carta do Baralho)

```csharp
public class Card
{
    public string Name { get; set; }          // Nome da carta
    public string Suit { get; set; }          // Naipe
    public int CountValue { get; set; }       // Valor Hi-Lo
    public string ImageUrl { get; set; }      // Caminho da imagem
}
```

**Exemplo de instância:**
```json
{
  "name": "5",
  "suit": "hearts",
  "countValue": 1,
  "imageUrl": "/cards/5_hearts.png"
}
```

---

### **GameState** (Estado em Memória)

```csharp
public class GameState
{
    public Card? CurrentCard { get; set; }         // Carta atual
    public int CorrectCount { get; set; }          // Contagem correta
    public int CardsShown { get; set; }            // Cartas mostradas
    public int CardsRemaining { get; set; }        // Cartas restantes
    public int Score { get; set; }                 // Pontuação
}
```

**Nota:** Este modelo **NÃO é persistido** no banco. É mantido em memória durante o jogo.

---

### **GameSession** (Sessão no Banco)

```csharp
public class GameSession
{
    public int Id { get; set; }                    // ID único
    public int? PlayerId { get; set; }             // FK para players
    public DateTime StartedAt { get; set; }        // Quando começou
    public DateTime? EndedAt { get; set; }         // Quando terminou
    public int TotalCardsShown { get; set; }       // Total de cartas
    public int TotalCorrectGuesses { get; set; }   // Acertos
    public int TotalWrongGuesses { get; set; }     // Erros
    public int FinalScore { get; set; }            // Pontuação final
    public bool IsActive { get; set; }             // Está ativa?
}
```

**Mapeia para a tabela:** `game_sessions`

---

### **GamePlay** (Jogada Individual)

```csharp
public class GamePlay
{
    public int Id { get; set; }                    // ID único
    public int SessionId { get; set; }             // FK para game_sessions
    public string CardName { get; set; }           // Nome da carta
    public string CardSuit { get; set; }           // Naipe
    public int CardCountValue { get; set; }        // Valor Hi-Lo
    public int CorrectCountAtMoment { get; set; }  // Contagem naquele momento
    public int? PlayerGuess { get; set; }          // Palpite do jogador
    public bool? WasCorrect { get; set; }          // Acertou?
    public DateTime PlayedAt { get; set; }         // Timestamp
}
```

**Mapeia para a tabela:** `game_plays`

---

## 📡 Endpoints

### **GET /api/game/start**

Inicia um novo jogo.

**Response:**
```json
{
  "currentCard": null,
  "correctCount": 0,
  "cardsShown": 0,
  "cardsRemaining": 52,
  "score": 0
}
```

**O que acontece internamente:**
1. `DeckService.Reset()` → Cria e embaralha 52 cartas
2. `GameRepository.CreateSessionAsync()` → Insere nova sessão no banco
3. Retorna estado inicial zerado

---

### **GET /api/game/card**

Retorna a próxima carta do baralho.

**Response:**
```json
{
  "currentCard": {
    "name": "5",
    "suit": "hearts",
    "countValue": 1,
    "imageUrl": "/cards/5_hearts.png"
  },
  "correctCount": 1,
  "cardsShown": 1,
  "cardsRemaining": 51,
  "score": 0
}
```

**O que acontece internamente:**
1. `DeckService.DrawCard()` → Pega próxima carta do baralho
2. `correctCount += card.CountValue` → Atualiza contagem Hi-Lo
3. `GameRepository.SavePlayAsync()` → Salva jogada no banco
4. Retorna estado atualizado

---

### **POST /api/game/guess**

Envia o palpite do jogador.

**Request Body:**
```json
3
```

**Response:**
```json
{
  "isCorrect": true,
  "correctAnswer": 3,
  "playerGuess": 3,
  "score": 1,
  "message": "Muito bem!"
}
```

**O que acontece internamente:**
1. Compara `playerGuess` com `correctCount`
2. Se acertou: `score++`
3. `GameRepository.UpdateSessionAsync()` → Atualiza estatísticas
4. Retorna resultado

---

### **GET /api/game/history**

Retorna histórico de sessões recentes.

**Response:**
```json
{
  "totalSessions": 5,
  "sessions": [
    {
      "id": 5,
      "startedAt": "2026-01-17T03:30:00",
      "totalCardsShown": 15,
      "totalCorrectGuesses": 12,
      "totalWrongGuesses": 3,
      "finalScore": 12,
      "accuracyPercentage": 80.0
    }
  ]
}
```

---

## 🗄️ Banco de Dados

### Tabelas

#### **players**
```sql
CREATE TABLE players (
    id SERIAL PRIMARY KEY,
    username VARCHAR(50) UNIQUE NOT NULL,
    email VARCHAR(100) UNIQUE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
```

#### **game_sessions**
```sql
CREATE TABLE game_sessions (
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
```

#### **game_plays**
```sql
CREATE TABLE game_plays (
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
```

### Relacionamentos

```
players (1) ──────< (N) game_sessions (1) ──────< (N) game_plays
```

---

## ⚙️ Configuração

### 1. Connection String

Edite `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "SupabaseConnection": "Host=SEU_HOST;Port=5432;Database=postgres;Username=SEU_USER;Password=SUA_SENHA;SSL Mode=Require"
  }
}
```

### 2. Dependências

**Pacotes NuGet instalados:**
- `Npgsql` (10.0.1) - Driver PostgreSQL
- `Dapper` (2.1.66) - Micro ORM
- `Swashbuckle.AspNetCore` - Swagger

### 3. Injeção de Dependências

Em `Program.cs`:

```csharp
// Singleton = Uma única instância compartilhada
builder.Services.AddSingleton<DeckService>();
builder.Services.AddSingleton<DatabaseService>();

// Scoped = Uma instância por requisição
builder.Services.AddScoped<GameRepository>();
```

**Por que Singleton para DeckService?**
- Todos compartilham o mesmo baralho (simplicidade)
- Em produção real, usaria Scoped com sessões por usuário

---

## 🚀 Como Rodar

### Pré-requisitos
- .NET SDK 8.0+
- PostgreSQL ou conta no Supabase

### 1. Restaurar pacotes
```bash
dotnet restore
```

### 2. Configurar banco
Edite `appsettings.Development.json` com sua connection string.

### 3. Rodar
```bash
dotnet run
```

A API estará em: **http://localhost:5063**

Swagger: **http://localhost:5063/swagger**

---

## 🧪 Testes

### Testar endpoints com curl

```bash
# Iniciar jogo
curl http://localhost:5063/api/game/start

# Pegar carta
curl http://localhost:5063/api/game/card

# Enviar palpite
curl -X POST http://localhost:5063/api/game/guess \
  -H "Content-Type: application/json" \
  -d "3"

# Ver histórico
curl http://localhost:5063/api/game/history
```

### Testar no Swagger

Acesse **http://localhost:5063/swagger** e teste visualmente cada endpoint.

---


## 📚 Recursos

- [Documentação ASP.NET Core](https://docs.microsoft.com/aspnet/core)
- [Dapper](https://github.com/DapperLib/Dapper)
- [Npgsql](https://www.npgsql.org/doc/)
- [Supabase](https://supabase.com/docs)

---

<div align="center">

**Desenvolvido usando C# e .NET 8.0**

</div>
