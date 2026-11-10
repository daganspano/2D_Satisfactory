
using System;
using _2D_Satisfactory.Screens.MainGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2D_Satisfactory.Screens.Crafting;

public class CraftingScreen : Screen
{
    Game _game;
    Action _onClose;
    SpriteBatch _spriteBatch;
    Rectangle _backgroundBounds;
    CraftButton _craftButton;

    public override ScreenState ScreenState
    {
        get => base.ScreenState;
        set
        {
            if (value == ScreenState.Inactive) _craftButton?.TurnOffCrafting();
            base.ScreenState = value;
        }
    }

    public CraftingScreen(Game game, Action onClose) : base(ScreenState.Inactive)
    {
        _game = game;
        _onClose = onClose;
        var width = 600;
        var height = 400;
        var x = ((int)GameData.ScreenSize.X - width) / 2;
        var y = ((int)GameData.ScreenSize.Y - height) / 2;
        _backgroundBounds = new Rectangle(x, y, width, height);
        CraftDisplay craftDisplay = new CraftDisplay(game);
        CraftHandler craftHandler = new CraftHandler(game, craftDisplay.UpdateDisplayInfo);
        _craftButton = new CraftButton(game, craftHandler.ToggleCrafting);
        Components.Add(_craftButton);
        Components.Add(craftDisplay);
        Components.Add(craftHandler);
    }

    public override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(_game.GraphicsDevice);
        base.LoadContent();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        var pixel = new Texture2D(_game.GraphicsDevice, 1, 1);
        pixel.SetData(new[] { Color.White });

        _spriteBatch.Begin();

        // Background
        _spriteBatch.Draw(pixel, _backgroundBounds, Color.Black * 0.4f);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}