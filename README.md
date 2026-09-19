# 2D_Satisfactory

As Satisfactory is a 3D rendition of Factorio, it has some differing game mechanics that would be fun to experiment with in 2D.

## Instructions

### Exiting

Click the button sprite with with the label "Exit"

### Interacting with buttons

- Idle state: The button isn't selected.
- Hover state: The button is selected.
- Pressed state: The button is clicked.

---

## 0.2.0 (Game Project 1)

### Assets Added

None

### Classes Modified / Added

- `FactoryGame.cs` (Modified):
  - Parameters given to `TitleScreen` are updated.
  - Now has a `GameState`, and `TitleScreen` buttons change the state.
  - Now has a `MainGame` and updates/draws it when `GameState` is `MainGame` or `MainGameOptions`
- `GameState.cs` (Added): Enum to represent the different states of the game.

#### Main Game

- `MainGame.cs` (Added): ...

#### Title Screen

- `TitleScreen.cs` (Modified):
  - Now has a `ButtonGroup` instead of `Button`s.
  - Now takes `gameDimensions` as a parameter instead of `gameWidth` and `gameHeight`.
  - Now takes the `Action`s for the `"Start"` and `"Options"` buttons as parameters (`"Options"` doesn't have functionality yet, but it takes the `Action` on initialization).
- `TitleCharacter.cs` (Previously `RunningSprite.cs`) (Modified): Now handles only random direction changing and bouncing off the walls. Additional functionality was moved to `Character`.
- `ButtonGroup.cs` (Added): Represents a group of `Button`s, allowing navigation and selection using mouse, keyboard, or gamepad input.
- `Button.cs` (Modified): No longer handles hover logic, as it was moved to `ButtonGroup`.

#### Components

- `Character.cs` (Added): Represents a character in the game, handling its position, animation, and rendering.
- `FrameEnums.cs` (Modified): Moved location.

### UI Modifications

#### Title Screen

The buttons now have keyboard and gamepad functionality:

| Action     | Mouse         | Keyboard         | Gamepad                       |
| ---------- | ------------- | ---------------- | ----------------------------- |
| Cycle up   | Hovering Over | Tab              | Left Stick Up or D-Pad Up     |
| Cycle down | Hovering Over | Left-Shift + Tab | Left Stick Down or D-Pad Down |
| Click      | Left Click    | Enter            | A                             |

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
