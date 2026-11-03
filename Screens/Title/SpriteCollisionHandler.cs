using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace _2D_Satisfactory.Screens.Title;

/// <summary>
/// Handles collision detection and response between TitleCharacter sprites in the title screen.
/// </summary>
public class SpriteCollisionHandler : GameComponent
{
    private readonly List<TitleCharacter> _characters;

    /// <summary>
    /// Initializes a new instance of the SpriteCollisionHandler class with the specified game and list of TitleCharacter sprites.
    /// </summary>
    /// <param name="game">The game instance to which the collision handler belongs.</param>
    /// <param name="characters">The list of TitleCharacter sprites to be managed for collisions.</param>
    public SpriteCollisionHandler(Game game, List<TitleCharacter> characters) : base(game)
    {
        _characters = characters;
    }

    /// <summary>
    /// Updates the collision detection and response for the TitleCharacter sprites. This method checks for collisions between each pair of characters and applies a bounce effect based on their speed ratios.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
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