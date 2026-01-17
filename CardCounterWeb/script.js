// ==========================================
// CONFIGURAÇÃO
// ==========================================

// URL base da API
const API_URL = 'http://localhost:5063/api/game';

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
