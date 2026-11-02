using Microsoft.Xna.Framework;

namespace _2D_Satisfactory.Screens;

public abstract class Screen
{
    public ScreenState ScreenState { get; set; }

    private GameComponentCollection _components = new GameComponentCollection();

    protected GameComponentCollection Components => _components;

    public Screen(ScreenState initialState)
    {
        ScreenState = initialState;
    }

    public virtual void LoadContent()
    {
        foreach (var component in _components)
        {
            component.Initialize();
        }
    }

    public virtual void Update(GameTime gameTime)
    {
        foreach (var component in _components)
        {
            if (component is IUpdateable)
            {
                ((IUpdateable)component).Update(gameTime);
            }
        }
    }

    public virtual void Draw(GameTime gameTime)
    {
        foreach (var component in _components)
        {
            if (component is IDrawable)
            {
                ((IDrawable)component).Draw(gameTime);
            }
        }
    }
}
