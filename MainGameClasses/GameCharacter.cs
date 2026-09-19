using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using _2D_Satisfactory.Components.Character;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace _2D_Satisfactory.MainGameClasses;

public class GameCharacter
{
    private readonly Character _character;

    public GameCharacter(Vector2 initialPosition, int character, int speed)
    {
        _character = new Character(initialPosition, character, 200);
    }

    public void LoadContent(ContentManager content)
    {
        _character.LoadContent(content);
    }

    public void Update(GameTime gameTime)
    {
        _character.Update(gameTime, GetDirection());
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _character.Draw(spriteBatch);
    }

    private Vector2 GetDirection()
    {
        // Get Input and adjust for edges
        Vector2 characterDirection = AdjustDirectionForEdges(GetInput());
        
        // Normalize
        if (characterDirection != Vector2.Zero) characterDirection.Normalize();

        return characterDirection;
    }

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
    /// Stops the character's movement in the direction of the edges if a collision is detected.
    /// </summary>
    private Vector2 AdjustDirectionForEdges(Vector2 direction)
    {
        List<string> collisions = _character.CheckEdgeCollisions(direction);
        if (collisions.Contains("x")) direction.X = 0;
        if (collisions.Contains("y")) direction.Y = 0;
        return direction;
    }
}
