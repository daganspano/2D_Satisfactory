using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace _2D_Satisfactory.Components.Machines.PopUpDisplay;

#region Interaction Enum
public enum RecipeInteraction
{
    None,
    Hover,
    Select
}
#endregion

public class RecipeItem
{
    #region Private Fields

    private Game _game;
    private Recipe _recipe;
    
    private Texture2D _pixel;
    private Rectangle _backgroundBounds;
    private RecipeInteraction _interactionType;
    private Dictionary<RecipeInteraction, Color> _interactionColors;

    private Texture2D _iconTexture;
    private Rectangle _iconSourceRectangle;
    private Vector2 _iconPosition;

    private SpriteFont _font;
    private float _fontScale;
    private Vector2 _textPosition;

    #endregion

    #region Public Properties

    public Rectangle Hitbox => _backgroundBounds;

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the RecipeItem class.
    /// </summary>
    /// <param name="game">The game instance.</param>
    /// <param name="recipe">The recipe associated with this item.</param>
    /// <param name="backgroundBounds">The bounds of the background rectangle.</param>
    public RecipeItem(Game game, Recipe recipe, Rectangle backgroundBounds)
    {
        _game = game;
        _recipe = recipe;
        _backgroundBounds = backgroundBounds;
        _interactionType = RecipeInteraction.None;
        _interactionColors = new Dictionary<RecipeInteraction, Color>
        {
            { RecipeInteraction.None, Color.DarkGray * 0f },
            { RecipeInteraction.Hover, Color.DarkGray },
            { RecipeInteraction.Select, Color.Orange }
        };
        _iconPosition = new Vector2(_backgroundBounds.X + 4, _backgroundBounds.Center.Y - 16);
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Loads the content required for the recipe item.
    /// </summary>
    public void LoadContent()
    {
        // Load pixel
        _pixel = new Texture2D(_game.GraphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);

        LoadIcon();
        LoadFont();
    }

    /// <summary>
    /// Updates the interaction state of the recipe item.
    /// </summary>
    /// <param name="interactionType">The new interaction state.</param>
    public void Update(RecipeInteraction interactionType)
    {
        _interactionType = interactionType;
    }

    /// <summary>
    /// Draws the recipe item.
    /// </summary>
    /// <param name="spriteBatch">The sprite batch used for drawing.</param>
    public void Draw(SpriteBatch spriteBatch)
    {
        // Draw Background
        spriteBatch.Draw(_pixel, _backgroundBounds, _interactionColors[_interactionType]);

        // Draw Recipe Icon
        spriteBatch.Draw(_iconTexture, _iconPosition, _iconSourceRectangle, Color.White);

        // Draw Recipe Name
        spriteBatch.DrawString(_font, _recipe.Name, _textPosition, Color.White, 0f, Vector2.Zero, _fontScale, SpriteEffects.None, 0f);
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Loads the icon texture and source rectangle for the recipe item.
    /// </summary>
    private void LoadIcon()
    {
        _iconTexture = _game.Content.Load<Texture2D>("items_atlas");
        var iconIndex = Data.GetRecipeImageIndex(_recipe);
        _iconSourceRectangle = new Rectangle(iconIndex * 32, 0, 32, 32);
    }

    /// <summary>
    /// Loads the font and calculates the text position for the recipe item.
    /// </summary>
    private void LoadFont()
    {
        _font = _game.Content.Load<SpriteFont>("Orbitron-Regular");
        _fontScale = 0.8f;
        _textPosition = new Vector2(_backgroundBounds.X + 40, _backgroundBounds.Center.Y - _font.MeasureString(_recipe.Name).Y * _fontScale / 2);
    }

    #endregion
}