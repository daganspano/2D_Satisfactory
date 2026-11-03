using System;
using System.Collections.Generic;
using _2D_Satisfactory.Components.CharacterClasses;
using _2D_Satisfactory.Screens.MainGame;
using Microsoft.Xna.Framework;

namespace _2D_Satisfactory.Screens.Title;

/// <summary>
/// Represents a character that moves around the title screen, bouncing off edges and changing direction at intervals.
/// </summary>
public class TitleCharacter : Character
{
    #region Private Fields

    private double _directionTimer;
    private readonly double _directionChangeInterval;
    private Vector2 _direction;

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets the hitbox of the character, which is used for collision detection.
    /// </summary>
    public Rectangle HitBox => new Rectangle(Position.ToPoint(), GameData.SpriteSize);

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="TitleCharacter"/> class with a random position and speed, and sets up its movement behavior.
    /// </summary>
    /// <param name="game">The game instance to which the character belongs.</param>
    public TitleCharacter(Game game) : base(game, GetCharacter())
    {
        _directionTimer = 2;
        _directionChangeInterval = 3.0;

        // Parent Fields
        Random random = new Random();
        Position = new Vector2(
            random.Next(0, (int)GameData.ScreenSize.X - GameData.PlayerHitBox.Width),
            random.Next(0, (int)GameData.ScreenSize.Y - GameData.PlayerHitBox.Height)
        );
        IsMoving = true;
        WasMoving = true;
        Speed = random.Next(50, 201);
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Bounces the character in the opposite direction of the provided overlap vector, normalizing the direction if it's not zero.
    /// </summary>
    /// <param name="overlap">The vector representing the overlap with another character, used to determine the bounce direction.</param>
    public void Bounce(Vector2 overlap)
    {
        _direction = overlap;

        if (_direction != Vector2.Zero)
            _direction.Normalize();
    }

    #endregion

    #region Override Methods

    /// <summary>
    /// Updates the character's movement and direction based on elapsed game time, including changing direction at intervals and bouncing off screen edges.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    public override void Update(GameTime gameTime)
    {
        UpdateMovement(gameTime);

        base.Update(gameTime);
    }

    #endregion
    
    #region Private Methods
    
    /// <summary>
    /// Generates a random character index between 0 and 15, which is used to select a character sprite for the title screen.
    /// </summary>
    /// <returns>A random integer between 0 and 15, representing the character index.</returns>
    private static int GetCharacter() => new Random().Next(0, 16);

    /// <summary>
    /// Updates the character's movement based on elapsed game time, including changing direction at set intervals and bouncing off the edges of the screen.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    private void UpdateMovement(GameTime gameTime)
    {
        // Update the frame timers
        _directionTimer += gameTime.ElapsedGameTime.TotalSeconds;

            
        // Change direction
        if (_directionTimer > _directionChangeInterval)
        {
            double angle = new Random().NextDouble() * Math.PI * 2.0;
            _direction.X = (float)Math.Cos(angle);
            _direction.Y = (float)Math.Sin(angle);
            _directionTimer -= _directionChangeInterval;
        }

        // Bounce off edges of the screen
        List<string> collisions = CollisionHandler.CheckEdgeCollisions(HitBox, _direction);
        if (collisions.Contains("x")) _direction.X = -_direction.X;
        if (collisions.Contains("y")) _direction.Y = -_direction.Y;

        Position += _direction * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (Math.Abs(_direction.X) < Math.Abs(_direction.Y)) DirectionFrame = _direction.Y > 0 ? DirectionFrame.Down : DirectionFrame.Up;
        else DirectionFrame = _direction.X > 0 ? DirectionFrame.Right : DirectionFrame.Left;
    }

    #endregion
}