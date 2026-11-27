using _2D_Satisfactory.Components.Machines.PopUpDisplay;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace _2D_Satisfactory.Screens.Crafting;

public class CraftingScreen : MachineScreen
{
    private Game _game;
    private List<Recipe> _recipes;
    private SetSelectedRecipeDelegate _setSelectedRecipe;
    private Action _toggleCrafting;
    private GetCraftingInfoDelegate _getCraftingInfo;

    public CraftingScreen(Game game) : base(game)
    {        
        _game = game;

        CraftHandler craftHandler = new CraftHandler(game);

        _recipes = Data.GetRecipesForMachine("bench");


        _setSelectedRecipe = craftHandler.SetSelectedRecipe;
        _toggleCrafting = craftHandler.ToggleCrafting;
        _getCraftingInfo = craftHandler.GetCraftingInfo;
        
        Components.Add(craftHandler);
    }

    protected override List<Recipe> GetRecipes() => _recipes;

    protected override SetSelectedRecipeDelegate GetSetSelectedRecipe() => _setSelectedRecipe;

    protected override GetCraftingInfoDelegate GetGetCraftingInfo() => _getCraftingInfo;

    protected override DrawableGameComponent GetBottomDisplay() => new CraftButton(_game, _toggleCrafting);
}