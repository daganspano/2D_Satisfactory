using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2D_Satisfactory.MainGameClasses;

/// <summary>
/// Represents an oval-shaped hitbox for collision detection.
/// </summary>
public struct OvalHitBox
{
    private Vector2 _center;
    private float _radiusX;
    private float _radiusY;

    public OvalHitBox(Vector2 center, float radiusX, float radiusY)
    {
        _center = center;
        _radiusX = radiusX;
        _radiusY = radiusY;
    }

    /// <summary>
    /// Determines whether the specified rectangle collides with this oval hitbox.
    /// </summary>
    /// <param name="rectangle">The rectangle to check for collision.</param>
    /// <returns>True if the rectangle collides with the oval hitbox; otherwise, false.</returns>
    public bool CollidesWith(Rectangle rectangle)
    {
        // Check if the rectangle's corners are inside the oval
        Vector2[] corners = new Vector2[]
        {
            new Vector2(rectangle.Left, rectangle.Top),
            new Vector2(rectangle.Right, rectangle.Top),
            new Vector2(rectangle.Left, rectangle.Bottom),
            new Vector2(rectangle.Right, rectangle.Bottom)
        };

        foreach (var corner in corners)
        {
            float normalizedX = (corner.X - _center.X) / _radiusX;
            float normalizedY = (corner.Y - _center.Y) / _radiusY;
            if ((normalizedX * normalizedX + normalizedY * normalizedY) <= 1)
            {
                return true;
            }
        }

        // Check if the oval's center is inside the rectangle
        if (rectangle.Contains(_center)) return true;

        return false;
    }
}

/// <summary>
/// Represents an iron ore deposit in the game world.
/// </summary>
public class IronOreDeposit : DrawableGameComponent
{
    // private SpriteBatch _spriteBatch;
    private Texture2D _texture;
    private OvalHitBox _hitBox;

    public IronOreDeposit(Game game) : base(game)
    {
        DrawOrder = 0;
        Visible = false;
        LoadContent();
        
        float spriteScale = 1.8f;
        Vector2 position = new Vector2(GameDimensions.Width - _texture.Width * 1.2f, GameDimensions.Height * 0.5f);
        Vector2 center = position + _texture.Bounds.Center.ToVector2() * spriteScale;
        float radiusX = _texture.Width * 0.4f * spriteScale;
        float radiusY = _texture.Height * 0.45f * spriteScale;
        _hitBox = new OvalHitBox(center, radiusX, radiusY);
    }

    /// <summary>
    /// Loads the content for the iron ore deposit, including its texture.
    /// </summary>
    protected override void LoadContent()
    {
        _texture = Game.Content.Load<Texture2D>("iron_ore_deposit");
    }

    /// <summary>
    /// Updates the iron ore deposit's visibility based on the current game state.
    /// </summary>
    /// <param name="gameTime">The current game time.</param>
    public override void Update(GameTime gameTime)
    {
        Visible = ((FactoryGame)Game).State == FactoryGameState.MainGame;
    }

    /// <summary>
    /// Draws the iron ore deposit on the screen if it is visible.
    /// </summary>
    /// <param name="spriteBatch">The sprite batch used for drawing.</param>
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, new Vector2(GameDimensions.Width - _texture.Width * 1.2f, GameDimensions.Height * 0.5f), null, Color.White, 0f, Vector2.Zero, 1.8f, SpriteEffects.None, 0f);
    }

    /// <summary>
    /// Determines whether the specified rectangle collides with the iron ore deposit's hitbox.
    /// </summary>
    /// <param name="rectangle">The rectangle to check for collision.</param>
    /// <returns>True if the rectangle collides with the iron ore deposit's hitbox; otherwise, false.</returns>
    public bool CollidesWith(Rectangle rectangle)
    {
        return _hitBox.CollidesWith(rectangle);
    }
}