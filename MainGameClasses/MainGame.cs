using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace _2D_Satisfactory.MainGameClasses;

public class MainGame
{
    // Sprite Fields
    private Texture2D _backgroundTexture;

    public MainGame()
    {
        // Initialize main game components here
    }

    public void LoadContent(ContentManager content)
    {
        // Load background
        _backgroundTexture = content.Load<Texture2D>("forest_background");
    }

    public void Update(GameTime gameTime)
    {
        // Update main game logic here
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        // Draw background
        spriteBatch.Draw(_backgroundTexture, new Vector2(0, 0), null, Color.White, 0f, Vector2.Zero, 1.8f, SpriteEffects.None, 0f);
    }
}