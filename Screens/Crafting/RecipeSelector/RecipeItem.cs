using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2D_Satisfactory.Screens.Crafting;

public enum RecipeInteraction
{
    None,
    Hover,
    Select
}

public class RecipeItem
{
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

    public Rectangle Hitbox => _backgroundBounds;

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

    public void LoadContent()
    {
        _pixel = new Texture2D(_game.GraphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);

        _iconTexture = _game.Content.Load<Texture2D>("items_atlas");
        var iconIndex = Data.GetRecipeImageIndex(_recipe);
        _iconSourceRectangle = new Rectangle(iconIndex * 32, 0, 32, 32);

        _font = _game.Content.Load<SpriteFont>("Orbitron-Regular");
        _fontScale = 0.8f;
        _textPosition = new Vector2(_backgroundBounds.X + 40, _backgroundBounds.Center.Y - _font.MeasureString(_recipe.Name).Y * _fontScale / 2);
    }

    public void Update(RecipeInteraction interactionType)
    {
        _interactionType = interactionType;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        // Draw Background
        spriteBatch.Draw(_pixel, _backgroundBounds, _interactionColors[_interactionType]);

        // Draw Recipe Icon
        spriteBatch.Draw(_iconTexture, _iconPosition, _iconSourceRectangle, Color.White);

        // Draw Recipe Name
        spriteBatch.DrawString(_font, _recipe.Name, _textPosition, Color.White, 0f, Vector2.Zero, _fontScale, SpriteEffects.None, 0f);
    }
}