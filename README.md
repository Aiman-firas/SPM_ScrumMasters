# Ngee Ann City

Ngee Ann City is a city-building strategy game developed using Unity. The player takes on the role of the mayor, aiming to build the happiest and most prosperous city possible.

## Table of Contents
- [Project Overview](#project-overview)
- [Game Modes](#game-modes)
- [Setup](#setup)
- [Usage](#usage)
- [Features](#features)
- [Development](#development)
- [Contributing](#contributing)
- [License](#license)
- [Acknowledgements](#acknowledgements)

## Project Overview
Ngee Ann City is a project developed by a team of students as part of the Software Project Management course. The game involves strategic city planning, resource management, and achieving high scores by effectively placing different types of buildings.

### Product Goal
Create a fun and engaging city-building game that challenges players to optimize their building strategies for maximum score and city prosperity.

### Definition of Done
- The game must have a main menu with options to start a new free play mode,start a new arcade mode, load a saved game, view high scores, and exit game.
- Both Arcade and Free Play modes should be fully functional.
- The game should save and load game states correctly.
- High scores should be tracked and displayed.
- The UI should be intuitive and user-friendly.
- The game should be free of critical bugs and performance issues.

## Game Modes
### Arcade Mode
- Limited number of coins and a 20x20 grid.
- Players select from two randomly chosen buildings per turn.
- Objective: Score as many points as possible with the given resources.

### Free Play Mode
- Unlimited coins and an expanding grid starting at 5x5.
- Players can choose any building to construct on any cell.
- Objective: Build and manage a sustainable city without going bankrupt.

## Setup
### Prerequisites
- Unity 2020.3 or higher
- Git (for version control)
- A code editor (e.g., Visual Studio Code)

### Installation
1. Clone the repository:
    ```sh
    git clone https://github.com/yourusername/ngee-ann-city.git
    ```
2. Open the project in Unity:
    - Launch Unity Hub.
    - Click on "Add" and select the cloned project directory.
3. Open the project and let Unity load all assets and dependencies.

## Usage
### Running the Game
1. Open the project in Unity.
2. Click on the "Play" button in the Unity Editor.
3. Navigate through the main menu to start a new game, load a saved game, or view high scores.

### Building the Game
1. Go to `File > Build Settings`.
2. Choose the target platform (e.g., PC, Mac, Linux).
3. Click on "Build" and select the desired output directory.

## Features
- **Main Menu:** Start a new free play mode, start a new arcade mode, load saved game, view high scores, exit game.
- **Arcade Mode:** Limited resources, strategic building placement.
- **Free Play Mode:** Unlimited resources, expanding city grid.
- **High Score Tracking:** Save and display top scores.
- **Save/Load Functionality:** Preserve game state between sessions.
- **Dynamic Scoring System:** Different buildings score differently based on their placement.

## Development
### Sprint Details
- **Sprint 1:**
  - Goal: Implement basic game mechanics and main menu.
  - Completed: Main menu UI, basic game loop, initial building mechanics.
  - Incomplete: Save/load functionality, high score tracking.
- **Sprint 2:**
  - Goal: Complete save/load functionality and implement scoring system.
  - Completed: Save/load functionality, scoring system.
  - Incomplete: UI polish, bug fixes.

## Contributing
We welcome contributions from the community! Please follow these steps:
1. Fork the repository.
2. Create a new branch (`git checkout -b feature/YourFeature`).
3. Commit your changes (`git commit -m 'Add your feature'`).
4. Push to the branch (`git push origin feature/YourFeature`).
5. Open a pull request.

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgements
- Our tutor for guidance and feedback.
- [Unity Documentation](https://docs.unity3d.com/Manual/index.html) for providing extensive resources.
- The community for providing invaluable support and inspiration.

