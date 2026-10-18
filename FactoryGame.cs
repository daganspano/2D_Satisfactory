using System;
using _2D_Satisfactory.MainGameClasses;
using _2D_Satisfactory.TitleScreenClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2D_Satisfactory;

/// <summary>
/// Represents the main game class for the 2D Satisfactory game.
/// </summary>
public class FactoryGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    public FactoryGameState State { get; private set; }
    private TitleScreen _titleScreen;
    private MainGame _mainGame;

    public FactoryGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        State = FactoryGameState.TitleScreen;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Components.Add(new MusicPlayer(this));
    }

    /// <summary>
    /// Initializes the game.
    /// </summary>
    protected override void Initialize()
    {
        GameDimensions.Set(GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight);

        // Initialize title screen
        _titleScreen = new TitleScreen(
            this,
            () => { State = FactoryGameState.MainGame; }, 
            () => { },
            Exit
        );

        // Initialize main game
        _mainGame = new MainGame(
            this,
            () => { State = FactoryGameState.TitleScreen; },
            Exit
        );

        _mainGame.Initialize();

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
        _titleScreen.LoadTitleScreen();
    }

    /// <summary>
    /// Updates the game.
    /// </summary>
    /// <param name="gameTime">The game time information.</param>
    protected override void Update(GameTime gameTime)
    {
        switch (State)
        {
            case FactoryGameState.MainGame:
                _mainGame.Update(gameTime);
                break;
            case FactoryGameState.TitleScreen:
                _titleScreen.Update(gameTime);
                break;
        }

        base.Update(gameTime);
    }

    /// <summary>
    /// Draws the game.
    /// </summary>
    /// <param name="gameTime">The game time information.</param>
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.SkyBlue);

        _spriteBatch.Begin();

        // Draw the current game state
        switch (State)
        {
            case FactoryGameState.MainGame:
                _mainGame.Draw(gameTime);
                break;
            case FactoryGameState.TitleScreen:
                _titleScreen.Draw(gameTime);
                break;
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
