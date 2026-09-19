
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
    // Timer Fields
    private double _animationTimer;

    // Sprite Fields
    private Texture2D _texture;
    private readonly Point _character;
    private int _speed;

    // Frame Fields
    private readonly Point _spriteSize;
    private readonly float _spriteScale;
    private Point _frame;
    private int _previousAnimationFrame;
    private Vector2 _previousDirection;
    private Rectangle _sourceRectangle;
    private Vector2 _position;

    /// <summary>
    /// The hit box of the character based on its current position and sprite size.
    /// </summary>
    public Rectangle HitBox => new Rectangle(_position.ToPoint(), (_spriteSize.ToVector2() * _spriteScale).ToPoint());

    /// <summary>
    /// The Speed of the Character
    /// </summary>
    public int Speed
    {
        set => _speed = value;
    }

    public Vector2 Position
    {
        get => _position;
        set => _position = value;
    }

    public Character(Vector2 initialPosition, int character, int speed)
    {
        _position = initialPosition;
        _spriteSize = new Point(16, 23);
        _spriteScale = 1.8f;
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
        spriteBatch.Draw(_texture, _position, _sourceRectangle, Color.White, 0f, Vector2.Zero, _spriteScale, SpriteEffects.None, 0f);
    }

    public List<string> CheckEdgeCollisions(Vector2 direction)
    {
        // return Collision.CheckEdgeCollisions(GetHitBox(), direction);
        return Collision.CheckEdgeCollisions(HitBox, direction);
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
            _animationTimer = 0;
            _previousDirection = Vector2.Zero;
            return;
        }

        // Start the running animation immediately when the character starts moving
        if (_previousDirection == Vector2.Zero) _animationTimer = animationSpeed;

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

        _previousDirection = direction;
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