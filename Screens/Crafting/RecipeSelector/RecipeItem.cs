using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2D_Satisfactory.Screens.Crafting;

public class RecipeItem
{
    private Game _game;
    private SpriteBatch _spriteBatch;
    private Recipe _recipe;
    private SpriteFont _font;
    private float _fontScale;
    private Vector2 _position;

    public int Height => (int)(_font.LineSpacing * _fontScale);

    public RecipeItem(Game game, Recipe recipe, Vector2 position)
    {
        _game = game;
        _recipe = recipe;
        _position = position;
    }

    public void LoadContent()
    {
        _spriteBatch = new SpriteBatch(_game.GraphicsDevice);
        _font = _game.Content.Load<SpriteFont>("Orbitron-Regular");
        _fontScale = 0.8f;
    }

    public void Draw()
    {
        _spriteBatch.DrawString(_font, _recipe.Name, _position, Color.White, 0f, Vector2.Zero, _fontScale, SpriteEffects.None, 0f);
    }
}