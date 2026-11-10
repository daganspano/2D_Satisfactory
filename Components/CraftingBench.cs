using System;
using _2D_Satisfactory.Screens.MainGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2D_Satisfactory.Components;

public class CraftingBench : DrawableGameComponent
{
    private SpriteBatch _spriteBatch;
    private Texture2D _pixel;
    private Rectangle _hitBox;
    private Rectangle _interactionBounds = new Rectangle(42 * 16, 14 * 16, 4 * 16, 5 * 16);

    public Rectangle HitBox => _hitBox;

    public CraftingBench(Game game, Point location, string orientation) : base(game)
    {
        if (orientation != "horizontal" && orientation != "vertical")
        {
            throw new ArgumentException("Orientation must be either 'horizontal' or 'vertical'.");
        }

        int x = location.X * 16;
        int y = location.Y * 16;
        int width = orientation == "horizontal" ? 48 : 32;
        int height = orientation == "horizontal" ? 32 : 48;

        _hitBox = new Rectangle(x, y, width, height);
        _interactionBounds = new Rectangle(x - 16, y - 16, width + 32, height + 32);
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
        _pixel = new Texture2D(Game.GraphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);
    }

    public override void Update(GameTime gameTime)
    {
        if (GameData.PlayerHitBox.Intersects(_interactionBounds))
            GameData.PlayerInteraction = "openCraftingMenu";
    }

    public override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin(transformMatrix: GameData.TransformMatrix);
        _spriteBatch.Draw(_pixel, _hitBox, Color.Red);
        _spriteBatch.Draw(_pixel, _interactionBounds, Color.Blue * 0.4f);
        _spriteBatch.End();
    }
}