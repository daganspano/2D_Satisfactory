using System.Collections.Generic;
using System.Reflection.PortableExecutable;

namespace _2D_Satisfactory;

public class Item
{
    public string Id { get; init; }
    public string Name { get; init; }
    public string Plural { get; init; }
    public int IconIndex { get; init; }
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
    public double? Time { get; init; }
}

public static class Data
{
    private static List<Item> _items = 
    [
        new Item { 
            Id = "ironOre", 
            Name = "Iron Ore", 
            Plural = "Iron Ore", 
            IconIndex = 0, 
        },
        new Item { 
            Id = "ironIngot", 
            Name = "Iron Ingot", 
            Plural = "Iron Ingots", 
            IconIndex = 1, 
        },
        new Item { 
            Id = "ironPlate", 
            Name = "Iron Plate", 
            Plural = "Iron Plates", 
            IconIndex = 2,
        },
        new Item { 
            Id = "ironRod",
            Name = "Iron Rod", 
            Plural = "Iron Rods", 
            IconIndex = 3,
        },
        new Item { 
            Id = "screws",
            Name = "Screws", 
            Plural = "Screws", 
            IconIndex = 4,
        },
        new Item { 
            Id = "reinforcedIronPlate",
            Name = "Reinforced Iron Plate", 
            Plural = "Reinforced Iron Plates", 
            IconIndex = 5,
        },
        new Item { 
            Id = "rotor",
            Name = "Rotor", 
            Plural = "Rotors", 
            IconIndex = 6,
        },
        new Item { 
            Id = "smartPlate",
            Name = "Smart Plate", 
            Plural = "Smart Plates", 
            IconIndex = 7,
        },
    ];

    private static List<Machine> _machines =
    [
        new Machine {
            Id = "bench", 
            Name = "Crafting Bench",
            Icon = "bench_icon.png", 
        },
        new Machine {
            Id = "portableMiner",
            Name = "Portable Miner",
            Icon = "portable_miner_icon.png",
        },
        new Machine {
            Id = "miner",
            Name = "Miner",
            Icon = "miner_icon.png",
        },
        new Machine {
            Id = "smelter",
            Name = "Smelter",
            Icon = "smelter_icon.png",
        },
        new Machine {
            Id = "constructor",
            Name = "Constructor",
            Icon = "constructor_icon.png",
        },
        new Machine {
            Id = "assembler",
            Name = "Assembler",
            Icon = "assembler_icon.png",
        },
    ];
    
    private static List<Recipe> _recipes =
    [
        new Recipe {
            Id = "ironOre_HandMine",
            Name = "Iron Ore",
            MachineId = "",
            Input = [],
            Output = [new Recipe.Item { Id = "ironOre", QuantityRequired = 1 }],
            Hits = 3,
        },
        new Recipe {
            Id = "ironOre_PortableMiner",
            Name = "Iron Ore",
            MachineId = "portableMiner",
            Input = [],
            Output = [new Recipe.Item { Id = "ironOre", QuantityRequired = 1 }],
            Time = 1.5,
        },
        new Recipe {
            Id = "ironOre_Miner",
            Name = "Iron Ore",
            MachineId = "miner",
            Input = [],
            Output = [new Recipe.Item { Id = "ironOre", QuantityRequired = 1 }],
            Time = 1,
        },
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
            Time = 2,
        },
        new Recipe {
            Id = "ironPlate_bench",
            Name = "Iron Plate",
            MachineId = "bench",
            Input = [new Recipe.Item { Id = "ironIngot", QuantityRequired = 3 }],
            Output = [new Recipe.Item { Id = "ironPlate", QuantityRequired = 2 }],
            Hits = 3,
        },
        new Recipe {
            Id = "ironPlate_constructor",
            Name = "Iron Plate",
            MachineId = "constructor",
            Input = [new Recipe.Item { Id = "ironIngot", QuantityRequired = 3 }],
            Output = [new Recipe.Item { Id = "ironPlate", QuantityRequired = 2 }],
            Time = 6,
        },
        new Recipe {
            Id = "ironRod_bench",
            Name = "Iron Rod",
            MachineId = "bench",
            Input = [new Recipe.Item { Id = "ironIngot", QuantityRequired = 1 }],
            Output = [new Recipe.Item { Id = "ironRod", QuantityRequired = 1 }],
            Hits = 1,
        },
        new Recipe {
            Id = "ironRod_constructor",
            Name = "Iron Rod",
            MachineId = "constructor",
            Input = [new Recipe.Item { Id = "ironIngot", QuantityRequired = 1 }],
            Output = [new Recipe.Item { Id = "ironRod", QuantityRequired = 1 }],
            Time = 4,
        },
        new Recipe {
            Id = "screws_bench",
            Name = "Screws",
            MachineId = "bench",
            Input = [new Recipe.Item { Id = "ironRod", QuantityRequired = 1 }],
            Output = [new Recipe.Item { Id = "screws", QuantityRequired = 4 }],
            Hits = 3,
        },
        new Recipe {
            Id = "screws_constructor",
            Name = "Screws",
            MachineId = "constructor",
            Input = [new Recipe.Item { Id = "ironRod", QuantityRequired = 1 }],
            Output = [new Recipe.Item { Id = "screws", QuantityRequired = 4 }],
            Time = 6,
        },
        new Recipe {
            Id = "reinforcedIronPlate_bench",
            Name = "Reinforced Iron Plate",
            MachineId = "bench",
            Input = [
                new Recipe.Item { Id = "ironPlate", QuantityRequired = 6 },
                new Recipe.Item { Id = "screws", QuantityRequired = 12 }
                ],
            Output = [new Recipe.Item { Id = "reinforcedIronPlate", QuantityRequired = 1 }],
            Hits = 6,
        },
        new Recipe {
            Id = "reinforcedIronPlate_assembler",
            Name = "Reinforced Iron Plate",
            MachineId = "assembler",
            Input = [
                new Recipe.Item { Id = "ironPlate", QuantityRequired = 6 },
                new Recipe.Item { Id = "screws", QuantityRequired = 12 }
                ],
            Output = [new Recipe.Item { Id = "reinforcedIronPlate", QuantityRequired = 1 }],
            Time = 12,
        },
        new Recipe {
            Id = "rotor_bench",
            Name = "Rotor",
            MachineId = "bench",
            Input = [
                new Recipe.Item { Id = "ironRod", QuantityRequired = 5 },
                new Recipe.Item { Id = "screws", QuantityRequired = 25 }
                ],
            Output = [new Recipe.Item { Id = "rotor", QuantityRequired = 1 }],
            Hits = 6,
        },
        new Recipe {
            Id = "rotor_assembler",
            Name = "Rotor",
            MachineId = "assembler",
            Input = [
                new Recipe.Item { Id = "ironRod", QuantityRequired = 5 },
                new Recipe.Item { Id = "screws", QuantityRequired = 25 }
                ],
            Output = [new Recipe.Item { Id = "rotor", QuantityRequired = 1 }],
            Time = 15,
        },
        new Recipe {
            Id = "smartPlating_assembler",
            Name = "Smart Plating",
            MachineId = "assembler",
            Input = [
                new Recipe.Item { Id = "reinforcedIronPlate", QuantityRequired = 2 },
                new Recipe.Item { Id = "rotor", QuantityRequired = 2 }
                ],
            Output = [new Recipe.Item { Id = "smartPlating", QuantityRequired = 2 }],
            Time = 30,
        },
    ];

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
}