using _2D_Satisfactory.Screens.MainGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace _2D_Satisfactory.Screens.WinScreenClasses;

public class WinProgressBar : DrawableGameComponent
{
    #region Private Fields

    private SpriteBatch _spriteBatch;
    private const int smartPlatesTargetAmount = 50;
    private Texture2D _pixel;
    private Rectangle _outlineBounds;
    private Rectangle _barBounds;
    private Rectangle _fillBounds;
    private SpriteFont _font;
    private string _text;
    private Vector2 _textPosition;

    #endregion

    #region Constructor

    public WinProgressBar(Game game) : base(game)
    {
        var gameWidth = Game.GraphicsDevice.Viewport.Width;
        var location = new Point(gameWidth / 4, 16);
        var size = new Point(gameWidth / 2, 30);
        _outlineBounds = new Rectangle(location, size);

        location += new Point(2, 2);
        size -= new Point(4, 4);
        _barBounds = new Rectangle(location, size);

        size.X = 0;
        _fillBounds = new Rectangle(location, size);
        _text = $"Smart Plates: 0/{smartPlatesTargetAmount}";
    }

    #endregion

    #region Override Methods

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Load the pixel texture.
        _pixel = new Texture2D(Game.GraphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);

        // Load the font.
        _font = Game.Content.Load<SpriteFont>("Orbitron-Regular");
        Vector2 textSize = _font.MeasureString(_text);
        _textPosition = new Vector2(_barBounds.Center.X - textSize.X / 2, _barBounds.Center.Y - textSize.Y / 2);
    }

    public override void Update(GameTime gameTime)
    {
        if (!Visible) return;
        
        int smartPlates = 0;
        if (GameData.PlayerInventory.ContainsKey("smartPlate")) 
        {
            smartPlates = GameData.PlayerInventory["smartPlate"];
        }

        _fillBounds.Width = (int)(_barBounds.Width * (Math.Min(smartPlates, smartPlatesTargetAmount) / (float)smartPlatesTargetAmount));
        _text = $"Smart Plates: {smartPlates}/{smartPlatesTargetAmount}";
        _textPosition.X = _barBounds.Center.X - _font.MeasureString(_text).X / 2;
    }

    public override void Draw(GameTime gameTime)
    {
        if (!Visible) return;

        _spriteBatch.Begin();

        _spriteBatch.Draw(_pixel, _outlineBounds, Color.Black);
        _spriteBatch.Draw(_pixel, _barBounds, Color.White);
        _spriteBatch.Draw(_pixel, _fillBounds, Color.Orange);
        _spriteBatch.DrawString(_font, _text, _textPosition, Color.Black);

        _spriteBatch.End();
    }

    #endregion
}