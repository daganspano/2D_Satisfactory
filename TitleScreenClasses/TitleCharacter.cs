using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using _2D_Satisfactory.Components.CharacterClasses;
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

    /// <summary>
    /// Gets the hit box of the running sprite based on its current position and sprite size.
    /// </summary>
    public Rectangle HitBox => _character.HitBox;

    /// <summary>
    /// Gets or sets the direction of the running sprite, represented as a 2D vector.
    /// </summary>
    public Vector2 Direction => _direction;
    
    public TitleCharacter(Vector2 initialPosition, int character, int speed)
    {
        // Timer Fields
        _directionTimer = 2;
        _directionChangeInterval = 2.0;

        // Position Fields
        _direction = Vector2.Zero;

        _character = new Character(initialPosition, character, speed);
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
    /// <param name="gameTime">The game time object containing timing information for the current frame.</param>
    public void Update(GameTime gameTime)
    {
        // Update the frame timers
        _directionTimer += gameTime.ElapsedGameTime.TotalSeconds;

        // Change direction
        if (_directionTimer > _directionChangeInterval) ChangeDirection();

        // Bounce off edges of the screen
        List<string> collisions = _character.CheckEdgeCollisions(_direction);
        if (collisions.Count > 0) BounceOffEdges(collisions);

        _character.Update(gameTime, _direction);
    }

    /// <summary>
    /// Resolves collisions between the running sprite and other sprites by adjusting its position and direction based on the collision sides and overlaps.
    /// </summary>
    /// <param name="collisionSides">A list of tuples containing the sides of the collision and the overlap amount.</param>
    public void ResolveSpriteCollision(List<(string, int)> collisionSides)
    {
        if (collisionSides.Count <= 0) return;

        foreach ((string side, int overlap) in collisionSides)
        {
            switch (side)
            {
                case "right":
                    if (_direction.X > 0)
                        _direction.X = -Math.Abs(_direction.X);
                    _character.Position += new Vector2(-overlap, 0);
                    break;

                case "left":
                    if (_direction.X < 0)
                        _direction.X = Math.Abs(_direction.X);
                    _character.Position += new Vector2(overlap, 0);
                    break;

                case "bottom":
                    if (_direction.Y > 0)
                        _direction.Y = -Math.Abs(_direction.Y);
                    _character.Position += new Vector2(0, -overlap);
                    break;

                case "top":
                    if (_direction.Y < 0)
                        _direction.Y = Math.Abs(_direction.Y);
                    _character.Position += new Vector2(0, overlap);
                    break;
            }
        }
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
    /// Changes the direction of the running sprite to a random direction by generating a random angle and updating the direction vector accordingly. The direction timer is reset after changing the direction.
    /// </summary>
    private void ChangeDirection()
    {
        double angle = new Random().NextDouble() * Math.PI * 2.0;
        _direction.X = (float)Math.Cos(angle);
        _direction.Y = (float)Math.Sin(angle);
        _directionTimer -= _directionChangeInterval;
    }

    /// <summary>
    /// Bounces the running sprite off the edges of the screen by reversing its direction along the x or y axis based on the detected collisions. The method checks for collisions with the edges and adjusts the direction vector accordingly.
    /// </summary>
    /// <param name="collisions">A list of strings representing the edges where collisions occurred ("x" for horizontal edges, "y" for vertical edges).</param>
    private void BounceOffEdges(List<string> collisions)
    {
        if (collisions.Contains("x")) _direction.X = -_direction.X;
        if (collisions.Contains("y")) _direction.Y = -_direction.Y;
    }
}
