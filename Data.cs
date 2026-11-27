using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable enable

namespace _2D_Satisfactory;

#region Types

public class Item
{
    public string Id { get; }
    public string Name { get; }
    public string Plural { get; }
    public int IconIndex { get; }
    public int? QuantityRequired { get; set; }
    public int? QuantityAvailable { get; set; }

    public Item(
        string id, 
        string name, 
        string plural, 
        int iconIndex, 
        int? quantityRequired = null, 
        int? quantityAvailable = null
    )
    {
        Id = id;
        Name = name;
        Plural = plural;
        IconIndex = iconIndex;
        QuantityRequired = quantityRequired;
        QuantityAvailable = quantityAvailable;
    }
}

// Without this, a MachineBuilding stored as a Building is saved and loaded as a plain Building.
[JsonDerivedType(typeof(MachineBuilding), "machine")]
public class Building
{
    public string Id { get; }
    public Point Location { get; }
    public string? Orientation { get; }

    public Building(string id, Point location, string? orientation = null)
    {
        Id = id;
        Location = location;
        Orientation = orientation;
    }
}

public class MachineBuilding : Building
{
    private Recipe _recipe;
    private float _timeCompleted;
    public Recipe Recipe
    {
        get => _recipe;
        set
        {
            if (_recipe.Id == value.Id) return;
            _recipe = value;
            Input = value.Input.ConvertAll(i => 
            {
                var item = Data.GetItem(i.Id); 
                item.QuantityRequired = i.QuantityRequired; 
                return item;
            });
            Output = value.Output.ConvertAll(i => 
            {
                var item = Data.GetItem(i.Id); 
                item.QuantityRequired = i.QuantityRequired; 
                return item;
            });
            _timeCompleted = 0;
        } 
    }
    public List<Item> Input { get; private set; }
    public List<Item> Output { get; private set; }
    public float Progress => _timeCompleted / Recipe.Time ?? 0;

    public MachineBuilding(string id, Point location, Recipe recipe, string? orientation = null)
        : base(id, location, orientation)
    {
        _recipe = recipe;
        _timeCompleted = 0;
        Input = new List<Item>();
        Output = new List<Item>();
    }

    public void UpdateProgress(float deltaTime)
    {
        _timeCompleted += deltaTime;

        if (_timeCompleted >= Recipe.Time)
        {
            _timeCompleted -= Recipe.Time ?? 0;
            Output.ForEach(item =>
            {
                if (!item.QuantityAvailable.HasValue) item.QuantityAvailable = 0;
                item.QuantityAvailable += item.QuantityRequired;
            });
        }
    }
}

public class Machine
{
    public string Id { get; }
    public string Name { get; }
    public string Icon { get; }

    public Machine(string id, string name, string icon)
    {
        Id = id;
        Name = name;
        Icon = icon;
    }
}

public class Recipe
{
    public class Item
    {
        public string Id { get; }
        public int QuantityRequired { get; }

        public Item(string id, int quantityRequired)
        {
            Id = id;
            QuantityRequired = quantityRequired;
        }
    }

    public string Id { get; }
    public string Name { get; }
    public string MachineId { get; }
    public List<Item> Input { get; }
    public List<Item> Output { get; }
    public int? Hits { get; }
    public float? Time { get; }

    public Recipe(
        string id, 
        string name, 
        string machineId, 
        List<Item> input, 
        List<Item> output, 
        int? hits = null, 
        float? time = null
    )
    {
        Id = id;
        Name = name;
        MachineId = machineId;
        Input = input;
        Output = output;
        Hits = hits;
        Time = time;
    }
}

#endregion

public static class Data
{
    #region Items

    private static List<Item> _items =
    [
        new Item("ironOre", "Iron Ore", "Iron Ore", 0),
        new Item("ironIngot", "Iron Ingot", "Iron Ingots", 1),
        new Item("ironPlate", "Iron Plate", "Iron Plates", 2),
        new Item("ironRod", "Iron Rod", "Iron Rods", 3),
        new Item("screws", "Screws", "Screws", 4),
        new Item("reinforcedIronPlate", "Reinforced Iron Plate", "Reinforced Iron Plates", 5),
        new Item("rotor", "Rotor", "Rotors", 6),
        new Item("smartPlate", "Smart Plate", "Smart Plates", 7)
    ];

    #endregion

    #region Machines

    private static List<Machine> _machines =
    [
        new Machine("bench", "Crafting Bench", "bench_icon.png"),
        new Machine("portableMiner", "Portable Miner", "portable_miner_icon.png"),
        new Machine("miner", "Miner", "miner_icon.png"),
        new Machine("smelter", "Smelter", "smelter_icon.png"),
        new Machine("conclassor", "Constructor", "constructor_icon.png"),
        new Machine("assembler", "Assembler", "assembler_icon.png"),
    ];
    
    #endregion

    #region Recipes

    private static List<Recipe> _recipes =
    [
        new Recipe(
            "ironOre_HandMine",
            "Iron Ore",
            "",
            new List<Recipe.Item>(),
            new List<Recipe.Item> { new Recipe.Item("ironOre", 1) },
            3
        ),
        new Recipe(
            "ironOre_PortableMiner",
            "Iron Ore",
            "portableMiner",
            new List<Recipe.Item>(),
            new List<Recipe.Item> { new Recipe.Item("ironOre", 1) },
            null,
            1.5f
        ),
        new Recipe(
            "ironOre_Miner",
            "Iron Ore",
            "miner",
            new List<Recipe.Item>(),
            new List<Recipe.Item> { new Recipe.Item("ironOre", 1) },
            null,
            1
        ),
        new Recipe(
            "ironIngot_bench",
            "Iron Ingot",
            "bench",
            new List<Recipe.Item> { new Recipe.Item("ironOre", 1) },
            new List<Recipe.Item> { new Recipe.Item("ironIngot", 1) },
            3
        ),
        new Recipe(
            "ironIngot_smelter",
            "Iron Ingot",
            "smelter",
            new List<Recipe.Item> { new Recipe.Item("ironOre", 1) },
            new List<Recipe.Item> { new Recipe.Item("ironIngot", 1) },
            null,
            2
        ),
        new Recipe(
            "ironPlate_bench",
            "Iron Plate",
            "bench",
            new List<Recipe.Item> { new Recipe.Item("ironIngot", 3) },
            new List<Recipe.Item> { new Recipe.Item("ironPlate", 2) },
            3
        ),
        new Recipe(
            "ironPlate_constructor",
            "Iron Plate",
            "constructor",
            new List<Recipe.Item> { new Recipe.Item("ironIngot", 3) },
            new List<Recipe.Item> { new Recipe.Item("ironPlate", 2) },
            null,
            6
        ),
        new Recipe(
            "ironRod_bench",
            "Iron Rod",
            "bench",
            new List<Recipe.Item> { new Recipe.Item("ironIngot", 1) },
            new List<Recipe.Item> { new Recipe.Item("ironRod", 1) },
            1
        ),
        new Recipe(
            "ironRod_constructor",
            "Iron Rod",
            "constructor",
            new List<Recipe.Item> { new Recipe.Item("ironIngot", 1) },
            new List<Recipe.Item> { new Recipe.Item("ironRod", 1) },
            null,
            4
        ),
        new Recipe(
            "screws_bench",
            "Screws",
            "bench",
            new List<Recipe.Item> { new Recipe.Item("ironRod", 1) },
            new List<Recipe.Item> { new Recipe.Item("screws", 4) },
            3
        ),
        new Recipe(
            "screws_constructor",
            "Screws",
            "constructor",
            new List<Recipe.Item> { new Recipe.Item("ironRod", 1) },
            new List<Recipe.Item> { new Recipe.Item("screws", 4) },
            null,
            6
        ),
        new Recipe(
            "reinforcedIronPlate_bench",
            "Reinforced Iron Plate",
            "bench",
            new List<Recipe.Item> { new Recipe.Item("ironPlate", 6), new Recipe.Item("screws", 12) },
            new List<Recipe.Item> { new Recipe.Item("reinforcedIronPlate", 1) },
            6
        ),
        new Recipe(
            "reinforcedIronPlate_assembler",
            "Reinforced Iron Plate",
            "assembler",
            new List<Recipe.Item> { new Recipe.Item("ironPlate", 6), new Recipe.Item("screws", 12) },
            new List<Recipe.Item> { new Recipe.Item("reinforcedIronPlate", 1) },
            null,
            12
        ),
        new Recipe(
            "rotor_bench",
            "Rotor",
            "bench",
            new List<Recipe.Item> { new Recipe.Item("ironRod", 5), new Recipe.Item("screws", 25) },
            new List<Recipe.Item> { new Recipe.Item("rotor", 1) },
            6
        ),
        new Recipe(
            "rotor_assembler",
            "Rotor",
            "assembler",
            new List<Recipe.Item> { new Recipe.Item("ironRod", 5), new Recipe.Item("screws", 25) },
            new List<Recipe.Item> { new Recipe.Item("rotor", 1) },
            null,
            15
        ),
        new Recipe(
            "smartPlating_assembler",
            "Smart Plating",
            "assembler",
            new List<Recipe.Item> { new Recipe.Item("reinforcedIronPlate", 2), new Recipe.Item("rotor", 2) },
            new List<Recipe.Item> { new Recipe.Item("smartPlate", 2) },
            null,
            30
        ),
    ];

    #endregion

    #region Public Methods

    public static List<Item> GetItems() => _items;

    public static Item GetItem(string id) => _items.Find(i => i.Id == id);
    public static Item GetItem(int index) => _items[index];

    public static List<Machine> GetMachines() => _machines;

    public static Machine GetMachine(string id) => _machines.Find(m => m.Id == id);
    public static Machine GetMachine(int index) => _machines[index];

    public static List<Recipe> GetRecipes() => _recipes;
    public static List<Recipe> GetRecipesForMachine(string machineId) 
    {
        return _recipes.FindAll(r => r.MachineId == machineId);
    }
    public static List<Recipe> GetInputRecipesForItem(string itemId) 
    {
        return _recipes.FindAll(r => r.Input.Exists(i => i.Id == itemId));
    }
    public static List<Recipe> GetOutputRecipesForItem(string itemId) 
    {
        return _recipes.FindAll(r => r.Output.Exists(i => i.Id == itemId));
    }

    public static Recipe GetRecipe(string id) => _recipes.Find(r => r.Id == id);
    public static Recipe GetRecipe(int index) => _recipes[index];

    public static int GetRecipeImageIndex(Recipe recipe)
    {
        if (recipe.Output.Count > 0)
        {
            string outputItemId = recipe.Output[0].Id;
            Item outputItem = GetItem(outputItemId);
            return outputItem.IconIndex;
        }
        return -1; // Return -1 if no output item is found
    }
    public static int GetRecipeImageIndex(string recipeId) => GetRecipeImageIndex(GetRecipe(recipeId));

    #endregion
}