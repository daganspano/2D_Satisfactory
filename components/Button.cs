using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _2D_Satisfactory
{
    /// <summary>
    /// A UI button with a normal, hover, and pressed state.
    /// </summary>
    public class Button
    {
        private FactoryGame _game;
        private string _state;
        private Texture2D _normalTexture;
        private Texture2D _hoverTexture;
        private Texture2D _pressedTexture;
        private string _text;
        private SpriteFont  _font;

        public Button(FactoryGame game, string text)
        {
            _game = game;
            _text = text;
            _state = "normal";
        }

        public void LoadContent() 
        {
            _normalTexture = _game.Content.Load<Texture2D>("button_normal");
            _hoverTexture = _game.Content.Load<Texture2D>("button_hover");
            _pressedTexture = _game.Content.Load<Texture2D>("button_pressed");
            _font = _game.Content.Load<SpriteFont>("Orbitron-Bold");
        }

        public void Update()
        {
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D currentBtnTexture;

            if (_state == "hover")
                currentBtnTexture = _hoverTexture;
            else if (_state == "pressed")
                currentBtnTexture = _pressedTexture;
            else
                currentBtnTexture = _normalTexture;

            spriteBatch.Draw(currentBtnTexture, Vector2.Zero, Color.White);

            if (!string.IsNullOrEmpty(_text))
            {
                Vector2 textSize = _font.MeasureString(_text);
                Vector2 textPosition = new Vector2((currentBtnTexture.Width - textSize.X) / 2, (currentBtnTexture.Height - textSize.Y) / 2);
                spriteBatch.DrawString(_font, _text, textPosition, Color.DarkOrange);
            }
        }
    }
}