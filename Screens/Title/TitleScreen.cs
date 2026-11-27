using _2D_Satisfactory.Components;
using _2D_Satisfactory.Components.ButtonClasses;
using _2D_Satisfactory.Screens.MainGame;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace _2D_Satisfactory.Screens.Title;

public class TitleScreen : Screen
{
    private Dictionary<string, Action> _buttonsWithContinue;
    private ButtonGroup _buttonGroup;

    public TitleScreen(Game game, PlayGameDelegate onStart, bool hasContinue) : base(ScreenState.Active)
    {
        float titleHeight = TitleBanner.GetHeight() + 8 + ButtonGroup.GetHeight(3);
        float bannerYPosition = (GameData.ScreenSize.Y - titleHeight) / 2;
        float buttonGroupYPosition = bannerYPosition + TitleBanner.GetHeight() + 8;

        List<TitleCharacter> characters = new List<TitleCharacter>()
        {
            new TitleCharacter(game),
            new TitleCharacter(game),
            new TitleCharacter(game),
            new TitleCharacter(game),
        };
        _buttonsWithContinue = new Dictionary<string, Action>
        {
            { "Continue", () => onStart(isNew: false) },
            { "Restart", () => onStart(isNew: true) },
            { "Exit", game.Exit }
        };
        Dictionary<string, Action> buttons = hasContinue ? _buttonsWithContinue : new Dictionary<string, Action>
        {
            { "Start", () => onStart(isNew: true) },
            { "Exit", game.Exit }
        };
        _buttonGroup = new ButtonGroup(game, buttons, buttonGroupYPosition);

        // components
        Components.Add(new SpriteCollisionHandler(game, characters));
        
        // Drawable components
        Components.Add(new Tilemap(game, disableOffset: true));
        foreach (var character in characters) Components.Add(character);
        Components.Add(new TitleBanner(game, bannerYPosition));
        Components.Add(_buttonGroup);
    }

    public void UpdateButtonsWithContinue()
    {
        _buttonGroup.SetButtons(_buttonsWithContinue);
    }
}