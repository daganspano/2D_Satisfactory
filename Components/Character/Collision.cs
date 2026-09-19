using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace _2D_Satisfactory.Components.Character;

public static class Collision
{
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