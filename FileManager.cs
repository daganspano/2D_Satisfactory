using _2D_Satisfactory.Components.CharacterClasses;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace _2D_Satisfactory;

/// <summary>
/// Represents the data structure for saving and loading game state.
/// </summary>
public class SaveData
{
    /// <summary>
    /// The character selected by the player, represented as an integer identifier.
    /// </summary>
    public int Character { get; init; }

    /// <summary>
    /// The speed of the player character, represented as an integer value.
    /// </summary>
    public int PlayerSpeed { get; init; }

    /// <summary>
    /// The position of the player character in the game world, represented as a Point structure.
    /// </summary>
    public Point PlayerPosition { get; init; }

    /// <summary>
    /// The direction the player character is facing, represented as a DirectionFrame enumeration.
    /// </summary>
    public DirectionFrame PlayerDirection { get; init; }

    /// <summary>
    /// The inventory of the player character, represented as a dictionary mapping item names to their respective quantities.
    /// </summary>
    public Dictionary<string, int> PlayerInventory { get; init; }
}

/// <summary>
/// Provides functionality for saving and loading game state to and from a JSON file.
/// </summary>
public static class FileManager
{
    /// <summary>
    /// The path to the save file where game state is stored. The save file is located in the "Saves" directory within the application's base directory and is named "Game1.JSON".
    /// </summary>
    private static readonly string SavePath = Path.Combine(AppContext.BaseDirectory, "Saves", "Game1.JSON");

    /// <summary>
    /// The options for JSON serialization and deserialization, including settings for indentation and field inclusion.
    /// </summary>
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        IncludeFields = true
    };

    /// <summary>
    /// Loads the game state from the save file. If the save file does not exist, a FileNotFoundException is thrown. If deserialization fails, an InvalidOperationException is thrown. Returns a SaveData object containing the loaded game state.
    /// </summary>
    /// <returns>A SaveData object containing the loaded game state.</returns>
    /// <exception cref="FileNotFoundException">Thrown if the save file does not exist at the specified path.</exception>
    /// <exception cref="InvalidOperationException">Thrown if deserialization of the save data fails.</exception>
    public static SaveData LoadGame()
    {
        if (!File.Exists(SavePath)) throw new FileNotFoundException($"Save file not found at {SavePath}");

        var saveData = JsonSerializer.Deserialize<SaveData>(File.ReadAllText(SavePath), JsonOptions);
        
        if (saveData is null) throw new InvalidOperationException("Failed to deserialize save data.");

        return saveData;
    }

    /// <summary>
    /// Saves the provided game state to the save file. If the directory for the save file does not exist, it is created. The game state is serialized to JSON format and written to the save file.
    /// </summary>
    /// <param name="saveData">The SaveData object containing the game state to be saved.</param>
    public static void SaveGame(SaveData saveData)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(SavePath)!);
        File.WriteAllText(SavePath, JsonSerializer.Serialize(saveData, JsonOptions));
    }

    /// <summary>
    /// Checks if a save file exists at the specified path. Returns true if the save file exists, otherwise returns false.
    /// </summary>
    /// <returns>True if the save file exists, otherwise false.</returns>
    public static bool GameExists()
    {
        return File.Exists(SavePath);
    }
}