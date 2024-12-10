// Variables
const board = document.getElementById("game-board");
const slot1 = document.getElementById("slot-1");
const slot2 = document.getElementById("slot-2");
const scoreElement = document.getElementById("score");
const feedback = document.getElementById("feedback-message");

let cards = [];
let flippedCards = [];
let matchedCards = [];
let streak = 0;
let score = 0;
let isAnimating = false;
let timerInterval;
let timeLeft = 60;
let isScoreSaved = false; // Prevents saving final score multiple times

// Initialize game
function initializeGame(category = null) {
    fetch(`/Game/GetCategoryItems?category=${category}`)
        .then(response => response.json())
        .then(data => {
            if (data?.items) {
                startNewGame(data.items);
            } else {
                console.error("Failed to load game items.");
            }
        })
        .catch(error => console.error("Error fetching game items:", error));
}

function startNewGame(items) {
    resetGameState();
    const uniqueItems = [...new Map(items.map(item => [item.id, item])).values()]; // Ensure unique items
    cards = shuffle([...uniqueItems, ...uniqueItems]); // Create pairs and shuffle
    ensureEvenCards(); // Make sure we have an even number of cards
    setupGameBoard(cards);
    startTimer(240); // Start 4-minute timer
}

function resetGameState() {
    board.innerHTML = slot1.innerHTML = slot2.innerHTML = "";
    cards = [];
    flippedCards = [];
    matchedCards = [];
    streak = score = 0;
    isScoreSaved = false; // Reset the score-saving flag
    stopTimer();
    updateScore(0);
    showMessage("", "");
}

function shuffle(array) {
    return array.sort(() => Math.random() - 0.5); // Simple shuffle
}

function ensureEvenCards() {
    if (cards.length % 2 !== 0) {
        cards.pop(); // Remove the extra card if the total is odd
    }
}

function setupGameBoard(cards) {
    board.innerHTML = "";
    board.style.gridTemplateColumns = `repeat(auto-fit, minmax(80px, 1fr))`; // Dynamically adjust columns

    cards.forEach((item, index) => {
        const card = document.createElement("div");
        card.className = "card";
        card.dataset.index = index;

        card.innerHTML = `
            <div class="card-back"></div>
            <img class="card-front" src="${item.imageUrl}" alt="${item.name}">
        `;

        card.addEventListener("click", () => flipCard(card, index));
        board.appendChild(card);
    });

    setTimeout(hideAllCards, 5000); // Hide cards after a preview
}

function hideAllCards() {
    document.querySelectorAll(".card").forEach(card => {
        card.querySelector(".card-back").style.display = "flex";
        card.querySelector(".card-front").style.display = "none";
    });
}

function flipCard(card, index) {
    if (isAnimating || flippedCards.length >= 2 || matchedCards.includes(index)) return;

    card.querySelector(".card-back").style.display = "none";
    card.querySelector(".card-front").style.display = "block";
    flippedCards.push({ card, index });

    // Display the flipped card in the slots
    if (flippedCards.length === 1) {
        slot1.innerHTML = createCardImageHTML(index);
    } else if (flippedCards.length === 2) {
        slot2.innerHTML = createCardImageHTML(index);
        isAnimating = true;
        checkMatch();
    }
}

function createCardImageHTML(index) {
    return cards[index]?.imageUrl
        ? `<img src="${cards[index].imageUrl}" alt="${cards[index].name}">`
        : `<div>No image available</div>`;
}

function checkMatch() {
    const [first, second] = flippedCards;

    setTimeout(() => {
        if (cards[first.index].id === cards[second.index].id) {
            handleMatch(first, second);
        } else {
            handleMismatch(first, second);
        }
        flippedCards = [];
        isAnimating = false;
    }, 1000);
}

function handleMatch(first, second) {
    matchedCards.push(first.index, second.index);
    first.card.classList.add("matched");
    second.card.classList.add("matched");
    streak++;
    updateScore(10 + (streak - 1) * 2);
    showMessage("Match Found!", "success");

    if (matchedCards.length < cards.length) {
        sendScoreToServer(score); // Save intermediate score
    } else {
        endGame("You won!"); // Trigger end game when all cards are matched
    }
}

function handleMismatch(first, second) {
    [first, second].forEach(({ card }) => {
        card.querySelector(".card-back").style.display = "flex";
        card.querySelector(".card-front").style.display = "none";
    });
    streak = 0;
    updateScore(-5);
    showMessage("Try Again!", "error");
}

function updateScore(points) {
    score = Math.max(0, score + points);
    scoreElement.textContent = `Score: ${score}`;
}

function showMessage(message, type) {
    feedback.textContent = message;
    feedback.className = type || "";
}

function startTimer(duration) {
    timeLeft = duration;
    const timerElement = document.getElementById("timer-value");
    clearInterval(timerInterval);
    timerInterval = setInterval(() => {
        timeLeft--;
        if (timerElement) timerElement.textContent = timeLeft;
        if (timeLeft <= 0) endGame("Time's up!");
    }, 1000);
}

function stopTimer() {
    clearInterval(timerInterval);
}

function endGame(message) {
    showMessage(message, "error");
    stopTimer();
    disableBoard();

    // Save the final score once
    if (!isScoreSaved) {
        sendScoreToServer(score, true); // Final score
        isScoreSaved = true; // Prevent multiple saves
    }
}

function disableBoard() {
    document.querySelectorAll(".card").forEach(card => {
        card.style.pointerEvents = "none";
    });
}

function sendScoreToServer(points, isFinal = false) {
    console.log(`Saving score: ${points}, Final: ${isFinal}`); // Debugging
    const category = board.dataset.category || "default";

    fetch('/Game/SaveScore', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ points, isFinal, category }),
    })
        .then(response => {
            if (!response.ok) {
                console.error("Failed to save score");
            } else {
                console.log("Score saved successfully.");
            }
        })
        .catch(error => console.error("Error saving score:", error));
}

// Initialize game
initializeGame();
