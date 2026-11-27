using _2D_Satisfactory.Screens.MainGame;
using _2D_Satisfactory.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace _2D_Satisfactory.Components.Machines.PopUpDisplay;

public delegate void SetSelectedRecipeDelegate(int recipeIndex);
public delegate (List<Item> inputItems, List<Item> outputItems, float progress) GetCraftingInfoDelegate();

public abstract class MachineScreen : Screen
{
    #region Private Fields

    private Game _game;
    private SpriteBatch _spriteBatch;
    private Texture2D _pixel;
    private Rectangle _backgroundBounds;
    private Rectangle _recipeSelectorBounds;

    #endregion

    #region Constructor

    public MachineScreen(Game game) : base(ScreenState.Inactive)
    {
        _game = game;

        InitializeBounds();
    }

    #endregion

    #region Protected Methods

    protected void setComponents()
    {
        // Initialize Recipe Selector
        RecipeSelector recipeSelector = new(
            _game,
            _recipeSelectorBounds,
            recipes: GetRecipes(),
            setSelectedRecipe: GetSetSelectedRecipe()
        );

        // Initialize Craft Display
        CraftDisplay craftDisplay = new(_game, getCraftingInfo: GetGetCraftingInfo());

        // Initialize Bottom Display
        DrawableGameComponent bottomDisplay = GetBottomDisplay();
        
        // Initialize Inventory Display


        // Add components to the screen
        Components.Add(recipeSelector);
        Components.Add(craftDisplay);
        if (bottomDisplay != null) Components.Add(bottomDisplay);
        /** Add Inventory Display */
    }

    #endregion

    #region Abstract Methods

    protected abstract List<Recipe> GetRecipes();

    protected abstract SetSelectedRecipeDelegate GetSetSelectedRecipe();

    protected abstract GetCraftingInfoDelegate GetGetCraftingInfo();

    #endregion

    #region Virtual Methods

    protected virtual DrawableGameComponent GetBottomDisplay()
    {
        return null;
    }

    #endregion

    #region Override Methods

    public override void LoadContent()
    {
        // Initialize the sprite batch and pixel texture for drawing
        _spriteBatch = new SpriteBatch(_game.GraphicsDevice);

        // Create a 1x1 white pixel texture for drawing rectangles
        _pixel = new Texture2D(_game.GraphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);

        // Set Components
        setComponents();

        base.LoadContent();
    }
    
    public override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin();

        // Draw Background
        _spriteBatch.Draw(_pixel, _backgroundBounds, Color.Black * 0.4f);

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    #endregion

    #region Private Methods

    private void InitializeBounds()
    {
        // Background Bounds
        Point size = new(1000, 400);
        Point location = new(
            ((int)GameData.ScreenSize.X - size.X) / 2, 
            ((int)GameData.ScreenSize.Y - size.Y) / 2
        );
        _backgroundBounds = new Rectangle(location, size);

        // Recipe Selector Bounds
        location += new Point(8, 8);
        size.X = 300;
        size.Y -= 16;
        _recipeSelectorBounds = new Rectangle(location, size);
    }

    #endregion
}