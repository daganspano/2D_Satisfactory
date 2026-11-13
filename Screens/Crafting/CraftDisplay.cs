using System.Collections.Generic;
using _2D_Satisfactory.Screens.MainGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2D_Satisfactory.Screens.Crafting;

public delegate void UpdateDisplayInfoDelegate(List<Item> inputItems, List<Item> outputItems, int hitsRequired, int hitsCompleted);

public class CraftDisplay : DrawableGameComponent
{
    private SpriteBatch _spriteBatch;
    private Texture2D _pixel;
    private Texture2D _arrow;
    private Rectangle _backgroundBounds;
    private Rectangle _arrowBounds;
    private Rectangle _ingredientBounds;
    private Rectangle _resultBounds;
    private Rectangle _progressBarBounds;
    private SpriteFont _labelFont;
    private Vector2 _labelPosition;

    private List<Item> _inputItems;
    private List<Item> _outputItems;
    private int _hitsRequired;
    private int _hitsCompleted;
    private IronIngot _ironIngot;

    public CraftDisplay(Game game) : base(game)
    {
    }

    public void UpdateDisplayInfo(List<Item> inputItem, List<Item> outputItem, int hitsRequired, int hitsCompleted)
    {
        _inputItems = inputItem;
        _outputItems = outputItem;
        _hitsRequired = hitsRequired;
        _hitsCompleted = hitsCompleted;
    }

    protected override void LoadContent()
    {
        LoadBounds(); // Initialize the bounds for the display elements.

        _spriteBatch = new SpriteBatch(Game.GraphicsDevice); // Initialize the SpriteBatch

        // Initialize the pixel texture.
        _pixel = new Texture2D(Game.GraphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);

        LoadArrow(); // Initialize the arrow texture.
        
        // Initialize the label font and position.
        _labelFont = Game.Content.Load<SpriteFont>("Orbitron-Regular");
        Vector2 _labelSize = _labelFont.MeasureString("Iron Ingots");
        _labelPosition = new Vector2(_backgroundBounds.Center.X - _labelSize.X / 2, _backgroundBounds.Top + 10);

        // Initialize the iron ingot.
        _ironIngot = new IronIngot(Game, _resultBounds);
    }

    public override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin();

        // Background
        _spriteBatch.Draw(_pixel, _backgroundBounds, Color.Black * 0.8f);

        // Name
        _spriteBatch.DrawString(_labelFont, "Iron Ingots", _labelPosition, Color.White);
        
        // Ingredient
        for (int i = 0; i < _inputItems.Count; i++)
        {
            _spriteBatch.Draw(_pixel, new Rectangle(
                _ingredientBounds.X - (_ingredientBounds.Width + 10) * i, 
                _ingredientBounds.Y, 
                _ingredientBounds.Width, 
                _ingredientBounds.Height
            ), Color.Blue);
            _spriteBatch.DrawString(_labelFont, _inputItems[i].Name, new Vector2(_ingredientBounds.X - (_ingredientBounds.Width + 10) * i, _ingredientBounds.Y), Color.White, rotation: 0f, origin: Vector2.Zero, scale: 0.5f, effects: SpriteEffects.None, layerDepth: 0f);
        }

        // Arrow
        _spriteBatch.Draw(_arrow, _arrowBounds, Color.White * 0.6f);

        // Result
        for (int i = 0; i < _outputItems.Count; i++)
        {
            _spriteBatch.Draw(_pixel, new Rectangle(
                _resultBounds.X + (_resultBounds.Width + 10) * i,
                _resultBounds.Y,
                _resultBounds.Width,
                _resultBounds.Height
            ), Color.DarkGreen * 0.6f);
        }


        // Progress bar
        _spriteBatch.Draw(_pixel, _progressBarBounds, Color.White * 0.5f);
        _spriteBatch.Draw(_pixel, new Rectangle(
            _progressBarBounds.Left,
            _progressBarBounds.Top,
            _progressBarBounds.Width * _hitsCompleted / _hitsRequired,
            _progressBarBounds.Height
        ), Color.DarkOrange);

        _spriteBatch.End();

        _ironIngot.Draw(); // Temporary draw call for the iron ingot.
    }

    private void LoadBounds()
    {
        int backgroundWidth = 300;
        int backgroundHeight = 200;
        int backgroundX = ((int)GameData.ScreenSize.X - backgroundWidth) / 2;
        int backgroundY = ((int)GameData.ScreenSize.Y - backgroundHeight) / 2 - 75;
        _backgroundBounds = new Rectangle(backgroundX, backgroundY, backgroundWidth, backgroundHeight);

        int ingredientSize = 40;
        int ingredientX = _backgroundBounds.Center.X - ingredientSize - 24;
        int ingredientY = _backgroundBounds.Center.Y - ingredientSize / 2;
        _ingredientBounds = new Rectangle(ingredientX, ingredientY, ingredientSize, ingredientSize);

        int arrowSize = 10;
        int arrowX = _backgroundBounds.Center.X - arrowSize / 2;
        int arrowY = _backgroundBounds.Center.Y - arrowSize / 2;
        _arrowBounds = new Rectangle(arrowX, arrowY, arrowSize, arrowSize);

        int resultSize = 60;
        int resultX = _backgroundBounds.Center.X + 24;
        int resultY = _backgroundBounds.Center.Y - resultSize / 2;
        _resultBounds = new Rectangle(resultX, resultY, resultSize, resultSize);

        int progressWidth = _backgroundBounds.Width - 40;
        int progressHeight = 10;
        int progressX = _backgroundBounds.Left + 20;
        int progressY = _backgroundBounds.Bottom - 30;
        _progressBarBounds = new Rectangle(progressX, progressY, progressWidth, progressHeight);
    }

    private void LoadArrow()
    {
        var data = new Color[_arrowBounds.Width * _arrowBounds.Height];
        for (int py = 0; py < _arrowBounds.Height; py++)
        {
            for (int px = 0; px < _arrowBounds.Width; px++)
            {
                var halfHeight = _arrowBounds.Height / 2f * (1 - (float)px / _arrowBounds.Width);
                var distFromCenter = System.Math.Abs(py + 0.5f - _arrowBounds.Height / 2f);
                data[py * _arrowBounds.Width + px] = distFromCenter <= halfHeight ? Color.White : Color.Transparent;
            }
        }
        _arrow = new Texture2D(Game.GraphicsDevice, _arrowBounds.Width, _arrowBounds.Height);
        _arrow.SetData(data);
    }
}