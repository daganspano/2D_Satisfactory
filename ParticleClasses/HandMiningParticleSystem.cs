using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2D_Satisfactory.ParticleClasses;

/// <summary>
/// A particle system for hand mining effects, generating particles that simulate debris or dust when a player mines resources by hand.
/// </summary>
public class HandMiningParticleSystem : ParticleSystem
{
    
    public HandMiningParticleSystem(Game game, int maxParticles): base(game,  maxParticles) {}

    /// <summary>
    /// Initializes the constants for the hand mining particle system, including texture, particle count, blend state, and draw order.
    /// </summary>
    protected override void InitializeConstants()
    {
        textureFilename = "particle";

        minNumParticles = 5;
        maxNumParticles = 10;

        blendState = BlendState.AlphaBlend;
        DrawOrder = 200;
    }

    /// <summary>
    /// Initializes a single particle with random properties such as velocity, lifetime, color, rotation, angular velocity, and scale. The particle is positioned at the specified location.
    /// </summary>
    /// <param name="p">The particle to initialize.</param>
    /// <param name="where">The position where the particle should be placed.</param>
    protected override void InitializeParticle(ref Particle p, Vector2 where)
    {
        Random random = new Random();

        float angle = (float)random.NextDouble() * MathHelper.TwoPi;
        var velocity = new Vector2(MathF.Cos(angle), MathF.Sin(angle)) * ((float)random.NextDouble() * 60 + 10);

        var lifetime = (float)random.NextDouble() * 0.25f + 0.25f;

        var acceleration = -velocity / lifetime;

        var colors = new Color[] { new Color(148, 97, 72), new Color(155, 147, 142) };
        var color = colors[random.Next(colors.Length)];

        var rotation = (float)random.NextDouble() * MathHelper.TwoPi;

        var angularVelocity = (float)random.NextDouble() * MathHelper.PiOver2 - MathHelper.PiOver4;

        var scale = (float)random.NextDouble() * 0.2f + 0.2f;

        p.Initialize(where, velocity, acceleration, color, lifetime: lifetime, rotation: rotation, angularVelocity: angularVelocity, scale: scale);
    }
    
    /// <summary>
    /// Places particles at the specified position by calling AddParticle, which adds a new particle to the system at that location. This method can be used to trigger particle effects when a player mines resources by hand.
    /// </summary>
    /// <param name="position">The position where the particles should be placed.</param>
    public void PlaceParticles(Vector2 position)
    {
        AddParticle(position);
    }
}