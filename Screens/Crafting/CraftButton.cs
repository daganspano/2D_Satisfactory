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
    private SpriteFont _labelFont;
    private Vector2 _labelPosition;
    private bool _wasPressed;

    public CraftButton(Game game, Action toggleCrafting) : base(game)
    {
        _toggleCrafting = toggleCrafting;
        var width = 300;
        var height = 75;
        var x = ((int)GameData.ScreenSize.X - width) / 2;
        var y = (int)GameData.ScreenSize.Y / 2 + 75;
        _hitBox = new Rectangle(x, y, width, height);
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
        
        // Initialize the label font and position.
        _labelFont = Game.Content.Load<SpriteFont>("Orbitron-Bold");
        Vector2 _labelSize = _labelFont.MeasureString("CRAFT");
        _labelPosition = new Vector2(_hitBox.Center.X - _labelSize.X / 2, _hitBox.Center.Y - _labelSize.Y / 2);
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
        _spriteBatch.DrawString(_labelFont, "CRAFT", _labelPosition, Color.Black);
        _spriteBatch.End();
    }
}