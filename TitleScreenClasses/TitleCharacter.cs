using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using _2D_Satisfactory.Components.Character;
using System.Collections.Generic;

namespace _2D_Satisfactory.TitleScreenClasses;

/// <summary>
/// Represents a running sprite that can move in different directions and animate between frames.
/// </summary>
public class TitleCharacter
{
    // Timer Fields
    private double _directionTimer;
    private readonly double _directionChangeInterval;

    // Position Fields
    private Vector2 _direction;


    private Character _character;
    
    public TitleCharacter(Vector2 gameDimensions, Vector2 initialPosition, int character, int speed)
    {
        // Timer Fields
        _directionTimer = 2;
        _directionChangeInterval = 2.0;

        // Position Fields
        _direction = Vector2.Zero;

        _character = new Character(gameDimensions, initialPosition, character, speed);
    }

    /// <summary>
    /// Loads the content for the running sprite.
    /// </summary>
    /// <param name="content">The content manager used to load the sprite texture.</param>
    public void LoadContent(ContentManager content)
    {
        _character.LoadContent(content);
    }

    /// <summary>
    /// Updates the running sprite's position, direction, and animation based on the elapsed game time.
    /// </summary>
    /// <param name="gameTime">The game time object containing the elapsed time since the last update.</param>
    public void Update(GameTime gameTime)
    {
        // Update the frame timers
        _directionTimer += gameTime.ElapsedGameTime.TotalSeconds;

        // Change direction
        if (_directionTimer > _directionChangeInterval) ChangeDirection();

        // Bounce off edges of the screen
        List<string> collisions = _character.CheckForCollision(_direction);
        if (collisions.Count > 0) BounceOffEdges(collisions);

        _character.Update(gameTime, _direction);
    }

    /// <summary>
    /// Draws the running sprite on the screen using the provided sprite batch.
    /// </summary>
    /// <param name="spriteBatch">The sprite batch used to draw the running sprite.</param>
    public void Draw(SpriteBatch spriteBatch)
    {
        _character.Draw(spriteBatch);
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
    private void BounceOffEdges(List<string> collision)
    {
        if (collision.Contains("x")) _direction.X = -_direction.X;
        if (collision.Contains("y")) _direction.Y = -_direction.Y;
    }
}