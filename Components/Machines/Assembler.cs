using System;
using _2D_Satisfactory.Screens.MainGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2D_Satisfactory.Components.Machines;

public class Assembler : DrawableGameComponent
{
    private SpriteBatch _spriteBatch;
    private Texture2D _pixel;
    private Rectangle _hitBox;
    private Rectangle _interactionBounds;

    public Rectangle HitBox => _hitBox;

    public Assembler(Game game, Point tileLocation, string orientation) : base(game)
    {
        if (orientation != "horizontal" && orientation != "vertical")
        {
            throw new ArgumentException("Orientation must be either 'horizontal' or 'vertical'.");
        }

        // Set the hitbox
        int tileSize = 16;
        Point location = tileLocation * tileSize;
        Point size = new Point(4, 4) * tileSize;
        _hitBox = new Rectangle(location, size);

        // Set the interaction bounds
        location -= new Point(1, 1) * tileSize;
        size += new Point(2, 2) * tileSize;
        _interactionBounds = new Rectangle(location, size);
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
            GameData.PlayerInteraction = "openAssemblerMenu";
    }

    public override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin(transformMatrix: GameData.TransformMatrix);

        _spriteBatch.Draw(_pixel, _hitBox, Color.Red);
        _spriteBatch.Draw(_pixel, _interactionBounds, Color.Blue * 0.4f);
        
        _spriteBatch.End();
    }
}