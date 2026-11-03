using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2D_Satisfactory.Screens.MainGame;

/// <summary>
/// Represents a display component that shows the current controls available to the player in the game.
/// </summary>
public class ControlsDisplay : DrawableGameComponent
{
    private SpriteBatch _spriteBatch;
    private SpriteFont _font;
    private List<string> _controlTexts;

    public ControlsDisplay(Game game) : base(game)
    {
    }
    
    /// <summary>
    /// Initializes the content required for the controls display, including the sprite batch and font used for rendering text. This method is called when the component is added to the game and prepares it for drawing control instructions on the screen.
    /// </summary>
    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
        _font = Game.Content.Load<SpriteFont>("Orbitron-Regular");
    }

    /// <summary>
    /// Updates the list of control instructions based on the player's current interaction state. This method is called every frame and adjusts the displayed controls to reflect the actions available to the player, such as pausing the game or moving the character. The control texts are stored in a list and updated according to the player's interaction context.
    /// </summary>
    /// <param name="gameTime">The current game time.</param>
    public override void Update(GameTime gameTime)
    {
        _controlTexts = new List<string> { "Pause: Esc", "Move: WASD" };

        switch (GameData.PlayerInteraction)
        {
            case "handMineIronOre":
                _controlTexts.Add("Mine: E");
                break;
        }
    }

    /// <summary>
    /// Draws the control instructions on the screen using the sprite batch and font. This method is called every frame and renders the current list of control texts at a specified position on the screen. The control instructions are displayed in a horizontal layout, with each instruction separated by a defined spacing. The text color is set to white for visibility against the game background.
    /// </summary>
    /// <param name="gameTime">The current game time.</param>
    public override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin();

        Vector2 textPosition = new Vector2(10, GameData.ScreenSize.Y - 30);

        foreach (string controlText in _controlTexts)
        {
            Vector2 textSize = _font.MeasureString(controlText);
            _spriteBatch.DrawString(_font, controlText, textPosition, Color.White);
            textPosition = new Vector2(textPosition.X + textSize.X + 20, textPosition.Y);
        }
        
        _spriteBatch.End();
    }
}