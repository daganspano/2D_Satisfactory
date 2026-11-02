using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace _2D_Satisfactory.Screens.MainGame;

public class ControlsDisplay : DrawableGameComponent
{
    private SpriteBatch _spriteBatch;
    private SpriteFont _font;
    private List<string> _controlTexts;

    public ControlsDisplay(Game game) : base(game)
    {
    }
    
    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
        _font = Game.Content.Load<SpriteFont>("Orbitron-Regular");
    }

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