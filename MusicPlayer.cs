using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace _2D_Satisfactory;

public class MusicPlayer : GameComponent
{
    private FactoryGame _game;
    private SongCollection _gameMusic;
    private Song _titleScreenMusic;
    private FactoryGameState _previousState;

    public MusicPlayer(Game game) : base(game)
    {
        _game = (FactoryGame)game;

        // Load the music content
        _titleScreenMusic = _game.Content.Load<Song>("AutomatedDawn");
        Song song2 = _game.Content.Load<Song>("AssemblyLineTrance");
        Song song3 = _game.Content.Load<Song>("ConveyorDawnHorizon");

        // Create a collection of game music
        _gameMusic = SongCollection.Empty;
        _gameMusic.Add(_titleScreenMusic);
        _gameMusic.Add(song2);
        _gameMusic.Add(song3);

        // Start MediaPlayer with the title screen music
        MediaPlayer.IsRepeating = true;
        MediaPlayer.Volume = 0.05f;
        MediaPlayer.Play(_titleScreenMusic);
    }

    public override void Update(GameTime gameTime)
    {
        switch (_game.State)
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

        _previousState = _game.State;
    }
}