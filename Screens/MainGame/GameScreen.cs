using _2D_Satisfactory.ParticleClasses;
using Microsoft.Xna.Framework;

namespace _2D_Satisfactory.Screens.MainGame;

public class GameScreen : Screen
{
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