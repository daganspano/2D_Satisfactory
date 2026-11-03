using _2D_Satisfactory.Screens.MainGame;
using _2D_Satisfactory.Screens.Title;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace _2D_Satisfactory.Screens;

/// <summary>
/// Manages the different screens in the game, handling transitions and updates between them.
/// </summary>
/// <param name="state">The new state of the music player.</param>
public delegate void setMusicPlayerStateDelegate(SongState state);

/// <summary>
/// Manages the different screens in the game, handling transitions and updates between them.
/// </summary>
public class ScreenManager : DrawableGameComponent
{
    private Dictionary<string, Screen> _screens = new Dictionary<string, Screen>();
    private bool _wasEscDown;
    private setMusicPlayerStateDelegate _setMusicPlayerState;

    /// <summary>
    /// Initializes a new instance of the <see cref="ScreenManager"/> class.
    /// </summary>
    /// <param name="game">The game instance.</param>
    /// <param name="setMusicPlayerState">The delegate to set the music player state.</param>
    public ScreenManager(Game game, setMusicPlayerStateDelegate setMusicPlayerState) : base(game)
    {
        _screens = new Dictionary<string, Screen>()
        { 
            { "TitleScreen", new TitleScreen(game, HandleStart, hasContinue: FileManager.GameExists()) },
            { "GameScreen", new GameScreen(game) },
            { "PauseScreen", new PauseScreen(game, onResume: HandleTogglePause, onExit: HandleExitGame) }
        };
        
        _setMusicPlayerState = setMusicPlayerState;
        
        _screens["TitleScreen"].ScreenState = ScreenState.Active;
        _screens["GameScreen"].ScreenState = ScreenState.Inactive;
        _screens["PauseScreen"].ScreenState = ScreenState.Inactive;
    }

    /// <summary>
    /// Loads the content for all screens managed by the ScreenManager.
    /// </summary>
    protected override void LoadContent()
    {
        foreach (var screen in _screens.Values)
        {
            screen.LoadContent();
        }

        base.LoadContent();
    }

    /// <summary>
    /// Updates the state of the ScreenManager and its managed screens based on user input and game time.
    /// </summary>
    /// <param name="gameTime">The elapsed game time since the last update.</param>
    public override void Update(GameTime gameTime)
    {
        bool isEscDown = Keyboard.GetState().IsKeyDown(Keys.Escape) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Start);

        if (isEscDown && !_wasEscDown)
        {
            HandleTogglePause();
        }

        _wasEscDown = isEscDown;

        foreach (var screen in _screens.Values)
        {
            if (screen.ScreenState == ScreenState.Active) screen.Update(gameTime);
        }
    
        base.Update(gameTime);
    }

    /// <summary>
    /// Draws the active screens managed by the ScreenManager.
    /// </summary>
    /// <param name="gameTime">The elapsed game time since the last draw.</param>
    public override void Draw(GameTime gameTime)
    {
        foreach (var screen in _screens.Values)
        {
            if (screen.ScreenState != ScreenState.Inactive) screen.Draw(gameTime);
        }

        base.Draw(gameTime);
    }

    /// <summary>
    /// Handles the start of a new game or loading an existing game, transitioning to the GameScreen and updating the music player state accordingly.
    /// </summary>
    /// <param name="isNew">Indicates whether to start a new game (true) or load an existing game (false).</param>
    private void HandleStart(bool isNew = false)
    {
        if (isNew) GameData.NewGame();
        else GameData.LoadGame();

        _screens["TitleScreen"].ScreenState = ScreenState.Inactive;
        _screens["GameScreen"].ScreenState = ScreenState.Active;
        _setMusicPlayerState(SongState.MainGame);
    }

    /// <summary>
    /// Toggles the pause state of the game, switching between the GameScreen and PauseScreen based on their current states.
    /// </summary>
    private void HandleTogglePause()
    {
        if (_screens["GameScreen"].ScreenState == ScreenState.Active && _screens["PauseScreen"].ScreenState == ScreenState.Inactive)
        {
            _screens["GameScreen"].ScreenState = ScreenState.Paused;
            _screens["PauseScreen"].ScreenState = ScreenState.Active;
        }
        else if (_screens["GameScreen"].ScreenState == ScreenState.Paused 
                 && _screens["PauseScreen"].ScreenState == ScreenState.Active)
        {
            _screens["GameScreen"].ScreenState = ScreenState.Active;
            _screens["PauseScreen"].ScreenState = ScreenState.Inactive;
        }
    }

    /// <summary>
    /// Handles the exit game action, saving the game state and transitioning to the TitleScreen or exiting to the desktop based on the provided parameter.
    /// </summary>
    /// <param name="toDesktop">Indicates whether to exit to the desktop (true) or return to the TitleScreen (false).</param>
    private void HandleExitGame(bool toDesktop)
    {
        GameData.SaveGame();
        if (toDesktop) Game.Exit();

        _screens["PauseScreen"].ScreenState = ScreenState.Inactive;
        _screens["GameScreen"].ScreenState = ScreenState.Inactive;
        _screens["TitleScreen"].ScreenState = ScreenState.Active;
        _setMusicPlayerState(SongState.TitleScreen);
    }
}