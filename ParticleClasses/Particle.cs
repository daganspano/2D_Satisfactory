using Microsoft.Xna.Framework;

namespace _2D_Satisfactory.ParticleClasses;

/// <summary>
/// Represents a single particle in a particle system, encapsulating properties such as position, velocity, acceleration, color, rotation, scale, and lifetime. This struct is used to define the behavior and appearance of individual particles within a particle system.
/// </summary>
public struct Particle
{
    /// <summary>
    /// The current position of the particle. Default (0,0).
    /// </summary>
    public Vector2 Position;

    /// <summary>
    /// The current velocity of the particle. Default (0,0).
    /// </summary>
    public Vector2 Velocity;

    /// <summary>
    /// The current acceleration of the particle. Default (0,0).
    /// </summary>
    public Vector2 Acceleration;

    /// <summary>
    /// The current rotation of the particle. Default 0.
    /// </summary>
    public float Rotation;

    /// <summary>
    /// The current angular velocity of the particle. Default 0.
    /// </summary>
    public float AngularVelocity;

    /// <summary>
    /// The current angular acceleration of the particle. Default 0.
    /// </summary>
    public float AngularAcceleration;

    /// <summary>
    /// The current scale of the particle.  Default 1.
    /// </summary>
    public float Scale;

    /// <summary>
    /// The current color of the particle. Default White
    /// </summary>
    public Color Color;

    /// <summary>
    /// The current lifetime of the particle (how long it will "live").  Default 1s.
    /// </summary>
    public float Lifetime;

    /// <summary>
    /// How long this particle has been alive 
    /// </summary>
    public float TimeSinceStart;

    /// <summary>
    /// If this particle is still alive, and should be rendered
    /// </summary>
    public bool Active => TimeSinceStart < Lifetime;

    /// <summary>
    /// Sets the particle up for first use 
    /// </summary>
    public void Initialize(
        Vector2 position, 
        Vector2 velocity, 
        Vector2 acceleration, 
        Color color,
        float lifetime = 1, 
        float scale = 1, 
        float rotation = 0, 
        float angularVelocity = 0, 
        float angularAcceleration = 0
    )
    {
        Position = position;
        Velocity = velocity;
        Acceleration = acceleration;
        Rotation = rotation;
        AngularVelocity = angularVelocity;
        AngularAcceleration = angularAcceleration;
        Scale = scale;
        Color = color;
        Lifetime = lifetime;
        TimeSinceStart = 0f;
    }
    
    /// <summary>
    /// Sets the particle up for first use 
    /// </summary>
    public void Initialize(
        Vector2 position, 
        float lifetime = 1, 
        float scale = 1, 
        float rotation = 0, 
        float angularVelocity = 0, 
        float angularAcceleration = 0
    )
    {
        Initialize(position, Vector2.Zero, Vector2.Zero, Color.White, lifetime, scale, rotation, angularVelocity, angularAcceleration);
    }
}