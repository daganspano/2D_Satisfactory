using System;
using System.Collections.Generic;
using _2D_Satisfactory.Components.CharacterClasses;
using _2D_Satisfactory.Screens.MainGame;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;

namespace _2D_Satisfactory.Screens.Title;

public class TitleCharacter : Character
{
    #region Private Fields

    private double _directionTimer;
    private readonly double _directionChangeInterval;
    private Vector2 _direction;

    #endregion

    #region Public Properties

    public Rectangle HitBox => new Rectangle(Position.ToPoint(), GameData.SpriteSize);

    #endregion

    #region Constructor

    public TitleCharacter(Game game) : base(game, GetCharacter())
    {
        _directionTimer = 2;
        _directionChangeInterval = 3.0;

        // Parent Fields
        Random random = new Random();
        Position = new Vector2(
            random.Next(0, (int)GameData.ScreenSize.X - GameData.PlayerHitBox.Width),
            random.Next(0, (int)GameData.ScreenSize.Y - GameData.PlayerHitBox.Height)
        );
        IsMoving = true;
        WasMoving = true;
        Speed = random.Next(50, 201);
    }

    #endregion

    #region Public Methods

    public void Bounce(Vector2 overlap)
    {
        _direction = overlap;

        if (_direction != Vector2.Zero)
            _direction.Normalize();
    }

    #endregion

    #region Override Methods

    public override void Update(GameTime gameTime)
    {
        UpdateMovement(gameTime);

        base.Update(gameTime);
    }

    #endregion
    
    #region Private Methods
    
    private static int GetCharacter() => new Random().Next(0, 16);

    private void UpdateMovement(GameTime gameTime)
    {
        // Update the frame timers
        _directionTimer += gameTime.ElapsedGameTime.TotalSeconds;

            
        // Change direction
        if (_directionTimer > _directionChangeInterval)
        {
            double angle = new Random().NextDouble() * Math.PI * 2.0;
            _direction.X = (float)Math.Cos(angle);
            _direction.Y = (float)Math.Sin(angle);
            _directionTimer -= _directionChangeInterval;
        }

        // Bounce off edges of the screen
        List<string> collisions = CollisionHandler.CheckEdgeCollisions(HitBox, _direction);
        if (collisions.Contains("x")) _direction.X = -_direction.X;
        if (collisions.Contains("y")) _direction.Y = -_direction.Y;

        Position += _direction * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (Math.Abs(_direction.X) < Math.Abs(_direction.Y)) DirectionFrame = _direction.Y > 0 ? DirectionFrame.Down : DirectionFrame.Up;
        else DirectionFrame = _direction.X > 0 ? DirectionFrame.Right : DirectionFrame.Left;
    }

    #endregion
}