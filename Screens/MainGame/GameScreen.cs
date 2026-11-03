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
    public GameScreen(Game game) : base(ScreenState.Inactive)
    {
        HandMiningParticleSystem miningParticles = new HandMiningParticleSystem(game, 100);
        Components.Add(new Tilemap(game));
        Components.Add(new InputHandler(game, miningParticles));
        Components.Add(miningParticles);
        Components.Add(new Player(game));
        Components.Add(new ControlsDisplay(game));
        Components.Add(new InventoryDisplay(game));
    }
}