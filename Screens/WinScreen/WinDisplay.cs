using _2D_Satisfactory.Components.ButtonClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using _2D_Satisfactory.Screens.MainGame;

namespace _2D_Satisfactory.Screens.WinScreenClasses;

public class WinDisplay : DrawableGameComponent
{
    #region Private Fields
    private SpriteBatch _spriteBatch;
    private Texture2D _pixel;
    private SpriteFont _font;
    private Rectangle _backgroundBounds;
    private string _text;
    private Vector2 _textPosition;

    #endregion

    #region Constructor

    public WinDisplay(Game game) : base(game)
    {
        var screenSize = GameData.ScreenSize.ToPoint();

        var location = new Point(screenSize.X / 4, (int)(screenSize.Y * 0.1f));
        var size = new Point(screenSize.X / 2, (int)(screenSize.Y * 0.8f));
        _backgroundBounds = new Rectangle(location, size);
        _text = "You Win!";
    }

    #endregion

    #region Public Methods

    public int GetButtonYPosition()
    {
        Vector2 textSize = Game.Content.Load<SpriteFont>("Orbitron-Regular").MeasureString("You Win!");
        int padding = 16;
        int displayHeight = (int)(textSize.Y + padding + ButtonGroup.GetHeight(4));
        int yPosition = ((int)GameData.ScreenSize.Y - displayHeight) / 2 + (int)textSize.Y + padding;
        return yPosition;
    }

    #endregion

    #region Override Methods

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Load the pixel texture.
        _pixel = new Texture2D(Game.GraphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);

        // Load the font
        _font = Game.Content.Load<SpriteFont>("Orbitron-Regular");
        Vector2 textSize = _font.MeasureString(_text);

        int padding = 16;
        int displayHeight = (int)(textSize.Y + padding + ButtonGroup.GetHeight(4));
        int yPosition = _backgroundBounds.Center.Y - displayHeight / 2;
        _textPosition = new Vector2(_backgroundBounds.Center.X - textSize.X / 2, yPosition);
    }

    public override void Draw(GameTime gameTime)
    {
        if (!Visible) return;

        _spriteBatch.Begin();

        // Draw background
        _spriteBatch.Draw(_pixel, _backgroundBounds, Color.Black * 0.5f);

        // Draw text
        _spriteBatch.DrawString(_font, _text, _textPosition, Color.White);

        _spriteBatch.End();
    }

    #endregion

    #region Private Methods

    #endregion
}