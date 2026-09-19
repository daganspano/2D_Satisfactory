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

    // FPS Fields for testing purposes
    private SpriteFont _font;
    private float _fps = 0f;
    private int _framesPerInterval = 0;
    private double _elapsedTime = 0;

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
        GameDimensions.Set(GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight);

        // Initialize title screen
        _titleScreen = new TitleScreen(
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

        // Load font for displaying FPS for testing purposes
        _font = Content.Load<SpriteFont>("Orbitron-Regular");
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

        // Update FPS calculation for testing purposes
        _framesPerInterval++;
        _elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;
        if (_elapsedTime >= 1.0)
        {
            _fps = _framesPerInterval / (float)_elapsedTime;
            _framesPerInterval = 0;
            _elapsedTime = 0;
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

        // Draw the current game state
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

        // Draw FPS for testing purposes
        _spriteBatch.DrawString(_font, $"FPS: {_fps:0.0}", new Vector2(10, 10), Color.White);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
