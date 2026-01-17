# 🃏 Projeto CardCounter - Blackjack Learning

## 📋 Visão Geral do Projeto

**Objetivo:** Criar um site simples para praticar contagem de cartas no Blackjack, estimulando raciocínio lógico e habilidades cognitivas.

**Stack Simplificada:**
- **Backend:** C# com ASP.NET Core (API REST)
- **Frontend:** HTML + CSS + JavaScript puro
- **Banco de Dados:** Não usaremos por enquanto (dados em memória)

> ⚠️ **Por que simplificar?** Para focar no aprendizado de C#, vamos começar SEM banco de dados, SEM React, SEM TypeScript. Depois você pode adicionar!

---

## 🗂️ Estrutura Final do Projeto

```
CardCounter/
│
├── CardCounterAPI/          ← Backend (C# ASP.NET)
│   ├── Controllers/
│   │   └── GameController.cs
│   ├── Models/
│   │   ├── Card.cs
│   │   └── GameState.cs
│   ├── Services/
│   │   └── DeckService.cs
│   └── Program.cs
│
└── CardCounterWeb/          ← Frontend (HTML/CSS/JS)
    ├── index.html
    ├── style.css
    └── script.js
```

---

## 📝 TASKS DE EXECUÇÃO

---

# TASK 1: Configurar o Ambiente

## 🎯 Objetivo
Instalar tudo que você precisa para começar a programar.

## 📚 O que você vai aprender
- Instalar o .NET SDK
- Instalar o VS Code
- Entender o que é cada ferramenta

## ✅ Checklist

### 1.1 Instalar o .NET SDK
1. Acesse: https://dotnet.microsoft.com/download
2. Baixe o **.NET 8 SDK** (versão mais recente)
3. Execute o instalador
4. Verifique a instalação abrindo o terminal e digitando:
```bash
dotnet --version
```
> Deve aparecer algo como `8.0.xxx`

### 1.2 Instalar o VS Code
1. Acesse: https://code.visualstudio.com/
2. Baixe e instale

### 1.3 Instalar Extensões do VS Code
Abra o VS Code e instale estas extensões (Ctrl+Shift+X):
- **C# Dev Kit** (da Microsoft)
- **REST Client** (para testar a API)

## 📖 Conceitos Importantes

| Termo | O que é? |
|-------|----------|
| **.NET SDK** | Kit de desenvolvimento para criar programas em C# |
| **VS Code** | Editor de código (onde você escreve o código) |
| **Terminal** | Janela onde você digita comandos |

---

# TASK 2: Criar o Projeto Backend

## 🎯 Objetivo
Criar a estrutura inicial da API em C#.

## 📚 O que você vai aprender
- Usar o comando `dotnet new`
- Entender a estrutura de um projeto ASP.NET
- O que é uma API

## ✅ Passos

### 2.1 Criar a pasta do projeto
Abra o terminal e digite:
```bash
mkdir CardCounter
cd CardCounter
```

### 2.2 Criar o projeto da API
```bash
dotnet new webapi -n CardCounterAPI --no-https
cd CardCounterAPI
```

> **O que esse comando faz?**
> - `dotnet new webapi` → Cria um projeto de API web
> - `-n CardCounterAPI` → Nome do projeto
> - `--no-https` → Simplifica (sem certificado SSL por enquanto)

### 2.3 Limpar arquivos desnecessários
Delete estes arquivos que vieram de exemplo:
- `Controllers/WeatherForecastController.cs`
- `WeatherForecast.cs`

### 2.4 Testar se está funcionando
```bash
dotnet run
```
> Acesse no navegador: `http://localhost:5000/swagger`
> Você verá a documentação da API (vazia por enquanto)

## 📖 Conceitos Importantes

| Termo | O que é? |
|-------|----------|
| **API** | "Garçom" que leva pedidos do site para o servidor e traz respostas |
| **Endpoint** | Um "caminho" da API (ex: `/api/game/card`) |
| **Controller** | Arquivo que define os endpoints |
| **Model** | Arquivo que define a estrutura dos dados |

---

# TASK 3: Criar o Model da Carta

## 🎯 Objetivo
Criar a classe que representa uma carta de baralho.

## 📚 O que você vai aprender
- Criar uma **classe** em C#
- O que são **propriedades**
- O que é um **enum**

## ✅ Passos

### 3.1 Criar a pasta Models
Dentro de `CardCounterAPI`, crie a pasta `Models`

### 3.2 Criar o arquivo Card.cs

📄 **Models/Card.cs**
```csharp
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
```

## 📖 Explicação Linha por Linha

```csharp
namespace CardCounterAPI.Models;
```
> **Namespace** = "Endereço" do arquivo. Ajuda a organizar o código.

```csharp
public class Card
```
> **Classe** = "Molde" para criar objetos. Card é o molde de uma carta.

```csharp
public string Name { get; set; } = "";
```
> **Propriedade** = Característica do objeto.
> - `public` = Qualquer código pode acessar
> - `string` = Tipo texto
> - `{ get; set; }` = Pode ler e alterar o valor
> - `= ""` = Valor inicial vazio

## 📖 Sistema Hi-Lo de Contagem

| Cartas | Valor | Por quê? |
|--------|-------|----------|
| 2, 3, 4, 5, 6 | +1 | Cartas baixas são boas para o dealer |
| 7, 8, 9 | 0 | Cartas neutras |
| 10, J, Q, K, A | -1 | Cartas altas são boas para o jogador |

> **Contagem positiva** = Mais cartas altas no baralho = Bom para o jogador!

---

# TASK 4: Criar o Model do Estado do Jogo

## 🎯 Objetivo
Criar a classe que guarda as informações do jogo atual.

## 📚 O que você vai aprender
- Trabalhar com listas em C#
- Entender estado de aplicação

## ✅ Passos

### 4.1 Criar o arquivo GameState.cs

📄 **Models/GameState.cs**
```csharp
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
```

## 📖 Explicação

```csharp
public Card? CurrentCard { get; set; }
```
> O `?` significa que pode ser **null** (vazio).
> No início do jogo, não tem carta ainda!

---

# TASK 5: Criar o Serviço do Baralho

## 🎯 Objetivo
Criar a lógica que gerencia o baralho de cartas.

## 📚 O que você vai aprender
- O que é um **Service** (serviço)
- Trabalhar com **Listas** (List)
- Criar **métodos**
- Usar **Random** para embaralhar

## ✅ Passos

### 5.1 Criar a pasta Services
Dentro de `CardCounterAPI`, crie a pasta `Services`

### 5.2 Criar o arquivo DeckService.cs

📄 **Services/DeckService.cs**
```csharp
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
```

## 📖 Explicação dos Conceitos

### O que é um Service?
> Um **Service** é uma classe que contém a **lógica de negócio**.
> Ele faz o "trabalho pesado" para que o Controller fique simples.

### Explicação do Código

```csharp
private List<Card> _deck = new List<Card>();
```
> - `private` = Só essa classe pode acessar
> - `List<Card>` = Uma lista que só aceita objetos Card
> - `_deck` = Nome com `_` indica que é privado (convenção)

```csharp
foreach (string suit in suits)
```
> **foreach** = "Para cada" item na lista, faça algo.
> É como um funcionário que passa por cada item.

```csharp
Card card = new Card { Name = "A", Suit = "hearts" };
```
> **new** = Cria um novo objeto a partir do molde (classe).
> As `{ }` permitem definir os valores na hora.

```csharp
_random.Next(i + 1)
```
> Gera um número aleatório entre 0 e i (inclusive).

---

# TASK 6: Criar o Controller do Jogo

## 🎯 Objetivo
Criar os endpoints da API que o site vai usar.

## 📚 O que você vai aprender
- O que é um **Controller**
- Como criar **endpoints** (rotas)
- Atributos `[HttpGet]`, `[HttpPost]`
- O que é **Injeção de Dependência**

## ✅ Passos

### 6.1 Criar o arquivo GameController.cs

📄 **Controllers/GameController.cs**
```csharp
using Microsoft.AspNetCore.Mvc;
using CardCounterAPI.Models;
using CardCounterAPI.Services;

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
    
    // Estado atual do jogo
    private static GameState _gameState = new GameState();

    /// <summary>
    /// Construtor - recebe o DeckService por injeção de dependência
    /// </summary>
    public GameController(DeckService deckService)
    {
        _deckService = deckService;
    }

    /// <summary>
    /// GET /api/game/start
    /// Inicia um novo jogo
    /// </summary>
    [HttpGet("start")]
    public ActionResult<GameState> StartGame()
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
        
        return Ok(_gameState);
    }

    /// <summary>
    /// GET /api/game/card
    /// Pega a próxima carta do baralho
    /// </summary>
    [HttpGet("card")]
    public ActionResult<GameState> GetNextCard()
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
        
        return Ok(_gameState);
    }

    /// <summary>
    /// POST /api/game/guess
    /// Jogador envia seu palpite da contagem
    /// </summary>
    [HttpPost("guess")]
    public ActionResult<object> SubmitGuess([FromBody] int playerGuess)
    {
        // Verifica se o palpite está correto
        bool isCorrect = playerGuess == _gameState.CorrectCount;
        
        // Se acertou, aumenta a pontuação
        if (isCorrect)
        {
            _gameState.Score++;
        }
        
        // Retorna o resultado
        return Ok(new
        {
            IsCorrect = isCorrect,
            CorrectAnswer = _gameState.CorrectCount,
            PlayerGuess = playerGuess,
            Score = _gameState.Score,
            Message = isCorrect ? "Muito bem! 🎉" : "Errou! Tente novamente."
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
}
```

## 📖 Explicação dos Conceitos

### Atributos (as coisas entre `[ ]`)

| Atributo | O que faz? |
|----------|-----------|
| `[ApiController]` | Indica que é um controller de API |
| `[Route("api/[controller]")]` | Define a URL base (api/game) |
| `[HttpGet("start")]` | Responde a GET /api/game/start |
| `[HttpPost("guess")]` | Responde a POST /api/game/guess |
| `[FromBody]` | Pega o dado do corpo da requisição |

### ActionResult
```csharp
public ActionResult<GameState> StartGame()
```
> **ActionResult** = Tipo de retorno que pode ser:
> - `Ok(dados)` = Sucesso (código 200)
> - `BadRequest(msg)` = Erro do cliente (código 400)
> - `NotFound()` = Não encontrado (código 404)

### Objeto Anônimo
```csharp
return Ok(new { IsCorrect = true, Score = 5 });
```
> Cria um objeto "na hora" sem precisar de uma classe.
> Útil para respostas rápidas.

---

# TASK 7: Configurar o Program.cs

## 🎯 Objetivo
Configurar a aplicação para funcionar corretamente.

## 📚 O que você vai aprender
- O que é o **Program.cs**
- Como registrar **Services**
- O que é **CORS**

## ✅ Passos

### 7.1 Editar o Program.cs

📄 **Program.cs**
```csharp
using CardCounterAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// CONFIGURAÇÃO DOS SERVIÇOS
// ==========================================

// Registra os controllers
builder.Services.AddControllers();

// Registra o DeckService como Singleton
// Singleton = Uma única instância para toda a aplicação
builder.Services.AddSingleton<DeckService>();

// Configura o Swagger (documentação da API)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configura o CORS (permite o site acessar a API)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()    // Qualquer origem
              .AllowAnyMethod()    // Qualquer método (GET, POST, etc)
              .AllowAnyHeader();   // Qualquer cabeçalho
    });
});

var app = builder.Build();

// ==========================================
// CONFIGURAÇÃO DO PIPELINE
// ==========================================

// Ativa o Swagger apenas em desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Ativa o CORS
app.UseCors("AllowAll");

// Mapeia os controllers
app.MapControllers();

// Inicia a aplicação
app.Run();
```

## 📖 Explicação dos Conceitos

### O que é o Program.cs?
> É o **ponto de entrada** da aplicação.
> Aqui você configura tudo que o sistema precisa.

### Injeção de Dependência
```csharp
builder.Services.AddSingleton<DeckService>();
```
> Registra o DeckService para ser "injetado" automaticamente.
> - **Singleton** = Uma única instância compartilhada
> - **Scoped** = Uma instância por requisição
> - **Transient** = Uma nova instância sempre

### O que é CORS?
> **CORS** (Cross-Origin Resource Sharing)
> Por segurança, navegadores bloqueiam requisições de um site para outro.
> O CORS permite que nosso site (porta 5500) acesse a API (porta 5000).

---

# TASK 8: Criar o Frontend (HTML)

## 🎯 Objetivo
Criar a página do jogo.

## 📚 O que você vai aprender
- Estrutura básica de HTML
- Como conectar HTML com CSS e JavaScript

## ✅ Passos

### 8.1 Criar a pasta do frontend
```bash
cd ..
mkdir CardCounterWeb
cd CardCounterWeb
```

### 8.2 Criar o index.html

📄 **index.html**
```html
<!DOCTYPE html>
<html lang="pt-BR">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>🃏 Card Counter - Blackjack</title>
    <link rel="stylesheet" href="style.css">
</head>
<body>
    <div class="container">
        <!-- Cabeçalho -->
        <header>
            <h1>🃏 Card Counter</h1>
            <p>Pratique contagem de cartas no Blackjack!</p>
        </header>

        <!-- Área do Jogo -->
        <main>
            <!-- Informações do Jogo -->
            <div class="game-info">
                <div class="info-box">
                    <span class="label">Cartas Vistas</span>
                    <span id="cards-shown">0</span>
                </div>
                <div class="info-box">
                    <span class="label">Restantes</span>
                    <span id="cards-remaining">52</span>
                </div>
                <div class="info-box">
                    <span class="label">Pontuação</span>
                    <span id="score">0</span>
                </div>
            </div>

            <!-- Área da Carta -->
            <div class="card-area">
                <div id="card-display" class="card">
                    <span class="card-placeholder">?</span>
                </div>
            </div>

            <!-- Área de Resposta -->
            <div class="answer-area">
                <p>Qual é a contagem atual?</p>
                <input 
                    type="number" 
                    id="guess-input" 
                    placeholder="Digite sua resposta"
                >
                <div class="buttons">
                    <button id="btn-guess" onclick="submitGuess()">
                        Verificar
                    </button>
                    <button id="btn-next" onclick="getNextCard()">
                        Próxima Carta
                    </button>
                </div>
            </div>

            <!-- Feedback -->
            <div id="feedback" class="feedback"></div>

            <!-- Botão Iniciar -->
            <button id="btn-start" class="btn-start" onclick="startGame()">
                🎮 Iniciar Novo Jogo
            </button>
        </main>

        <!-- Tabela de Referência -->
        <aside class="reference">
            <h3>📊 Tabela Hi-Lo</h3>
            <table>
                <tr>
                    <td>2, 3, 4, 5, 6</td>
                    <td class="positive">+1</td>
                </tr>
                <tr>
                    <td>7, 8, 9</td>
                    <td class="neutral">0</td>
                </tr>
                <tr>
                    <td>10, J, Q, K, A</td>
                    <td class="negative">-1</td>
                </tr>
            </table>
        </aside>
    </div>

    <script src="script.js"></script>
</body>
</html>
```

---

# TASK 9: Criar o CSS

## 🎯 Objetivo
Estilizar a página para ficar bonita.

## ✅ Passos

📄 **style.css**
```css
/* Reset básico */
* {
    margin: 0;
    padding: 0;
    box-sizing: border-box;
}

body {
    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
    background: linear-gradient(135deg, #1a1a2e 0%, #16213e 100%);
    min-height: 100vh;
    color: white;
}

.container {
    max-width: 600px;
    margin: 0 auto;
    padding: 20px;
}

/* Cabeçalho */
header {
    text-align: center;
    margin-bottom: 30px;
}

header h1 {
    font-size: 2.5rem;
    margin-bottom: 10px;
}

header p {
    color: #888;
}

/* Informações do Jogo */
.game-info {
    display: flex;
    justify-content: space-around;
    margin-bottom: 30px;
}

.info-box {
    background: rgba(255, 255, 255, 0.1);
    padding: 15px 25px;
    border-radius: 10px;
    text-align: center;
}

.info-box .label {
    display: block;
    font-size: 0.8rem;
    color: #888;
    margin-bottom: 5px;
}

.info-box span:last-child {
    font-size: 1.5rem;
    font-weight: bold;
    color: #4ade80;
}

/* Área da Carta */
.card-area {
    display: flex;
    justify-content: center;
    margin-bottom: 30px;
}

.card {
    width: 150px;
    height: 210px;
    background: white;
    border-radius: 10px;
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 3rem;
    color: #333;
    box-shadow: 0 10px 30px rgba(0, 0, 0, 0.3);
}

.card-placeholder {
    color: #ccc;
}

/* Carta Vermelha */
.card.red {
    color: #ef4444;
}

/* Carta Preta */
.card.black {
    color: #1a1a2e;
}

/* Área de Resposta */
.answer-area {
    text-align: center;
    margin-bottom: 20px;
}

.answer-area p {
    margin-bottom: 15px;
    font-size: 1.1rem;
}

.answer-area input {
    width: 200px;
    padding: 12px 20px;
    font-size: 1.2rem;
    border: none;
    border-radius: 8px;
    text-align: center;
    margin-bottom: 15px;
}

.buttons {
    display: flex;
    gap: 10px;
    justify-content: center;
}

.buttons button {
    padding: 12px 30px;
    font-size: 1rem;
    border: none;
    border-radius: 8px;
    cursor: pointer;
    transition: transform 0.2s, background 0.2s;
}

#btn-guess {
    background: #4ade80;
    color: #1a1a2e;
}

#btn-next {
    background: #3b82f6;
    color: white;
}

.buttons button:hover {
    transform: scale(1.05);
}

/* Feedback */
.feedback {
    text-align: center;
    padding: 15px;
    border-radius: 8px;
    margin-bottom: 20px;
    font-size: 1.1rem;
    min-height: 50px;
}

.feedback.correct {
    background: rgba(74, 222, 128, 0.2);
    color: #4ade80;
}

.feedback.wrong {
    background: rgba(239, 68, 68, 0.2);
    color: #ef4444;
}

/* Botão Iniciar */
.btn-start {
    width: 100%;
    padding: 15px;
    font-size: 1.2rem;
    background: #8b5cf6;
    color: white;
    border: none;
    border-radius: 10px;
    cursor: pointer;
    transition: background 0.2s;
}

.btn-start:hover {
    background: #7c3aed;
}

/* Tabela de Referência */
.reference {
    margin-top: 40px;
    background: rgba(255, 255, 255, 0.05);
    padding: 20px;
    border-radius: 10px;
}

.reference h3 {
    text-align: center;
    margin-bottom: 15px;
}

.reference table {
    width: 100%;
    border-collapse: collapse;
}

.reference td {
    padding: 10px;
    text-align: center;
    border-bottom: 1px solid rgba(255, 255, 255, 0.1);
}

.positive { color: #4ade80; font-weight: bold; }
.neutral { color: #fbbf24; font-weight: bold; }
.negative { color: #ef4444; font-weight: bold; }
```

---

# TASK 10: Criar o JavaScript

## 🎯 Objetivo
Fazer o site se comunicar com a API.

## 📚 O que você vai aprender
- Fazer requisições HTTP com **fetch**
- Manipular o **DOM** (elementos da página)
- Trabalhar com **async/await**

## ✅ Passos

📄 **script.js**
```javascript
// ==========================================
// CONFIGURAÇÃO
// ==========================================

// URL base da API
const API_URL = 'http://localhost:5000/api/game';

// ==========================================
// FUNÇÕES AUXILIARES
// ==========================================

/**
 * Atualiza um elemento na tela
 * @param {string} id - ID do elemento
 * @param {string} value - Novo valor
 */
function updateElement(id, value) {
    document.getElementById(id).textContent = value;
}

/**
 * Retorna o símbolo do naipe
 * @param {string} suit - Nome do naipe em inglês
 * @returns {string} Símbolo do naipe
 */
function getSuitSymbol(suit) {
    const suits = {
        'hearts': '♥',
        'diamonds': '♦',
        'clubs': '♣',
        'spades': '♠'
    };
    return suits[suit] || '?';
}

/**
 * Verifica se o naipe é vermelho
 * @param {string} suit - Nome do naipe
 * @returns {boolean}
 */
function isRedSuit(suit) {
    return suit === 'hearts' || suit === 'diamonds';
}

// ==========================================
// FUNÇÕES DO JOGO
// ==========================================

/**
 * Inicia um novo jogo
 */
async function startGame() {
    try {
        // Faz a requisição para a API
        const response = await fetch(`${API_URL}/start`);
        
        // Converte a resposta para JSON
        const data = await response.json();
        
        // Atualiza a tela
        updateElement('cards-shown', data.cardsShown);
        updateElement('cards-remaining', data.cardsRemaining);
        updateElement('score', data.score);
        
        // Limpa a carta e o feedback
        document.getElementById('card-display').innerHTML = 
            '<span class="card-placeholder">?</span>';
        document.getElementById('card-display').className = 'card';
        document.getElementById('feedback').textContent = '';
        document.getElementById('feedback').className = 'feedback';
        document.getElementById('guess-input').value = '';
        
        // Mostra mensagem
        document.getElementById('feedback').textContent = 
            'Jogo iniciado! Clique em "Próxima Carta"';
            
    } catch (error) {
        console.error('Erro ao iniciar:', error);
        alert('Erro ao conectar com a API. Verifique se ela está rodando!');
    }
}

/**
 * Pega a próxima carta do baralho
 */
async function getNextCard() {
    try {
        const response = await fetch(`${API_URL}/card`);
        
        // Se der erro (baralho acabou)
        if (!response.ok) {
            const error = await response.text();
            document.getElementById('feedback').textContent = error;
            document.getElementById('feedback').className = 'feedback wrong';
            return;
        }
        
        const data = await response.json();
        
        // Atualiza as informações
        updateElement('cards-shown', data.cardsShown);
        updateElement('cards-remaining', data.cardsRemaining);
        
        // Mostra a carta
        const card = data.currentCard;
        const cardDisplay = document.getElementById('card-display');
        const symbol = getSuitSymbol(card.suit);
        const color = isRedSuit(card.suit) ? 'red' : 'black';
        
        cardDisplay.innerHTML = `
            <div>
                <div style="font-size: 2rem;">${card.name}</div>
                <div style="font-size: 3rem;">${symbol}</div>
            </div>
        `;
        cardDisplay.className = `card ${color}`;
        
        // Limpa o feedback
        document.getElementById('feedback').textContent = '';
        document.getElementById('feedback').className = 'feedback';
        
    } catch (error) {
        console.error('Erro ao pegar carta:', error);
    }
}

/**
 * Envia o palpite do jogador
 */
async function submitGuess() {
    // Pega o valor digitado
    const input = document.getElementById('guess-input');
    const guess = parseInt(input.value);
    
    // Valida se digitou algo
    if (isNaN(guess)) {
        alert('Digite um número!');
        return;
    }
    
    try {
        const response = await fetch(`${API_URL}/guess`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(guess)
        });
        
        const data = await response.json();
        
        // Mostra o feedback
        const feedback = document.getElementById('feedback');
        feedback.textContent = `${data.message} (Resposta correta: ${data.correctAnswer})`;
        feedback.className = `feedback ${data.isCorrect ? 'correct' : 'wrong'}`;
        
        // Atualiza a pontuação
        updateElement('score', data.score);
        
        // Limpa o input
        input.value = '';
        
    } catch (error) {
        console.error('Erro ao enviar palpite:', error);
    }
}

// ==========================================
// EVENTOS
// ==========================================

// Permite enviar com Enter
document.getElementById('guess-input').addEventListener('keypress', function(e) {
    if (e.key === 'Enter') {
        submitGuess();
    }
});
```

## 📖 Explicação dos Conceitos

### async/await
```javascript
async function startGame() {
    const response = await fetch(url);
    const data = await response.json();
}
```
> - `async` = Marca a função como assíncrona
> - `await` = "Espera" a operação terminar antes de continuar
> - Usado para operações que demoram (como requisições HTTP)

### fetch
```javascript
fetch(`${API_URL}/start`)
```
> **fetch** = Função nativa do JavaScript para fazer requisições HTTP.
> Retorna uma **Promise** (promessa de resposta futura).

### try/catch
```javascript
try {
    // Código que pode dar erro
} catch (error) {
    // O que fazer se der erro
}
```
> Protege o código contra erros inesperados.

---

# TASK 11: Testar Tudo

## 🎯 Objetivo
Rodar o projeto e verificar se está funcionando.

## ✅ Passos

### 11.1 Iniciar a API
Abra um terminal na pasta `CardCounterAPI`:
```bash
dotnet run
```
> A API vai rodar em `http://localhost:5000`

### 11.2 Testar a API no Swagger
Acesse: `http://localhost:5000/swagger`

Teste os endpoints na ordem:
1. **GET /api/game/start** - Inicia o jogo
2. **GET /api/game/card** - Pega uma carta
3. **POST /api/game/guess** - Envia um palpite

### 11.3 Abrir o Frontend
Opção 1: Instale a extensão **Live Server** no VS Code
- Clique com botão direito no `index.html`
- Selecione "Open with Live Server"

Opção 2: Abra diretamente no navegador
- Abra o arquivo `index.html` no navegador

### 11.4 Jogar!
1. Clique em "Iniciar Novo Jogo"
2. Clique em "Próxima Carta"
3. Some/Subtraia mentalmente o valor Hi-Lo
4. Digite sua resposta e clique em "Verificar"
5. Repita!

---

# 📚 GLOSSÁRIO DE TERMOS

| Termo | Explicação Simples |
|-------|-------------------|
| **API** | "Garçom" entre o site e o servidor |
| **Endpoint** | Caminho/endereço de uma função da API |
| **Controller** | Arquivo que organiza os endpoints |
| **Model** | "Molde" que define como os dados são |
| **Service** | Onde fica a lógica do programa |
| **Classe** | Molde para criar objetos |
| **Objeto** | Uma "coisa" criada a partir de uma classe |
| **Propriedade** | Característica de um objeto |
| **Método** | Função que pertence a uma classe |
| **Namespace** | "Endereço" que organiza o código |
| **async/await** | Forma de esperar operações demoradas |
| **CORS** | Regra que permite sites acessarem APIs |
| **Swagger** | Documentação automática da API |
| **JSON** | Formato de texto para trocar dados |
| **HTTP** | Protocolo de comunicação da web |
| **GET** | Requisição para PEGAR dados |
| **POST** | Requisição para ENVIAR dados |

---

# 🎯 PRÓXIMOS PASSOS (Futuro)

Depois de completar tudo, você pode evoluir o projeto:

1. **Adicionar Banco de Dados**
   - Instalar PostgreSQL
   - Usar Dapper para consultas
   - Salvar histórico de jogos

2. **Adicionar Autenticação**
   - Sistema de login
   - Ranking de jogadores

3. **Migrar para React**
   - Componentizar a interface
   - Adicionar TypeScript

4. **Adicionar mais jogos**
   - Outros exercícios de memória
   - Diferentes sistemas de contagem

---

# ✅ CHECKLIST FINAL

- [ ] Task 1: Ambiente configurado
- [ ] Task 2: Projeto backend criado
- [ ] Task 3: Model Card criado
- [ ] Task 4: Model GameState criado
- [ ] Task 5: DeckService criado
- [ ] Task 6: GameController criado
- [ ] Task 7: Program.cs configurado
- [ ] Task 8: HTML criado
- [ ] Task 9: CSS criado
- [ ] Task 10: JavaScript criado
- [ ] Task 11: Tudo testado e funcionando!

---

**Bons estudos, Diego! 🚀**

> Lembre-se: programação se aprende FAZENDO. 
> Não tenha medo de errar, cada erro é um aprendizado!
