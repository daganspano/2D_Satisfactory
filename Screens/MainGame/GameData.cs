
using _2D_Satisfactory.Components.CharacterClasses;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace _2D_Satisfactory.Screens.MainGame;

/// <summary>
/// Represents a static class that holds game-related data and properties.
/// </summary>
public static class GameData
{
    #region Game Properties

    /// <summary>
    /// The size of the game screen.
    /// </summary>
    public static Vector2 ScreenSize { get; set; } = Vector2.Zero;

    public static List<Rectangle> ObjectHitBoxes { get; set; } = new List<Rectangle>();

    #endregion

    #region Player Properties

    /// <summary>
    /// The size of the player sprite.
    /// </summary>
    public static Point SpriteSize => new Point(16, 23);

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

    /// <summary>
    /// The direction the player is facing, represented by a DirectionFrame enum value.
    /// </summary>
    public static DirectionFrame PlayerDirection { get; set; } = DirectionFrame.Down;

    /// <summary>
    /// The player's inventory, represented as a dictionary where the key is the item name and the value is the quantity of that item. This property allows for tracking and managing the items the player has collected during gameplay.
    /// </summary>
    public static Dictionary<string, int> PlayerInventory {get; set;} = new Dictionary<string, int>();

    /// <summary>
    /// Whether the player is currently moving.
    /// </summary>
    public static bool IsPlayerMoving { get; set; } = false;

    /// <summary>
    /// The current interaction state of the player, represented as a string. This property is used to determine what actions the player can perform in the game, such as mining resources or interacting with objects. The value of this property is updated based on the player's actions and context within the game world.
    /// </summary>
    public static string PlayerInteraction { get; set; } = string.Empty;

    #endregion

    #region Drawing Properties

    /// <summary>
    /// The draw offset for rendering the game world based on the player's position.
    /// </summary>
    public static Vector2 DrawOffset => Vector2.Floor(0.5f * (ScreenSize - PlayerHitBox.Size.ToVector2())) - PlayerHitBox.Location.ToVector2();

    public static Matrix TransformMatrix => Matrix.CreateTranslation(DrawOffset.X, DrawOffset.Y, 0f);

    #endregion

    #region Storage Methods

    /// <summary>
    /// Initializes a new game by setting default values for the player's character, speed, position, direction, and inventory. This method creates a new SaveData object with these default values and saves it using the FileManager. It is typically called when starting a new game to ensure that the player begins with a clean state and predefined settings.
    /// </summary>
    public static void NewGame()
    {
        Character = 12;
        PlayerSpeed = 200;
        PlayerPosition = (0.5f * ScreenSize).ToPoint();
        PlayerDirection = DirectionFrame.Down;
        PlayerInventory = new Dictionary<string, int>();

        SaveData defaultData = new SaveData()
        {
            Character = Character,
            PlayerSpeed = PlayerSpeed,
            PlayerPosition = PlayerPosition,
            PlayerDirection = PlayerDirection,
            PlayerInventory = PlayerInventory
        };
        
        FileManager.SaveGame(defaultData);
    }

    /// <summary>
    /// Loads the game state from a saved file using the FileManager. This method retrieves the saved data, including the player's character, speed, position, direction, and inventory, and updates the corresponding properties in the GameData class. It is typically called when resuming a previously saved game to restore the player's progress and settings.
    /// </summary>
    public static void LoadGame()
    {
        SaveData saveData = FileManager.LoadGame();

        Character = saveData.Character;
        PlayerSpeed = saveData.PlayerSpeed;
        PlayerPosition = saveData.PlayerPosition;
        PlayerDirection = saveData.PlayerDirection;
        PlayerInventory = saveData.PlayerInventory;
    }

    /// <summary>
    /// Saves the current game state to a file using the FileManager. This method creates a SaveData object containing the player's current character, speed, position, direction, and inventory, and then saves it to a file. It is typically called when the player chooses to save their progress in the game, ensuring that their current state can be restored later.
    /// </summary>
    public static void SaveGame()
    {
        SaveData saveData = new SaveData()
        {
            Character = Character,
            PlayerSpeed = PlayerSpeed,
            PlayerPosition = PlayerPosition,
            PlayerDirection = PlayerDirection,
            PlayerInventory = PlayerInventory
        };

        FileManager.SaveGame(saveData);
    }

    #endregion
}