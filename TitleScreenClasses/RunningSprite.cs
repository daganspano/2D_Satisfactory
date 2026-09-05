using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace _2D_Satisfactory.TitleScreenClasses;

/// <summary>
/// Represents a running sprite that can move in different directions and animate between frames.
/// </summary>
public class RunningSprite
{
    // Game Fields
    private readonly int _gameWidth;
    private readonly int _gameHeight;

    // Timer Fields
    private double _animationTimer;
    private double _directionTimer;
    private readonly double _animationSpeed;
    private readonly double _directionChangeInterval;

    // Sprite Fields
    private Texture2D _runningSpriteTexture;
    private readonly Point _character;
    private readonly int _spriteSpeed;

    // Texture Frame Fields
    private readonly Point _textureStartPoint;
    private readonly Point _characterSeparation;
    private readonly Point _spriteSeparation;
    private readonly Point _spriteSize;
    private Point _frame;
    private int _previousAnimationFrame;
    private Rectangle _sourceRectangle;

    // Position Fields
    private Vector2 _position;
    private Vector2 _direction;
    
    public RunningSprite(int gameWidth, int gameHeight, int character, int spriteSpeed, Vector2 initialPosition)
    {
        // Game Fields
        _gameWidth = gameWidth;
        _gameHeight = gameHeight;

        // Timer Fields
        _animationTimer = 0;
        _directionTimer = 2;
        _animationSpeed = 0.15 * 100 / 140 * Math.Sqrt(140.0 / spriteSpeed);
        _directionChangeInterval = 2.0;

        // Sprite Fields
        _character = new Point(character % 4, character / 4);
        _spriteSpeed = spriteSpeed;

        // Texture Frame Fields
        _textureStartPoint = new Point(1, 2);
        _characterSeparation = new Point(126, 104);
        _spriteSeparation = new Point(18, 26);
        _spriteSize = new Point(16, 23);
        _frame = new Point(1, 0);
        _previousAnimationFrame = 0;
        _sourceRectangle = new Rectangle();

        // Position Fields
        _position = initialPosition;
        _direction = Vector2.Zero;
    }

    /// <summary>
    /// Loads the content for the running sprite.
    /// </summary>
    /// <param name="content">The content manager used to load the sprite texture.</param>
    public void LoadContent(ContentManager content)
    {
        _runningSpriteTexture = content.Load<Texture2D>("construction_workers");
    }

    /// <summary>
    /// Updates the running sprite's position, direction, and animation based on the elapsed game time.
    /// </summary>
    /// <param name="gameTime">The game time object containing the elapsed time since the last update.</param>
    public void Update(GameTime gameTime)
    {
        // Update the frame timers
        _animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
        _directionTimer += gameTime.ElapsedGameTime.TotalSeconds;

        // Change direction
        if (_directionTimer > _directionChangeInterval) ChangeDirection();

        // Bounce off edges of the screen
        BounceOffEdges();

        // Update the x (animation) frame every 0.5 seconds
        if (_animationTimer > _animationSpeed) UpdateAnimation();

        // Update the y (direction) frame
        UpdateDirection();

        // Update the source rectangle based on the current frame and character
        UpdateSourceRectangle();

        // Update the position and direction frame
        _position += _direction * _spriteSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    /// <summary>
    /// Draws the running sprite on the screen using the provided sprite batch.
    /// </summary>
    /// <param name="spriteBatch">The sprite batch used to draw the running sprite.</param>
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_runningSpriteTexture, _position, _sourceRectangle, Color.White, 0f, Vector2.Zero, 1.8f, SpriteEffects.None, 0f);
    }

    /// <summary>
    /// Changes the direction of the running sprite to a random angle and resets the direction timer.
    /// </summary>
    /// <remarks>
    /// This method randomly selects a new direction for the sprite to move in.
    /// </remarks>
    private void ChangeDirection()
    {
        double angle = new Random().NextDouble() * Math.PI * 2.0;
        _direction.X = (float)Math.Cos(angle);
        _direction.Y = (float)Math.Sin(angle);
        _directionTimer -= _directionChangeInterval;
    }

    /// <summary>
    /// Bounces the running sprite off the edges of the screen by reversing its direction when it reaches the boundaries.
    /// </summary>
    private void BounceOffEdges()
    {
        if (_position.X <= 0 && _direction.X < 0) _direction.X = -_direction.X;
        if (_position.X >= _gameWidth - _spriteSize.X && _direction.X > 0) _direction.X = -_direction.X;
        if (_position.Y <= 200 && _direction.Y < 0) _direction.Y = -_direction.Y;
        if (_position.Y >= _gameHeight - _spriteSize.Y && _direction.Y > 0) _direction.Y = -_direction.Y;
    }

    /// <summary>
    /// Updates the animation frame of the running sprite based on the current frame and previous animation frame.
    /// </summary>
    private void UpdateAnimation()
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
        _animationTimer -= _animationSpeed;
    }
    
    /// <summary>
    /// Updates the direction frame of the running sprite based on its current movement direction.
    /// </summary>
    private void UpdateDirection()
    {
        if (Math.Abs(_direction.X) > Math.Abs(_direction.Y)) _frame.Y = _direction.X > 0 ? (int)DirectionFrame.Right : (int)DirectionFrame.Left;
        else _frame.Y = _direction.Y > 0 ? (int)DirectionFrame.Down : (int)DirectionFrame.Up;
    }

    /// <summary>
    /// Updates the source rectangle of the running sprite based on the current frame, character, and texture properties.
    /// </summary>
    private void UpdateSourceRectangle()
    {
        _sourceRectangle.X = _textureStartPoint.X + _character.X * _characterSeparation.X + (_frame.X * _spriteSeparation.X);
        _sourceRectangle.Y = _textureStartPoint.Y + _character.Y * _characterSeparation.Y + (_frame.Y * _spriteSeparation.Y);
        _sourceRectangle.Size = _spriteSize;
    }
}