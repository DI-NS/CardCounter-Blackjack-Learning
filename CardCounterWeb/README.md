# 🎨 CardCounter Web - Frontend

<div align="center">

![HTML5](https://img.shields.io/badge/HTML5-E34F26?logo=html5&logoColor=white)
![CSS3](https://img.shields.io/badge/CSS3-1572B6?logo=css3&logoColor=white)
![JavaScript](https://img.shields.io/badge/JavaScript-ES6+-F7DF1E?logo=javascript&logoColor=black)

**Interface web moderna para o jogo CardCounter**

</div>

---

## 📋 Índice

- [Sobre](#-sobre)
- [Estrutura de Arquivos](#-estrutura-de-arquivos)
- [Arquitetura Frontend](#-arquitetura-frontend)
- [HTML - Estrutura](#-html---estrutura)
- [CSS - Estilização](#-css---estilização)
- [JavaScript - Lógica](#-javascript---lógica)
- [Comunicação com API](#-comunicação-com-api)
- [Fluxo de Dados](#-fluxo-de-dados)
- [Como Rodar](#-como-rodar)
- [Customização](#-customização)

---

## 🎯 Sobre

O frontend do **CardCounter** é uma aplicação **Single Page Application (SPA)** construída com tecnologias web puras (HTML, CSS e JavaScript), sem frameworks ou bibliotecas externas.

### Responsabilidades do Frontend

✅ **Exibir a interface** do jogo de forma visual e responsiva  
✅ **Capturar ações do usuário** (cliques, digitação)  
✅ **Comunicar com a API** via requisições HTTP  
✅ **Atualizar a interface** dinamicamente com os dados recebidos  
✅ **Validar entradas** do usuário antes de enviar para API  
✅ **Fornecer feedback** visual (acertos, erros, pontuação)  

### Por que Vanilla JavaScript?

- 🎯 **Simplicidade** - Foco no aprendizado, sem curva de aprendizado de frameworks
- ⚡ **Performance** - Sem overhead de frameworks (React, Vue, etc.)
- 📦 **Sem build** - Não precisa de Webpack, Babel, npm
- 🔍 **Transparência** - Código fácil de entender e debugar

---

## 📁 Estrutura de Arquivos

```
CardCounterWeb/
│
├── 📖 README.md          ← Este arquivo
├── 📄 index.html         ← Estrutura da página (DOM)
├── 🎨 style.css          ← Estilos visuais
└── ⚡ script.js          ← Lógica e comunicação com API
```

**Apenas 3 arquivos!** Simplicidade é a chave.

---

## 🏗️ Arquitetura Frontend

```
┌─────────────────────────────────────────────┐
│              index.html                     │
│         (Estrutura DOM)                     │
│  - Elementos da interface                   │
│  - Botões, inputs, divs                     │
└──────────────┬──────────────────────────────┘
               │
               ├──> style.css (Visual)
               │    - Cores, fontes, layout
               │    - Gradientes, sombras
               │    - Responsividade
               │
               └──> script.js (Lógica)
                    - Manipulação do DOM
                    - Requisições HTTP (fetch)
                    - Event listeners
                    - Validações
                    │
                    ↓
              ┌─────────────┐
              │   Backend   │
              │   (API)     │
              └─────────────┘
```

---

## 📄 HTML - Estrutura

O arquivo `index.html` define a **estrutura semântica** da página.

### Elementos Principais

```html
<!-- Cabeçalho com título -->
<header>
    <h1>Card Counter</h1>
    <p>Pratique contagem de cartas no Blackjack!</p>
</header>

<!-- Informações do jogo -->
<div class="game-info">
    <div class="info-box">
        <span class="label">Cartas Vistas</span>
        <span id="cards-shown">0</span>  ← Atualizado via JS
    </div>
    <!-- ... -->
</div>

<!-- Carta atual -->
<div class="card-area">
    <div id="card-display" class="card">
        <span class="card-placeholder">?</span>
    </div>
</div>

<!-- Input do jogador -->
<div class="answer-area">
    <input type="number" id="guess-input" placeholder="Digite sua resposta">
    <button onclick="submitGuess()">Verificar</button>
    <button onclick="getNextCard()">Próxima Carta</button>
</div>

<!-- Feedback -->
<div id="feedback" class="feedback"></div>

<!-- Botão iniciar -->
<button onclick="startGame()">Iniciar Novo Jogo</button>

<!-- Tabela de referência Hi-Lo -->
<aside class="reference">
    <h3>Tabela Hi-Lo</h3>
    <table>
        <!-- ... -->
    </table>
</aside>
```

### IDs Importantes

| ID | Elemento | Responsabilidade |
|----|----------|------------------|
| `cards-shown` | `<span>` | Exibe quantas cartas foram mostradas |
| `cards-remaining` | `<span>` | Exibe quantas cartas restam |
| `score` | `<span>` | Exibe pontuação do jogador |
| `card-display` | `<div>` | Mostra a carta atual (visual) |
| `guess-input` | `<input>` | Campo onde jogador digita |
| `feedback` | `<div>` | Mostra se acertou ou errou |

**Por que IDs?** JavaScript usa `document.getElementById()` para manipular esses elementos.

---

## 🎨 CSS - Estilização

O arquivo `style.css` define a **aparência visual** da aplicação.

### Técnicas CSS Utilizadas

#### 1. **Reset CSS**
```css
* {
    margin: 0;
    padding: 0;
    box-sizing: border-box;
}
```
Remove estilos padrão do navegador para consistência.

#### 2. **Gradiente de Fundo**
```css
body {
    background: linear-gradient(135deg, #1a1a2e 0%, #16213e 100%);
    min-height: 100vh;
}
```
Cria fundo escuro com gradiente diagonal.

#### 3. **Flexbox para Layout**
```css
.game-info {
    display: flex;
    justify-content: space-around;
}
```
Distribui os elementos de forma responsiva.

#### 4. **Sombras e Bordas Arredondadas**
```css
.card {
    border-radius: 10px;
    box-shadow: 0 10px 30px rgba(0, 0, 0, 0.3);
}
```
Dá profundidade visual aos elementos.

#### 5. **Classes Dinâmicas**
```css
/* Carta vermelha (copas e ouros) */
.card.red {
    color: #ef4444;
}

/* Carta preta (paus e espadas) */
.card.black {
    color: #1a1a2e;
}

/* Feedback de acerto */
.feedback.correct {
    background: rgba(74, 222, 128, 0.2);
    color: #4ade80;
}

/* Feedback de erro */
.feedback.wrong {
    background: rgba(239, 68, 68, 0.2);
    color: #ef4444;
}
```

**JavaScript adiciona/remove essas classes dinamicamente!**

#### 6. **Transições e Hover**
```css
button {
    transition: transform 0.2s, background 0.2s;
}

button:hover {
    transform: scale(1.05);  /* Cresce 5% ao passar mouse */
}
```

---

## ⚡ JavaScript - Lógica

O arquivo `script.js` contém toda a **lógica da aplicação**.

### Estrutura do Código

```javascript
// ==========================================
// CONFIGURAÇÃO
// ==========================================
const API_URL = 'http://localhost:5063/api/game';

// ==========================================
// FUNÇÕES AUXILIARES
// ==========================================
function updateElement(id, value) { ... }
function getSuitSymbol(suit) { ... }
function isRedSuit(suit) { ... }

// ==========================================
// FUNÇÕES DO JOGO
// ==========================================
async function startGame() { ... }
async function getNextCard() { ... }
async function submitGuess() { ... }

// ==========================================
// EVENTOS
// ==========================================
document.getElementById('guess-input').addEventListener('keypress', ...);
```

---

### Funções Principais

#### 1. **startGame()** - Iniciar Jogo

```javascript
async function startGame() {
    try {
        // 1. Faz requisição GET para /api/game/start
        const response = await fetch(`${API_URL}/start`);
        
        // 2. Converte resposta JSON em objeto JavaScript
        const data = await response.json();
        
        // 3. Atualiza elementos da tela
        updateElement('cards-shown', data.cardsShown);        // 0
        updateElement('cards-remaining', data.cardsRemaining); // 52
        updateElement('score', data.score);                    // 0
        
        // 4. Limpa a carta e mostra mensagem
        document.getElementById('card-display').innerHTML = 
            '<span class="card-placeholder">?</span>';
        document.getElementById('feedback').textContent = 
            'Jogo iniciado! Clique em "Próxima Carta"';
            
    } catch (error) {
        console.error('Erro ao iniciar:', error);
        alert('Erro ao conectar com a API!');
    }
}
```

**O que acontece:**
1. Faz requisição HTTP GET para a API
2. Espera a resposta (`await`)
3. Converte JSON para objeto JavaScript
4. Atualiza os elementos HTML com os valores

---

#### 2. **getNextCard()** - Pegar Próxima Carta

```javascript
async function getNextCard() {
    try {
        // 1. Requisição GET para /api/game/card
        const response = await fetch(`${API_URL}/card`);
        
        // 2. Verifica se houve erro (baralho acabou)
        if (!response.ok) {
            const error = await response.text();
            document.getElementById('feedback').textContent = error;
            return;
        }
        
        // 3. Converte resposta
        const data = await response.json();
        
        // 4. Atualiza informações
        updateElement('cards-shown', data.cardsShown);
        updateElement('cards-remaining', data.cardsRemaining);
        
        // 5. Mostra a carta visualmente
        const card = data.currentCard;
        const symbol = getSuitSymbol(card.suit);     // ♥, ♦, ♣, ♠
        const color = isRedSuit(card.suit) ? 'red' : 'black';
        
        document.getElementById('card-display').innerHTML = `
            <div>
                <div style="font-size: 2rem;">${card.name}</div>
                <div style="font-size: 3rem;">${symbol}</div>
            </div>
        `;
        
        // Adiciona classe CSS para cor
        document.getElementById('card-display').className = `card ${color}`;
        
    } catch (error) {
        console.error('Erro ao pegar carta:', error);
    }
}
```

**Conversão de Naipes:**

```javascript
function getSuitSymbol(suit) {
    const suits = {
        'hearts': '♥',
        'diamonds': '♦',
        'clubs': '♣',
        'spades': '♠'
    };
    return suits[suit] || '?';
}

function isRedSuit(suit) {
    return suit === 'hearts' || suit === 'diamonds';
}
```

---

#### 3. **submitGuess()** - Enviar Palpite

```javascript
async function submitGuess() {
    // 1. Pega o valor digitado
    const input = document.getElementById('guess-input');
    const guess = parseInt(input.value);
    
    // 2. Valida
    if (isNaN(guess)) {
        alert('Digite um número!');
        return;
    }
    
    try {
        // 3. Requisição POST para /api/game/guess
        const response = await fetch(`${API_URL}/guess`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(guess)  // Converte para JSON
        });
        
        // 4. Recebe resposta
        const data = await response.json();
        
        // 5. Mostra feedback visual
        const feedback = document.getElementById('feedback');
        feedback.textContent = `${data.message} (Resposta correta: ${data.correctAnswer})`;
        
        // Adiciona classe CSS (correct ou wrong)
        feedback.className = `feedback ${data.isCorrect ? 'correct' : 'wrong'}`;
        
        // 6. Atualiza pontuação
        updateElement('score', data.score);
        
        // 7. Limpa input
        input.value = '';
        
    } catch (error) {
        console.error('Erro ao enviar palpite:', error);
    }
}
```

---

## 📡 Comunicação com API

### Fetch API

O frontend usa a **Fetch API** nativa do JavaScript para comunicação HTTP.

#### GET Request (Buscar dados)

```javascript
const response = await fetch('http://localhost:5063/api/game/start');
const data = await response.json();
```

#### POST Request (Enviar dados)

```javascript
const response = await fetch('http://localhost:5063/api/game/guess', {
    method: 'POST',
    headers: {
        'Content-Type': 'application/json'  // Diz que está enviando JSON
    },
    body: JSON.stringify(guess)  // Converte número para JSON
});
const data = await response.json();
```

### Async/Await

**Por que usar?**

```javascript
// ❌ SEM async/await (callback hell)
fetch(url)
    .then(response => response.json())
    .then(data => {
        console.log(data);
    })
    .catch(error => {
        console.error(error);
    });

// ✅ COM async/await (mais limpo)
async function getData() {
    try {
        const response = await fetch(url);
        const data = await response.json();
        console.log(data);
    } catch (error) {
        console.error(error);
    }
}
```

---

## 🔄 Fluxo de Dados

### 1️⃣ Usuário Inicia Jogo

```
[USUÁRIO] Clica "Iniciar Novo Jogo"
    ↓
[script.js] startGame() é chamada
    ↓
[Fetch API] GET http://localhost:5063/api/game/start
    ↓
[API Backend] Cria baralho, retorna estado inicial
    ↓
[script.js] Recebe JSON { cardsShown: 0, cardsRemaining: 52, ... }
    ↓
[script.js] updateElement('cards-shown', 0)
    ↓
[DOM] <span id="cards-shown">0</span>  ← Atualizado!
    ↓
[USUÁRIO] Vê "Cartas Vistas: 0" na tela
```

---

### 2️⃣ Usuário Pega Carta

```
[USUÁRIO] Clica "Próxima Carta"
    ↓
[script.js] getNextCard() é chamada
    ↓
[Fetch API] GET http://localhost:5063/api/game/card
    ↓
[API Backend] Retorna { currentCard: { name: "5", suit: "hearts", ... } }
    ↓
[script.js] Recebe dados da carta
    ↓
[script.js] getSuitSymbol('hearts') → '♥'
[script.js] isRedSuit('hearts') → true
    ↓
[script.js] innerHTML = '<div>5</div><div>♥</div>'
[script.js] className = 'card red'
    ↓
[CSS] .card.red { color: #ef4444; }  ← Aplica cor vermelha
    ↓
[USUÁRIO] Vê carta 5♥ em vermelho na tela
```

---

### 3️⃣ Usuário Envia Palpite

```
[USUÁRIO] Digita "3" no input e clica "Verificar"
    ↓
[script.js] submitGuess() é chamada
    ↓
[script.js] parseInt(input.value) → 3
    ↓
[Fetch API] POST http://localhost:5063/api/game/guess
            Body: 3 (convertido para JSON)
    ↓
[API Backend] Compara 3 com contagem correta
               Retorna { isCorrect: true, message: "Muito bem!", ... }
    ↓
[script.js] Recebe resultado
    ↓
[script.js] feedback.textContent = "Muito bem! (Resposta correta: 3)"
[script.js] feedback.className = "feedback correct"
    ↓
[CSS] .feedback.correct { background: green; }  ← Aplica estilo
    ↓
[USUÁRIO] Vê mensagem verde "Muito bem!"
```

---

## 🚀 Como Rodar

### Opção 1: Live Server (Recomendado)

1. Instale a extensão **Live Server** no VS Code
2. Clique com botão direito em `index.html`
3. Selecione **"Open with Live Server"**
4. Abrirá automaticamente em `http://localhost:5500`

### Opção 2: Navegador Direto

1. Abra o arquivo `index.html` diretamente no navegador
2. URL será algo como: `file:///C:/Users/.../index.html`

**⚠️ Nota:** Certifique-se de que a API está rodando em `http://localhost:5063`!

---

## 🎨 Customização

### Mudar a URL da API

Em `script.js`, linha 6:

```javascript
const API_URL = 'http://localhost:5063/api/game';
// Mude para o endereço da sua API
```

### Mudar Cores

Em `style.css`:

```css
/* Cor de fundo */
body {
    background: linear-gradient(135deg, #1a1a2e 0%, #16213e 100%);
}

/* Cor de acerto */
.feedback.correct {
    color: #4ade80;  /* Verde */
}

/* Cor de erro */
.feedback.wrong {
    color: #ef4444;  /* Vermelho */
}
```

### Adicionar Sons

```javascript
async function submitGuess() {
    // ...
    if (data.isCorrect) {
        new Audio('/sounds/correct.mp3').play();
    } else {
        new Audio('/sounds/wrong.mp3').play();
    }
}
```

---

## 📚 Recursos de Aprendizado

### HTML
- [MDN - HTML](https://developer.mozilla.org/pt-BR/docs/Web/HTML)

### CSS
- [MDN - CSS](https://developer.mozilla.org/pt-BR/docs/Web/CSS)
- [CSS Tricks](https://css-tricks.com/)

### JavaScript
- [MDN - JavaScript](https://developer.mozilla.org/pt-BR/docs/Web/JavaScript)
- [JavaScript.info](https://javascript.info/)

### Fetch API
- [MDN - Fetch API](https://developer.mozilla.org/pt-BR/docs/Web/API/Fetch_API)

---

<div align="center">

**Desenvolvido usando HTML, CSS e JavaScript puro**

</div>
