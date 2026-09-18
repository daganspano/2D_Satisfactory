using _2D_Satisfactory.MainGameClasses;
using _2D_Satisfactory.TitleScreenClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _2D_Satisfactory;

/// <summary>
/// Represents the main game class for the 2D Satisfactory game.
/// </summary>
public class FactoryGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private GameState _currentGameState;
    private TitleScreen _titleScreen;
    private MainGame _mainGame;

    public FactoryGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        _currentGameState = GameState.TitleScreen;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    /// <summary>
    /// Initializes the game.
    /// </summary>
    protected override void Initialize()
    {
        Vector2 dimensions = new Vector2(GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight);


        // Initialize title screen
        _titleScreen = new TitleScreen(
            dimensions, 
            () => { _currentGameState = GameState.MainGame; }, 
            () => { _currentGameState = GameState.TitleScreenOptions; },
            Exit
        );

        // Initialize main game
        _mainGame = new MainGame();

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

        // Load main game content
        _mainGame.LoadContent(Content);
    }

    /// <summary>
    /// Updates the game.
    /// </summary>
    /// <param name="gameTime">The game time information.</param>
    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        switch (_currentGameState)
        {
            case GameState.MainGame:
                _mainGame.Update(gameTime);
                // Update main game logic here
                break;
            case GameState.MainGameOptions:
                _mainGame.Update(gameTime);
                // Update main game options logic here
                break;
            case GameState.TitleScreen:
                _titleScreen.Update(gameTime);
                break;
            case GameState.TitleScreenOptions:
                _titleScreen.Update(gameTime);
                // Update title screen options logic here
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
        GraphicsDevice.Clear(new Color(0x36, 0x4D, 0x28));

        _spriteBatch.Begin();

        switch (_currentGameState)
        {
            case GameState.MainGame:
                _mainGame.Draw(_spriteBatch);
                break;
            case GameState.MainGameOptions:
                _mainGame.Draw(_spriteBatch);
                // Update main game options logic here
                break;
            case GameState.TitleScreen:
                _titleScreen.Draw(_spriteBatch);
                break;
            case GameState.TitleScreenOptions:
                _titleScreen.Draw(_spriteBatch);
                // Update title screen options logic here
                break;
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
