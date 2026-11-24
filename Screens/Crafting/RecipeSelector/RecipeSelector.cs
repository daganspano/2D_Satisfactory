using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _2D_Satisfactory.Screens.Crafting;

public class RecipeSelector : DrawableGameComponent
{
    private SpriteBatch _spriteBatch;
    private Texture2D _pixel;
    private Rectangle _backgroundBounds;
    private int _selectedRecipeIndex;
    private int _hoveredRecipeIndex;
    private List<Recipe> _recipes;
    private List<RecipeItem> _recipeItems;
    private SetSelectedRecipeDelegate _setSelectedRecipe;
    public RecipeSelector(Game game, Rectangle backgroundBounds, List<Recipe> recipes, SetSelectedRecipeDelegate setSelectedRecipe) : base(game)
    {
        _backgroundBounds = backgroundBounds;
        _selectedRecipeIndex = 0;
        _hoveredRecipeIndex = -1;
        _recipes = recipes;

        Rectangle itemBounds = new Rectangle(_backgroundBounds.X + 4, _backgroundBounds.Y + 4, _backgroundBounds.Width - 8, 40);
        _recipeItems = new List<RecipeItem>();
        foreach (var recipe in _recipes)
        {
            RecipeItem newItem = new RecipeItem(Game, recipe, itemBounds);
            _recipeItems.Add(newItem);
            itemBounds.Y += 44;
        }
        _setSelectedRecipe = setSelectedRecipe;
    }

    protected override void LoadContent()
    {
        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);

        _spriteBatch = new SpriteBatch(GraphicsDevice);
        
        // Load content for each RecipeItem
        foreach (var recipeItem in _recipeItems) recipeItem.LoadContent();
    }

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

    public override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin();

        // Draw Background
        _spriteBatch.Draw(_pixel, _backgroundBounds, Color.Black * 0.8f);

        // Draw Recipes
        foreach (var recipeItem in _recipeItems) recipeItem.Draw(_spriteBatch);

        _spriteBatch.End();
    }
}