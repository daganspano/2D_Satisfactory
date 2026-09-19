namespace _2D_Satisfactory;

/// <summary>
/// Represents the dimensions of the game window, providing properties to get and set the width and height of the game.
/// </summary>
public static class GameDimensions
{
    private static float _width;
    private static float _height;

    /// <summary>
    /// The width of the game window.
    /// </summary>
    public static float Width => _width;

    /// <summary>
    /// The height of the game window.
    /// </summary>
    public static float Height => _height;

    /// <summary>
    /// The X dimension of the game window, representing the width.
    /// </summary>
    public static float X => _width;

    /// <summary>
    /// The Y dimension of the game window, representing the height.
    /// </summary>
    public static float Y => _height;

    /// <summary>
    /// Sets the dimensions of the game window by specifying the width and height. This method allows for updating both dimensions simultaneously.
    /// </summary>
    /// <param name="width">The width of the game window.</param>
    /// <param name="height">The height of the game window.</param>
    public static void Set(float width, float height)
    {
        _width = width;
        _height = height;
    }
}