using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2D_Satisfactory.Screens.MainGame;

/// <summary>
/// Represents the inventory display component in the game, responsible for rendering the player's inventory on the screen.
/// </summary>
public class InventoryDisplay : DrawableGameComponent
{
    private SpriteBatch _spriteBatch;
    private SpriteFont _font;

    public InventoryDisplay(Game game) : base(game)
    {
    }
    
    /// <summary>
    /// Loads the content required for the inventory display, including the sprite batch and font.
    /// </summary>
    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
        _font = Game.Content.Load<SpriteFont>("Orbitron-Regular");
    }

    /// <summary>
    /// Draws the inventory display on the screen.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    public override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin();
        
        Vector2 inventoryPosition = new Vector2(10, 10);
        _spriteBatch.DrawString(_font, "Inventory:", inventoryPosition, Color.White);
        foreach (var item in GameData.PlayerInventory)
        {
            if (item.Value > 0)
            {
                inventoryPosition.Y += 30;
                _spriteBatch.DrawString(_font, $"   {Data.GetItem(item.Key).Name}: {item.Value}", inventoryPosition, Color.White);
            }
        }

        _spriteBatch.End();
    }
}