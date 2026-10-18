using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2D_Satisfactory;

/// <summary>
/// Represents a tilemap component in the game, responsible for rendering the map's tiles and handling interactions with specific tile types, such as iron ore deposits. This class extends DrawableGameComponent and provides methods to load the map, update its offset, and draw the tiles on the screen.
/// </summary>
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

    /// <summary>
    /// Loads the content required for rendering the tilemap, including the sprite batch, texture atlas, and tile rectangles. This method initializes the necessary resources for drawing the map's tiles on the screen.
    /// </summary>
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

    /// <summary>
    /// Updates the offset of the tilemap, allowing for scrolling or panning of the map. This method takes a Vector2 parameter representing the new offset and updates the internal offset value accordingly. The offset is used to adjust the position of the tiles when drawing them on the screen.
    /// </summary>
    /// <param name="offset">The new offset for the tilemap.</param>
    public void UpdateOffset(Vector2 offset)
    {
        _offset = offset;
    }

    /// <summary>
    /// Draws the tilemap on the screen using the provided SpriteBatch. This method iterates through the map's tile data and draws each tile at its corresponding position, adjusted by the current offset. The tiles are drawn using the texture atlas and the appropriate source rectangles for each tile type.
    /// </summary>
    /// <param name="spriteBatch">The SpriteBatch used for drawing the tilemap.</param>
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

    /// <summary>
    /// Draws the tilemap on the screen using the internal SpriteBatch. This method is called during the game's draw phase and handles rendering the map's tiles with the current offset applied. It begins a new sprite batch, iterates through the map's tile data, and draws each tile at its corresponding position, then ends the sprite batch.
    /// </summary>
    /// <param name="gameTime">The current game time.</param>
    public override void Draw(GameTime gameTime)
    {
        Matrix transform = Matrix.CreateTranslation(_offset.X, _offset.Y, 0f);
        _spriteBatch.Begin(transformMatrix: transform);

        for (int y = 0; y < Map.MapSize.Y; y++)
        {
            for (int x = 0; x < Map.MapSize.X; x++)
            {
                Vector2 position = new Vector2(x * Map.TileSize.X, y * Map.TileSize.Y) + _offset;
                int index = Map.Tiles[y][x];
                _spriteBatch.Draw(_atlas, position, _tiles[index], Color.White);
            }
        }
                
        _spriteBatch.End();
    }

    /// <summary>
    /// Determines whether a given hitbox (represented as a Rectangle) can mine iron ore deposits on the map. This method checks for collisions between the hitbox and any iron ore deposit tiles by calling the Map's IsCollidingWithIronOreDeposit method. If a collision is detected, it indicates that the hitbox can mine iron ore, and the method returns true; otherwise, it returns false.
    /// </summary>
    /// <param name="hitBox">The hitbox to check for mining iron ore deposits.</param>
    /// <returns>True if the hitbox can mine iron ore deposits; otherwise, false.</returns>
    public bool CanMineIronOre(Rectangle hitBox)
    {
        return Map.IsCollidingWithIronOreDeposit(hitBox);
    }
}