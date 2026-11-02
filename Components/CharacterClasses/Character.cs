using _2D_Satisfactory.Screens.MainGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace _2D_Satisfactory.Components.CharacterClasses;

public abstract class Character : DrawableGameComponent
{
    #region Private Fields

    private SpriteBatch _spriteBatch;
    private Texture2D _texture;
    private Rectangle _sourceRectangle;
    private readonly Point _character;
    private int _speed;
    private AnimationFrame _animationFrame;
    private DirectionFrame _directionFrame;
    private AnimationFrame _previousAnimationFrame;
    private double _animationTimer;
    private double _animationSpeed;

    #endregion

    #region Protected Fields

    protected Vector2 Position { get; set;}

    protected DirectionFrame DirectionFrame
    {
        get => _directionFrame;
        set => _directionFrame = value;
    }

    protected bool IsMoving { get; set; }
    protected bool WasMoving { get; set; }

    public int Speed
    {
        get => _speed;
        set 
        {
            _speed = value;
            _animationSpeed = 0.2 * 100 / 140 * Math.Sqrt(140.0 / _speed);
        }
    }

    #endregion

    #region Constructor

    protected Character(Game game, int character) : base(game)
    {
        _sourceRectangle = new Rectangle(Point.Zero, GameData.PlayerHitBox.Size);
        _character = new Point(character % 4, character / 4);
        _animationFrame = AnimationFrame.Stationary;
        _directionFrame = DirectionFrame.Down;
        _animationTimer = 0;
        _animationSpeed = 0.2 * 100 / 140 * Math.Sqrt(140.0 / _speed);
    }

    #endregion

    #region Override Methods

    /// <summary>
    /// Loads the content required for rendering the character, including the sprite batch, texture, and sound effect. This method initializes the necessary resources for drawing the character on the screen and playing walking sounds.
    /// </summary>
    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
        _texture = Game.Content.Load<Texture2D>("construction_workers");
    }

    public override void Update(GameTime gameTime)
    {
        // Update frame for the animation and direction based on the current direction vector
        UpdateFrame(gameTime);

        // Update the source rectangle based on the current frame and character
        UpdateSourceRectangle();
    }

    /// <summary>
    /// Draws the character on the screen using the internal SpriteBatch. This method is called during the game's draw phase and handles rendering the character sprite at its current position, applying the appropriate source rectangle for the character's frame and orientation. The draw offset is applied to ensure the character is rendered correctly relative to the game world.
    /// </summary>
    /// <param name="gameTime">The current game time.</param>
    public override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin();

        // HitBox
        var pixel = new Texture2D(Game.GraphicsDevice, 1, 1);
        pixel.SetData([ Color.White ]);
        _spriteBatch.Draw(pixel, new Rectangle(Position.ToPoint(), GameData.PlayerHitBox.Size), new Color(255, 0, 0, 50));

        // Character
        _spriteBatch.Draw(_texture, Position, _sourceRectangle, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

        _spriteBatch.End();
    }

    #endregion
    
    #region Private Methods
    
    private void UpdateFrame(GameTime gameTime)
    {
        // Update the animation timer
        _animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

        if (!IsMoving)
        {
            _animationFrame = AnimationFrame.Stationary;
            _animationTimer = 0;
            return;
        }

        // Start the running animation immediately when the character starts moving
        if (!WasMoving) _animationTimer = _animationSpeed;

        // Update the animation
        if (_animationTimer > _animationSpeed)
        {
            switch (_animationFrame)
            {
                case AnimationFrame.Running1:
                case AnimationFrame.Running2:
                    _animationFrame = AnimationFrame.Stationary;
                    break;
                case AnimationFrame.Stationary:
                    _animationFrame = _previousAnimationFrame == AnimationFrame.Running1 ? AnimationFrame.Running2 : AnimationFrame.Running1;
                    _previousAnimationFrame = _animationFrame;
                    break;
            }
            _animationTimer -= _animationSpeed;
        }
    }

    /// <summary>
    /// Updates the source rectangle of the running sprite based on the current frame, character, and texture properties.
    /// </summary>
    private void UpdateSourceRectangle()
    {
        Point start = new Point(1, 2);
        Point character = new Point(126, 104) * _character;
        Point frame = new Point(18 * (int)_animationFrame, 26 * (int)_directionFrame);

        _sourceRectangle.Location = start + character + frame;
    }
    
    #endregion
}