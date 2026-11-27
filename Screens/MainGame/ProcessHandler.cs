using Microsoft.Xna.Framework;

namespace _2D_Satisfactory.Screens.MainGame;

public class ProcessHandler : GameComponent
{
    public ProcessHandler(Game game) : base(game)
    {
    }

    public override void Update(GameTime gameTime)
    {
        foreach (var building in GameData.Buildings.Values)
        {
            if (!(building is MachineBuilding machineBuilding)) continue;
            machineBuilding.UpdateProgress((float)gameTime.ElapsedGameTime.TotalSeconds);

        }
    }
}