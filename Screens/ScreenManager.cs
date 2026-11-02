using _2D_Satisfactory.Screens.MainGame;
using _2D_Satisfactory.Screens.Title;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace _2D_Satisfactory.Screens;

public delegate void setSongState(SongState state);

public class ScreenManager : DrawableGameComponent
{
    private Dictionary<string, Screen> _screens = new Dictionary<string, Screen>();
    private bool _wasEscDown;
    private setSongState _setMusicPlayerState;

    public ScreenManager(Game game, setSongState setMusicPlayerState) : base(game)
    {
        _screens = new Dictionary<string, Screen>()
        { 
            { "TitleScreen", new TitleScreen(game, onStart: HandleStart, onOpenOptions: () => {}) },
            { "GameScreen", new GameScreen(game) },
            { "PauseScreen", new PauseScreen(
                game, onResume: HandleTogglePause, onOpenOptions: () => {}, onExitToMenu: HandleExitToMenu
            )}
        };
        
        _setMusicPlayerState = setMusicPlayerState;
        
        _screens["TitleScreen"].ScreenState = ScreenState.Active;
        _screens["GameScreen"].ScreenState = ScreenState.Inactive;
        _screens["PauseScreen"].ScreenState = ScreenState.Inactive;
    }

    protected override void LoadContent()
    {
        foreach (var screen in _screens.Values)
        {
            screen.LoadContent();
        }

        base.LoadContent();
    }

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

    public override void Draw(GameTime gameTime)
    {
        foreach (var screen in _screens.Values)
        {
            if (screen.ScreenState != ScreenState.Inactive) screen.Draw(gameTime);
        }

        base.Draw(gameTime);
    }

    private void HandleStart()
    {
        _screens["TitleScreen"].ScreenState = ScreenState.Inactive;
        _screens["GameScreen"].ScreenState = ScreenState.Active;
        _setMusicPlayerState(SongState.MainGame);
    }

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

    private void HandleExitToMenu()
    {
        _screens["PauseScreen"].ScreenState = ScreenState.Inactive;
        _screens["GameScreen"].ScreenState = ScreenState.Inactive;
        _screens["TitleScreen"].ScreenState = ScreenState.Active;
        _setMusicPlayerState(SongState.TitleScreen);
    }
}