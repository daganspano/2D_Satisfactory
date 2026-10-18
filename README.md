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
- Stand on the iron ore deposit and hold "E" on keyboard or "X" on the gamepad to collect iron ore.
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

## 0.4.0 (Game Project 3)

### Assets Added / Removed

#### Sprites

- `background_atlas.png` (Added)
- `forest_background.png` (Removed)
- `iron_ore_deposit.png` (Removed)
- `particle.png` (Added)

### Classes Modified / Added / Removed

- `FactoryGame.cs` (Modified):
  - Changed background color.
  - Made changes for children's changed parameters.
- `GameDimensions.cs` (Modified): Added Center property.
- `Map.cs` (Added): Represents the map of the game, including its dimensions, tile size, and tile data. This class provides properties and methods to access the map's characteristics and check for collisions with specific tile types, such as iron ore deposits.
- `Tilemap.cs` (Added): Represents a tilemap component in the game, responsible for rendering the map's tiles and handling interactions with specific tile types, such as iron ore deposits. This class extends DrawableGameComponent and provides methods to load the map, update its offset, and draw the tiles on the screen.

#### Main Game Classes

- `GameCharacter.cs` (Modified): Added `FacingDirection` that gives the `Character`'s `FacingDirection`.
- `IronOreDeposit.cs` (Removed): Tilemap now has iron ore deposits built in.
- `MainGame.cs` (Modified):
  - Converted to be a `DrawableGameComponent`.
  - Removed background texture and `IronOreDeposit` and gave their logic to added `Tilemap`.
  - Added logic for offset transforming.
  - Added a `HandMiningParticleSystem` and the logic to have it function when hand mining.

#### Title Screen Classes

- `TitleScreen.cs` (Modified):
  - Converted to be a `DrawableGameComponent`.
  - Removed background texture and gave its logic to added `Tilemap`.

#### Particle Classes

- `HandMiningParticleSystem.cs` (Added): A particle system for hand mining effects, generating particles that simulate debris or dust when a player mines resources by hand.
- `Particle.cs` (Added): Represents a single particle in a particle system, encapsulating properties such as position, velocity, acceleration, color, rotation, scale, and lifetime. This struct is used to define the behavior and appearance of individual particles within a particle system.
- `ParticleSystem.cs` (Added): An abstract base class for particle systems, providing functionality to manage and render a collection of particles with properties such as position, velocity, acceleration, color, rotation, scale, and lifetime. Subclasses can define specific particle behaviors and appearances by overriding methods for initialization and updating.

#### Components / Character Classes

- `Character.cs` (Modified):
  - Changed scale from 1.8 to 1.0.
  - Added `FacingDirection` to give the current facing direction of the character based on its movement.
- `Collision.cs` (Modified): Removed 200px border on top of screen since the background with trees was removed.

### UI Modifications

### Title Screen

- Now uses a tile map instead of the old forest background.
- Now allows characters to run on the top of the screen.

### Main Game

- Now uses a tile map instead of the old forest background.
- Now allows character to run on the top of the screen.
- Now has the iron ore deposit built into the tile map. The functionality is still the same as the old deposit.

---

## 0.3.0 (Game Project 2)

### Assets Added

#### Sounds

- `AssemblyLineTrance.mp3`
- `AutomatedDawn.mp3`
- `ConveyorDawnHorizon.mp3`
- `grassFootSteps.wav`

#### Sprites

- `iron_ore_deposit.png`

### Classes Modified / Added

- `FactoryGame.cs` (Modified):
  - Added the new `MusicPlayer` component.
  - Now exposes the current `FactoryGameState` as a public `State` property so other components can react to it..
- `MusicPlayer.cs` (Added): A game component that manages background music for the game.

#### Main Game Classes

- `MainGame.cs` (Modified):
  - Added an `IronOreDeposit`
  - Now lets the player hand-mine iron ore by holding "E" on keyboard or "X" on the gamepad while colliding with the deposit
  - Now tracks a simple inventory
  - Now draws the inventory and contextual control hints to the screen.
- `GameCharacter.cs` (Modified): Now exposes the character's `HitBox` so other classes can perform collision checks against it.
- `IronOreDeposits.cs` (Added): Represents an iron ore deposit in the game world.
  - `OvalHitBox` (Added): Represents an oval-shaped hitbox for collision detection.

#### Title Screen Classes

- `TitleScreen.cs` (Modified): Minor cleanup.
- `TitleCharacter.cs` (Modified): Disables the new walking sound effect for title screen characters.

#### Components / Character Classes

- `Character.cs` (Modified):
  - Added a footstep sound effect that plays while the character moves (toggleable via a new constructor parameter)
  - Refactored the animation speed into a reusable field shared with the new sound timing logic.

### UI Modifications

#### Main Game

- There is now an iron ore deposit on the screen.
  - Standing on it (colliding with it) shows text at the bottom showing that "E" mines iron ore.
  - Standing on it and holding "E" adds an iron ore into the user's inventory every 0.5 seconds.
- There is now a temporary inventory display in the top left.

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
