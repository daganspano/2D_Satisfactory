using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using _2D_Satisfactory.Components.ButtonClasses;
using System.Collections.Generic;
using System;

namespace _2D_Satisfactory.MainGameClasses;

/// <summary>
/// Represents the main game class that handles the game loop, including updating and drawing the game state, character, and UI elements.
/// </summary>
public class MainGame
{
    // Sprite Fields
    private Texture2D _backgroundTexture;
    private IronOreDeposit _ironOreDeposit;
    private GameCharacter _character;
    private ButtonGroup _buttonGroup;
    private SpriteFont _font;

    private string _controlsText;
    private Dictionary<string, string> _controls;
    private MainGameState _state;
    private bool _wasPreviouslyEscPressed;

    private Dictionary<string, int> _inventory;
    private double _actionTimer;
    private double _miningSpeed;

    public MainGame(Game game, Action exitToMenu, Action exitToDesktop)
    {
        Vector2 initialPosition = new Vector2(GameDimensions.X / 2, GameDimensions.Y / 2);
        int character = 2;
        int speed = 100;
        _ironOreDeposit = new IronOreDeposit(game);
        _character = new GameCharacter(initialPosition, character, speed);
        
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
        
        _controls = new Dictionary<string, string>
        {
            { "Mine_Ore", "Mine iron ore: E" }
        };
        _state = MainGameState.Idle;

        _inventory = new Dictionary<string, int>();
        _actionTimer = 0f;
        _miningSpeed = 0.5f;
    }

    /// <summary>
    /// Loads the content for the main game, including the background texture, character, and buttons.
    /// </summary>
    /// <param name="content">The content manager used to load the game's assets.</param>
    public void LoadContent(ContentManager content)
    {
        // Load background
        _backgroundTexture = content.Load<Texture2D>("forest_background");

        // Load character
        _character.LoadContent(content);

        // Load buttons
        _buttonGroup.LoadContent(content);

        _font = content.Load<SpriteFont>("Orbitron-Regular");
    }

    /// <summary>
    /// Updates the game state, including character movement, collision detection, and button interactions based on the current game state.
    /// </summary>
    /// <param name="gameTime">The game time object containing timing information for the current frame.</param>
    public void Update(GameTime gameTime)
    {
        KeyboardState keyboardState = Keyboard.GetState();
        GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

        bool isEscPressed = gamePadState.Buttons.Start == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape);
        if (isEscPressed && !_wasPreviouslyEscPressed) _state = _state == MainGameState.Idle ? MainGameState.Paused : MainGameState.Idle;
        _wasPreviouslyEscPressed = isEscPressed;

        // Update Iron Ore Deposit
        _ironOreDeposit.Update(gameTime);

        // Update character
        if (_state == MainGameState.Idle) _character.Update(gameTime);

        // Update buttons when the game is paused
        if (_state == MainGameState.Paused) _buttonGroup.Update();

        _controlsText = " ";

        // Check for collision between character and Iron Ore Deposit
        if (_ironOreDeposit.CollidesWith(_character.HitBox))
        {
            _controlsText += _controls["Mine_Ore"] + " ";
            
            if (keyboardState.IsKeyDown(Keys.E) || gamePadState.Buttons.X == ButtonState.Pressed)
            {
                _actionTimer += gameTime.ElapsedGameTime.TotalSeconds;
                if (_actionTimer >= _miningSpeed)
                {
                    // Add iron ore to inventory
                    if (!_inventory.ContainsKey("Iron Ore")) _inventory["Iron Ore"] = 0;
                    _inventory["Iron Ore"]++;
                    _actionTimer -= _miningSpeed;
                }
            }
            else
            {
                _actionTimer = 0f;
            }
        }
    }

    /// <summary>
    /// Draws the game elements, including the background, character, and buttons, based on the current game state.
    /// </summary>
    /// <param name="spriteBatch">The sprite batch used to draw the game elements.</param>
    public void Draw(SpriteBatch spriteBatch)
    {
        // Draw background
        spriteBatch.Draw(_backgroundTexture, new Vector2(0, 0), null, Color.White, 0f, Vector2.Zero, 1.8f, SpriteEffects.None, 0f);

        // Draw Iron Ore Deposit
        _ironOreDeposit.Draw(spriteBatch);

        // Draw character
        _character.Draw(spriteBatch);

        // Draw buttons when the game is paused
        if (_state == MainGameState.Paused) _buttonGroup.Draw(spriteBatch);

        // Draw inventory
        Vector2 inventoryPosition = new Vector2(10, 10);
        spriteBatch.DrawString(_font, "Inventory:", inventoryPosition, Color.White);
        foreach (var item in _inventory)
        {
            if (item.Value > 0)
            {
                inventoryPosition.Y += 30;
                spriteBatch.DrawString(_font, $"   {item.Key}: {item.Value}", inventoryPosition, Color.White);
            }
        }

        // Draw game controls
        Vector2 textSize = _font.MeasureString(_controlsText ?? " ");
        Vector2 textPosition = new Vector2((GameDimensions.Width - textSize.X) / 2, GameDimensions.Height - textSize.Y - 10);
        spriteBatch.DrawString(_font, _controlsText ?? " ", textPosition, Color.White);
    }
}