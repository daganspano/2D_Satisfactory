using _2D_Satisfactory.Components.ButtonClasses;
using _2D_Satisfactory.Screens.MainGame;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace _2D_Satisfactory.Screens.Title;

public class TitleScreen : Screen
{
    public TitleScreen(Game game, Action onStart, Action onOpenOptions) : base(ScreenState.Active)
    {
        float titleHeight = TitleBanner.GetHeight() + 8 + ButtonGroup.GetHeight(3);
        float bannerYPosition = (GameData.ScreenSize.Y - titleHeight) / 2;
        float buttonGroupYPosition = bannerYPosition + TitleBanner.GetHeight() + 8;

        var characters = new List<TitleCharacter>()
        {
            new TitleCharacter(game),
            new TitleCharacter(game),
            new TitleCharacter(game),
            new TitleCharacter(game),
        };

        Components.Add(new Tilemap(game, disableOffset: true));
        foreach (var character in characters) Components.Add(character);
        Components.Add(new SpriteCollisionHandler(game, characters));
        Components.Add(new TitleBanner(game, bannerYPosition));
        Components.Add(new ButtonGroup(game, new Dictionary<string, Action>
        {
            { "Start", onStart },
            { "Options", onOpenOptions },
            { "Exit", game.Exit }
        }, buttonGroupYPosition));
    }
}