using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using _2D_Satisfactory.Components.ButtonClasses;
using System.Collections.Generic;
using System;

namespace _2D_Satisfactory.MainGameClasses;

public class MainGame
{
    // Sprite Fields
    private Texture2D _backgroundTexture;
    private GameCharacter _character;
    private ButtonGroup _buttonGroup;
    private MainGameState _state;
    private bool _wasPreviouslyEscPressed;

    public MainGame(Action exitToMenu, Action exitToDesktop)
    {
        Vector2 initialPosition = new Vector2(GameDimensions.X / 2, GameDimensions.Y / 2);
        int character = 2;
        int speed = 100;
        _character = new GameCharacter(initialPosition, character, speed);
        _state = MainGameState.Idle;

        // Get button group y position
        const float buttonScale = 0.7f;
        float buttonHeightScaled = 88 * buttonScale;
        const int buttonPadding = 8;
        float buttonGroupHeight = (3 * buttonPadding) + (4 * buttonHeightScaled);
        float buttonGroupYPosition = (GameDimensions.Y - buttonGroupHeight) / 2;

        // Create the buttons
        _buttonGroup = new ButtonGroup(
            new Dictionary<string, Action>
            {
                { "Resume", () => _state = MainGameState.Idle },
                { "Options", () => {} },
                { "Exit to Menu", () => { _state = MainGameState.Idle; exitToMenu(); } },
                { "Exit to Desktop", exitToDesktop }
            }, 
            buttonGroupYPosition
        );
    }

    public void LoadContent(ContentManager content)
    {
        // Load background
        _backgroundTexture = content.Load<Texture2D>("forest_background");

        // Load character
        _character.LoadContent(content);

        // Load buttons
        _buttonGroup.LoadContent(content);
    }

    public void Update(GameTime gameTime)
    {
        bool isEscPressed = GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape);
        if (isEscPressed && !_wasPreviouslyEscPressed) _state = _state == MainGameState.Idle ? MainGameState.Paused : MainGameState.Idle;
        _wasPreviouslyEscPressed = isEscPressed;

        // Update character
        if (_state == MainGameState.Idle) _character.Update(gameTime);

        // Update buttons when the game is paused
        if (_state == MainGameState.Paused) _buttonGroup.Update();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        // Draw background
        spriteBatch.Draw(_backgroundTexture, new Vector2(0, 0), null, Color.White, 0f, Vector2.Zero, 1.8f, SpriteEffects.None, 0f);

        // Draw character
        _character.Draw(spriteBatch);

        // Draw buttons when the game is paused
        if (_state == MainGameState.Paused) _buttonGroup.Draw(spriteBatch);
    }
}