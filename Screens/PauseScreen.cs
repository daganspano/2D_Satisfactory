using _2D_Satisfactory.Components.ButtonClasses;
using _2D_Satisfactory.Screens.MainGame;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace _2D_Satisfactory.Screens;

/// <summary>
/// Represents the pause screen in the game, providing options to resume or exit the game.
/// </summary>
public class PauseScreen : Screen
{
    public PauseScreen(Game game, Action onResume, ExitGameDelegate onExit) : base(ScreenState.Inactive)
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