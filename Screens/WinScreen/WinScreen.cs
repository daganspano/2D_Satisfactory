using _2D_Satisfactory.Components.ButtonClasses;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace _2D_Satisfactory.Screens.WinScreenClasses;

public delegate void SetVisibilityDelegate(bool visible);

public class WinScreen : Screen
{
    public WinScreen(Game game, Action onRestart, ExitGameDelegate onExit) : base(ScreenState.Inactive)
    {
        // Progress Bar and Win Display
        WinProgressBar winProgressBar = new WinProgressBar(game){ Visible = true};
        WinDisplay winDisplay = new WinDisplay(game) { Visible = false };

        // Handler for managing visibility of progress bar and win display
        WinHandler winHandler = new WinHandler(game); 

        // Win Display Buttons
        var handleExit = (bool toDesktop) => { winHandler.Continue(); onExit(toDesktop); };
        Dictionary<string, Action> labelsAndActions = new Dictionary<string, Action>
        {
            {"Continue", winHandler.Continue},
            {"Restart", onRestart},
            {"Exit to Menu", () => handleExit(false)},
            {"Exit to Desktop",  () => handleExit(true) }
        };
        float yPosition = winDisplay.GetButtonYPosition();
        ButtonGroup buttonGroup = new ButtonGroup(game, labelsAndActions, yPosition) { Visible = false }; 

        // Set delegates for winHandler to manage visibility of progress bar and win display
        SetVisibilityDelegate setProgressBarVisibility = visible => winProgressBar.Visible = visible;
        SetVisibilityDelegate setWinDisplayVisibility = visible => {
            winDisplay.Visible = visible;
            buttonGroup.Visible = visible;
        };
        winHandler.AssignVisibilityCallbacks(setProgressBarVisibility, setWinDisplayVisibility);

        Components.Add(winProgressBar);
        Components.Add(winDisplay);
        Components.Add(buttonGroup);
        Components.Add(winHandler);
    }
}