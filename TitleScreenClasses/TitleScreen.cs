using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using _2D_Satisfactory.Components.Character;

namespace _2D_Satisfactory.TitleScreenClasses;

/// <summary>
/// Represents the title screen of the game.
/// </summary>
public class TitleScreen
{
    // Sprite Fields
    private Texture2D _backgroundTexture;
    private readonly List<TitleCharacter> _titleCharacters;
    private Texture2D _titleBanner;
    private ButtonGroup _buttonGroup;

    // Banner Position & Scale Fields
    private Vector2 _bannerPosition;
    private readonly float _bannerScale;
    

    public TitleScreen(Action onStartClick, Action onOptionsClick, Action onExitClick)
    {
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
        _bannerPosition = new Vector2((GameDimensions.X - bannerWidthScaled) / 2, (GameDimensions.Y - titleHeight) / 2);
        float buttonGroupYPosition = _bannerPosition.Y + bannerHeightScaled + buttonPadding;
        
        // Initialize running sprites
        _titleCharacters = new List<TitleCharacter>();
        for (int i = 0; i < 4; i++) 
        {
            Vector2 initialPosition = new Vector2(
                new Random().Next((int)(i * GameDimensions.X / 4), (int)((i + 1) * GameDimensions.X / 4)),
                new Random().Next(200, (int)GameDimensions.Y)
            ); // Random initial position for each sprite
            int character = i; // Assign a unique character index for each sprite
            int speed = 50 + i * 30; // Assign a unique speed for each sprite
            _titleCharacters.Add(new TitleCharacter(initialPosition, character, speed));
        }

        // Initialize buttons
        _buttonGroup = new ButtonGroup(
            new Dictionary<string, Action>
            {
                { "Start", onStartClick },
                { "Options", onOptionsClick },
                { "Exit", onExitClick }
            }, 
            buttonGroupYPosition
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
        foreach (var c in _titleCharacters) c.LoadContent(content);

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
        foreach (TitleCharacter c in _titleCharacters) c.Update(gameTime);

        List<(Rectangle, Vector2)> hitBoxesAndDirections = _titleCharacters.Select(c => (c.HitBox, c.Direction)).ToList();

        Dictionary<int, List<(string, int)>> collisionSides = Collision.CheckSpriteCollisions(hitBoxesAndDirections);

        for (int i = 0; i < _titleCharacters.Count; i++)
        {
            if (collisionSides.TryGetValue(i, out List<(string, int)> sides))
            {
                TitleCharacter titleCharacter = _titleCharacters[i];
                titleCharacter.ResolveSpriteCollision(sides);
            }
        }

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
        foreach (var c in _titleCharacters) c.Draw(spriteBatch);

        // Draw banner
        spriteBatch.Draw(_titleBanner, _bannerPosition, null, Color.White, 0f, Vector2.Zero, _bannerScale, SpriteEffects.None, 0f);

        // Draw buttons
        _buttonGroup.Draw(spriteBatch);
    }
}