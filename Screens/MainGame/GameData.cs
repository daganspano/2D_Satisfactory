
using _2D_Satisfactory.Components.CharacterClasses;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace _2D_Satisfactory.Screens.MainGame;

public static class GameData
{
    #region Game Properties

    /// <summary>
    /// The size of the game screen.
    /// </summary>
    public static Vector2 ScreenSize { get; set; } = Vector2.Zero;

    #endregion

    #region Player Properties

    /// <summary>
    /// The size of the player sprite.
    /// </summary>
    public static Point SpriteSize { get; set; } = new Point(16, 23);

    /// <summary>
    /// The character index for the player sprite.
    /// </summary>
    public static int Character { get; set; } = 12;

    /// <summary>
    /// The speed of the player.
    /// </summary>
    public static int PlayerSpeed { get; set; } = 200;

    /// <summary>
    /// The hit box of the player.
    /// </summary>
    public static Rectangle PlayerHitBox { get; private set; } = new Rectangle(
        Point.Zero,
        SpriteSize
    );

    /// <summary>
    /// The position of the player in the game world.
    /// </summary>
    public static Point PlayerPosition
    {
        get => PlayerHitBox.Location;
        set => PlayerHitBox = new Rectangle(
            value,
            SpriteSize
        );
    }

    public static DirectionFrame PlayerDirection { get; set; } = DirectionFrame.Down;

    public static Dictionary<string, int> PlayerInventory {get; set;} = new Dictionary<string, int>()
    {
        { "Iron Ore", 0 },
        { "Copper Ore", 0 },
        { "Coal", 0 }
    };

    /// <summary>
    /// Whether the player is currently moving.
    /// </summary>
    public static bool IsPlayerMoving { get; set; } = false;

    public static string PlayerInteraction { get; set; } = string.Empty;

    #endregion

    #region Drawing Properties

    /// <summary>
    /// The draw offset for rendering the game world based on the player's position.
    /// </summary>
    public static Vector2 DrawOffset => (0.5f * ScreenSize) - PlayerHitBox.Location.ToVector2();

    #endregion
}