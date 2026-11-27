using Microsoft.Xna.Framework;
using _2D_Satisfactory.Screens.MainGame;
using System.Collections.Generic;
using System;

namespace _2D_Satisfactory.Screens.AssemblerClasses;

public class AssemblerHandler : GameComponent
{
    private static readonly List<Item> _noItems = new List<Item>();

    // Resolved on every use: GameData.Buildings is empty until a game is started and is replaced on every new/loaded game.
    private Func<MachineBuilding> _getMachine;
    private List<Recipe> _recipes;

    public AssemblerHandler(Game game, Func<MachineBuilding> getMachine) : base(game)
    {
        _getMachine = getMachine;
        _recipes = Data.GetRecipesForMachine("assembler");
    }

    public void SetSelectedRecipe(int recipeIndex)
    {
        MachineBuilding machine = _getMachine();
        if (machine != null) machine.Recipe = _recipes[recipeIndex];
    }

    public (List<Item> inputItems, List<Item> outputItems, float progress) GetCraftingInfo()
    {
        MachineBuilding machine = _getMachine();
        if (machine == null) return (_noItems, _noItems, 0f);
        return (machine.Input, machine.Output, machine.Progress);
    }

    public override void Update(GameTime gameTime)
    {
        _getMachine()?.UpdateProgress((float)gameTime.ElapsedGameTime.TotalSeconds);
    }
}