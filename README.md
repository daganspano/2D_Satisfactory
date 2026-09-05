# 2D_Satisfactory

As Satisfactory is a 3D rendition of Factorio, it has some differing game mechanics that would be fun to experiment with in 2D.

## Instructions

### Exiting

Click the button sprite with with the label "Exit"

### Interacting with buttons

* Idle state: Don't have your mouse over the button.
* Hover state: Hover your mouse over the button. It appears to light up.
* Pressed state: Click the button. It will be slightly smaller and have a darker shade than idle.

## 0.1.0

### Assets Added

* Orbitron Font
* Title Banner
* Button Atlas
* Construction Workers Atlas
* Forest Background

### Classes Modified / Added

* `FactoryGame.cs` (Modified): Initializes, loads, updates, and draws the TitleScreen class.
* `Button.cs` (Added): A UI button with a normal, hover, and pressed state.
* `TitleScreen.cs` (Added): Represents the title screen of the game. Contains a background, title banner, 3 reactive `Button`s, and 4 `RunningSprite`s.
* `RunningSprite.cs` (Added): Represents a running sprite that can move in random directions and animate between frames.
* `FrameEnums.cs` (Added): Contains enumerators for the animation and direction frames of `RunningSprite`s.

### UI Modifications

* There is now a title screen with the following:
  * A background
  * A title banner
  * 3 Reactive buttons (one that exits the game)
  * 4 character sprites that run across the background
