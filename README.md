# 🃏 CardCounter - Blackjack Learning

<div align="center">

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-Supabase-4169E1?logo=postgresql)
![JavaScript](https://img.shields.io/badge/JavaScript-ES6+-F7DF1E?logo=javascript&logoColor=black)
![License](https://img.shields.io/badge/license-MIT-green)

**Aplicação web educacional para praticar contagem de cartas usando o sistema Hi-Lo do Blackjack**

[Demo](#-como-rodar-o-projeto) • [Documentação](#-estrutura-do-projeto) • [Contribuir](#-contribuindo)

</div>

---

## 📋 Índice

- [Sobre o Projeto](#-sobre-o-projeto)
- [Features](#-features)
- [Tecnologias](#-tecnologias-utilizadas)
- [Arquitetura](#-arquitetura)
- [Estrutura do Projeto](#-estrutura-do-projeto)
- [Como Rodar](#-como-rodar-o-projeto)
- [Endpoints da API](#-endpoints-da-api)
- [Banco de Dados](#-banco-de-dados)
- [Segurança](#-segurança)
- [Contribuindo](#-contribuindo)
- [Licença](#-licença)

---

## 🎯 Sobre o Projeto

**CardCounter** é uma aplicação web educacional que ensina a técnica de contagem de cartas no Blackjack usando o **sistema Hi-Lo**. O projeto foi desenvolvido como ferramenta de aprendizado para:

- ✅ Desenvolvimento backend com **C# e ASP.NET Core**
- ✅ Integração com **PostgreSQL** (Supabase)
- ✅ Desenvolvimento frontend com **JavaScript puro** (Vanilla JS)
- ✅ Arquitetura **REST API**
- ✅ Padrões de projeto (**Repository Pattern**, **Dependency Injection**)

### O Sistema Hi-Lo

| Cartas | Valor | Descrição |
|--------|-------|-----------|
| **2, 3, 4, 5, 6** | +1 | Cartas baixas (favoráveis ao dealer) |
| **7, 8, 9** | 0 | Cartas neutras |
| **10, J, Q, K, A** | -1 | Cartas altas (favoráveis ao jogador) |

**Objetivo:** Treinar o cálculo mental rápido da contagem conforme as cartas aparecem.

---

## ✨ Features

- 🎴 **Baralho completo** com 52 cartas
- 🔀 **Embaralhamento** usando algoritmo Fisher-Yates
- 📊 **Sistema de pontuação** e feedback em tempo real
- 💾 **Persistência de dados** no PostgreSQL
- 📈 **Histórico de sessões** com estatísticas
- 🎨 **Interface responsiva** e moderna
- 📖 **Documentação automática** com Swagger
- 🔒 **Segurança** com CORS configurado

---

## 🛠️ Tecnologias Utilizadas

### Backend
- **[.NET 8.0](https://dotnet.microsoft.com/)** - Framework principal
- **[ASP.NET Core](https://docs.microsoft.com/aspnet/core)** - Web API
- **[Npgsql](https://www.npgsql.org/)** - Driver PostgreSQL
- **[Dapper](https://github.com/DapperLib/Dapper)** - Micro ORM
- **[Swagger/OpenAPI](https://swagger.io/)** - Documentação

### Frontend
- **HTML5** - Estrutura
- **CSS3** - Estilização (Gradientes, Flexbox)
- **JavaScript ES6+** - Lógica (Async/Await, Fetch API)

### Banco de Dados
- **[PostgreSQL](https://www.postgresql.org/)** - Banco relacional
- **[Supabase](https://supabase.com/)** - Hosting PostgreSQL

---

## 🏗️ Arquitetura

```
┌─────────────────┐
│    Frontend     │  HTML + CSS + JavaScript
│  (CardCounterWeb)│  → Faz requisições HTTP
└────────┬────────┘
         │
         │ HTTP/REST
         ↓
┌─────────────────┐
│   Backend API   │  ASP.NET Core
│ (CardCounterAPI)│  → Processa lógica do jogo
└────────┬────────┘
         │
         │ SQL
         ↓
┌─────────────────┐
│   PostgreSQL    │  Supabase
│  (Database)     │  → Armazena sessões e jogadas
└─────────────────┘
```

**Padrões de Projeto Utilizados:**
- **Repository Pattern** - Abstração do acesso a dados
- **Dependency Injection** - Inversão de controle
- **DTO (Data Transfer Objects)** - Modelos de transferência

---

## 📁 Estrutura do Projeto

```
CardCounter/
│
├── 📖 README.md                          ← Este arquivo
├── 🛡️ .gitignore                         ← Proteção Git
│
├── 📂 CardCounterAPI/                    ← BACKEND
│   ├── 📖 README.md                      ← Documentação do backend
│   ├── Controllers/                      ← Endpoints REST
│   │   └── GameController.cs
│   ├── Models/                           ← Entidades de dados
│   │   ├── Card.cs
│   │   ├── GameState.cs
│   │   ├── GameSession.cs
│   │   └── GamePlay.cs
│   ├── Services/                         ← Lógica de negócio
│   │   ├── DeckService.cs
│   │   └── DatabaseService.cs
│   ├── Repositories/                     ← Acesso ao banco
│   │   └── GameRepository.cs
│   ├── Program.cs                        ← Configuração da API
│   ├── appsettings.json                  ← Configurações públicas
│   ├── appsettings.Development.json      ← Senhas (NÃO SUBIR!)
│   └── database_schema.sql               ← Schema SQL de referência
│
└── 📂 CardCounterWeb/                    ← FRONTEND
    ├── 📖 README.md                      ← Documentação do frontend
    ├── index.html                        ← Interface do jogo
    ├── style.css                         ← Estilos visuais
    └── script.js                         ← Lógica e comunicação com API
```

### Responsabilidades

| Camada | Responsabilidade |
|--------|------------------|
| **Frontend** | Interface do usuário, validações básicas, requisições HTTP |
| **Controllers** | Receber requisições, validar entrada, chamar serviços |
| **Services** | Lógica de negócio (embaralhamento, contagem, regras) |
| **Repositories** | Acesso ao banco de dados (queries SQL) |
| **Models** | Estrutura dos dados (classes C#) |

---

## 🚀 Como Rodar o Projeto

### 📋 Pré-requisitos

- [.NET SDK 8.0+](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/) ou conta no [Supabase](https://supabase.com/)
- Navegador web moderno

### 1️⃣ Clonar o Repositório

```bash
git clone https://github.com/seu-usuario/cardcounter.git
cd cardcounter
```

### 2️⃣ Configurar o Banco de Dados

**Opção A - Supabase (Recomendado):**
1. Crie uma conta gratuita em [supabase.com](https://supabase.com)
2. Crie um novo projeto
3. Copie a connection string

**Opção B - PostgreSQL Local:**
1. Instale o PostgreSQL
2. Crie um banco de dados: `CREATE DATABASE cardcounter;`

### 3️⃣ Configurar Connection String

Edite o arquivo `CardCounterAPI/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "SupabaseConnection": "Host=SEU_HOST;Port=5432;Database=postgres;Username=SEU_USER;Password=SUA_SENHA;SSL Mode=Require"
  }
}
```

### 4️⃣ Rodar o Backend

```bash
cd CardCounterAPI
dotnet restore
dotnet run
```

A API estará disponível em: **http://localhost:5063**

Swagger em: **http://localhost:5063/swagger**

### 5️⃣ Abrir o Frontend

**Opção A - Live Server (VS Code):**
1. Instale a extensão "Live Server"
2. Clique direito em `CardCounterWeb/index.html`
3. Selecione "Open with Live Server"

**Opção B - Navegador direto:**
Abra o arquivo `CardCounterWeb/index.html` no navegador

### 6️⃣ Jogar! 🎮

1. Clique em **"Iniciar Novo Jogo"**
2. Clique em **"Próxima Carta"**
3. Calcule a contagem mentalmente
4. Digite e verifique sua resposta
5. Acompanhe sua pontuação!

---

## 📡 Endpoints da API

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| **GET** | `/api/game/start` | Inicia um novo jogo |
| **GET** | `/api/game/card` | Retorna a próxima carta |
| **POST** | `/api/game/guess` | Envia palpite do jogador |
| **GET** | `/api/game/state` | Retorna estado atual do jogo |
| **GET** | `/api/game/history` | Histórico de sessões |

**Exemplo de uso:**

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

📖 **Documentação completa:** http://localhost:5063/swagger

---

## 🗄️ Banco de Dados

### Schema

**Tabela `players`**
```sql
id SERIAL PRIMARY KEY
username VARCHAR(50) UNIQUE NOT NULL
email VARCHAR(100) UNIQUE
created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
```

**Tabela `game_sessions`**
```sql
id SERIAL PRIMARY KEY
player_id INTEGER REFERENCES players(id)
started_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
ended_at TIMESTAMP
total_cards_shown INTEGER DEFAULT 0
total_correct_guesses INTEGER DEFAULT 0
total_wrong_guesses INTEGER DEFAULT 0
final_score INTEGER DEFAULT 0
is_active BOOLEAN DEFAULT true
```

**Tabela `game_plays`**
```sql
id SERIAL PRIMARY KEY
session_id INTEGER REFERENCES game_sessions(id)
card_name VARCHAR(3) NOT NULL
card_suit VARCHAR(10) NOT NULL
card_count_value INTEGER NOT NULL
correct_count_at_moment INTEGER NOT NULL
player_guess INTEGER
was_correct BOOLEAN
played_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
```

### Diagrama de Relacionamento

```
players (1) ─────< (N) game_sessions (1) ─────< (N) game_plays
```

---


## 📚 Recursos de Aprendizado

### C# e .NET
- [Documentação oficial .NET](https://learn.microsoft.com/dotnet/)
- [Tutorial ASP.NET Core](https://learn.microsoft.com/aspnet/core)

### PostgreSQL
- [PostgreSQL Tutorial](https://www.postgresqltutorial.com/)
- [Supabase Docs](https://supabase.com/docs)

### JavaScript
- [MDN Web Docs](https://developer.mozilla.org/pt-BR/)
- [JavaScript.info](https://javascript.info/)

---

## 📄 Licença

Este projeto é de código aberto e está disponível sob a licença MIT.

---

## 👨‍💻 Autor

**Diego**

Projeto desenvolvido para aprendizado de C#, ASP.NET Core, PostgreSQL e desenvolvimento web full-stack.

---

<div align="center">

**🃏 Bons estudos e boa contagem! 🎲**

⭐ Se este projeto  teajudou, considere dar uma estrela!

</div>
