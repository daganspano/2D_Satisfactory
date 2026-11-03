using _2D_Satisfactory.Screens;
using _2D_Satisfactory.Screens.MainGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2D_Satisfactory;

/// <summary>
/// Represents the main game class for the 2D Satisfactory game, responsible for 
/// initializing and managing the game's graphics, content, and screens.
/// </summary>
public class FactoryGame : Game
{
    /// <summary>
    /// The graphics device manager used to manage the graphics settings and device for the game.
    /// </summary>
    private GraphicsDeviceManager _graphics;

    /// <summary>
    /// Initializes a new instance of the <see cref="FactoryGame"/> class, setting up the 
    /// graphics device manager, content root directory, and game components.
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
    /// Draws the game screen, clearing it with a sky blue color before rendering the game's components.
    /// </summary>
    /// <param name="gameTime">The game time object providing timing information for the current frame.</param>
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.SkyBlue);
        base.Draw(gameTime);
    }
}
