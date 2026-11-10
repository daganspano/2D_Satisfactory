using Microsoft.Xna.Framework;

namespace _2D_Satisfactory.Screens;

/// <summary>
/// Represents a screen in the game, managing its state and components.
/// </summary>
public abstract class Screen
{
    private GameComponentCollection _components = new GameComponentCollection();

    /// <summary>
    /// Gets the collection of components associated with the screen.
    /// </summary>
    protected GameComponentCollection Components => _components;

    /// <summary>
    /// Gets or sets the current state of the screen.
    /// </summary>
    public virtual ScreenState ScreenState { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Screen"/> class with the specified initial state.
    /// </summary>
    /// <param name="initialState">The initial state of the screen.</param>
    public Screen(ScreenState initialState)
    {
        ScreenState = initialState;
    }

    /// <summary>
    /// Loads the content for the screen and its components. This method should be called when the screen is first displayed or when its content needs to be refreshed.
    /// </summary>
    public virtual void LoadContent()
    {
        foreach (var component in _components)
        {
            component.Initialize();
        }
    }

    /// <summary>
    /// Updates the screen and its components based on the elapsed game time. This method should be called in the game's update loop to handle input, animations, and other dynamic behavior.
    /// </summary>
    /// <param name="gameTime">The elapsed game time since the last update.</param>
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

    /// <summary>
    /// Draws the screen and its components. This method should be called in the game's draw loop to render the visual elements of the screen.
    /// </summary>
    /// <param name="gameTime">The elapsed game time since the last draw.</param>
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
