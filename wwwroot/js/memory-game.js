// Variables
const board = document.getElementById("game-board");
const slot1 = document.getElementById("slot-1");
const slot2 = document.getElementById("slot-2");

let cards = []; // Array of card objects (image paths)
let flippedCards = []; // Cards currently flipped
let matchedCards = []; // Track matched cards

// Initialize game
function initializeGame() {
    // Duplicate and shuffle the card images
    const images = [
        "/images/dog.png",
        "/images/cat.png",
        "/images/apple.png",
        "/images/banana.png",
    ];
    cards = [...images, ...images, ...images, ...images];
    cards = shuffle(cards);

    // Render cards
    cards.forEach((image, index) => {
        const card = document.createElement("div");
        card.classList.add("card");
        card.dataset.index = index;

        // Add back and front images
        card.innerHTML = `
    <div class="card-back"></div>
    <img class="card-front" src="${image}" alt="Card Image" style="display: none;">
    `;


        card.addEventListener("click", () => flipCard(card, index));
        board.appendChild(card);
    });
    

    

}

// Shuffle array
function shuffle(array) {
    for (let i = array.length - 1; i > 0; i--) {
        const j = Math.floor(Math.random() * (i + 1));
        [array[i], array[j]] = [array[j], array[i]];
    }
    return array;
}

let isAnimating = false; // Track if the game is waiting for animations

function flipCard(card, index) {
    if (isAnimating || flippedCards.length >= 2 || matchedCards.includes(index) || flippedCards.some(c => c.index === index)) {
        return; // Block clicking during animation, on matched cards, or on the same card
    }

    // Flip card
    card.querySelector(".card-back").style.display = "none"; // Hide the back
    card.querySelector(".card-front").style.display = "block"; // Show the front
    flippedCards.push({ card, index });

    // Display in top slots
    if (flippedCards.length === 1) {
        slot1.innerHTML = `<img src="${cards[index]}" alt="Selected Card">`;
    } else if (flippedCards.length === 2) {
        slot2.innerHTML = `<img src="${cards[index]}" alt="Selected Card">`;

        // Check for match
        isAnimating = true; // Block interactions until match/mismatch is resolved
        setTimeout(checkMatch, 1000);
    }
}

function checkMatch() {
    const [first, second] = flippedCards;

    if (first.index === second.index) {
        // Same card clicked twice, reset
        setTimeout(() => {
            first.card.querySelector(".card-back").style.display = "flex"; // Show the back
            first.card.querySelector(".card-front").style.display = "none"; // Hide the front
            resetInteraction();
        }, 500);
        showMessage("You clicked the same card!", "error");
    } else if (cards[first.index] === cards[second.index]) {
        // Cards match
        matchedCards.push(first.index, second.index);

        // Add matched class
        first.card.classList.add("matched");
        second.card.classList.add("matched");

        slot1.innerHTML = slot2.innerHTML = ""; // Clear slots
        showMessage("Match Found!", "success");
        resetInteraction();
    } else {
        // Cards do not match
        setTimeout(() => {
            first.card.querySelector(".card-back").style.display = "flex"; // Show the back
            first.card.querySelector(".card-front").style.display = "none"; // Hide the front

            second.card.querySelector(".card-back").style.display = "flex"; // Show the back
            second.card.querySelector(".card-front").style.display = "none"; // Hide the front

            slot1.innerHTML = slot2.innerHTML = ""; // Clear slots
            showMessage("Try Again!", "error");
            resetInteraction();
        }, 1000);
    }

    flippedCards = [];
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


// Initialize
initializeGame();
