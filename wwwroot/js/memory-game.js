// Variables
const board = document.getElementById("game-board");
const slot1 = document.getElementById("slot-1");
const slot2 = document.getElementById("slot-2");

let cards = []; // Array of card objects
let flippedCards = []; // Cards currently flipped
let matchedCards = []; // Track matched cards
let streak = 0; // Tracks consecutive correct guesses
let score = 0; // Tracks the player's score
let isAnimating = false; // Prevents multiple clicks during animations
let hasFinalScoreSaved = false; // Tracks if the final score is saved

// Initialize game
function initializeGame() {
    hasFinalScoreSaved = false; // Reset final score flag for new game
    fetch("/Game/GetRandomCategory")
        .then(response => response.json())
        .then(data => {
            if (data && data.items) {
                cards = [...data.items, ...data.items]; // Create pairs
                setupGameBoard(cards);
            } else {
                console.error("Failed to load game items.");
            }
        })
        .catch(error => console.error("Error fetching game items:", error));
}

// Shuffle array
function shuffle(array) {
    for (let i = array.length - 1; i > 0; i--) {
        const j = Math.floor(Math.random() * (i + 1));
        [array[i], array[j]] = [array[j], array[i]];
    }
    return array;
}

function setupGameBoard(items) {
    board.innerHTML = ""; // Clear the game board
    slot1.innerHTML = slot2.innerHTML = ""; // Clear top slots

    // Select up to 8 unique items from the category
    const uniqueItems = items.slice(0, 8);

    // Duplicate and shuffle selected items
    cards = shuffle([...uniqueItems, ...uniqueItems]);

    // Render cards
    cards.forEach((item, index) => {
        const card = document.createElement("div");
        card.classList.add("card");
        card.dataset.index = index;

        // Add back and front images
        card.innerHTML = `
            <div class="card-back"></div>
            <img class="card-front" src="${item.imageUrl}" alt="${item.name}" style="display: block;"> <!-- Show images initially -->
        `;

        card.addEventListener("click", () => flipCard(card, index));
        board.appendChild(card);
    });

    // Add a 3-second delay to hide all images after the preview
    setTimeout(() => {
        hideAllCards();
    }, 5000);
}

function hideAllCards() {
    const allCards = document.querySelectorAll(".card");
    allCards.forEach(card => {
        card.querySelector(".card-back").style.display = "flex"; // Show back of the card
        card.querySelector(".card-front").style.display = "none"; // Hide front of the card
    });
}

function flipCard(card, index) {
    if (isAnimating || flippedCards.length >= 2 || matchedCards.includes(index) || flippedCards.some(c => c.index === index)) {
        return; // Prevent interaction during animations or invalid clicks
    }

    // Flip card
    card.querySelector(".card-back").style.display = "none";
    card.querySelector(".card-front").style.display = "block";
    flippedCards.push({ card, index });

    // Display in top slots
    if (flippedCards.length === 1) {
        slot1.innerHTML = `<img src="${cards[index].imageUrl}" alt="${cards[index].name}">`;
    } else if (flippedCards.length === 2) {
        slot2.innerHTML = `<img src="${cards[index].imageUrl}" alt="${cards[index].name}">`;

        // Check for match
        isAnimating = true; // Block further interactions until animations complete
        setTimeout(checkMatch, 1000);
    }
}

function checkMatch() {
    const [first, second] = flippedCards;

    if (!first || !second) {
        console.error("Error: Flipped cards not properly set.");
        resetInteraction();
        return;
    }

    // Check if the cards match
    if (cards[first.index].id === cards[second.index].id) {
        // Cards match
        matchedCards.push(first.index, second.index);

        first.card.classList.add("matched");
        second.card.classList.add("matched");

        slot1.innerHTML = slot2.innerHTML = ""; // Clear top slots
        showMessage("Match Found!", "success");

        // Add streak bonus and update score
        streak++;
        const pointsToAdd = 10 + (streak - 1) * 2; // Bonus 2 points for each consecutive match
        updateScore(pointsToAdd);
        sendScoreToServer(score); // Save incremental score

        // Check if the game is completed
        if (matchedCards.length === cards.length) {
            setTimeout(() => {
                showMessage("You won!", "success");
                sendScoreToServer(score, true); // Finalize the score
                resetInteraction();
            }, 500);
        } else {
            resetInteraction();
        }
    } else {
        // Cards do not match
        setTimeout(() => {
            first.card.querySelector(".card-back").style.display = "flex";
            first.card.querySelector(".card-front").style.display = "none";

            second.card.querySelector(".card-back").style.display = "flex";
            second.card.querySelector(".card-front").style.display = "none";

            slot1.innerHTML = slot2.innerHTML = ""; // Clear top slots
            showMessage("Try Again!", "error");

            // Deduct points for incorrect match and reset streak
            streak = 0;
            updateScore(-5); // Deduct 5 points for incorrect guess

            resetInteraction();
        }, 1000);
    }

    flippedCards = [];
}

function updateScore(points) {
    score += points;
    if (score < 0) score = 0; // Prevent negative scores

    const scoreElement = document.getElementById("score");
    if (scoreElement) {
        scoreElement.textContent = `Score: ${score}`;
    } else {
        console.error("Score element not found!");
    }
}

function resetInteraction() {
    isAnimating = false; // Allow interactions again
}

function showMessage(message, type) {
    const feedback = document.getElementById("feedback-message");
    if (!feedback) {
        console.error("Feedback element not found!");
        return;
    }
    feedback.textContent = message;
    feedback.className = type; // Apply 'success' or 'error' class
}

function sendScoreToServer(points, isFinal = false) {
    if (isAnimating) return; // Prevent duplicate submissions during animations

    const category = document.getElementById("game-board").dataset.category; // Retrieve category from the game board
    if (!category) {
        console.error("Category not found");
        return;
    }

    fetch('/Game/SaveScore', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'X-CSRF-TOKEN': document.querySelector('input[name="__RequestVerificationToken"]')?.value || ''
        },
        body: JSON.stringify({ points, isFinal, category }) // Include category
    })
        .then(response => {
            if (!response.ok) {
                console.error("Failed to save score");
            } else {
                console.log("Score saved successfully!");
            }
        })
        .catch(error => console.error("Error saving score:", error));
}



// Initialize
initializeGame();
