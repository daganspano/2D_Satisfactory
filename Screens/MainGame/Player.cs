using System;
using _2D_Satisfactory.Components.CharacterClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace _2D_Satisfactory.Screens.MainGame;

public class Player : Character
{

    #region Private Fields

    private SoundEffect _walkingSoundEffect;
    private double _soundTimer;
    private double _soundSpeed;

    #endregion

    #region Constructor

    public Player(Game game) : base(game, GameData.Character)
    {
        _soundTimer = 0;
        _soundSpeed = 0.4 * 100 / 140 * Math.Sqrt(140.0 / GameData.PlayerSpeed);

        // Parent Fields
        Position = (GameData.ScreenSize - GameData.PlayerHitBox.Size.ToVector2()) / 2;
        DirectionFrame = GameData.PlayerDirection;
        IsMoving = false;
        WasMoving = false;
        Speed = GameData.PlayerSpeed;
    }

    #endregion

    #region Override Methods

    protected override void LoadContent()
    {
        base.LoadContent();

        // Load the walking sound effect
        _walkingSoundEffect = Game.Content.Load<SoundEffect>("grassFootSteps");
    }

    public override void Update(GameTime gameTime)
    {
        DirectionFrame = GameData.PlayerDirection;
        WasMoving = IsMoving;
        IsMoving = GameData.IsPlayerMoving;

        base.Update(gameTime);
            
        // Play walking sound effect
        PlayWalkingSound(gameTime);
    }

    #endregion
    
    #region Private Methods
    
    private void PlayWalkingSound(GameTime gameTime)
    {   
        // Update the sound timer
        _soundTimer += gameTime.ElapsedGameTime.TotalSeconds;

        if (!IsMoving)
        {
            _soundTimer = 0;
            return;
        }

        // Start the walking sound immediately when the character starts moving
        if (!WasMoving) _soundTimer = _soundSpeed;

        // Play the sound effect
        if (_soundTimer > _soundSpeed)
        {
            _walkingSoundEffect.Play(volume: 0.25f, pitch: -0.125f, pan: 0f);
            _soundTimer -= _soundSpeed;
        }
    }

    #endregion
}