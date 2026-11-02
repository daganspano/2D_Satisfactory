using _2D_Satisfactory.Screens.MainGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2D_Satisfactory.Screens.Title;

public class TitleBanner : DrawableGameComponent
{
    private SpriteBatch _spriteBatch;
    private Texture2D _texture;
    private Vector2 _position;
    private float _scale;

    public TitleBanner(Game game, float yPosition) : base(game)
    {
        _scale = 0.4f;
        _position = new Vector2((GameData.ScreenSize.X - 1147 * _scale) / 2, yPosition);
    }

    public static float GetHeight()
    {
        return 193 * 0.4f;
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
        _texture = Game.Content.Load<Texture2D>("title_banner");
    }

    public override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin();
        _spriteBatch.Draw(_texture, _position, null, Color.White, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
        _spriteBatch.End();
    }
}