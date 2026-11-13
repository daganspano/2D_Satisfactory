
using System;
using _2D_Satisfactory.Screens.MainGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2D_Satisfactory.Screens.Crafting;

/// <summary>
/// Represents the crafting screen in the game, providing a user interface for players to craft items using available recipes. It manages the display of crafting options, handles user interactions, and coordinates with the CraftHandler to perform crafting operations.
/// </summary>
public class CraftingScreen : Screen
{
    private Game _game;
    private SpriteBatch _spriteBatch;
    private Rectangle _backgroundBounds;
    private CraftButton _craftButton;

    /// <summary>
    /// Initializes a new instance of the CraftingScreen class with the specified game instance. Sets up the background bounds, crafting display, and crafting button, and adds them to the screen's components.
    /// </summary>
    /// <param name="game">The game instance.</param>
    public CraftingScreen(Game game) : base(ScreenState.Inactive)
    {
        _game = game;
        var width = 600;
        var height = 400;
        var x = ((int)GameData.ScreenSize.X - width) / 2;
        var y = ((int)GameData.ScreenSize.Y - height) / 2;
        _backgroundBounds = new Rectangle(x, y, width, height);
        CraftDisplay craftDisplay = new CraftDisplay(game);
        CraftHandler craftHandler = new CraftHandler(game, craftDisplay.UpdateDisplayInfo);
        _craftButton = new CraftButton(game, craftHandler.ToggleCrafting);
        Components.Add(_craftButton);
        Components.Add(craftDisplay);
        Components.Add(craftHandler);
    }

    /// <summary>
    /// Loads the content for the crafting screen, including initializing the sprite batch for rendering. This method is called when the screen is activated and prepares the necessary resources for drawing the crafting interface.
    /// </summary>
    public override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(_game.GraphicsDevice);
        base.LoadContent();
    }

    /// <summary>
    /// Draws the crafting screen, including the background and any active components. This method is called during the game's draw cycle and renders the crafting interface to the screen, providing visual feedback to the player.
    /// </summary>
    /// <param name="gameTime">The elapsed game time since the last draw.</param>
    public override void Draw(GameTime gameTime)
    {
        var pixel = new Texture2D(_game.GraphicsDevice, 1, 1);
        pixel.SetData([Color.White]);

        _spriteBatch.Begin();

        // Background
        _spriteBatch.Draw(pixel, _backgroundBounds, Color.Black * 0.4f);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}