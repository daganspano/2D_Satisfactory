using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using _2D_Satisfactory.Screens.MainGame;

namespace _2D_Satisfactory;

/// <summary>
/// Provides static methods for checking collisions between characters and the edges of the game screen.
/// </summary>
public static class CollisionHandler
{
    public static List<string> CheckCollisions(Rectangle hitBox, Vector2 direction)
    {
        return
        [
            .. CheckEdgeCollisions(hitBox, direction),
            .. CheckObjectCollisions(hitBox, direction),
        ];
    }

    /// <summary>
    /// Checks for collisions between a character's hit box and the edges of the game screen based on the provided direction vector.
    /// </summary>
    /// <param name="hitBox">The hit box of the character.</param>
    /// <param name="direction">The direction vector indicating the character's movement direction.</param>
    /// <returns>A list of strings representing the edges with which the character is colliding.</returns>
    public static List<string> CheckEdgeCollisions(Rectangle hitBox, Vector2 direction)
    {
        List<string> collisions = new List<string>();
        hitBox.Location += direction.ToPoint();

        if (
            (hitBox.Left <= 0 && direction.X < 0) || 
            (hitBox.Right >= GameData.ScreenSize.X && direction.X > 0)
        ) 
            collisions.Add("x");

        if (
            (hitBox.Top <= 0 && direction.Y < 0) || 
            (hitBox.Bottom >= GameData.ScreenSize.Y && direction.Y > 0)
        ) 
            collisions.Add("y");

        return collisions;
    }

    public static List<string> CheckObjectCollisions(Rectangle hitBox, Vector2 direction)
    {
        List<string> collisions = new List<string>();
        Rectangle movedHitBox = hitBox;
        movedHitBox.Location += direction.ToPoint();

        foreach (Rectangle objectHitBox in GameData.ObjectHitBoxes)
        {
            if (!movedHitBox.Intersects(objectHitBox)) continue;

            // The axis with the smaller overlap is the side being collided with
            Rectangle overlap = Rectangle.Intersect(movedHitBox, objectHitBox);

            // Only block movement toward the object so the player can always move away
            if (overlap.Width <= overlap.Height &&
                ((direction.X > 0 && hitBox.Center.X < objectHitBox.Center.X) ||
                 (direction.X < 0 && hitBox.Center.X > objectHitBox.Center.X)) &&
                !collisions.Contains("x"))
                collisions.Add("x");

            if (overlap.Height <= overlap.Width &&
                ((direction.Y > 0 && hitBox.Center.Y < objectHitBox.Center.Y) ||
                 (direction.Y < 0 && hitBox.Center.Y > objectHitBox.Center.Y)) &&
                !collisions.Contains("y"))
                collisions.Add("y");

            if (collisions.Count == 2) break;
        }

        return collisions;
    }
}