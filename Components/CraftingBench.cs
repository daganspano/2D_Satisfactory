using System;
using _2D_Satisfactory.Screens.MainGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2D_Satisfactory.Components;

/// <summary>
/// Represents a crafting bench in the game, allowing players to interact with it for crafting purposes.
/// </summary>
public class CraftingBench : DrawableGameComponent
{
    private SpriteBatch _spriteBatch;
    private Texture2D _pixel;
    private Rectangle _hitBox;
    private Rectangle _interactionBounds = new Rectangle(42 * 16, 14 * 16, 4 * 16, 5 * 16);

    /// <summary>
    /// Gets the hit box of the crafting bench, which defines the area it occupies in the game world. This property is used for collision detection and interaction with the player character.
    /// </summary>
    public Rectangle HitBox => _hitBox;

    /// <summary>
    /// Initializes a new instance of the <see cref="CraftingBench"/> class with the specified game, location, and orientation. The constructor sets up the hit box and interaction bounds based on the provided parameters, ensuring that the crafting bench is correctly positioned and oriented in the game world.
    /// </summary>
    /// <param name="game">The game instance that this crafting bench belongs to.</param>
    /// <param name="location">The location of the crafting bench in the game world, specified as a point.</param>
    /// <param name="orientation">The orientation of the crafting bench, either "horizontal" or "vertical".</param>
    /// <exception cref="ArgumentException">Thrown if the orientation is not "horizontal" or "vertical".</exception>
    public CraftingBench(Game game, Point location, string orientation) : base(game)
    {
        if (orientation != "horizontal" && orientation != "vertical")
        {
            throw new ArgumentException("Orientation must be either 'horizontal' or 'vertical'.");
        }

        int x = location.X * 16;
        int y = location.Y * 16;
        int width = orientation == "horizontal" ? 48 : 32;
        int height = orientation == "horizontal" ? 32 : 48;

        _hitBox = new Rectangle(x, y, width, height);
        _interactionBounds = new Rectangle(x - 16, y - 16, width + 32, height + 32);
    }

    /// <summary>
    /// Loads the content required for the crafting bench, including initializing the sprite batch and creating a pixel texture for rendering. This method is called when the game component is added to the game and is responsible for setting up the necessary graphics resources.
    /// </summary>
    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
        _pixel = new Texture2D(Game.GraphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);
    }

    /// <summary>
    /// Updates the state of the crafting bench, checking for player interaction. If the player's hit box intersects with the interaction bounds of the crafting bench, it sets the player's interaction state to "openCraftingMenu". This method is called during the game's update phase and handles the logic for detecting player interactions with the crafting bench.
    /// </summary>
    /// <param name="gameTime">The current game time.</param>
    public override void Update(GameTime gameTime)
    {
        if (GameData.PlayerHitBox.Intersects(_interactionBounds))
            GameData.PlayerInteraction = "openCraftingMenu";
    }

    /// <summary>
    /// Draws the crafting bench on the screen, including its hit box and interaction bounds for debugging purposes. This method is called during the game's draw phase and handles rendering the visual representation of the crafting bench, as well as highlighting its hit box and interaction area to assist with development and testing.
    /// </summary>
    /// <param name="gameTime">The current game time.</param>
    public override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin(transformMatrix: GameData.TransformMatrix);
        _spriteBatch.Draw(_pixel, _hitBox, Color.Red);
        _spriteBatch.Draw(_pixel, _interactionBounds, Color.Blue * 0.4f);
        _spriteBatch.End();
    }
}