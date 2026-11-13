using System.Collections.Generic;
using _2D_Satisfactory.Screens.MainGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2D_Satisfactory.Screens.Crafting;

/// <summary>
/// Delegate for updating the display information in the crafting screen. This delegate is used to pass the input items, output items, hits required, and hits completed to the CraftDisplay component, allowing it to update its visual representation based on the current crafting state.
/// </summary>
/// <param name="inputItems">The list of input items required for crafting.</param>
/// <param name="outputItems">The list of output items produced by crafting.</param>
/// <param name="hitsRequired">The number of hits required to complete the crafting.</param>
/// <param name="hitsCompleted">The number of hits completed so far.</param>
public delegate void UpdateDisplayInfoDelegate(List<Item> inputItems, List<Item> outputItems, int hitsRequired, int hitsCompleted);

/// <summary>
/// Represents the display component in the crafting screen that shows the input items, output items, and progress of the crafting process. This class handles the rendering of the crafting interface, including the background, labels, ingredient and result slots, and progress bar. It also manages the state of the crafting process by updating the display based on the provided input and output items, as well as the hits required and completed.
/// </summary>
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

    /// <summary>
    /// Initializes a new instance of the <see cref="CraftDisplay"/> class with the specified game. The constructor sets up the necessary properties for the crafting display, including initializing the input and output item lists, hits required, and hits completed. It also prepares the display for rendering by setting up the background, arrow, ingredient, result, and progress bar bounds.
    /// </summary>
    /// <param name="game">The game instance that this craft display belongs to.</param>
    public CraftDisplay(Game game) : base(game)
    {
    }

    /// <summary>
    /// Updates the display information for the crafting screen, including the input items, output items, hits required, and hits completed. This method is called to refresh the visual representation of the crafting process based on the current state of the crafting system. It allows the CraftDisplay component to reflect changes in the crafting state and provide feedback to the player.
    /// </summary>
    /// <param name="inputItems">The list of input items required for crafting.</param>
    /// <param name="outputItems">The list of output items produced by crafting.</param>
    /// <param name="hitsRequired">The number of hits required to complete the crafting.</param>
    /// <param name="hitsCompleted">The number of hits completed so far.</param>
    public void UpdateDisplayInfo(List<Item> inputItems, List<Item> outputItems, int hitsRequired, int hitsCompleted)
    {
        _inputItems = inputItems;
        _outputItems = outputItems;
        _hitsRequired = hitsRequired;
        _hitsCompleted = hitsCompleted;
    }

    /// <summary>
    /// Loads the content for the crafting display, including initializing the bounds for the display elements, creating the SpriteBatch, loading the pixel texture, loading the arrow texture, and setting up the label font and position. This method is called when the component is added to the game and is responsible for preparing the visual elements of the crafting display for rendering.
    /// </summary>
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

    /// <summary>
    /// Draws the crafting display on the screen, including the background, labels, ingredient and result slots, progress bar, and any other visual elements associated with the crafting process. This method is called during the game's draw cycle and is responsible for rendering the current state of the crafting display based on the provided input and output items, as well as the hits required and completed. It ensures that the player can visually track their progress in crafting items.
    /// </summary>
    /// <param name="gameTime">The current game time.</param>
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
            _spriteBatch.DrawString(_labelFont, _inputItems[i].QuantityAvailable.ToString() + "/" + _inputItems[i].QuantityRequired.ToString(), new Vector2(_ingredientBounds.X - (_ingredientBounds.Width + 10) * i + 2, _ingredientBounds.Y + _ingredientBounds.Height - 14), Color.White, rotation: 0f, origin: Vector2.Zero, scale: 0.5f, effects: SpriteEffects.None, layerDepth: 0f);
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
            _spriteBatch.DrawString(_labelFont, _outputItems[i].QuantityRequired.ToString(), new Vector2(_resultBounds.X + (_resultBounds.Width + 10) * i + 2, _resultBounds.Y + _resultBounds.Height - 14), Color.White, rotation: 0f, origin: Vector2.Zero, scale: 0.5f, effects: SpriteEffects.None, layerDepth: 0f);
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

    /// <summary>
    /// Loads the bounds for the crafting display elements, including the background, ingredient slot, arrow, result slot, and progress bar. This method calculates the positions and sizes of these elements based on the screen size and sets up the corresponding rectangles for rendering. It ensures that the crafting display is properly aligned and visually organized on the screen.
    /// </summary>
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

    /// <summary>
    /// Loads the arrow texture used in the crafting display. This method generates a simple arrow shape by creating a texture with a gradient effect, where the arrow is white and the surrounding area is transparent. The arrow is drawn in the center of the crafting display to indicate the direction of crafting from input items to output items. The generated texture is then assigned to the _arrow field for rendering during the Draw method.
    /// </summary>
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