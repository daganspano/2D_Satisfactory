using System.Collections.Generic;
using System.Reflection.PortableExecutable;

namespace _2D_Satisfactory;

public class Item
{
    public string Id { get; init; }
    public string Name { get; init; }
    public string Plural { get; init; }
    public string Icon { get; init; }
    public int? QuantityRequired { get; init; }
    public int? QuantityAvailable { get; init; }
}

public class Machine
{
    public string Id { get; init; }
    public string Name { get; init; }
    public string Icon { get; init; }
}

public class Recipe
{
    public class Item
    {
        public string Id { get; init; }
        public int QuantityRequired { get; init; }
    }

    public string Id { get; init; }
    public string Name { get; init; }
    public string MachineId { get; init; }
    public List<Item> Input { get; init; }
    public List<Item> Output { get; init; }
    public int? Hits { get; init; }
    public int? Speed { get; init; }
}

public static class Data
{
    private static List<Item> _items = 
    [
        new Item { 
            Id = "ironOre", 
            Name = "Iron Ore", 
            Plural = "Iron Ore", 
            Icon = "iron_ore_icon.png", 
        },
        new Item { 
            Id = "ironIngot", 
            Name = "Iron Ingot", 
            Plural = "Iron Ingots", 
            Icon = "iron_ingot_icon.png", 
        },
        new Item { 
            Id = "ironOre", 
            Name = "Iron Ore", 
            Plural = "Iron Ore", 
            Icon = "iron_ore_icon.png", 
        },
    ];

    private static List<Machine> _machines =
    [
        new Machine {
            Id = "bench", 
            Name = "Crafting Bench",
            Icon = "bench_icon.png", 
        },
    ];
    
    private static List<Recipe> _recipes =
    [
        new Recipe {
            Id = "ironIngot_bench",
            Name = "Iron Ingot",
            MachineId = "bench",
            Input = [new Recipe.Item { Id = "ironOre", QuantityRequired = 1 }],
            Output = [new Recipe.Item { Id = "ironIngot", QuantityRequired = 1 }],
            Hits = 3,
        },
        new Recipe {
            Id = "ironIngot_smelter",
            Name = "Iron Ingot",
            MachineId = "smelter",
            Input = [new Recipe.Item { Id = "ironOre", QuantityRequired = 1 }],
            Output = [new Recipe.Item { Id = "ironIngot", QuantityRequired = 1 }],
            Speed = 30,
        },
    ];

    public static List<Item> GetItems() => _items;

    public static Item GetItem(string id) => _items.Find(i => i.Id == id);
    public static Item GetItem(int index) => _items[index];

    public static List<Machine> GetMachines() => _machines;

    public static Machine GetMachine(string id) => _machines.Find(m => m.Id == id);
    public static Machine GetMachine(int index) => _machines[index];

    public static List<Recipe> GetRecipes() => _recipes;

    public static Recipe GetRecipe(string id) => _recipes.Find(r => r.Id == id);
    public static Recipe GetRecipe(int index) => _recipes[index];
}