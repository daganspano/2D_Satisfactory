using _2D_Satisfactory.Components.ButtonClasses;
using _2D_Satisfactory.Screens.MainGame;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace _2D_Satisfactory.Screens;

public delegate void ExitDelegate(bool toDesktop);

public class PauseScreen : Screen
{
    public PauseScreen(Game game, Action onResume, ExitDelegate onExit) : base(ScreenState.Inactive)
    {
        float buttonGroupYPosition = (GameData.ScreenSize.Y - ButtonGroup.GetHeight(3)) / 2;

        Components.Add(new ButtonGroup(game, new Dictionary<string, Action>
        {
            { "Resume", onResume },
            { "Exit to Menu", () => onExit(toDesktop: false) },
            { "Exit to Desktop", () => onExit(toDesktop: true) }
        }, buttonGroupYPosition));
    }
}