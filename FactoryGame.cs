using _2D_Satisfactory.Screens;
using _2D_Satisfactory.Screens.MainGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2D_Satisfactory;

/// <summary>
/// Represents the main game class for the 2D Satisfactory game, responsible for initializing the game, managing the graphics device, and handling the game's main loop. This class extends the Game class from the MonoGame framework and provides methods for drawing the game screen and managing game components.
/// </summary>
public class FactoryGame : Game
{
    /// <summary>
    /// The graphics device manager responsible for managing the graphics settings and device for the game. It is used to configure the game's display properties, such as resolution and fullscreen mode.
    /// </summary>
    private GraphicsDeviceManager _graphics;

    /// <summary>
    /// Initializes a new instance of the FactoryGame class, setting up the graphics device manager, content root directory, and adding the ScreenManager component to manage game screens.
    /// </summary>
    public FactoryGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        GameData.ScreenSize = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
        GameData.PlayerPosition = (0.5f * GameData.ScreenSize).ToPoint();

        MusicPlayer musicPlayer = new MusicPlayer(this);
        Components.Add(new ScreenManager(this, musicPlayer.SetState));
        Components.Add(musicPlayer);
    }

    /// <summary>
    /// Called when the game should draw a frame. Draws the DrawableGameComponent instances attached to this game.
    /// </summary>
    /// <param name="gameTime">The game time.</param>
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.SkyBlue);
        base.Draw(gameTime);
    }
}
