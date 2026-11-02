# Underpin-Assignment

## Game Overview

This is a 2D Slot Machine game developed using **Unity 6.3 LTS** as part of a game development internship assignment.

The game features three spinning reels, each displaying symbols such as Seven, Cherries, Bell, and BAR. The player starts with 100 coins and spends 10 coins on every spin.

The objective is to get three matching symbols in the **middle row** across all three reels to win.

### Game Rules

- Starting balance: 100 coins
- Cost per spin: 10 coins
- Winning reward: 50 coins
- A win occurs when all three middle-row symbols match.
- If the player's balance reaches zero, a losing message is displayed and the balance resets to 100 coins.

## Instructions to Run WebGL Build

The game is hosted on **itch.io** and can be played directly through a web browser.

1. Open the following game link:
   **https://filthylemon.itch.io/underpin-assignment**
2. Enter this password: "Underpin"
3. Click **Run Game** (or the play button) to launch the game.
4. Enjoy playing!

> **Note:** No installation or download is required. The game runs directly in your browser.

## Bonus Features

The game includes the following additional features:

- **Animated Lever:** A visual lever animation plays when the player starts a spin.
- **Sequential Reel Stopping:** The three reels stop one after another with smooth deceleration.
- **Randomized Results:** Each reel stops at a randomly selected symbol.
- **Coin System:** Players earn and spend coins according to the game rules.
- **Win/Lose Feedback:** Dedicated result panels display "YOU WIN!" or "YOU LOSE!" messages.
- **Sound Effects:** Separate sound effects play when the player wins or loses.
- **Automatic Balance Reset:** When the player runs out of coins, the balance resets to 100 after the losing message is displayed.
- **Input Prevention:** The player cannot start another spin while the current spin or result sequence is in progress.

## Thought Process and Approach

I approached this project by dividing the game into smaller systems and implementing them one at a time.

### 1. Reel System
I first created the three reels and implemented the symbol generation system. Each reel contains four different symbols, with three symbols visible at a time. The symbols move vertically to create the spinning effect.

### 2. Reel Stopping Mechanism
After implementing continuous reel movement, I worked on the stopping mechanism. Each reel stops independently with a smooth deceleration effect. I also ensured that the symbols align correctly when the reels stop.

### 3. Game Logic
Once the reel system was working, I implemented the winning condition. The game checks only the middle row of the three reels. If all three symbols match, the player receives 50 coins.

### 4. Coin Management
I implemented a coin system to manage the starting balance, spin cost, and winning rewards. I also added a balance reset when the player runs out of coins.

### 5. User Interface and Feedback
Finally, I added the coin balance display, lever animation, win/lose result panels, and sound effects to make the game more interactive and complete.

### Overall Approach

My main focus was to build the project in a modular way, keeping the reel logic, lever controls, and game management separate. This made the project easier to develop, test, debug, and maintain.

## Technologies Used

- Unity 6.3 LTS
- C#
- TextMeshPro
- Unity UI System
- Unity Audio System
- WebGL

---

**Developed as part of a Game Development Internship Assignment.**
