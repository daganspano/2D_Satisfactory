using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace _2D_Satisfactory.Components.CharacterClasses;

/// <summary>
/// Provides static methods for checking collisions between characters and the edges of the game screen, as well as between characters themselves.
/// </summary>
public static class Collision
{
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
            (hitBox.Right >= GameDimensions.X && direction.X > 0)
        ) 
            collisions.Add("x");

        if (
            (hitBox.Top <= 200 && direction.Y < 0) || 
            (hitBox.Bottom >= GameDimensions.Y && direction.Y > 0)
        ) 
            collisions.Add("y");

        return collisions;
    }

    /// <summary>
    /// Checks for collisions between multiple characters based on their hit boxes and direction vectors.
    /// </summary>
    /// <param name="hitBoxesAndDirections">A list of tuples, each containing a character's hit box and direction vector.</param>
    /// <returns>A dictionary mapping each character's index to a list of collision sides and the indices of the characters they are colliding with.</returns>
    public static Dictionary<int, List<(string, int)>> CheckSpriteCollisions(List<(Rectangle, Vector2)> hitBoxesAndDirections)
    {
        Dictionary<int, List<(string, int)>> collisionSides = new Dictionary<int, List<(string, int)>>();

        for (int i = 0; i < hitBoxesAndDirections.Count; i++)
        {
            for (int j = i + 1; j < hitBoxesAndDirections.Count; j++)
            {
                (Rectangle hitBox1, Vector2 direction1) = hitBoxesAndDirections[i];
                (Rectangle hitBox2, Vector2 direction2) = hitBoxesAndDirections[j];

                if (hitBox1.Right <= hitBox2.Left || hitBox1.Left >= hitBox2.Right ||
                    hitBox1.Bottom <= hitBox2.Top || hitBox1.Top >= hitBox2.Bottom)
                {
                    continue;
                }

                int overlapX = Math.Min(hitBox1.Right, hitBox2.Right) - Math.Max(hitBox1.Left, hitBox2.Left);
                int overlapY = Math.Min(hitBox1.Bottom, hitBox2.Bottom) - Math.Max(hitBox1.Top, hitBox2.Top);

                if (overlapX <= overlapY)
                {
                    (int overlap1, int overlap2) = CalculateOverlaps(hitBox1.Center.X, direction1.X, hitBox2.Center.X, direction2.X, overlapX);

                    if (hitBox1.Center.X < hitBox2.Center.X)
                    {
                        AddCollisionSide(collisionSides, i, "right", overlap1);
                        AddCollisionSide(collisionSides, j, "left", overlap2);
                    }
                    else
                    {
                        AddCollisionSide(collisionSides, i, "left", overlap1);
                        AddCollisionSide(collisionSides, j, "right", overlap2);
                    }
                }
                else
                {
                    (int overlap1, int overlap2) = CalculateOverlaps(hitBox1.Center.Y, direction1.Y, hitBox2.Center.Y, direction2.Y, overlapY);

                    if (hitBox1.Center.Y < hitBox2.Center.Y)
                    {
                        AddCollisionSide(collisionSides, i, "bottom", overlap1);
                        AddCollisionSide(collisionSides, j, "top", overlap2);
                    }
                    else
                    {
                        AddCollisionSide(collisionSides, i, "top", overlap1);
                        AddCollisionSide(collisionSides, j, "bottom", overlap2);
                    }
                }
            }
        }

        return collisionSides;
    }

    /// <summary>
    /// Calculates the overlaps for two colliding hit boxes based on their centers, direction vectors, and the total overlap.
    /// </summary>
    /// <param name="center1">The center coordinate of the first hit box along the relevant axis (X or Y).</param>
    /// <param name="direction1">The movement direction of the first hit box along the relevant axis (X or Y).</param>
    /// <param name="center2">The center coordinate of the second hit box along the relevant axis (X or Y).</param>
    /// <param name="direction2">The movement direction of the second hit box along the relevant axis (X or Y).</param>
    /// <param name="overlap">The total overlap between the two hit boxes along the relevant axis (X or Y).</param>
    /// <returns>A tuple containing the overlaps for the first and second hit boxes along the relevant axis.</returns>
    private static (int, int) CalculateOverlaps(int center1, float direction1, int center2, float direction2, int overlap)
    {
        bool isHB1MovingToHB2 = (center1 < center2 && direction1 > 0) || (center1 > center2 && direction1 < 0);
        bool isHB2MovingToHB1 = (center2 < center1 && direction2 > 0) || (center2 > center1 && direction2 < 0);

        float totalDirection = Math.Abs(direction1) + Math.Abs(direction2);
        int overlap1 = 0;
        int overlap2 = 0;

        if (isHB1MovingToHB2 && isHB2MovingToHB1)
        {
            float ratio1 = Math.Abs(direction1) / totalDirection;

            if (totalDirection > 0)
            {
                overlap1 = (int)Math.Round(overlap * ratio1);
                overlap2 = overlap - overlap1;
            }
            else
            {
                overlap1 = (int)Math.Round(overlap * ratio1);
                overlap2 = overlap - overlap1;
            }
        }
        else if (isHB1MovingToHB2) overlap1 = overlap;
        else if (isHB2MovingToHB1) overlap2 = overlap;
        else
        {
            overlap1 = overlap / 2;
            overlap2 = overlap - overlap1;
        }

        return (overlap1, overlap2);
    }

    /// <summary>
    /// Adds a collision side to the dictionary for a specific character index, ensuring that duplicate sides are not added.
    /// </summary>
    /// <param name="collisionSides">The dictionary containing collision sides for each character index.</param>
    /// <param name="index">The index of the character for which the collision side is being added.</param>
    /// <param name="side">The side of the character that is colliding (e.g., "top", "bottom", "left", "right").</param>
    /// <param name="overlap">The overlap distance for the collision side.</param>
    private static void AddCollisionSide(Dictionary<int, List<(string, int)>> collisionSides, int index, string side, int overlap)
    {
        if (!collisionSides.TryGetValue(index, out List<(string, int)> sides))
        {
            sides = new List<(string, int)>();
            collisionSides[index] = sides;
        }

        if (!sides.Exists(s => s.Item1 == side))
            sides.Add((side, overlap));
    }
}