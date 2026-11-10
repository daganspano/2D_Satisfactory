using System;
using _2D_Satisfactory.Screens.MainGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _2D_Satisfactory.Screens.Crafting;

public class CraftButton : DrawableGameComponent
{
    private readonly Action _toggleCrafting;
    private SpriteBatch _spriteBatch;
    private Rectangle _hitBox;
    private Color _hitBoxColor;
    private bool _wasPressed;

    public CraftButton(Game game, Action toggleCrafting) : base(game)
    {
        _toggleCrafting = toggleCrafting;
        var width = 300;
        var height = 75;
        var x = ((int)GameData.ScreenSize.X - width) / 2;
        var y = (int)GameData.ScreenSize.Y / 2 + 75;
        _hitBox = new Rectangle(x, y, width, height);
        _hitBoxColor = new Color(255, 255, 0);
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
    }

    public override void Update(GameTime gameTime)
    {
        MouseState mouseState = Mouse.GetState();
        bool isPressed = Keyboard.GetState().IsKeyDown(Keys.Space) || 
            GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.X) ||
            (_hitBox.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed);

        if (isPressed && !_wasPressed)
        {
            _toggleCrafting();
        }

        _wasPressed = isPressed;
    }

    public override void Draw(GameTime gameTime)
    {
        var pixel = new Texture2D(Game.GraphicsDevice, 1, 1);
        pixel.SetData([Color.White]);

        _spriteBatch.Begin();
        _spriteBatch.Draw(pixel, _hitBox, new Color(200, 200, 0));
        _spriteBatch.End();
    }
}