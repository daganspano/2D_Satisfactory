using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2D_Satisfactory.MainGameClasses;

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

    protected override void LoadContent()
    {
        // _spriteBatch = new SpriteBatch(GraphicsDevice);
        _texture = Game.Content.Load<Texture2D>("iron_ore_deposit");
    }

    public override void Update(GameTime gameTime)
    {
        Visible = ((FactoryGame)Game).State == FactoryGameState.MainGame;

        // Add logic for whether player is near the iron ore deposit and can mine it
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        // _spriteBatch.Begin();

        

        spriteBatch.Draw(_texture, new Vector2(GameDimensions.Width - _texture.Width * 1.2f, GameDimensions.Height * 0.5f), null, Color.White, 0f, Vector2.Zero, 1.8f, SpriteEffects.None, 0f);

        // spriteBatch.End();
    }

    public bool CollidesWith(Rectangle rectangle)
    {
        return _hitBox.CollidesWith(rectangle);
    }
}