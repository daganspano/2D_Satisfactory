using _2D_Satisfactory.Components.ButtonClasses;
using _2D_Satisfactory.Screens.MainGame;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace _2D_Satisfactory.Screens;

public class PauseScreen : Screen
{
    public PauseScreen(Game game, Action onResume, Action onOpenOptions, Action onExitToMenu) : base(ScreenState.Inactive)
    {
        float buttonGroupYPosition = (GameData.ScreenSize.Y - ButtonGroup.GetHeight(4)) / 2;

        Components.Add(new ButtonGroup(game, new Dictionary<string, Action>
        {
            { "Resume", onResume },
            { "Options", onOpenOptions },
            { "Exit to Menu", onExitToMenu },
            { "Exit to Desktop", game.Exit }
        }, buttonGroupYPosition));
    }
}