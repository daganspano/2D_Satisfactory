using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using _2D_Satisfactory.TitleScreenClasses;
using System;

namespace _2D_Satisfactory;

/// <summary>
/// Represents the main game class for the 2D Satisfactory game.
/// </summary>
public class FactoryGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private TitleScreen _titleScreen;

    public FactoryGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    /// <summary>
    /// Initializes the game.
    /// </summary>
    protected override void Initialize()
    {
        int width = GraphicsDevice.PresentationParameters.BackBufferWidth;
        int height = GraphicsDevice.PresentationParameters.BackBufferHeight;

        // Initialize title screen
        _titleScreen = new TitleScreen(width, height, Exit);

        // Base initialization
        base.Initialize();
    }

    /// <summary>
    /// Loads the game content.
    /// </summary>
    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Load title screen content
        _titleScreen.LoadContent(Content);
    }

    /// <summary>
    /// Updates the game.
    /// </summary>
    /// <param name="gameTime">The game time information.</param>
    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // Update title screen
        _titleScreen.Update(gameTime);

        base.Update(gameTime);
    }

    /// <summary>
    /// Draws the game.
    /// </summary>
    /// <param name="gameTime">The game time information.</param>
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new Color(0x36, 0x4D, 0x28));

        _spriteBatch.Begin();

        // Draw title screen
        _titleScreen.Draw(_spriteBatch);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
