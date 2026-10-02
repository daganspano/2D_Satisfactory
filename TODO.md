# FACTORY GAME TO-DO'S

## New Content

- Crafting Bench
  - Get crafting bench sprite.
  - Add 1 sprite onto screen.
  - Allow user to hand-craft by pressing `"e"`.
    - One hit is 0.25 seconds.
    - Iron ingots craft 1 iron ore -> 1 iron ingot every 3 hits.
    - Iron rods craft 1 iron ingot -> 1 iron rods every 1 hit.
    - Iron plates craft 3 iron ingot -> 2 iron plates every 3 hits.

- Inventory
  - Make an inventory popup that opens when pressing tab.

- Options
  - Add popup when "Options" button is clicked.

- Background
  - Create a map sprite that's at least the current game size
  - Create Background class to contain the map sprite (will later allow moving the map instead of the character).
  - Update the Character sprite to be 1x scale.

## Refactoring

- Convert classes to `GameComponent`s, `DrawableGameComponent`s, and `GameScreen`s
