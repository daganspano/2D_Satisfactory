# FACTORY GAME TO-DO'S

## Necessary

### [Tilemap](Components\Tilemap.cs)

- Use for loops for map tilemap creation.
- Increase size to 1280 x 720 pixels (80 x 45 tiles).

### Machines

Refer to [CraftingBench.cs](Components\Machines\CraftingBench.cs), [ScreenManager.cs](Screens\ScreenManager.cs), [ScreenState.cs](Screens\ScreenState.cs), and [Screen.cs](Screens\Screen.cs).

- Create an `Assembler` component similar to the [`CraftingBench`](Components\Machines\CraftingBench.cs) (does not need power until functionality for it is added).
- Add an assembler to [`GameScreen`](Screens\MainGame\GameScreen.cs) (temporary until building functionality is added).
- Create crafting bench and assembler sprites.

---

## Optional

### Improvements

---

### Controls

Refer to [InputHandler.cs](Screens\MainGame\InputHandler.cs), [ButtonGroup.cs](Components\ButtonClasses\ButtonGroup.cs), [Button.cs](Components\ButtonClasses\Button.cs), [CraftButton.cs](Screens\Crafting\CraftButton.cs), [RecipeSelector.cs](Screens\Crafting\RecipeSelector\RecipeSelector.cs), [MusicPlayer.cs](MusicPlayer.cs), and [ScreenManager.cs](Screens\ScreenManager.cs).

- Add mouse, keyboard, and gamepad controls for everything (within reason).
- Maybe centralize controls?

### [Control Display](Screens\MainGame\ControlsDisplay.cs)

Refer to [InputHandler.cs](Screens\MainGame\InputHandler.cs), [ButtonGroup.cs](Components\ButtonClasses\ButtonGroup.cs), [Button.cs](Components\ButtonClasses\Button.cs), [CraftButton.cs](Screens\Crafting\CraftButton.cs), [RecipeSelector.cs](Screens\Crafting\RecipeSelector\RecipeSelector.cs), [MusicPlayer.cs](MusicPlayer.cs), and [ScreenManager.cs](Screens\ScreenManager.cs).

- Add new controls.
- Improve UI/UX (text background).

### [Inventory Display / Screen](Screens\MainGame\InventoryDisplay.cs)

Refer to [ScreenManager.cs](Screens\ScreenManager.cs), [ScreenState.cs](Screens\ScreenState.cs), and [Screen.cs](Screens\Screen.cs).

- Convert the display to a basic popup screen that opens when pressing tab.
- Add new controls to [`ControlDisplay`](Screens\MainGame\ControlsDisplay.cs) if applicable.

### [Crafting Screen](Screens\Crafting\CraftingScreen.cs)

- Improve UI/UX.
  - Add inventory to right.
  - Make quantities more visible.
  - Adjust colors.
  - Adjust spacing.
  - ...
- Add new controls to [`ControlDisplay`](Screens\MainGame\ControlsDisplay.cs) if applicable.

### [Win Screen](Screens/WinScreen/WinScreen.cs)

- Improve UI/UX.
- Add new controls to [`ControlDisplay`](Screens\MainGame\ControlsDisplay.cs) if applicable.

### New Content

---

### Options Screen

Refer to [ScreenManager.cs](Screens\ScreenManager.cs), [ScreenState.cs](Screens\ScreenState.cs), and [Screen.cs](Screens\Screen.cs).

- Add a "Options" button to the menu and a basic popup screen for when it is clicked.
- Add new controls to [`ControlDisplay`](Screens\MainGame\ControlsDisplay.cs) if applicable.

### Building

- Add functionality for building.
  - Built building's hitbox tiles are a color to show inability to build there (red when attempting to build there).
  - Building that is being built's hitbox tiles are different color to show the active placement spot.
- Remove crafting bench and assembler from map.
- Add new controls to [`ControlDisplay`](Screens\MainGame\ControlsDisplay.cs) if applicable.

### [Data](Data.cs)

Refer to [SATISFACTORY.TH.GL](https://satisfactory.th.gl/db/structures) and [Satisfactory Wiki](https://satisfactory.fandom.com/wiki/Satisfactory_Wiki).

- Add items and recipes for copper ore, copper ingots, wire, and cable.
- Add building costs to machines.

### [Tilemap](Components\Tilemap.cs)

Refer to [PIXLR](https://pixlr.com/editor/).

- Add textures to atlas.
  - Copper ore
  - Iron ore node
  - Copper ore node
- Add a copper node in top right.

### Copper

Refer to [Tilemap.cs](Components\Tilemap.cs), [ControlsDisplay.cs](Screens\MainGame\ControlsDisplay.cs), and [InputHandler.cs](Screens\MainGame\InputHandler.cs).

- Add copper ore mining capabilities.
- Add new controls to [`ControlDisplay`](Screens\MainGame\ControlsDisplay.cs).

### Belts

- Create new `Belt` component.
- Add functionality to build and use belts with machines.
- Add new controls to [`ControlDisplay`](Screens\MainGame\ControlsDisplay.cs) if applicable.

### Power

- Add Bushes to [`GameScreen`](Screens\MainGame\GameScreen.cs).
- Add functionality to collect bushes.
- Add a biomass burner.
- Add power poles.
- Add power functionality to machines.
- Add new controls to [`ControlDisplay`](Screens\MainGame\ControlsDisplay.cs) if applicable.
