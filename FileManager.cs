using _2D_Satisfactory.Components.CharacterClasses;
using _2D_Satisfactory.Screens.MainGame;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace _2D_Satisfactory;

public class SaveData
{
    public int Character { get; init; }
    public int PlayerSpeed { get; init; }
    public Point PlayerPosition { get; init; }
    public DirectionFrame PlayerDirection { get; init; }
    public Dictionary<string, int> PlayerInventory { get; init; }
}

public static class FileManager
{
    private static readonly string SavePath = Path.Combine(AppContext.BaseDirectory, "Saves", "Game1.JSON");

    // IncludeFields is required so MonoGame's Point (public fields X/Y) serializes correctly.
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        IncludeFields = true
    };

    public static SaveData LoadGame()
    {
        if (!File.Exists(SavePath)) throw new FileNotFoundException($"Save file not found at {SavePath}");

        var saveData = JsonSerializer.Deserialize<SaveData>(File.ReadAllText(SavePath), JsonOptions);
        
        if (saveData is null) throw new InvalidOperationException("Failed to deserialize save data.");

        return saveData;
    }

    public static void SaveGame(SaveData saveData)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(SavePath)!);
        File.WriteAllText(SavePath, JsonSerializer.Serialize(saveData, JsonOptions));
    }

    public static bool GameExists()
    {
        return File.Exists(SavePath);
    }
}