using _2D_Satisfactory.Components.CharacterClasses;
using _2D_Satisfactory.ParticleClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace _2D_Satisfactory.Screens.MainGame;

/// <summary>
/// Handles player input for movement and interactions within the game.
/// </summary>
public class InputHandler : GameComponent
{
    private double _interactionTimer;
    private double _miningSpeed;
    private HandMiningParticleSystem _miningParticles;

    private Action _handleOpenCraftingMenu;

    /// <summary>
    /// Initializes a new instance of the InputHandler class.
    /// </summary>
    /// <param name="game">The game instance to which this input handler belongs.</param>
    /// <param name="miningParticles">The particle system used for hand mining effects.</param>
    public InputHandler(Game game, HandMiningParticleSystem miningParticles, Action onOpenCraftingMenu) : base(game)
    {
        _interactionTimer = 0f;
        _miningSpeed = 0.5f;
        _miningParticles = miningParticles;
        _handleOpenCraftingMenu = onOpenCraftingMenu;
    }

    /// <summary>
    /// Updates the input handler, processing player movement and interactions based on keyboard and gamepad input.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    public override void Update(GameTime gameTime)
    {
        if (Game.IsActive)
        {
            HandleMovement(gameTime);
            HandleInteraction(gameTime);
        }
        base.Update(gameTime);
    }

    /// <summary>
    /// Handles player movement based on keyboard and gamepad input, updating the player's position and direction accordingly. It also checks for edge collisions to prevent the player from moving outside the game boundaries.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    private void HandleMovement(GameTime gameTime)
    {
        // Get Keyboard Input
        KeyboardState keyboardState = Keyboard.GetState();
        Vector2 keyboardMovement = new Vector2(
            (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right)) ? 1 
                : (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left)) ? -1 : 0,
            (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down)) ? 1 
                : (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up)) ? -1 : 0
        );
        
        // Get Gamepad Input
        Vector2 leftThumbStick = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left * new Vector2(1, -1);

        // Combine inputs and clamp to ensure the direction vector is within the range of -1 to 1 for both axes
        Vector2 characterDirection = Vector2.Clamp(leftThumbStick + keyboardMovement, new Vector2(-1, -1), new Vector2(1, 1));

        // Adjust for edges
        List<string> collisions = CollisionHandler.CheckCollisions(GameData.PlayerHitBox, characterDirection);
        if (collisions.Contains("x")) characterDirection.X = 0;
        if (collisions.Contains("y")) characterDirection.Y = 0;
        
        // Update whether the player is and was moving
        GameData.IsPlayerMoving = characterDirection != Vector2.Zero;

        // Update the player's position if the character is moving.
        if (characterDirection != Vector2.Zero)
        {
            characterDirection.Normalize();
            GameData.PlayerPosition += (characterDirection * GameData.PlayerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds).ToPoint();

            // Update the player's direction based on the movement direction
            if (Math.Abs(characterDirection.X) < Math.Abs(characterDirection.Y)) GameData.PlayerDirection = characterDirection.Y > 0 ? DirectionFrame.Down : DirectionFrame.Up;
            else GameData.PlayerDirection = characterDirection.X > 0 ? DirectionFrame.Right : DirectionFrame.Left;
        }
    }

    /// <summary>
    /// Handles player interactions based on keyboard and gamepad input, allowing the player to perform actions such as mining. The method checks for specific interaction keys and updates the player's inventory and particle effects accordingly.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    private void HandleInteraction(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.E) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.X))
        {
            switch (GameData.PlayerInteraction)
            {
                case "handMineIronOre":
                    _interactionTimer += gameTime.ElapsedGameTime.TotalSeconds;

                    if (_interactionTimer >= _miningSpeed)
                    {
                        // Add iron ore to inventory
                        if (!GameData.PlayerInventory.ContainsKey("ironOre")) GameData.PlayerInventory["ironOre"] = 0;
                        GameData.PlayerInventory["ironOre"]++;
                        _interactionTimer -= _miningSpeed;

                        // Emit mining particles
                        var particleDisplacement = GameData.PlayerDirection switch
                        {
                            DirectionFrame.Down => new Vector2(0, GameData.PlayerHitBox.Height * 0.75f),
                            DirectionFrame.Left => new Vector2(-GameData.PlayerHitBox.Width * 0.75f, GameData.PlayerHitBox.Height * 0.25f),
                            DirectionFrame.Right => new Vector2(GameData.PlayerHitBox.Width * 0.75f, GameData.PlayerHitBox.Height * 0.25f),
                            _ => Vector2.Zero
                        };
                        _miningParticles.PlaceParticles(GameData.PlayerHitBox.Center.ToVector2() + particleDisplacement);
                    }
                    break;
                case "openCraftingMenu":
                    _handleOpenCraftingMenu();
                    break;
                default:
                    _interactionTimer = 0;
                    break;
            }
        }
        else _interactionTimer = 0;
    }
}