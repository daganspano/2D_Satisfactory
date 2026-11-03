using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace _2D_Satisfactory;

/// <summary>
/// Represents the different states of the game for music playback purposes.
/// </summary>
public enum SongState
{
    TitleScreen,
    MainGame
}

/// <summary>
/// A game component that manages background music for the game.
/// </summary>
public class MusicPlayer : GameComponent
{
    private SongCollection _gameMusic;
    private Song _titleScreenMusic;
    private SongState _currentState;
    private SongState _previousState;

    public MusicPlayer(Game game) : base(game)
    {
        _currentState = SongState.TitleScreen;
        _previousState = SongState.TitleScreen;

        // Load the music content
        _titleScreenMusic = Game.Content.Load<Song>("AutomatedDawn");
        Song song2 = Game.Content.Load<Song>("AssemblyLineTrance");
        Song song3 = Game.Content.Load<Song>("ConveyorDawnHorizon");

        // Create a collection of game music
        _gameMusic = SongCollection.Empty;
        _gameMusic.Add(_titleScreenMusic);
        _gameMusic.Add(song2);
        _gameMusic.Add(song3);

        // Start MediaPlayer with the title screen music
        MediaPlayer.IsRepeating = true;
        MediaPlayer.Volume = 0.08f;
        MediaPlayer.Play(_titleScreenMusic);
    }

    /// <summary>
    /// Sets the current state of the game, which determines which music to play.
    /// </summary>
    /// <param name="state">The new state of the game.</param>
    public void SetState(SongState state)
    {
        _currentState = state;
    }

    /// <summary>
    /// Updates the music playback based on the current game state.
    /// </summary>
    /// <param name="gameTime">The current game time.</param>
    public override void Update(GameTime gameTime)
    {
        if (_currentState != _previousState)
        {
            switch (_currentState)
            {
            case SongState.MainGame:
                MediaPlayer.Stop();
                MediaPlayer.Play(_gameMusic);
                break;

            case SongState.TitleScreen:
                MediaPlayer.Stop();
                MediaPlayer.Play(_titleScreenMusic);
                break;
            }

            _previousState = _currentState;
        }
    }
}