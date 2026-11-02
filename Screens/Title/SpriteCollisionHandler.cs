using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace _2D_Satisfactory.Screens.Title;

public class SpriteCollisionHandler : GameComponent
{
    private readonly List<TitleCharacter> _characters;

    public SpriteCollisionHandler(Game game, List<TitleCharacter> characters) : base(game)
    {
        _characters = characters;
    }

    public override void Update(GameTime gameTime)
    {
        for (int i = 0; i < _characters.Count; i++)
        {
            for (int j = i + 1; j < _characters.Count; j++)
            {
                if (_characters[i].HitBox.Intersects(_characters[j].HitBox))
                {
                    // Get the overlap of the collision
                    Vector2 overlap = _characters[i].HitBox.Center.ToVector2() - _characters[j].HitBox.Center.ToVector2();

                    // Get the ratio of the speeds of the two characters
                    float speedRatio = (float)_characters[i].Speed / _characters[j].Speed;

                    // Calculate the overlap for each character based on their speed ratio
                    Vector2 overlapI;
                    if (speedRatio < 1) overlapI = overlap * speedRatio;
                    else overlapI = overlap / speedRatio;
                    Vector2 overlapJ = overlapI - overlap;

                    _characters[i].Bounce(overlapI);
                    _characters[j].Bounce(overlapJ);
                }
            }
        }

        base.Update(gameTime);
    }
}