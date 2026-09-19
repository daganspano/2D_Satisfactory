using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using _2D_Satisfactory.Components.CharacterClasses;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace _2D_Satisfactory.MainGameClasses;

/// <summary>
/// Represents a character in the main game, handling its position, animation, rendering, and input for movement.
/// </summary>
public class GameCharacter
{
    private readonly Character _character;

    public GameCharacter(Vector2 initialPosition, int character, int speed)
    {
        _character = new Character(initialPosition, character, speed);
    }

    /// <summary>
    /// Loads the content for the game character.
    /// </summary>
    /// <param name="content">The content manager used to load the character's assets.</param>
    public void LoadContent(ContentManager content)
    {
        _character.LoadContent(content);
    }

    /// <summary>
    /// Updates the game character's state, including its position and animation, based on user input and elapsed game time.
    /// </summary>
    /// <param name="gameTime">The game time object containing timing information for the current frame.</param>
    public void Update(GameTime gameTime)
    {
        _character.Update(gameTime, GetDirection());
    }

    /// <summary>
    /// Draws the game character on the screen using the provided sprite batch.
    /// </summary>
    /// <param name="spriteBatch">The sprite batch used to draw the character.</param>
    public void Draw(SpriteBatch spriteBatch)
    {
        _character.Draw(spriteBatch);
    }

    /// <summary>
    /// Calculates the direction vector for the character's movement based on user input and edge collisions.
    /// </summary>
    /// <returns>The direction vector for the character's movement.</returns>
    private Vector2 GetDirection()
    {
        // Get Input and adjust for edges
        Vector2 characterDirection = AdjustDirectionForEdges(GetInput());
        
        // Normalize
        if (characterDirection != Vector2.Zero) characterDirection.Normalize();

        return characterDirection;
    }

    /// <summary>
    /// Gets the input from the keyboard and gamepad to determine the character's movement direction.
    /// </summary>
    /// <returns>The direction vector based on user input.</returns>
    private Vector2 GetInput()
    {
        KeyboardState keyboardState = Keyboard.GetState();
        Vector2 keyboardMovement = new Vector2(
            (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right)) ? 1 
                : (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left)) ? -1 : 0,
            (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down)) ? 1 
                : (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up)) ? -1 : 0
        );

        Vector2 leftThumbStick = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left * new Vector2(1, -1);

        return Vector2.Clamp(leftThumbStick + keyboardMovement, new Vector2(-1, -1), new Vector2(1, 1));
    }

    /// <summary>
    /// Adjusts the character's movement direction to prevent it from moving off the edges of the game screen.
    /// </summary>
    /// <param name="direction">The desired movement direction of the character.</param>
    /// <returns>The adjusted movement direction, preventing the character from moving off the edges.</returns>
    private Vector2 AdjustDirectionForEdges(Vector2 direction)
    {
        List<string> collisions = _character.CheckEdgeCollisions(direction);
        if (collisions.Contains("x")) direction.X = 0;
        if (collisions.Contains("y")) direction.Y = 0;
        return direction;
    }
}
