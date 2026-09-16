using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace _2D_Satisfactory.TitleScreenClasses;

/// <summary>
/// Represents the title screen of the game.
/// </summary>
public class TitleScreen
{
    // Game Fields
    private readonly int _gameWidth;
    private readonly int _gameHeight;

    // Sprite Fields
    private Texture2D _backgroundTexture;
    private readonly List<RunningSprite> _runningSprites;
    private Texture2D _titleBanner;
    private ButtonGroup _buttonGroup;

    // Banner Position & Scale Fields
    private Vector2 _bannerPosition;
    private readonly float _bannerScale;
    

    public TitleScreen(int gameWidth, int gameHeight, Action onExitClick)
    {
        // Game Fields
        _gameWidth = gameWidth;
        _gameHeight = gameHeight;

        // Banner Size & Scale Fields
        _bannerScale = 0.4f;
        float bannerWidthScaled = 1147 * _bannerScale;
        float bannerHeightScaled = 193 * _bannerScale;

        // Button Size & Scale Fields
        const float buttonScale = 0.7f;
        float buttonHeightScaled = 88 * buttonScale;
        const int buttonPadding = 8;
        
        // Total Size Calculation for Centering
        float titleHeight = bannerHeightScaled + 3 * (buttonPadding + buttonHeightScaled);

        // Position Fields
        _bannerPosition = new Vector2((_gameWidth - bannerWidthScaled) / 2, (_gameHeight - titleHeight) / 2);
        float buttonStartingYPosition = _bannerPosition.Y + bannerHeightScaled + buttonPadding;
        
        // Initialize running sprites
        _runningSprites = new List<RunningSprite>();
        for (int i = 0; i < 4; i++) 
            _runningSprites.Add(new RunningSprite(_gameWidth, _gameHeight, i, 50 + i * 30, new Vector2(new Random().Next(i * _gameWidth / 4, (i + 1) * _gameWidth / 4), new Random().Next(200, _gameHeight))));

        // Initialize buttons
        _buttonGroup = new ButtonGroup(
            new Dictionary<string, Action>
            {
                { "Start", () => {} },
                { "Options", () => {} },
                { "Exit", onExitClick }
            }, 
            gameWidth, buttonStartingYPosition
        );
    }

    /// <summary>
    /// Loads the content for the title screen.
    /// </summary>
    /// <param name="content">The content manager used to load assets.</param>
    public void LoadContent(ContentManager content)
    {
        // Load background
        _backgroundTexture = content.Load<Texture2D>("forest_background");

        // Load running character
        foreach (var runningSprite in _runningSprites) runningSprite.LoadContent(content);

        // Load banner
        _titleBanner = content.Load<Texture2D>("title_banner");

        // Load buttons
        _buttonGroup.LoadContent(content);

    }

    /// <summary>
    /// Updates the buttons and running characters on the title screen.
    /// </summary>
    /// <param name="gameTime">The game time object containing timing information.</param>
    public void Update(GameTime gameTime)
    {
        // Update running character
        foreach (var runningSprite in _runningSprites) runningSprite.Update(gameTime);

        // Update buttons
        _buttonGroup.Update();
    }

    /// <summary>
    /// Draws the title screen.
    /// </summary>
    /// <param name="spriteBatch">The sprite batch used to draw textures.</param>
    public void Draw(SpriteBatch spriteBatch)
    {
        // Draw background
        spriteBatch.Draw(_backgroundTexture, new Vector2(0, 0), null, Color.White, 0f, Vector2.Zero, 1.8f, SpriteEffects.None, 0f);

        // Draw running character
        foreach (var runningSprite in _runningSprites) runningSprite.Draw(spriteBatch);

        // Draw banner
        spriteBatch.Draw(_titleBanner, _bannerPosition, null, Color.White, 0f, Vector2.Zero, _bannerScale, SpriteEffects.None, 0f);

        // Draw buttons
        _buttonGroup.Draw(spriteBatch);
    }
}