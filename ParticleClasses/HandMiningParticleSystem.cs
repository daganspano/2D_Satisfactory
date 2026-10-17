using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2D_Satisfactory.ParticleClasses;

public class HandMiningParticleSystem : ParticleSystem
{
    
    public HandMiningParticleSystem(Game game, int maxParticles): base(game,  maxParticles) {}

    protected override void InitializeConstants()
    {
        textureFilename = "particle";

        minNumParticles = 5;
        maxNumParticles = 10;

        blendState = BlendState.AlphaBlend;
        DrawOrder = 200;
    }

    protected override void InitializeParticle(ref Particle p, Vector2 where)
    {
        Random random = new Random();

        float angle = (float)random.NextDouble() * MathHelper.TwoPi;
        var velocity = new Vector2(MathF.Cos(angle), MathF.Sin(angle)) * ((float)random.NextDouble() * 100 + 20);
        // var velocity = Vector2.Zero;

        var lifetime = (float)random.NextDouble() * 0.25f + 0.25f;

        var acceleration = -velocity / lifetime;

        var colors = new Color[] { new Color(148, 97, 72), new Color(155, 147, 142) };
        var color = colors[random.Next(colors.Length)];

        var rotation = (float)random.NextDouble() * MathHelper.TwoPi;

        var angularVelocity = (float)random.NextDouble() * MathHelper.PiOver2 - MathHelper.PiOver4;

        var scale = (float)random.NextDouble() * 0.3f + 0.3f;

        p.Initialize(where, velocity, acceleration, color, lifetime: lifetime, rotation: rotation, angularVelocity: angularVelocity, scale: scale);
    }
    
    public void PlaceParticles(Vector2 position)
    {
        AddParticle(position);
    }
}