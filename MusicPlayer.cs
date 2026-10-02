using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace _2D_Satisfactory;

/// <summary>
/// A game component that manages background music for the game.
/// </summary>
public class MusicPlayer : GameComponent
{
    private SongCollection _gameMusic;
    private Song _titleScreenMusic;
    private FactoryGameState _previousState;

    public MusicPlayer(Game game) : base(game)
    {
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
    /// Updates the music playback based on the current game state.
    /// </summary>
    /// <param name="gameTime">The current game time.</param>
    public override void Update(GameTime gameTime)
    {
        switch (((FactoryGame)Game).State)
        {
            case FactoryGameState.MainGame:
                if (_previousState != FactoryGameState.MainGame) MediaPlayer.Stop();
                if (MediaPlayer.State == MediaState.Stopped) MediaPlayer.Play(_gameMusic);
                break;

            case FactoryGameState.TitleScreen:
                if (_previousState != FactoryGameState.TitleScreen) MediaPlayer.Stop();
                if (MediaPlayer.State == MediaState.Stopped) MediaPlayer.Play(_titleScreenMusic);
                break;
        }

        _previousState = ((FactoryGame)Game).State;
    }
}