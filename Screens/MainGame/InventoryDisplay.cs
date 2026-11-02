using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2D_Satisfactory.Screens.MainGame;

public class InventoryDisplay : DrawableGameComponent
{
    private SpriteBatch _spriteBatch;
    private SpriteFont _font;

    public InventoryDisplay(Game game) : base(game)
    {
    }
    
    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
        _font = Game.Content.Load<SpriteFont>("Orbitron-Regular");
    }


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
                _spriteBatch.DrawString(_font, $"   {item.Key}: {item.Value}", inventoryPosition, Color.White);
            }
        }

        _spriteBatch.End();
    }
}