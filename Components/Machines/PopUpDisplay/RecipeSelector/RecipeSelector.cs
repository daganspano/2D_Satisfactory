using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace _2D_Satisfactory.Components.Machines.PopUpDisplay;

public class RecipeSelector : DrawableGameComponent
{
    #region Private Fields

    private SpriteBatch _spriteBatch;

    // Background Fields
    private Texture2D _pixel;
    private Rectangle _backgroundBounds;

    // Recipe Items Fields
    private int _selectedRecipeIndex;
    private int _hoveredRecipeIndex;
    private List<RecipeItem> _recipeItems;
    private SetSelectedRecipeDelegate _setSelectedRecipe;

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the RecipeSelector component.
    /// </summary>
    /// <param name="game">The game instance.</param>
    /// <param name="backgroundBounds">The bounds of the background rectangle.</param>
    /// <param name="recipes">The list of recipes to display.</param>
    /// <param name="setSelectedRecipe">The delegate to set the selected recipe.</param>
    public RecipeSelector(
        Game game, 
        Rectangle backgroundBounds, 
        List<Recipe> recipes, 
        SetSelectedRecipeDelegate setSelectedRecipe
    ) : base(game)
    {
        _backgroundBounds = backgroundBounds;
        _selectedRecipeIndex = 0;
        _hoveredRecipeIndex = -1;

        InitializeRecipeItems(recipes);
        _setSelectedRecipe = setSelectedRecipe;
    }

    #endregion

    #region Override Methods
    
    /// <summary>
    /// Loads the content for the RecipeSelector component.
    /// </summary>
    protected override void LoadContent()
    {
        // Load Pixel
        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);

        // Load SpriteBatch
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        
        // Load content for each RecipeItem
        foreach (var recipeItem in _recipeItems) recipeItem.LoadContent();
    }

    /// <summary>
    /// Updates the RecipeSelector component, handling mouse interactions and updating recipe items accordingly.
    /// </summary>
    /// <param name="gameTime">The current game time.</param>
    public override void Update(GameTime gameTime)
    {
        MouseState mouseState = Mouse.GetState();
        _hoveredRecipeIndex = -1;
        for (int i = 0; i < _recipeItems.Count; i++)
        {
            if (_recipeItems[i].Hitbox.Contains(mouseState.Position))
            {
                if (mouseState.LeftButton == ButtonState.Pressed) 
                {
                    _selectedRecipeIndex = i;
                    _setSelectedRecipe(i);
                }
                else _hoveredRecipeIndex = i;
            }
        }
        for (int i = 0; i < _recipeItems.Count; i++)
        {
            RecipeInteraction interactionType = i == _selectedRecipeIndex ? RecipeInteraction.Select : 
                (i == _hoveredRecipeIndex ? RecipeInteraction.Hover : RecipeInteraction.None);
            _recipeItems[i].Update(interactionType);
        }
    }

    /// <summary>
    /// Draws the RecipeSelector component, including the background and all recipe items.
    /// </summary>
    /// <param name="gameTime">The current game time.</param>
    public override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin();

        // Draw Background
        _spriteBatch.Draw(_pixel, _backgroundBounds, Color.Black * 0.8f);

        // Draw Recipes
        foreach (var recipeItem in _recipeItems) recipeItem.Draw(_spriteBatch);

        _spriteBatch.End();
    }

    #endregion

    #region Private Methods
    
    /// <summary>
    /// Initializes the list of RecipeItem objects based on the provided recipes and background bounds.
    /// </summary>
    /// <param name="recipes">The list of recipes to create RecipeItem objects for.</param>
    private void InitializeRecipeItems(List<Recipe> recipes)
    {
        Point location = new(_backgroundBounds.X + 4, _backgroundBounds.Y + 4);
        Point size = new(_backgroundBounds.Width - 8, 40);
        Rectangle itemBounds = new Rectangle(location, size);

        _recipeItems = new List<RecipeItem>();
        foreach (var recipe in recipes)
        {
            RecipeItem newItem = new RecipeItem(Game, recipe, itemBounds);
            _recipeItems.Add(newItem);
            itemBounds.Y += 44;
        }
    }

    #endregion
}