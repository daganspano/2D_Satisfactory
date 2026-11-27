using System;
using System.Collections.Generic;
using _2D_Satisfactory.Screens.MainGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2D_Satisfactory.Components.Machines.PopUpDisplay;

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
    #region Private Fields

    // delegate for getting the current crafting information
    private GetCraftingInfoDelegate _getCraftingInfo;

    // sprite batch for rendering the crafting display
    private SpriteBatch _spriteBatch;

    // background fields
    private Texture2D _pixel;
    private Rectangle _backgroundBounds;

    // arrow fields
    private Texture2D _arrow;
    private Rectangle _arrowBounds;

    // ingredient/result fields
    private Texture2D _itemAtlas;
    private List<Item> _inputItems = new List<Item>();
    private List<Item> _outputItems = new List<Item>();
    private List<Rectangle> _ingredientsBounds;
    private List<Vector2> _ingredientsItemPositions;
    private List<Vector2> _ingredientsLabelPositions;
    private List<Rectangle> _resultsBounds;
    private List<Vector2> _resultsItemPositions;
    private List<Vector2> _resultsLabelPositions;

    // progress bar fields
    private float _progress;
    private Rectangle _progressBarBounds;

    // label fields
    private SpriteFont _labelFont;
    private Vector2 _labelPosition;

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CraftDisplay"/> class with the specified game. The constructor sets up the necessary properties for the crafting display, including initializing the input and output item lists, hits required, and hits completed. It also prepares the display for rendering by setting up the background, arrow, ingredient, result, and progress bar bounds.
    /// </summary>
    /// <param name="game">The game instance that this craft display belongs to.</param>
    public CraftDisplay(Game game, GetCraftingInfoDelegate getCraftingInfo) : base(game)
    {
        _getCraftingInfo = getCraftingInfo;
        SetBounds();
    }

    #endregion

    #region Override Methods

    /// <summary>
    /// Loads the content for the crafting display, including initializing the bounds for the display elements, creating the SpriteBatch, loading the pixel texture, loading the arrow texture, and setting up the label font and position. This method is called when the component is added to the game and is responsible for preparing the visual elements of the crafting display for rendering.
    /// </summary>
    protected override void LoadContent()
    {
        // Initialize the SpriteBatch
        _spriteBatch = new SpriteBatch(Game.GraphicsDevice);

        // Initialize the pixel texture.
        _pixel = new Texture2D(Game.GraphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);

        LoadArrow();

        // Load the item atlas texture.
        _itemAtlas = Game.Content.Load<Texture2D>("items_atlas");
        
        // Initialize the label.
        _labelFont = Game.Content.Load<SpriteFont>("Orbitron-Regular");
        Vector2 _labelSize = _labelFont.MeasureString("Iron Ingots");
        _labelPosition = new Vector2(_backgroundBounds.Center.X - _labelSize.X / 2, _backgroundBounds.Top + 10);
    }

    public override void Update(GameTime gameTime)
    {
        // Get the updated crafting information from the delegate
        var (inputItems, outputItems, progress) = _getCraftingInfo();

        // Check if the layout of input or output items has changed
        bool layoutChanged = inputItems.Count != _inputItems.Count || outputItems.Count != _outputItems.Count;

        // Update the display information with the new crafting data
        _inputItems = inputItems;
        _outputItems = outputItems;
        _progress = progress;

        // If the layout has changed, recalculate the item bounds
        if (layoutChanged) SetItemBounds();
    }

    /// <summary>
    /// Draws the crafting display on the screen, including the background, labels, ingredient and result slots, progress bar, and any other visual elements associated with the crafting process. This method is called during the game's draw cycle and is responsible for rendering the current state of the crafting display based on the provided input and output items, as well as the hits required and completed. It ensures that the player can visually track their progress in crafting items.
    /// </summary>
    /// <param name="gameTime">The current game time.</param>
    public override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin();

        // Draw Background
        _spriteBatch.Draw(_pixel, _backgroundBounds, Color.Black * 0.8f);

        // Draw Recipe Name
        _spriteBatch.DrawString(_labelFont, "Iron Ingots", _labelPosition, Color.White);
        
        DrawIngredients();

        // Draw Arrow
        _spriteBatch.Draw(_arrow, _arrowBounds, Color.White * 0.6f);

        DrawResults();

        DrawProgressBar();

        _spriteBatch.End();
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Loads the bounds for the crafting display elements, including the background, ingredient slot, arrow, result slot, and progress bar. This method calculates the positions and sizes of these elements based on the screen size and sets up the corresponding rectangles for rendering. It ensures that the crafting display is properly aligned and visually organized on the screen.
    /// </summary>
    private void SetBounds()
    {
        // Set the background bounds to be centered on the screen
        int backgroundWidth = 300;
        int backgroundHeight = 200;
        int backgroundX = ((int)GameData.ScreenSize.X - backgroundWidth) / 2;
        int backgroundY = ((int)GameData.ScreenSize.Y - backgroundHeight) / 2 - 75;
        _backgroundBounds = new Rectangle(backgroundX, backgroundY, backgroundWidth, backgroundHeight);

        SetItemBounds();

        // Set the progress bar bounds
        int progressWidth = _backgroundBounds.Width - 40;
        int progressHeight = 10;
        int progressX = _backgroundBounds.Left + 20;
        int progressY = _backgroundBounds.Bottom - 30;
        _progressBarBounds = new Rectangle(progressX, progressY, progressWidth, progressHeight);
    }

    private void SetItemBounds() 
    {
        // Set the padding and sizes, and get the total width of the items to center them
        int padding = 16;
        Point ingredientSize = new Point(40, 40);
        Point arrowSize = new Point(10, 10);
        Point resultSize = new Point(60, 60);
        int itemsWidth = (ingredientSize.X + padding) * _inputItems.Count + arrowSize.X + padding 
            + (resultSize.X + padding) * _outputItems.Count - padding;

        // Set the ingredient bounds
        Point location = new Point(_backgroundBounds.Center.X - itemsWidth / 2, _backgroundBounds.Center.Y - ingredientSize.Y / 2);
        _ingredientsBounds = new List<Rectangle>();
        _ingredientsItemPositions = new List<Vector2>();
        _ingredientsLabelPositions = new List<Vector2>();
        for (int i = 0; i < _inputItems.Count; i++)
        {
            Rectangle ingredientBounds = new Rectangle(location, ingredientSize);
            _ingredientsBounds.Add(ingredientBounds);
            _ingredientsItemPositions.Add(ingredientBounds.Center.ToVector2() - new Vector2(16, 16));
            _ingredientsLabelPositions.Add(new Vector2(ingredientBounds.Left + 2, ingredientBounds.Bottom - 14));
            location.X += ingredientSize.X + padding;
        }

        // Set the arrow bounds
        location.Y = _backgroundBounds.Center.Y - arrowSize.Y / 2;
        _arrowBounds = new Rectangle(location, arrowSize);
        location.X += arrowSize.X + padding;

        // Set the result bounds
        location.Y = _backgroundBounds.Center.Y - resultSize.Y / 2;
        _resultsBounds = new List<Rectangle>();
        _resultsItemPositions = new List<Vector2>();
        _resultsLabelPositions = new List<Vector2>();
        for (int i = 0; i < _outputItems.Count; i++)
        {
            Rectangle resultBounds = new Rectangle(location, resultSize);
            _resultsBounds.Add(resultBounds);
            _resultsItemPositions.Add(resultBounds.Center.ToVector2() - new Vector2(24, 24));
            _resultsLabelPositions.Add(new Vector2(resultBounds.Left + 2, resultBounds.Bottom - 14));

            location.X += resultSize.X + padding;
        }
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

    /// <summary>
    /// Draws the input ingredients for the crafting process on the screen.
    /// </summary>
    private void DrawIngredients()
    {
        for (int i = 0; i < _inputItems.Count; i++)
        {
            // Draw Background
            _spriteBatch.Draw(_pixel, _ingredientsBounds[i], Color.LightGray);
            
            // Draw Ingredient Icon
            _spriteBatch.Draw(_itemAtlas, _ingredientsItemPositions[i], new Rectangle(_inputItems[i].IconIndex * 32, 0, 32, 32), Color.White);

            // Draw Quantity Available / Quantity Required
            _spriteBatch.DrawString(_labelFont, _inputItems[i].QuantityAvailable.ToString() + "/" + _inputItems[i].QuantityRequired.ToString(), _ingredientsLabelPositions[i], Color.Black, rotation: 0f, origin: Vector2.Zero, scale: 0.5f, effects: SpriteEffects.None, layerDepth: 0f);
        }
    }

    /// <summary>
    /// Draws the output results for the crafting process on the screen.
    /// </summary>
    private void DrawResults()
    {
        for (int i = 0; i < _outputItems.Count; i++)
        {
            // Draw Background
            _spriteBatch.Draw(_pixel, _resultsBounds[i], Color.LightGray);

            // Draw Result Icon
            _spriteBatch.Draw(_itemAtlas, _resultsItemPositions[i], new Rectangle(_outputItems[i].IconIndex * 32, 0, 32, 32), Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);

            // Draw Output Amount
            _spriteBatch.DrawString(_labelFont, _outputItems[i].QuantityRequired.ToString(), _resultsLabelPositions[i], Color.Black, rotation: 0f, origin: Vector2.Zero, scale: 0.5f, effects: SpriteEffects.None, layerDepth: 0f);
        }
    }

    private void DrawProgressBar()
    {
        _spriteBatch.Draw(_pixel, _progressBarBounds, Color.White * 0.5f);
        if (_progress <= 0) return;
        _spriteBatch.Draw(_pixel, new Rectangle(
            _progressBarBounds.Left,
            _progressBarBounds.Top,
            (int)(_progressBarBounds.Width * _progress),
            _progressBarBounds.Height
        ), Color.DarkOrange);
    }

    #endregion
}