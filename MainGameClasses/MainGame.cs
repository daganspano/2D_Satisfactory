using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using _2D_Satisfactory.Components.ButtonClasses;
using System.Collections.Generic;
using System;
using _2D_Satisfactory.ParticleClasses;
using _2D_Satisfactory.Components.CharacterClasses;

namespace _2D_Satisfactory.MainGameClasses;

/// <summary>
/// Represents the main game class that handles the game loop, including updating and drawing the game state, character, and UI elements.
/// </summary>
public class MainGame: DrawableGameComponent
{
    // Sprite Fields
    private static SpriteBatch _spriteBatch;
    private Tilemap _tilemap;
    private GameCharacter _character;
    private ButtonGroup _buttonGroup;
    private SpriteFont _font;

    private Vector2 _offset;
    private string _controlsText;
    private Dictionary<string, string> _controls;
    private MainGameState _state;
    private bool _wasPreviouslyEscPressed;

    private Dictionary<string, int> _inventory;
    private double _actionTimer;
    private double _miningSpeed;
    private HandMiningParticleSystem _miningParticles;

    public MainGame(Game game, Action exitToMenu, Action exitToDesktop): base(game)
    {
        Vector2 initialPosition = new Vector2(GameDimensions.X / 2, GameDimensions.Y / 2);
        int character = 12;
        int speed = 200;
        _character = new GameCharacter(initialPosition, character, speed);
        initialPosition.X -= _character.HitBox.Width / 2;
        initialPosition.Y -= _character.HitBox.Height / 2;
        _character = new GameCharacter(initialPosition, character, speed);
        
        _miningParticles = new HandMiningParticleSystem(game, 20);
        game.Components.Add(_miningParticles);

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
        
        _offset = Vector2.Zero;
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
    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(Game.GraphicsDevice);

        // Load tilemap
        _tilemap = new Tilemap(Game);
        _tilemap.LoadMap();

        // Load character
        _character.LoadContent(Game.Content);

        // Load buttons
        _buttonGroup.LoadContent(Game.Content);

        _font = Game.Content.Load<SpriteFont>("Orbitron-Regular");
    }

    /// <summary>
    /// Updates the game state, including character movement, collision detection, and button interactions based on the current game state.
    /// </summary>
    /// <param name="gameTime">The game time object containing timing information for the current frame.</param>
    public override void Update(GameTime gameTime)
    {
        KeyboardState keyboardState = Keyboard.GetState();
        GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

        bool isEscPressed = gamePadState.Buttons.Start == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape);
        if (isEscPressed && !_wasPreviouslyEscPressed) _state = _state == MainGameState.Idle ? MainGameState.Paused : MainGameState.Idle;
        _wasPreviouslyEscPressed = isEscPressed;

        // Update character
        if (_state == MainGameState.Idle) _character.Update(gameTime);

        // Update draw offset based on character position
        _offset.X = (GameDimensions.Width / 2) - _character.HitBox.Center.X;
        _offset.Y = (GameDimensions.Height / 2) - _character.HitBox.Center.Y;

        // Update tilemap offset
        // _tilemap.UpdateOffset(_offset);

        // Update buttons when the game is paused
        if (_state == MainGameState.Paused) _buttonGroup.Update();

        _controlsText = " ";

        // Check for collision between character and Iron Ore Deposit
        if (_tilemap.CanMineIronOre(_character.HitBox))
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

                    // Emit mining particles
                    var particleDisplacement = _character.FacingDirection switch
                    {
                        DirectionFrame.Down => new Vector2(0, _character.HitBox.Height * 0.75f),
                        DirectionFrame.Left => new Vector2(-_character.HitBox.Width * 0.75f, _character.HitBox.Height * 0.25f),
                        DirectionFrame.Right => new Vector2(_character.HitBox.Width * 0.75f, _character.HitBox.Height * 0.25f),
                        _ => Vector2.Zero
                    };
                    _miningParticles.PlaceParticles(GameDimensions.Center + particleDisplacement);
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
    /// <param name="gameTime">The game time object containing timing information for the current frame.</param>
    public override void Draw(GameTime gameTime)
    {
        Matrix transform = Matrix.CreateTranslation(_offset.X, _offset.Y, 0f);
        _spriteBatch.Begin(transformMatrix: transform);

        // Draw tilemap
        _tilemap.Draw(_spriteBatch);

        // Draw Iron Ore Deposit
        // _ironOreDeposit.Draw(_spriteBatch);

        // Draw character
        _character.Draw(_spriteBatch);

        _spriteBatch.End();

        _spriteBatch.Begin();

        // Draw inventory
        Vector2 inventoryPosition = new Vector2(10, 10);
        _spriteBatch.DrawString(_font, "Inventory:", inventoryPosition, Color.White);
        foreach (var item in _inventory)
        {
            if (item.Value > 0)
            {
                inventoryPosition.Y += 30;
                _spriteBatch.DrawString(_font, $"   {item.Key}: {item.Value}", inventoryPosition, Color.White);
            }
        }

        // Draw game controls
        Vector2 textSize = _font.MeasureString(_controlsText ?? " ");
        Vector2 textPosition = new Vector2((GameDimensions.Width - textSize.X) / 2, GameDimensions.Height - textSize.Y - 10);
        _spriteBatch.DrawString(_font, _controlsText ?? " ", textPosition, Color.White);
        
        // Draw buttons when the game is paused
        if (_state == MainGameState.Paused) _buttonGroup.Draw(_spriteBatch);

        _spriteBatch.End();
    }
}