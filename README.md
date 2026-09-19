# 2D_Satisfactory

As Satisfactory is a 3D rendition of Factorio, it has some differing game mechanics that would be fun to experiment with in 2D.

## Instructions

### Exiting

- Title Screen: click the "Exit" `Button`.
- Main Game: press "Esc" on the keyboard or "Start" on the gamepad, then click the "Exit to Desktop" `Button`.

### Title Screen

- Click the "Start" `Button` to begin the main game.
- Click the "Exit" `Button` to close the game.
- Watch the `TitleCharacter`s bounce around the screen and collide with one another and the screen borders.
- The "Options" `Button` is present but currently has no implemented functionality.

### Main Game

- Move the `GameCharacter` with WASD or the arrow keys on the keyboard, or use the left joystick on the gamepad.
- Press "Esc" on the keyboard or "Start" on the gamepad to open and close the pause menu.
- Click the "Resume" `Button` to close the pause menu and continue playing.
- Click the "Exit to Menu" `Button` to return to the title screen.
- Click the "Exit to Desktop" `Button` to exit the game.
- The "Options" `Button` is present but currently has no implemented functionality.

### Interacting with buttons

- Idle state: the button is not selected.
- Hover state: the button is selected.
- Pressed state: the button is clicked.

| Action     | Mouse         | Keyboard         | Gamepad                       |
| ---------- | ------------- | ---------------- | ----------------------------- |
| Cycle up   | Hovering Over | Tab              | Left Stick Up or D-Pad Up     |
| Cycle down | Hovering Over | Left-Shift + Tab | Left Stick Down or D-Pad Down |
| Click      | Left Click    | Enter            | A                             |

---

## 0.2.0 (Game Project 1)

### Classes Modified / Added

- `FactoryGame.cs` (Modified): Manages the top-level game loop, initializes both the title screen and main gameplay scene, tracks the current state, and updates/draws the active screen based on the current mode.
- `FactoryGameState.cs` (Added): Defines the game state enum used to switch between the title screen and the main game.
- `GameDimensions.cs` (Added): Stores the current screen dimensions so UI, characters, and collision logic can position themselves consistently across the game.

#### Main Game Classes

- `GameCharacter.cs` (Added): Handles the player-controlled character, including keyboard/gamepad movement input and boundary collision adjustments.
- `MainGame.cs` (Added): Represents the main gameplay state, including the background, player character, and pause menu with interactive buttons.
- `MainGameState.cs` (Added): Tracks whether the game is currently idle or paused.

#### Title Screen Classes

- `TitleCharacter.cs` (Modified and Renamed (previously `RunningSprite`)): Handles random direction changes, wall bounces, and sprite-to-sprite collision responses while delegating shared animation and movement logic to `Character`.
- `TitleScreen.cs` (Modified): Creates the title screen layout, initializes animated characters, loads the menu assets, and updates/draws the screen and button group.

#### Components / Button Classes

- `Button.cs` (Modified and Moved): Represents an individual UI button, manages its visual state, and triggers the associated click action when selected and activated.
- `ButtonGroup.cs` (Added): Manages a collection of buttons, handles keyboard/gamepad navigation, and supports mouse hover selection for the UI.

#### Components / Character Classes

- `Character.cs` (Added): Provides shared character functionality for animation, position, direction, movement, and rendering.
- `Collision.cs` (Added): Includes utility methods for detecting edge collisions and resolving overlaps between character hitboxes.
- `FrameEnums.cs` (Modified and Moved): Stores the animation and direction frame enumerations used by the character sprites.

### UI Modifications

#### Title Screen

- The "Start" `Button` now transitions from the title screen into the main game.
- The `TitleCharacter`s now bump into each other and react to screen boundaries.
- The buttons now support keyboard and gamepad navigation and activation.

| Action     | Mouse         | Keyboard         | Gamepad                       |
| ---------- | ------------- | ---------------- | ----------------------------- |
| Cycle up   | Hovering Over | Tab              | Left Stick Up or D-Pad Up     |
| Cycle down | Hovering Over | Left-Shift + Tab | Left Stick Down or D-Pad Down |
| Click      | Left Click    | Enter            | A                             |

#### Main Game

- Pressing "Esc" on the keyboard or "Start" on the gamepad toggles the pause menu on and off.
- The pause menu mirrors the title screen button flow and includes:
  - Resume: continues the current game session.
  - Options: currently has no functionality.
  - Exit to Menu: returns to the title screen.
  - Exit to Desktop: exits the game.
- The `GameCharacter` can be moved with WASD and the arrow keys on the keyboard, or with the left joystick on the gamepad.

---

## 0.1.0 (Game Project 0)

### Assets Added

- Orbitron Font
- Title Banner
- Button Atlas
- Construction Workers Atlas
- Forest Background

### Classes Modified / Added

- `FactoryGame.cs` (Modified): Initializes, loads, updates, and draws the TitleScreen class.
- `Button.cs` (Added): A UI button with a normal, hover, and pressed state.
- `TitleScreen.cs` (Added): Represents the title screen of the game. Contains a background, title banner, 3 reactive `Button`s, and 4 `RunningSprite`s.
- `RunningSprite.cs` (Added): Represents a running sprite that can move in random directions and animate between frames.
- `FrameEnums.cs` (Added): Contains enumerators for the animation and direction frames of `RunningSprite`s.

### UI Modifications

- There is now a title screen with the following:
  - A background
  - A title banner
  - 3 Reactive buttons (one that exits the game)
  - 4 character sprites that run across the background
