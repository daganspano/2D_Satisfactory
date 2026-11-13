using System;
using _2D_Satisfactory.Screens.MainGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _2D_Satisfactory.Screens.Crafting;

/// <summary>
/// Represents a button in the crafting screen that allows players to initiate crafting actions. This class handles the rendering and interaction logic for the craft button, enabling players to trigger crafting when the button is pressed.
/// </summary>
public class CraftButton : DrawableGameComponent
{
    private readonly Action _toggleCrafting;
    private SpriteBatch _spriteBatch;
    private Rectangle _hitBox;
    private SpriteFont _labelFont;
    private Vector2 _labelPosition;
    private bool _wasPressed;

    /// <summary>
    /// Initializes a new instance of the <see cref="CraftButton"/> class with the specified game and action to toggle crafting. The constructor sets up the hit box for the button based on the screen size and positions it appropriately in the game world. It also stores the action that will be invoked when the button is pressed, allowing for interaction with the crafting system.
    /// </summary>
    /// <param name="game">The game instance that this craft button belongs to.</param>
    /// <param name="toggleCrafting">The action to invoke when the craft button is pressed.</param>
    public CraftButton(Game game, Action toggleCrafting) : base(game)
    {
        _toggleCrafting = toggleCrafting;
        var width = 300;
        var height = 75;
        var x = ((int)GameData.ScreenSize.X - width) / 2;
        var y = (int)GameData.ScreenSize.Y / 2 + 75;
        _hitBox = new Rectangle(x, y, width, height);
    }

    /// <summary>
    /// Loads the content required for the craft button, including initializing the sprite batch and loading the font for the button label. This method is called when the game component is added to the game and is responsible for setting up the necessary graphics resources for rendering the button and its label.
    /// </summary>
    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
        
        // Initialize the label font and position.
        _labelFont = Game.Content.Load<SpriteFont>("Orbitron-Bold");
        Vector2 _labelSize = _labelFont.MeasureString("CRAFT");
        _labelPosition = new Vector2(_hitBox.Center.X - _labelSize.X / 2, _hitBox.Center.Y - _labelSize.Y / 2);
    }

    /// <summary>
    /// Updates the state of the craft button, checking for player input. This method handles the logic for detecting when the button is pressed and invokes the associated action if necessary.
    /// </summary>
    /// <param name="gameTime">The current game time.</param>
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

    /// <summary>
    /// Draws the craft button on the screen, including its hit box and label. This method is called during the game's draw phase and handles rendering the visual representation of the button, allowing players to see and interact with it in the crafting screen.
    /// </summary>
    /// <param name="gameTime">The current game time.</param>
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