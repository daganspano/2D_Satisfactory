using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace _2D_Satisfactory.ParticleClasses;

/// <summary>
/// An abstract base class for particle systems, providing functionality to manage and render a collection of particles with properties such as position, velocity, acceleration, color, rotation, scale, and lifetime. Subclasses can define specific particle behaviors and appearances by overriding methods for initialization and updating.
/// </summary>
public abstract class ParticleSystem : DrawableGameComponent
{
    #region Fields

    /// <summary>
    /// A SpriteBatch to share amongst the various particle systems
    /// </summary>
    protected static SpriteBatch spriteBatch;

    /// <summary>
    /// A ContentManager to share amongst the various particle systems
    /// </summary>
    protected static ContentManager contentManager;

    /// <summary>
    /// The collection of particles 
    /// </summary>
    private Particle[] particles;

    /// <summary>
    /// A Queue containing indices of unused particles in the Particles array
    /// </summary>
    private Queue<int> freeParticles;

    /// <summary>
    /// The texture this particle system uses 
    /// </summary>
    private Texture2D texture;

    /// <summary>
    /// The origin when we're drawing textures 
    /// </summary>
    private Vector2 origin;

    /// <summary>
    /// The BlendState to use with this particle system
    /// </summary>
    protected BlendState blendState = BlendState.AlphaBlend;

    /// <summary>
    /// The filename of the texture to use for the particles
    /// </summary>
    protected string textureFilename;

    /// <summary>The minimum number of particles to add when AddParticles() is called</summary>
    protected int minNumParticles;

    /// <summary>The maximum number of particles to add when AddParticles() is called</summary>
    protected int maxNumParticles;

    #endregion

    #region Constructor

    /// <summary>
    /// Constructs a new instance of a particle system
    /// </summary>
    /// <param name="game">The game this particle system belongs to</param>
    /// <param name="maxParticles">The maximum number of particles this system can handle</param>
    public ParticleSystem(Game game, int maxParticles) : base(game) 
    {
        // Create our particles
        particles = new Particle[maxParticles];
        freeParticles = new Queue<int>(maxParticles);
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].Initialize(Vector2.Zero);
            freeParticles.Enqueue(i);
        }
        // Run the InitializeConstants hook
        InitializeConstants();
    }

    #endregion

    #region Virtual Methods

    /// <summary>
    /// Used to do the initial configuration of the particle engine.  The 
    /// protected constants `textureFilename`, `minNumParticles`, and `maxNumParticles`
    /// should be set in the override.
    /// </summary>
    protected abstract void InitializeConstants();

    /// <summary>
    /// InitializeParticle randomizes some properties for a particle, then
    /// calls initialize on it. It can be overridden by subclasses if they 
    /// want to modify the way particles are created.
    /// </summary>
    /// <param name="p">the particle to initialize</param>
    /// <param name="where">the position on the screen that the particle should be
    /// </param>
    protected virtual void InitializeParticle(ref Particle p, Vector2 where)
    {
        // Initialize the particle with default values
        p.Initialize(where);
    }

    /// <summary>
    /// UpdateParticle is called by the Update method on every particle that is currently active. It updates the particle's position, velocity, rotation, and other properties based on the elapsed time (dt). Subclasses can override this method to implement custom behavior for particles.
    /// </summary>
    /// <param name="particle">The particle to update.</param>
    /// <param name="dt">The elapsed time since the last update, in seconds.</param>
    protected virtual void UpdateParticle(ref Particle particle, float dt)
    {
        // Update particle's linear motion values
        particle.Velocity += particle.Acceleration * dt;
        particle.Position += particle.Velocity * dt;

        // Update the particle's angular motion values
        particle.AngularVelocity += particle.AngularAcceleration * dt;
        particle.Rotation += particle.AngularVelocity * dt;

        // Update the time the particle has been alive 
        particle.TimeSinceStart += dt;
    }

    #endregion

    #region Override Methods

    /// <summary>
    /// Loads the content for the particle system, including the texture specified by the `textureFilename` property. This method also initializes the shared static `ContentManager` and `SpriteBatch` if they haven't been created yet. Subclasses must ensure that `textureFilename` is set before calling this method, or an `InvalidOperationException` will be thrown.
    /// </summary>
    protected override void LoadContent()
    {
        // create the shared static ContentManager and SpriteBatch,
        // if this hasn't already been done by another particle engine
        if (contentManager == null) contentManager = new ContentManager(Game.Services, "Content");
        if (spriteBatch == null) spriteBatch = new SpriteBatch(Game.GraphicsDevice);

        // make sure sub classes properly set textureFilename.
        if (string.IsNullOrEmpty(textureFilename))
        {
            string message = "textureFilename wasn't set properly, so the " +
                "particle system doesn't know what texture to load. Make " +
                "sure your particle system's InitializeConstants function " +
                "properly sets textureFilename.";
            throw new InvalidOperationException(message);
        }
        // load the texture....
        texture = contentManager.Load<Texture2D>(textureFilename);

        // ... and calculate the center. this'll be used in the draw call, we
        // always want to rotate and scale around this point.
        origin.X = texture.Width / 2;
        origin.Y = texture.Height / 2;

        base.LoadContent();
    }

    /// <summary>
    /// Updates the particle system by iterating through all particles, updating their properties based on elapsed time, and managing the queue of free particles. This method is called automatically by the game loop and should not be called directly. Subclasses can override this method to implement custom update behavior for the particle system.
    /// </summary>
    /// <param name="gameTime">The current game time.</param>
    public override void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        for(int i = 0; i < particles.Length; i++)
        {

            if (particles[i].Active)
            {
                UpdateParticle(ref particles[i], dt);

                if (!particles[i].Active)
                {
                    freeParticles.Enqueue(i);
                }
            }
        }

        base.Update(gameTime);
    }

    /// <summary>
    /// Draws the active particles in the particle system using the specified SpriteBatch and BlendState. This method is called automatically by the game loop and should not be called directly. Subclasses can override this method to implement custom drawing behavior for the particle system.
    /// </summary>
    /// <param name="gameTime">The current game time.</param>
    public override void Draw(GameTime gameTime)
    {
        spriteBatch.Begin(blendState: blendState);

        foreach (Particle p in particles)
        {
            if (p.Active) spriteBatch.Draw(texture, p.Position, null, p.Color, p.Rotation, origin, p.Scale, SpriteEffects.None, 0.0f);
        }

        spriteBatch.End();

        base.Draw(gameTime);
    }

    #endregion

    #region Add Particle Method

    /// <summary>
    /// Adds a random number of particles to the particle system at the specified position. The number of particles added is determined by generating a random integer between the `minNumParticles` and `maxNumParticles` properties. Each new particle is initialized using the `InitializeParticle` method, which can be overridden by subclasses to customize particle behavior. This method is typically called when an event occurs that should trigger particle effects, such as an explosion or collision.
    /// </summary>
    /// <param name="position">The position where the particles should be added.</param>
    protected void AddParticle(Vector2 position)
    {
        // the number of particles we want for this effect is a random number
        // somewhere between the two constants specified by the subclasses.
        int numParticles =
            new Random().Next(minNumParticles, maxNumParticles);

        // create that many particles, if you can.
        for (int i = 0; i < numParticles && freeParticles.Count > 0; i++)
        {
            // grab a particle from the freeParticles queue, and Initialize it.
            int index = freeParticles.Dequeue();
            InitializeParticle(ref particles[index], position);
        }
    }

    #endregion
}