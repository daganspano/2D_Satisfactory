using _2D_Satisfactory.Components.Machines.PopUpDisplay;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace _2D_Satisfactory.Screens.AssemblerClasses;

public class AssemblerScreen : MachineScreen
{
    private List<Recipe> _recipes;
    private SetSelectedRecipeDelegate _setSelectedRecipe;
    private GetCraftingInfoDelegate _getCraftingInfo;

    public AssemblerScreen(Game game, Func<MachineBuilding> getMachine) : base(game)
    {
        var assemblerHandler = new AssemblerHandler(game, getMachine);

        _recipes = Data.GetRecipesForMachine("assembler");
        _setSelectedRecipe = assemblerHandler.SetSelectedRecipe;
        _getCraftingInfo = assemblerHandler.GetCraftingInfo;

        Components.Add(assemblerHandler);
    }

    protected override List<Recipe> GetRecipes() => _recipes;

    protected override SetSelectedRecipeDelegate GetSetSelectedRecipe() => _setSelectedRecipe;

    protected override GetCraftingInfoDelegate GetGetCraftingInfo() => _getCraftingInfo;

}