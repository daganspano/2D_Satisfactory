using System;
using _2D_Satisfactory.Components;
using _2D_Satisfactory.ParticleClasses;
using Microsoft.Xna.Framework;

namespace _2D_Satisfactory.Screens.MainGame;

/// <summary>
/// Represents the main game screen, managing the game's components and their interactions.
/// </summary>
public class GameScreen : Screen
{
    /// <summary>
    /// Initializes a new instance of the GameScreen class, setting up the necessary components for the main game screen.
    /// </summary>
    /// <param name="game">The game instance to which this screen belongs.</param>
    public GameScreen(Game game, Action onOpenCraftingMenu) : base(ScreenState.Inactive)
    {
        HandMiningParticleSystem miningParticles = new HandMiningParticleSystem(game, 100);
        CraftingBench craftingBench = new CraftingBench(game, location: new Point(43, 15), orientation: "vertical");

        GameData.ObjectHitBoxes.Add(craftingBench.HitBox);

        Components.Add(new Tilemap(game));
        Components.Add(craftingBench);
        Components.Add(new InputHandler(game, miningParticles, onOpenCraftingMenu));
        Components.Add(miningParticles);
        Components.Add(new Player(game));
        Components.Add(new ControlsDisplay(game));
        Components.Add(new InventoryDisplay(game));
    }
}