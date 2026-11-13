using System;
using System.Collections.Generic;
using _2D_Satisfactory.Screens.MainGame;
using Microsoft.Xna.Framework;

namespace _2D_Satisfactory.Screens.Crafting;

/// <summary>
/// Handles the crafting logic for the game, including managing recipes, tracking crafting progress, and updating the display with relevant information.
/// </summary>
public class CraftHandler : GameComponent
{
    private UpdateDisplayInfoDelegate _updateDisplayInfo;
    private List<Recipe> _recipes;
    private Recipe _selectedRecipe;
    private bool _isCrafting;
    private int _hitsCompleted;
    private double _hitTimer;
    private static double _hitSpeed = 0.25;

    /// <summary>
    /// Initializes a new instance of the CraftHandler class with the specified game and display update delegate.
    /// </summary>
    /// <param name="game">The game instance.</param>
    /// <param name="updateDisplayInfo">The delegate used to update the crafting display.</param>
    public CraftHandler(Game game, UpdateDisplayInfoDelegate updateDisplayInfo) : base(game)
    {
        _updateDisplayInfo = updateDisplayInfo;
        var allRecipes = Data.GetRecipes();
        _recipes = allRecipes.FindAll(r => r.MachineId == "bench");
        _selectedRecipe = _recipes[0];
        _hitsCompleted = 0;
    }

    /// <summary>
    /// Toggles the crafting state. If crafting is currently active, it will stop crafting; if it is inactive, it will start crafting. Validates the availability of required items before toggling.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the required items for crafting are not available.</exception>
    public void ToggleCrafting()
    {
        try
        {
            ValidateCraftingAvailability();
            _isCrafting = !_isCrafting;
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
            _isCrafting = false;
        }
        
        _hitsCompleted = 0;
    }

    /// <summary>
    /// Updates the crafting process based on the elapsed game time. If crafting is active, it checks for item availability, increments the hit timer, and processes completed hits to update the player's inventory accordingly.
    /// </summary>
    /// <param name="gameTime">The elapsed game time since the last update.</param>
    /// <exception cref="InvalidOperationException">Thrown if the required items for crafting are not available.</exception>
    public override void Update(GameTime gameTime)
    {
        UpdateDisplay();

        if (_isCrafting)
        {
            try
            {
                ValidateCraftingAvailability();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
                _isCrafting = false;
            }
        
            _hitTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (_hitTimer >= _hitSpeed)
            {
                _hitsCompleted++;
                _hitTimer = 0;

                if (_hitsCompleted >= (_selectedRecipe.Hits ?? 0))
                {
                    _hitsCompleted = 0;

                    // Remove input items from inventory
                    foreach (var inputItem in _selectedRecipe.Input)
                    {
                        if (!GameData.PlayerInventory.ContainsKey(inputItem.Id)) throw new InvalidOperationException($"Not enough {inputItem.Id} to craft {_selectedRecipe.Name}.");
                        GameData.PlayerInventory[inputItem.Id] -= inputItem.QuantityRequired;
                    }

                    // Add output items to inventory
                    foreach (var outputItem in _selectedRecipe.Output)
                    {
                        if (!GameData.PlayerInventory.ContainsKey(outputItem.Id)) GameData.PlayerInventory[outputItem.Id] = 0;
                        GameData.PlayerInventory[outputItem.Id] += outputItem.QuantityRequired;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Updates the display with the current input and output items, their quantities, and the crafting progress. This method retrieves the necessary item information from the data source and prepares it for display.
    /// </summary>
    private void UpdateDisplay()
    {
        List<Item> inputItems = new List<Item>();
        foreach (Recipe.Item recipeItem in _selectedRecipe.Input)
        {
            Item item = Data.GetItem(recipeItem.Id);
            inputItems.Add(new Item
            {
                Id = item.Id,
                Name = item.Name,
                Plural = item.Plural,
                Icon = item.Icon,
                QuantityRequired = recipeItem.QuantityRequired,
                QuantityAvailable = GameData.PlayerInventory.ContainsKey(item.Id) ? GameData.PlayerInventory[item.Id] : 0
            });
        }

        List<Item> outputItems = new List<Item>();
        foreach (Recipe.Item recipeItem in _selectedRecipe.Output)
        {
            Item item = Data.GetItem(recipeItem.Id);
            outputItems.Add(new Item
            {
                Id = item.Id,
                Name = item.Name,
                Plural = item.Plural,
                Icon = item.Icon,
                QuantityRequired = recipeItem.QuantityRequired,
            });
        }
        
        _updateDisplayInfo(inputItems, outputItems, _selectedRecipe.Hits ?? 0, _hitsCompleted);
    }

    /// <summary>
    /// Validates whether the player has enough of each required input item to craft the selected recipe. If any required item is insufficient, an InvalidOperationException is thrown with a message indicating the missing item and the recipe name.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the required items for crafting are not available.</exception>
    private void ValidateCraftingAvailability()
    {
        if (_selectedRecipe == null) throw new InvalidOperationException("No recipe selected.");
        for (int i = 0; i < _selectedRecipe.Input.Count; i++)
        {
            var inputItem = _selectedRecipe.Input[i];
            if (!GameData.PlayerInventory.ContainsKey(inputItem.Id) || GameData.PlayerInventory[inputItem.Id] < inputItem.QuantityRequired)
            {
                throw new InvalidOperationException($"Not enough {inputItem.Id} to craft {_selectedRecipe.Name}.");
            }
        }
    }
}