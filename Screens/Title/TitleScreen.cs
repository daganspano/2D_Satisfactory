using _2D_Satisfactory.Components;
using _2D_Satisfactory.Components.ButtonClasses;
using _2D_Satisfactory.Screens.MainGame;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace _2D_Satisfactory.Screens.Title;

public delegate void PlayDelegate(bool isNew = false);

public class TitleScreen : Screen
{
    public TitleScreen(Game game, PlayDelegate handlePlay, bool hasContinue) : base(ScreenState.Active)
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
        Dictionary<string, Action> buttons = hasContinue ? new Dictionary<string, Action>
        {
            { "Continue", () => handlePlay(isNew: false) },
            { "Restart", () => handlePlay(isNew: true) },
            { "Exit", game.Exit }
        } : new Dictionary<string, Action>
        {
            { "Start", () => handlePlay(isNew: true) },
            { "Exit", game.Exit }
        };

        // components
        Components.Add(new SpriteCollisionHandler(game, characters));
        
        // Drawable components
        Components.Add(new Tilemap(game, disableOffset: true));
        foreach (var character in characters) Components.Add(character);
        Components.Add(new TitleBanner(game, bannerYPosition));
        Components.Add(new ButtonGroup(game, buttons, buttonGroupYPosition));
    }
}