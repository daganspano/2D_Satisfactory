using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2D_Satisfactory.Screens.Crafting;

public class RecipeSelector : DrawableGameComponent
{
    private SpriteBatch _spriteBatch;
    private Texture2D _pixel;
    private Rectangle _backgroundBounds;
    private List<Recipe> _recipes;
    private List<RecipeItem> _recipeItems;

    public RecipeSelector(Game game, Rectangle backgroundBounds) : base(game)
    {
        _backgroundBounds = backgroundBounds;
        _recipes = Data.GetRecipesForMachine("bench"); /////// Get Recipes from CraftHandler ////////////////


        var position = new Vector2(_backgroundBounds.X + 4, _backgroundBounds.Y + 4);
        _recipeItems = new List<RecipeItem>();
        foreach (var recipe in _recipes)
        {
            _recipeItems.Add(new RecipeItem(Game, recipe, position));
            position.Y += recipe.Height;
        }
    }

    protected override void LoadContent()
    {
        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);

        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    public override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin();

        // Draw Background
        _spriteBatch.Draw(_pixel, _backgroundBounds, Color.Gray * 0.8f);

        // Draw Recipes
        foreach (var recipeItem in _recipeItems)
        {
            recipeItem.Draw();
        }

        _spriteBatch.End();
    }
}