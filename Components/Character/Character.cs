
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace _2D_Satisfactory.Components.Character;

/// <summary>
/// Represents a character in the game, handling its position, animation, and rendering.
/// </summary>
public class Character
{
    // Game Dimensions
    private readonly Vector2 _gameDimensions;

    // Timer Fields
    private double _animationTimer;

    // Sprite Fields
    private Texture2D _texture;
    private readonly Point _character;
    private int _speed;

    // Frame Fields
    private readonly Point _spriteSize;
    private Point _frame;
    private int _previousAnimationFrame;
    private Rectangle _sourceRectangle;
    private Vector2 _position;

    /// <summary>
    /// The Speed of the Character
    /// </summary>
    public int Speed
    {
        set => _speed = value;
    }
    
    public Character(Vector2 gameDimensions, Vector2 initialPosition, int character, int speed)
    {
        _gameDimensions = gameDimensions;
        _position = initialPosition;
        _spriteSize = new Point(16, 23);
        _sourceRectangle = new Rectangle(Point.Zero, _spriteSize);
        _speed = speed;
        _character = new Point(character % 4, character / 4);
        _frame = new Point((int)AnimationFrame.Stationary, (int)DirectionFrame.Down);
        _animationTimer = 0;
        _previousAnimationFrame = 0;
    }

    /// <summary>
    /// Loads the content for the running sprite.
    /// </summary>
    /// <param name="content">The content manager used to load the sprite texture.</param>
    public void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("construction_workers");
    }

    /// <summary>
    /// Updates the running sprite's position, direction, and animation based on the elapsed game time.
    /// </summary>
    /// <param name="gameTime">The game time object containing the elapsed time since the last update.</param>
    public void Update(GameTime gameTime, Vector2 direction)
    {
        // Update frame for the animation and direction based on the current direction vector
        UpdateFrame(gameTime, direction);

        // Update the source rectangle based on the current frame and character
        UpdateSourceRectangle();

        // Update the position and direction frame
        _position += direction * _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    /// <summary>
    /// Draws the running sprite on the screen using the provided sprite batch.
    /// </summary>
    /// <param name="spriteBatch">The sprite batch used to draw the running sprite.</param>
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, _position, _sourceRectangle, Color.White, 0f, Vector2.Zero, 1.8f, SpriteEffects.None, 0f);
    }

    /// <summary>
    /// Detects if the character is colliding with the edges of the game area based on its current position and direction.
    /// </summary>
    /// <param name="direction">The direction vector of the character's movement.</param>
    /// <returns>The edge of the game area the character is colliding with, or null if no collision.</returns>
    public List<string> CheckForCollision(Vector2 direction)
    {
        List<string> collisions = new List<string>();
        if ((_position.X <= 0 && direction.X < 0) || (_position.X >= _gameDimensions.X - _spriteSize.X && direction.X > 0)) collisions.Add("x");
        if ((_position.Y <= 200 && direction.Y < 0) || (_position.Y >= _gameDimensions.Y - _spriteSize.Y && direction.Y > 0)) collisions.Add("y");
        return collisions;
    }
 
    /// <summary>
    /// Updates the animation frame of the running sprite based on the current frame and previous animation frame.
    /// </summary>
    private void UpdateFrame(GameTime gameTime, Vector2 direction)
    {
        // Update the animation timer
        _animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
        double animationSpeed = 0.15 * 100 / 140 * Math.Sqrt(140.0 / _speed);

        if (direction == Vector2.Zero)
        {
            _frame.X = (int)AnimationFrame.Stationary;
            return;
        }

        // Update the animation
        if (_animationTimer > animationSpeed)
        {
            switch (_frame.X)
            {
                case (int)AnimationFrame.Running1:
                case (int)AnimationFrame.Running2:
                    _frame.X = (int)AnimationFrame.Stationary;
                    break;
                case (int)AnimationFrame.Stationary:
                    _frame.X = _previousAnimationFrame == (int)AnimationFrame.Running1 ? (int)AnimationFrame.Running2 : (int)AnimationFrame.Running1;
                    _previousAnimationFrame = _frame.X;
                    break;
            }
            _animationTimer -= animationSpeed;
        }
        
        // Update the direction
        if (Math.Abs(direction.X) > Math.Abs(direction.Y)) _frame.Y = direction.X > 0 ? (int)DirectionFrame.Right : (int)DirectionFrame.Left;
        else _frame.Y = direction.Y > 0 ? (int)DirectionFrame.Down : (int)DirectionFrame.Up;
    }

    /// <summary>
    /// Updates the source rectangle of the running sprite based on the current frame, character, and texture properties.
    /// </summary>
    private void UpdateSourceRectangle()
    {
        Point startPoint = new Point(1, 2);
        Point characterOffset = new Point(126, 104);
        Point frameOffset = new Point(18, 26);

        // Start point + character position + frame position
        _sourceRectangle.Location = startPoint + _character * characterOffset + _frame * frameOffset;
    }
}