using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace _2D_Satisfactory.MainGameClasses;

public class MainGame
{
    // Sprite Fields
    private Texture2D _backgroundTexture;
    private GameCharacter _character;

    public MainGame()
    {
        Vector2 initialPosition = new Vector2(GameDimensions.X / 2, GameDimensions.Y / 2);
        int character = 0;
        int speed = 50;
        _character = new GameCharacter(initialPosition, character, speed);
    }

    public void LoadContent(ContentManager content)
    {
        // Load background
        _backgroundTexture = content.Load<Texture2D>("forest_background");

        // Load character
        _character.LoadContent(content);
    }

    public void Update(GameTime gameTime)
    {
        // Update character
        _character.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        // Draw background
        spriteBatch.Draw(_backgroundTexture, new Vector2(0, 0), null, Color.White, 0f, Vector2.Zero, 1.8f, SpriteEffects.None, 0f);

        // Draw character
        _character.Draw(spriteBatch);
    }
}