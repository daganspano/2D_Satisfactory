using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2D_Satisfactory;

public class Tilemap: DrawableGameComponent
{
    private static SpriteBatch _spriteBatch;
    private Texture2D _atlas;
    private Vector2 _offset;

    private Rectangle[] _tiles;

    public Tilemap(Game game): base(game) {}

    public void LoadMap()
    {
        LoadContent();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(Game.GraphicsDevice);

        _atlas = Game.Content.Load<Texture2D>(Map.Filename);

        _offset = Vector2.Zero;

        _tiles = new Rectangle[Map.AtlasDimensions.X * Map.AtlasDimensions.Y];
        for (int y = 0; y < Map.AtlasDimensions.Y; y++)
        {
            for (int x = 0; x < Map.AtlasDimensions.X; x++)
            {
                _tiles[y * Map.AtlasDimensions.X + x] = new Rectangle(x * Map.TileSize.X, y * Map.TileSize.Y, Map.TileSize.X, Map.TileSize.Y);
            }
        }
    }

    public void UpdateOffset(Vector2 offset)
    {
        _offset = offset;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        for (int y = 0; y < Map.MapSize.Y; y++)
        {
            for (int x = 0; x < Map.MapSize.X; x++)
            {
                Vector2 position = new Vector2(x * Map.TileSize.X, y * Map.TileSize.Y) + _offset;
                int index = Map.Tiles[y][x];
                spriteBatch.Draw(_atlas, position, _tiles[index], Color.White);
            }
        }
    }

    public override void Draw(GameTime gameTime)
    {
        Matrix transform = Matrix.CreateTranslation(_offset.X, _offset.Y, 0f);
        _spriteBatch.Begin(transformMatrix: transform);

        _spriteBatch.Draw(_atlas, _offset, new Rectangle(Point.Zero, Map.TileSize), Color.White, 0f, Vector2.Zero, 1.8f, SpriteEffects.None, 0f);
                
        _spriteBatch.End();
    }

    public bool CanMineIronOre(Rectangle hitBox)
    {
        return Map.IsCollidingWithIronOreDeposit(hitBox);
    }
}