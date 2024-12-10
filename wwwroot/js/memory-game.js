// Variables
const board = document.getElementById("game-board");
const slot1 = document.getElementById("slot-1");
const slot2 = document.getElementById("slot-2");

let cards = [];
let flippedCards = [];
let matchedCards = [];
let streak = 0;
let score = 0;
let isAnimating = false;
let hasFinalScoreSaved = false;

let timerInterval;
let timeLeft = 60; // Default time for the game

// Initialize game
function initializeGame(category = null) {
    fetch(`/Game/GetCategoryItems?category=${category}`)
        .then(response => response.json())
        .then(data => {
            if (data && data.items) {
                resetGameState();
                const uniqueItems = data.items.filter(
                    (item, index, self) => index === self.findIndex(i => i.id === item.id)
                );
                cards = shuffle([...uniqueItems, ...uniqueItems]);
                setupGameBoard(cards);
                startTimer(240);
            } else {
                console.error("Failed to load game items.");
            }
        })
        .catch(error => console.error("Error fetching game items:", error));
}

function resetGameState() {
    board.innerHTML = "";
    slot1.innerHTML = "";
    slot2.innerHTML = "";
    cards = [];
    flippedCards = [];
    matchedCards = [];
    streak = 0;
    score = 0;
    hasFinalScoreSaved = false;
    stopTimer();
}

function shuffle(array) {
    for (let i = array.length - 1; i > 0; i--) {
        const j = Math.floor(Math.random() * (i + 1));
        [array[i], array[j]] = [array[j], array[i]];
    }
    return array;
}

function setupGameBoard(items) {
    board.innerHTML = "";
    slot1.innerHTML = slot2.innerHTML = "";

    // Ensure we have 8 unique items for matching
    const uniqueItems = items.slice(0, 8); // Limit to 8 unique items

    // Duplicate the unique items to create pairs
    const pairs = [...uniqueItems, ...uniqueItems];

    // Shuffle the pairs
    cards = shuffle(pairs);

    // Render cards
    cards.forEach((item, index) => {
        const card = document.createElement("div");
        card.classList.add("card");
        card.dataset.index = index;

        card.innerHTML = `
            <div class="card-back"></div>
            <img class="card-front" src="${item.imageUrl}" alt="${item.name}" style="display: block;">
        `;

        card.addEventListener("click", () => flipCard(card, index));
        board.appendChild(card);
    });

    setTimeout(() => {
        hideAllCards();
    }, 3000);
}



function hideAllCards() {
    const allCards = document.querySelectorAll(".card");
    allCards.forEach(card => {
        card.querySelector(".card-back").style.display = "flex";
        card.querySelector(".card-front").style.display = "none";
    });
}

function flipCard(card, index) {
    if (isAnimating || flippedCards.length >= 2 || matchedCards.includes(index) || flippedCards.some(c => c.index === index)) {
        return;
    }

    card.querySelector(".card-back").style.display = "none";
    card.querySelector(".card-front").style.display = "block";
    flippedCards.push({ card, index });

    if (flippedCards.length === 1) {
        slot1.innerHTML = `<img src="${cards[index].imageUrl}" alt="${cards[index].name}">`;
    } else if (flippedCards.length === 2) {
        slot2.innerHTML = `<img src="${cards[index].imageUrl}" alt="${cards[index].name}">`;
        isAnimating = true;
        setTimeout(checkMatch, 1000);
    }
}

function checkMatch() {
    const [first, second] = flippedCards;

    if (cards[first.index].id === cards[second.index].id) {
        matchedCards.push(first.index, second.index);
        first.card.classList.add("matched");
        second.card.classList.add("matched");
        slot1.innerHTML = slot2.innerHTML = "";
        showMessage("Match Found!", "success");
        streak++;
        updateScore(10 + (streak - 1) * 2);

        if (!hasFinalScoreSaved && matchedCards.length < cards.length) {
            sendScoreToServer(score);
        }

        if (matchedCards.length === cards.length) {
            stopTimer();
            endGame();
        } else {
            resetInteraction();
        }
    } else {
        setTimeout(() => {
            first.card.querySelector(".card-back").style.display = "flex";
            first.card.querySelector(".card-front").style.display = "none";
            second.card.querySelector(".card-back").style.display = "flex";
            second.card.querySelector(".card-front").style.display = "none";
            slot1.innerHTML = slot2.innerHTML = "";
            showMessage("Try Again!", "error");
            streak = 0;
            updateScore(-5);
            resetInteraction();
        }, 1000);
    }

    flippedCards = [];
}

function updateScore(points) {
    score += points;
    if (score < 0) score = 0;

    const scoreElement = document.getElementById("score");
    if (scoreElement) {
        scoreElement.textContent = `Score: ${score}`;
    }
}

function resetInteraction() {
    isAnimating = false;
}

function showMessage(message, type) {
    const feedback = document.getElementById("feedback-message");
    if (feedback) {
        feedback.textContent = message;
        feedback.className = type;
    }
}

function sendScoreToServer(points, isFinal = false) {
    if (isFinal && hasFinalScoreSaved) return;

    const category = document.getElementById("game-board").dataset.category;
    if (!category) {
        console.error("Category not found");
        return;
    }

    fetch('/Game/SaveScore', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ points, isFinal, category })
    })
        .then(response => {
            if (!response.ok) {
                console.error("Failed to save score");
            } else if (isFinal) {
                hasFinalScoreSaved = true;
            }
        })
        .catch(error => console.error("Error saving score:", error));
}

function startTimer(duration) {
    timeLeft = duration;
    const timerElement = document.getElementById("timer-value");

    if (timerElement) {
        timerElement.textContent = timeLeft;
    }

    clearInterval(timerInterval);

    timerInterval = setInterval(() => {
        timeLeft--;
        if (timerElement) {
            timerElement.textContent = timeLeft;
        }

        if (timeLeft <= 0) {
            clearInterval(timerInterval);
            endGame();
        }
    }, 1000);
}

function stopTimer() {
    clearInterval(timerInterval);
}

function endGame() {
    if (hasFinalScoreSaved) return;

    showMessage(timeLeft <= 0 ? "Time's up!" : "Game Over!", "error");
    stopTimer();
    disableBoard();
    sendScoreToServer(score, true);
}

function disableBoard() {
    const allCards = document.querySelectorAll(".card");
    allCards.forEach(card => {
        card.style.pointerEvents = "none";
    });
}

// Initialize game
initializeGame();
