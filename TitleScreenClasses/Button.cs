using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace _2D_Satisfactory.TitleScreenClasses;

/// <summary>
/// A UI button with a normal, hover, and pressed state.
/// </summary>
public class Button
{

    // Texture and font fields
    private Texture2D _buttonTexture;
    private SpriteFont _font;

    // Button fields
    private readonly float _buttonScale;
    private readonly int _buttonWidthRaw;
    private readonly float _buttonWidthScaled;
    private readonly int _buttonHeightRaw;
    private readonly float _buttonHeightScaled;
    private readonly Vector2 _buttonPosition;
    private Rectangle _sourceRectangle;

    // Text fields
    private readonly string _text;
    private Color _textColor;
    private Vector2 _textPosition;

    // Function to call when the button is clicked
    private readonly System.Action _onClick;
    private bool _isClicked;



    public Vector2 ButtonPosition => _buttonPosition;
    public float ButtonWidthScaled => _buttonWidthScaled;
    public float ButtonHeightScaled => _buttonHeightScaled;
    
    public Button(string text, float buttonScale, int gameWidth, float yPosition, System.Action onClick)
    {
        _text = text;
        _buttonScale = buttonScale;
        _onClick = onClick;
        _isClicked = false;
        
        // Load button parameters
        _buttonWidthRaw = 320;
        _buttonWidthScaled = _buttonWidthRaw * _buttonScale;
        _buttonHeightRaw = 88;
        _buttonHeightScaled = _buttonHeightRaw * _buttonScale;
        _buttonPosition = new Vector2((gameWidth - _buttonWidthScaled) / 2, yPosition);
    }

    /// <summary>
    /// Loads the button texture and font, and calculates the text position based on the button size and scale.
    /// </summary>
    /// <param name="content">The content manager used to load the button texture and font.</param>
    public void LoadContent(ContentManager content) 
    {
        _buttonTexture = content.Load<Texture2D>("button_atlas");
        _font = content.Load<SpriteFont>("Orbitron-Bold");
        
        Vector2 textSize = _font.MeasureString(_text);
        _textPosition = _buttonPosition + new Vector2((_buttonWidthScaled - textSize.X) / 2, (_buttonHeightScaled - textSize.Y) / 2);
    }

    /// <summary>
    /// Updates the button state based on mouse input and triggers the click action if the button is pressed.
    /// </summary>
    public void Update(bool isSelected)
    {
        int startX = 0;
        Color newTextColor = new Color(0x96, 0x52, 0x14);

        if (_isClicked)
        {
            _onClick();
            _isClicked = false;
            startX = 2;
            newTextColor = new Color(0x6E, 0x3C, 0x12);
        }
        else if (isSelected)
        {
            if (
                Mouse.GetState().LeftButton == ButtonState.Pressed || 
                Keyboard.GetState().IsKeyDown(Keys.Enter) || 
                GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A)
            )
            {
                startX = 2;
                newTextColor = new Color(0x6E, 0x3C, 0x12);
                _isClicked = true;
            }
            else 
            {
                startX = 1;
                newTextColor = new Color(0xFF, 0x91, 0x2D);
            }
        }

        _sourceRectangle = new Rectangle(startX * _buttonWidthRaw, 0, _buttonWidthRaw, _buttonHeightRaw);
        _textColor = newTextColor;
    }

    /// <summary>
    /// Draws the button and its text using the provided sprite batch.
    /// </summary>
    /// <param name="spriteBatch">The sprite batch used to draw the button and its text.</param>
    public void Draw(SpriteBatch spriteBatch)
    {
        // Draw the button texture
        spriteBatch.Draw(_buttonTexture, _buttonPosition, _sourceRectangle, Color.White, 0f, Vector2.Zero, _buttonScale, SpriteEffects.None, 0f);

        // Draw the button text
        spriteBatch.DrawString(_font, _text, _textPosition, _textColor);
    }
}
