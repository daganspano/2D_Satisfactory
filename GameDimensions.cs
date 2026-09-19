namespace _2D_Satisfactory;

public static class GameDimensions
{
    private static float _width;
    private static float _height;

    public static float Width
    {
        get => _width;
        set => _width = value;
    }

    public static float Height
    {
        get => _height;
        set => _height = value;
    }

    public static float X
    {
        get => _width;
        set => _width = value;
    }

    public static float Y
    {
        get => _height;
        set => _height = value;
    }

    public static void Set(float width, float height)
    {
        _width = width;
        _height = height;
    }
}