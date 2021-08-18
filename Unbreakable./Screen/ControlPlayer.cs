using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Unbreakable
{
    public class ControlBat
    {
        Bat _bat1;
        Bat _bat2;
        bool _stopUpdating;
        public enum _states { Idle, Moving, Jumping, Falling, Shooting };
        private _states _currentState1;
        private _states _currentState2;
        private int _jumpHeight1 = 150;
        private int _jumpSpeed1 = 350;

        private int _jumpHeight2 = 150;
        private int _jumpSpeed2 = 350;

        private int _jumpFrom1;
        private float _CurrentJumpProgress1;
        private bool _StartJump1 = false;
        private bool _StartFall1 = false;
        private List<Ball> _Player1Bullets = new List<Ball>();
        private bool _P1CooldownOn = false;
        private bool _P2CooldownOn = false;
        private float _Player1CooldownTimer;
        private float _Player2CooldownTimer;
        private float _CooldownTime = 1f;
        private int _jumpFrom2;
        private float _CurrentJumpProgress2;
        private bool _StartJump2 = false;
        private bool _StartFall2 = false;
        private List<Ball> _Player2Bullets = new List<Ball>();

        private Texture2D _HealthPickupImage;
        private Texture2D _DamagePickupImage;
        private Texture2D _SpeedPickupImage;
        private Texture2D _JumpPickupImage;
        private Texture2D _Chain;

        private SpriteAnimation _walkAnim = new SpriteAnimation();
        private SpriteAnimation _shootAnim = new SpriteAnimation();
        private SpriteAnimation _idleAnim = new SpriteAnimation();
        private SpriteAnimation _jumpAnim = new SpriteAnimation();

        private SpriteAnimation _walkAnim2 = new SpriteAnimation();
        private SpriteAnimation _shootAnim2 = new SpriteAnimation();
        private SpriteAnimation _idleAnim2 = new SpriteAnimation();
        private SpriteAnimation _jumpAnim2 = new SpriteAnimation();

        private SoundEffect _fireBurst;
        private SoundEffect _fireGun;
        private SoundEffect _fireGun2;
        private SoundEffect _jumpSound;
        private SoundEffect _jumpSound2;
        private SoundEffect _jumpLand;
        private SoundEffect _jumpLand2;
        private SoundEffect _running;
        private SoundEffect _running2;
        private SoundEffect _hitImpact;
        private SoundEffect _hitImpact2;
        private SoundEffect _bgMusic1;
        private float _runningDuration = 0.5f;
        private float _runningSaveTime = 0.0f;
        private float _running2SaveTime = 0.0f;
        private float _bgMusic1Duration = 193.0f;
        private float _bgMusic1SaveTime = 0.0f;

        private bool _PickupOn = false;
        private bool _DamagePickUp1 = false;
        private bool _SpeedPickUp1 = false;
        private bool _JumpPickUp1 = false;

        private bool _DamagePickUp2 = false;
        private bool _SpeedPickUp2 = false;
        private bool _JumpPickUp2 = false;

        private int _damage1 = 10;
        private int _damage2 = 20;

        private float _PickupTimer = 0;
        private float _PickupResetTime = 3;
        public int _health1;
        public int _health2;

        public Pickups _CurrentPickup = new Pickups();

        public ControlBat(Bat bat1, Bat bat2)
        {
            _bat1 = bat1;
            _bat2 = bat2;
        }

        public bool StopUpdating
        {
            get { return _stopUpdating; }
            set { _stopUpdating = value; }
        }

        public _states CurrentStatePlayer1
        {
            get { return _currentState1; }
            set { _currentState1 = value; }
        }

        public _states CurrentStatePlayer2
        {
            get { return _currentState2; }
            set { _currentState2 = value; }
        }

        public void LoadContent(ContentManager content)
        {

            RandPicup();
            Texture2D image = content.Load<Texture2D>("Sprites/walkSprite");
            Vector2 position = Vector2.Zero;
            _walkAnim.LoadContent(content, image, "", position);
            image = content.Load<Texture2D>("Sprites/shootSprite");
            _shootAnim.LoadContent(content, image, "", position);

            image = content.Load<Texture2D>("Sprites/idleSprite");
            _idleAnim.LoadContent(content, image, "", position);
            _idleAnim.Frames = new Vector2(1, 1);

             image = content.Load<Texture2D>("Sprites/jumpSprite");
            _jumpAnim.LoadContent(content, image, "", position);
            _jumpAnim.Frames = new Vector2(1, 1);

            Texture2D image2 = content.Load<Texture2D>("Sprites/walkSprite2");
            Vector2 position2 = Vector2.Zero;
            _walkAnim2.LoadContent(content, image2, "", position2);
            image2 = content.Load<Texture2D>("Sprites/shootSprite2");
            _shootAnim2.LoadContent(content, image2, "", position2);

            
            _HealthPickupImage = content.Load<Texture2D>("Sprites/Red");
            _DamagePickupImage = content.Load<Texture2D>("Sprites/Purple");
            _SpeedPickupImage = content.Load<Texture2D>("Sprites/Green");
            _JumpPickupImage = content.Load<Texture2D>("Sprites/Blue");

            _Chain = content.Load<Texture2D>("Sprites/Fence");
            image2 = content.Load<Texture2D>("Sprites/idleAnim2");
            _idleAnim2.LoadContent(content, image2, "", position2);
            _idleAnim2.Frames = new Vector2(1, 1);

            image2 = content.Load<Texture2D>("Sprites/jumpAnim2");
            _jumpAnim2.LoadContent(content, image2, "", position2);
            _jumpAnim2.Frames = new Vector2(1, 1);

            _fireGun = content.Load<SoundEffect>("Audio/9mm");
            _fireGun2 = content.Load<SoundEffect>("Audio/HKMP540singleShot");
            _jumpSound = content.Load<SoundEffect>("Audio/jumpoff");
            _jumpSound2 = content.Load<SoundEffect>("Audio/jumpoff2");
            _jumpLand = content.Load<SoundEffect>("Audio/landthud");
            _jumpLand2 = content.Load<SoundEffect>("Audio/landthud2");
            _running = content.Load<SoundEffect>("Audio/running2");
            _running2 = content.Load<SoundEffect>("Audio/running1");
            _hitImpact = content.Load<SoundEffect>("Audio/doubleimpact");
            _hitImpact2 = content.Load<SoundEffect>("Audio/doubleimpact2");
            _bgMusic1 = content.Load<SoundEffect>("Audio/bgMusic1");

            _bat1.Speed = 350;
            _bat2.Speed = 350;
            _health1 = 100;
            _health2 = 100;
        }

        public void Update(ContentManager content, GameTime gameTime, InputManager inputManager, bool AI)
        {
            if (!_stopUpdating)
            {


                if (_SpeedPickUp1)
                {
                    _bat1.Speed = 1000;
                    _jumpSpeed1 = 1000;
                }
                else 
                {
                    _bat1.Speed = 350;
                }

                if (_DamagePickUp1)
                {
                    _damage1 = 20;
                }
                else 
                {
                    _damage1 = 10;
                }

                if (_JumpPickUp1)
                {
                    _jumpHeight1 = 300;
                }
                else 
                {
                    _jumpHeight1 = 150;
                }


                if (_SpeedPickUp2)
                {
                    _bat2.Speed = 1000;
                    _jumpSpeed2 = 1000;
                }
                else
                {
                    _bat2.Speed = 350;
                }

                if (_DamagePickUp2)
                {
                    _damage2 = 20;
                }
                else
                {
                    _damage2 = 10;
                }

                if (_JumpPickUp2)
                {
                    _jumpHeight2 = 300;
                }
                else
                {
                    _jumpHeight2 = 150;
                }



                if(!_PickupOn)
                {
                    _PickupTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if(_PickupTimer >= _PickupResetTime)
                    {
                        _PickupOn = true;
                        _PickupTimer = 0;
                    }
                }
                /***********************************************************************************/
                ///Handles All of Player 1 Jumping

                CheckForCollision(gameTime);

                if (_currentState1 == _states.Jumping)
                {
                    if (_CurrentJumpProgress1 >= 1 || _StartJump1)
                    {
                        _CurrentJumpProgress1 = _bat1.Position.Y / (_jumpFrom1 - _jumpHeight1);
                        if (_StartJump1)
                            _bat1.Speed = _jumpSpeed1;
                        _bat1.Position = new Vector2(_bat1.Position.X,
                        _bat1.Position.Y - (float)(_bat1.Speed * gameTime.ElapsedGameTime.TotalSeconds));
                        _StartJump1 = false;
                    }
                    else
                    {
                        _StartFall1 = true;
                        _currentState1 = _states.Falling;
                    }
                }
                if (_currentState1 == _states.Falling || _StartFall1)
                {
                    _bat1.Position = new Vector2(_bat1.Position.X,
                    _bat1.Position.Y + (float)(_bat1.Speed * gameTime.ElapsedGameTime.TotalSeconds));
                }

                /***********************************************************************************/

                /***********************************************************************************/
                ///Handles All of Player 2 Jumping

                if (_currentState2 == _states.Jumping)
                {
                    if (_CurrentJumpProgress2 >= 1 || _StartJump2)
                    {
                        _CurrentJumpProgress2 = _bat2.Position.Y / (_jumpFrom2 - _jumpHeight2);
                        if (_StartJump2)
                            _bat2.Speed = _jumpSpeed2;
                        _bat2.Position = new Vector2(_bat2.Position.X,
                        _bat2.Position.Y - (float)(_bat2.Speed * gameTime.ElapsedGameTime.TotalSeconds));
                        _StartJump2 = false;
                    }
                    else
                    {
                        _StartFall2 = true;
                        _currentState2 = _states.Falling;
                    }
                }
                if (_currentState2 == _states.Falling || _StartFall2)
                {
                    _bat2.Position = new Vector2(_bat2.Position.X, 
                    _bat2.Position.Y + (float)(_bat2.Speed * gameTime.ElapsedGameTime.TotalSeconds));
                }

                /***********************************************************************************/


                /***********************************************************************************/

                // Get Input
                // Jump for Player 1
                if (inputManager.KeyDown(Keys.W) && _currentState1 != _states.Jumping && _currentState1 != _states.Falling && !_StartJump1 && !_StartFall1)
                {
                    _currentState1 = _states.Jumping;
                    _jumpFrom1 = (int)_bat1.Position.Y;
                    _StartJump1 = true;
                    _StartFall1 = false;
                    _jumpSound.Play();
                }

                else if (inputManager.KeyDown(Keys.A))
                {

                    _bat1.Position = new Vector2(_bat1.Position.X - (float)(_bat1.Speed * gameTime.ElapsedGameTime.TotalSeconds),
                        _bat1.Position.Y);
                    if (_currentState1 != _states.Falling && _currentState1 != _states.Jumping)
                        _currentState1 = _states.Moving;
                    //_spriteAnim.CurrentFrame = new Vector2(_spriteAnim.CurrentFrame.X, 0);


                }
                else if (inputManager.KeyDown(Keys.D))
                {
                    _bat1.Position = new Vector2(_bat1.Position.X + (float)(_bat1.Speed * gameTime.ElapsedGameTime.TotalSeconds),
                        _bat1.Position.Y);

                    if (_currentState1 != _states.Falling && _currentState1 != _states.Jumping)
                        _currentState1 = _states.Moving;

                }
                else if ((inputManager.KeyDown(Keys.R) || inputManager.KeyPressed(Keys.R)) && !_P1CooldownOn)
                {
                    if (_currentState1 != _states.Falling && _currentState1 != _states.Jumping)
                        _currentState1 = _states.Shooting;
                    Ball _ball = new Ball();
                    _ball.LoadContent(content);
                    Vector2 bulletPos = new Vector2(_bat1.Position.X+60, _bat1.Position.Y +12+ (_ball.Image.Height / 2));
                    _ball.Position = bulletPos;
                    _ball.VSpeed = 0;
                    _ball.HSpeed = 350;
                    _Player1Bullets.Add(_ball);
                    _P1CooldownOn = true;
                    _Player1CooldownTimer = 0;
                    _fireGun.Play();                
                }
                else
                {
                    if(!_P1CooldownOn && (_currentState1 != _states.Jumping && _currentState1 != _states.Falling))
                        _currentState1 = _states.Idle;
                }


                if (_P1CooldownOn)
                {
                    _Player1CooldownTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (_Player1CooldownTimer >= _CooldownTime)
                        _P1CooldownOn = false;
                }

                // Player vs Player
                if (!AI)
                {
                    // Up for Player 2
                    if (inputManager.KeyDown(Keys.Up, Keys.I) && _currentState2 != _states.Jumping && _currentState2 != _states.Falling)
                    {
                        _currentState2 = _states.Jumping;
                        // PHIL: shouldn't the following be _bat2?
                        _jumpFrom2 = (int)_bat2.Position.Y;
                        _StartJump2 = true;
                        _StartFall2 = false;
                        _jumpSound2.Play();
                    }
                    // Down for Player 2
                    /*if (inputManager.KeyDown(Keys.Down, Keys.K))
                    {
                        _bat2.Position = new Vector2(_bat2.Position.X,
                            _bat2.Position.Y + (float)(_bat2.Speed * gameTime.ElapsedGameTime.TotalSeconds));
                    }*/
                    ///Left for Player 2
                    else if (inputManager.KeyDown(Keys.Left, Keys.J))
                    {
                        _bat2.Position = new Vector2(_bat2.Position.X - (float)(_bat2.Speed * gameTime.ElapsedGameTime.TotalSeconds),
                            _bat2.Position.Y);

                        if (_currentState2 != _states.Falling && _currentState2 != _states.Jumping)
                            _currentState2 = _states.Moving;
                    }
                    // Right for Player 2
                    else if (inputManager.KeyDown(Keys.Right, Keys.L))
                    {
                        _bat2.Position = new Vector2(_bat2.Position.X + (float)(_bat2.Speed * gameTime.ElapsedGameTime.TotalSeconds),
                            _bat2.Position.Y);

                        if (_currentState2 != _states.Falling && _currentState2 != _states.Jumping)
                            _currentState2 = _states.Moving;
                    }

                    else if (inputManager.KeyDown(Keys.Y) && !_P2CooldownOn)
                    {
                        if (_currentState2 != _states.Falling && _currentState2 != _states.Jumping)
                            _currentState2 = _states.Shooting;
                        Ball _ball = new Ball();
                        _ball.LoadContent(content);
                        //Vector2 bulletPos = new Vector2(_bat1.Position.X + 60, _bat1.Position.Y + 12 + (_ball.Image.Height / 2));
                        Vector2 bulletPos = new Vector2(_bat2.Position.X-60, _bat2.Position.Y +12+(_ball.Image.Height / 2));
                        _ball.Position = bulletPos;
                        _ball.VSpeed = 0;
                        _ball.HSpeed = -350;
                        _Player2Bullets.Add(_ball);
                        _P2CooldownOn = true;
                        _Player2CooldownTimer = 0;
                        
                        _fireGun2.Play();
                    }
                    else
                    {
                        if (!_P2CooldownOn && (_currentState2 != _states.Jumping && _currentState2 != _states.Falling))
                            _currentState2 = _states.Idle;
                    }
                    if (_P2CooldownOn)
                    {
                        _Player2CooldownTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                        if (_Player2CooldownTimer >= _CooldownTime)
                            _P2CooldownOn = false;
                    }

                   


                }
                // Player vs AI
                else
                {
                    // Up for Player 2
                    /*if (_bat2.Position.Y + (_bat2.Image.Height / 2) > _ball1.Position.Y)
                    {
                        _bat2.Position = new Vector2(_bat2.Position.X,
                            _bat2.Position.Y - (float)(_bat2.Speed * gameTime.ElapsedGameTime.TotalSeconds));
                    }
                    // Down for Player 2
                    if (_bat2.Position.Y + (_bat2.Image.Height / 2) < _ball1.Position.Y)
                    {
                        _bat2.Position = new Vector2(_bat2.Position.X,
                            _bat2.Position.Y + (float)(_bat2.Speed * gameTime.ElapsedGameTime.TotalSeconds));
                    }*/
                }

                // Check Boundary Conditions
                int MinY = 10;
                int MinX = 10;
                int MaxY = (int)ScreenManager.Instance.Dimensions.Y - MinY;
                int MaxX = ((int)ScreenManager.Instance.Dimensions.X - MinX) / 2 - (int)_bat1.Image.Width;

                int MaxX2 = (int)ScreenManager.Instance.Dimensions.X - MinX;
                /*****************************************************************************/

                for (int i = 0; i < _Player1Bullets.Count(); i++)
                {
                    if (_Player1Bullets[i].Position.X >= ScreenManager.Instance.Dimensions.X)
                    {
                        _Player1Bullets.RemoveAt(i);
                    }
                  
                }

                for (int i = 0; i < _Player2Bullets.Count(); i++)
                {
                    if (_Player2Bullets[i].Position.X <= 0)
                    {
                        _Player2Bullets.RemoveAt(i);
                    }
                }
                ///Y-Axis
                // Bottom for Player 1
                if (_bat1.Position.Y + _bat1.Image.Height >= MaxY)
                {
                    if ((_currentState1 == _states.Jumping || _currentState1 == _states.Falling) && _StartFall1)
                    {
                        _StartFall1 = false;
                        _currentState1 = _states.Idle;
                        _jumpLand.Play();
                    }
                    _bat1.Position = new Vector2(_bat1.Position.X, MaxY - _bat1.Image.Height);
                }
                // Up for Player 1
                else if (_bat1.Position.Y <= MinY)
                {
                    _bat1.Position = new Vector2(_bat1.Position.X, MinY);
                }
                // Bottom for Player 2
                if (_bat2.Position.Y + _bat2.Image.Height >= MaxY)
                {
                    if ((_currentState2 == _states.Jumping || _currentState2 == _states.Falling) && _StartFall2)
                    {
                        _currentState2 = _states.Idle;
                        _jumpLand2.Play();
                    }
                    _bat2.Position = new Vector2(_bat2.Position.X, MaxY - _bat2.Image.Height);
                }
                // Up for Player 2
                else if (_bat2.Position.Y <= MinY)
                {
                    _bat2.Position = new Vector2(_bat2.Position.X, MinY);
                }
                /*****************************************************************************/

                /*****************************************************************************/
                ///X-Axis
                // Right for Player 1
                if (_bat1.Position.X + _bat1.Image.Width >= MaxX)
                {
                    _bat1.Position = new Vector2(MaxX - _bat1.Image.Width, _bat1.Position.Y);
                }
                // Left for Player 1
                else if (_bat1.Position.X <= MinX)
                {
                    _bat1.Position = new Vector2(MinX, _bat1.Position.Y);
                }
                // Right for Player 2
                if (_bat2.Position.X + _bat2.Image.Width >= MaxX2)
                {
                    _bat2.Position = new Vector2(MaxX2 - _bat2.Image.Width, _bat2.Position.Y);
                }
                // Left for Player 2
                else if (_bat2.Position.X <= MaxX + 2 * _bat2.Image.Width)
                {
                    _bat2.Position = new Vector2(MaxX + 2 * _bat2.Image.Width, _bat2.Position.Y);
                }
                /*****************************************************************************/

                /*****************************************************************************/
                //Play background music
                if (_bgMusic1Duration < (((float)gameTime.TotalGameTime.TotalSeconds) - _bgMusic1SaveTime) || _bgMusic1SaveTime == 0.0)
                {
                    _bgMusic1SaveTime = (float)gameTime.TotalGameTime.TotalSeconds;
                    _bgMusic1.Play();
                }

                /*****************************************************************************/
            }

            /*****************************************************************************/
            /************************ SPRITE ANIMATIONS **********************************/
            
            /*** PLAYER 1 SPRITE ANIMATIONS ***/
            _idleAnim.IsActive = true;
            _walkAnim.IsActive = true;
            _shootAnim.IsActive = true;
            _jumpAnim.IsActive = true;
            if(CurrentStatePlayer1 == _states.Moving)
            {
                _shootAnim.IsActive = false;
                _idleAnim.IsActive = false;
                _jumpAnim.IsActive = false;
                _walkAnim.CurrentFrame = new Vector2(_walkAnim.CurrentFrame.X, 0);

                //Play running sound
                if (_runningDuration < (((float)gameTime.TotalGameTime.TotalSeconds) - _runningSaveTime))
                {
                    _runningSaveTime = (float)gameTime.TotalGameTime.TotalSeconds;
                    _running.Play();
                }
            }
            else if(CurrentStatePlayer1 == _states.Shooting)
            {
                _walkAnim.IsActive = false;
                _idleAnim.IsActive = false;
                _jumpAnim.IsActive = false;
                _shootAnim.CurrentFrame = new Vector2(_shootAnim.CurrentFrame.X, 0);
            }
            else if(CurrentStatePlayer1 == _states.Idle)
            {
                _walkAnim.IsActive = false;
                _shootAnim.IsActive = false;
                _jumpAnim.IsActive = false;
                _shootAnim.CurrentFrame = new Vector2(_idleAnim.CurrentFrame.X, 0);
            }
            else if(CurrentStatePlayer1 == _states.Jumping || CurrentStatePlayer1 == _states.Falling)
            {
                _walkAnim.IsActive = false;
                _shootAnim.IsActive = false;
                _idleAnim.IsActive = false;
                _jumpAnim.CurrentFrame = new Vector2(_jumpAnim.CurrentFrame.X, 0);
            }
            _walkAnim.Position = new Vector2(_bat1.Position.X - 20, _bat1.Position.Y);
            _walkAnim.Update(gameTime);
            _shootAnim.Position = new Vector2(_bat1.Position.X - 20, _bat1.Position.Y);
            _shootAnim.Update(gameTime);
            _idleAnim.Position = new Vector2(_bat1.Position.X - 25, _bat1.Position.Y);
            _idleAnim.Update(gameTime);
            _jumpAnim.Position = new Vector2(_bat1.Position.X - 20, _bat1.Position.Y);
            _jumpAnim.Update(gameTime);

            /*** PLAYER 2 SPRITE ANIMATIONS ***/
            _idleAnim2.IsActive = true;
            _walkAnim2.IsActive = true;
            _shootAnim2.IsActive = true;
            _jumpAnim2.IsActive = true;
            if (CurrentStatePlayer2 == _states.Moving)
            {
                _shootAnim2.IsActive = false;
                _idleAnim2.IsActive = false;
                _jumpAnim2.IsActive = false;
                _walkAnim2.CurrentFrame = new Vector2(_walkAnim2.CurrentFrame.X, 0);

                //Play running sound
                if (_runningDuration < (((float)gameTime.TotalGameTime.TotalSeconds) - _running2SaveTime))
                {
                    _running2SaveTime = (float)gameTime.TotalGameTime.TotalSeconds;
                    _running2.Play();
                }
            }
            else if (CurrentStatePlayer2 == _states.Shooting)
            {
                _walkAnim2.IsActive = false;
                _idleAnim2.IsActive = false;
                _jumpAnim2.IsActive = false;
                _shootAnim2.CurrentFrame = new Vector2(_shootAnim2.CurrentFrame.X, 0);
            }
            else if (CurrentStatePlayer2 == _states.Idle)
            {
                _walkAnim2.IsActive = false;
                _shootAnim2.IsActive = false;
                _jumpAnim2.IsActive = false;
                _shootAnim2.CurrentFrame = new Vector2(_idleAnim2.CurrentFrame.X, 0);
            }
            else if (CurrentStatePlayer2 == _states.Jumping || CurrentStatePlayer2 == _states.Falling)
            {
                _walkAnim2.IsActive = false;
                _shootAnim2.IsActive = false;
                _idleAnim2.IsActive = false;
                _jumpAnim2.CurrentFrame = new Vector2(_jumpAnim2.CurrentFrame.X, 0);
            }
            _walkAnim2.Position = new Vector2(_bat2.Position.X - 65, _bat2.Position.Y);
            _walkAnim2.Update(gameTime);
            _shootAnim2.Position = new Vector2(_bat2.Position.X - 65, _bat2.Position.Y);
            _shootAnim2.Update(gameTime);
            _idleAnim2.Position = new Vector2(_bat2.Position.X - 65, _bat2.Position.Y - 5);
            _idleAnim2.Update(gameTime);
            _jumpAnim2.Position = new Vector2(_bat2.Position.X - 65, _bat2.Position.Y);
            _jumpAnim2.Update(gameTime);



            /*****************************************************************************/

            if (_Player1Bullets != null)
            {
                for (int i = 0; i < _Player1Bullets.Count(); i++)
                    _Player1Bullets[i].Update(gameTime);
            }

            if (_Player2Bullets != null)
            {
                for (int i = 0; i < _Player2Bullets.Count(); i++)
                    _Player2Bullets[i].Update(gameTime);
            }

        }

        private void StopAllAnimations()
        {
            _walkAnim.IsActive = false;
            _shootAnim.IsActive = false;
        }

        /******************************************************************************************/
        /// <summary>
        /// Test for if the bullets collides with the players
        /// </summary>
        void CheckForCollision(GameTime gt)
        {
            for (int i = 0; i < _Player1Bullets.Count(); i++)
            {
                BoundingBox bb1 = new BoundingBox(new Vector3(_bat2.Position.X - (_bat2.Image.Width / 2), _bat2.Position.Y - (_bat2.Image.Height / 2), 0),
                    new Vector3(_bat2.Position.X + (_bat2.Image.Width / 2), _bat2.Position.Y + (_bat2.Image.Height / 2), 0));

                BoundingBox bb2 = new BoundingBox(new Vector3(_Player1Bullets[i].Position.X - (_Player1Bullets[i].Image.Width / 2), _Player1Bullets[i].Position.Y - (_Player1Bullets[i].Image.Height / 2), 0), 
                    new Vector3(_Player1Bullets[i].Position.X + (_Player1Bullets[i].Image.Width / 2), _Player1Bullets[i].Position.Y + (_Player1Bullets[i].Image.Height / 2), 0));

                if (bb2.Intersects(bb1))
                {
                    _Player1Bullets.RemoveAt(i);
                    _hitImpact.Play(); 

                    // Take Player 2 Health 
                    _health2 -= _damage1;
                    _bat1.increaseHit();
                }
            }

            for (int i = 0; i < _Player2Bullets.Count(); i++)
            {
           
                BoundingBox bb1 = new BoundingBox(new Vector3(_bat1.Position.X - (_bat1.Image.Width / 2), _bat1.Position.Y - (_bat1.Image.Height / 2), 0),
                   new Vector3(_bat1.Position.X + (_bat1.Image.Width / 2), _bat1.Position.Y + (_bat1.Image.Height / 2), 0));

                BoundingBox bb2 = new BoundingBox(new Vector3(_Player2Bullets[i].Position.X - (_Player2Bullets[i].Image.Width / 2), _Player2Bullets[i].Position.Y - (_Player2Bullets[i].Image.Height / 2), 0),
                    new Vector3(_Player2Bullets[i].Position.X + (_Player2Bullets[i].Image.Width / 2), _Player2Bullets[i].Position.Y + (_Player2Bullets[i].Image.Height / 2), 0));

                if (bb2.Intersects(bb1))
                {
                    _Player2Bullets.RemoveAt(i);
                    _hitImpact2.Play(); 

                    // Take Player 1 Health 
                    _health1 -= _damage2;
                    _bat2.increaseHit();
                }
            }

            if (_PickupOn) 
            {
                BoundingBox bb1 = new BoundingBox(new Vector3(_bat1.Position.X - (_bat1.Image.Width / 2), _bat1.Position.Y - (_bat1.Image.Height / 2), 0),
                  new Vector3(_bat1.Position.X + (_bat1.Image.Width / 2), _bat1.Position.Y + (_bat1.Image.Height / 2), 0));

                BoundingBox bb2 = new BoundingBox(new Vector3(_bat2.Position.X - (_bat2.Image.Width / 2), _bat2.Position.Y - (_bat2.Image.Height / 2), 0),
                    new Vector3(_bat2.Position.X + (_bat2.Image.Width / 2), _bat2.Position.Y + (_bat2.Image.Height / 2), 0));

                BoundingBox bb3 = new BoundingBox(new Vector3(((int)ScreenManager.Instance.Dimensions.X) / 2 - 75, (int)ScreenManager.Instance.Dimensions.Y - 75, 0),
                    new Vector3(((int)ScreenManager.Instance.Dimensions.X) / 2 + 75, (int)ScreenManager.Instance.Dimensions.Y + 75, 0));

                if (bb2.Intersects(bb3))
                {
                    switch (_CurrentPickup.GetType())
                    {
                        case 0:
                            _health2 += 30;
                            if(_health2 > 100)
                            {
                                _health2 = 100;
                            }
                            _JumpPickUp2 = false;
                            _DamagePickUp2 = false;
                            _SpeedPickUp2 = false;
                            break;
                        case 1:
                            _DamagePickUp2 = true;
                            _SpeedPickUp2 = false;
                            _JumpPickUp2 = false;
                            break;
                        case 2:
                            _SpeedPickUp2 = true;
                            _DamagePickUp2 = false;
                            _JumpPickUp2 = false;
                            break;
                        case 3:
                            _JumpPickUp2 = true;
                            _DamagePickUp2 = false;
                            _SpeedPickUp2 = false;
                            break;
                    }

                    RandPicup();
                    _PickupOn = false;
                }
                if (bb1.Intersects(bb3))
                {
                    switch (_CurrentPickup.GetType()) 
                    {
                        case 0:
                            _health1 += 30;
                            if (_health1 > 100)
                            {
                                _health1 = 100;
                            }
                            _JumpPickUp1 = false;
                            _DamagePickUp1 = false;
                            _SpeedPickUp1 = false;
                            break;
                        case 1:
                            _DamagePickUp1 = true;
                            _SpeedPickUp1 = false;
                            _JumpPickUp1 = false;
                            break;
                        case 2:
                            _SpeedPickUp1 = true;
                            _DamagePickUp1 = false;
                            _JumpPickUp1 = false;
                            break;
                        case 3:
                            _JumpPickUp1 = true;
                            _DamagePickUp1 = false;
                            _SpeedPickUp1 = false;
                            break;
                    }
                    RandPicup();
                    _PickupOn = false;
                }
            
            }
        }

        /******************************************************************************************/

        public void Draw(SpriteBatch spriteBatch)
        {
          spriteBatch.Draw(_Chain, new Rectangle((((int)ScreenManager.Instance.Dimensions.X) / 2) - 30, 0, _Chain.Width, _Chain.Height), Color.White);
          
          if (_Player1Bullets != null)
            {
                for (int i = 0; i < _Player1Bullets.Count(); i++)
                    _Player1Bullets[i].Draw(spriteBatch);
            }

            if (_Player2Bullets != null)
            {
                for (int i = 0; i < _Player2Bullets.Count(); i++)
                    _Player2Bullets[i].Draw(spriteBatch);
            }
          if(_walkAnim.IsActive)_walkAnim.Draw(spriteBatch);
          if(_shootAnim.IsActive)_shootAnim.Draw(spriteBatch);
          if (_idleAnim.IsActive) _idleAnim.Draw(spriteBatch);
          if (_jumpAnim.IsActive) _jumpAnim.Draw(spriteBatch);

          if (_walkAnim2.IsActive) _walkAnim2.Draw(spriteBatch);
          if (_shootAnim2.IsActive) _shootAnim2.Draw(spriteBatch);
          if (_idleAnim2.IsActive) _idleAnim2.Draw(spriteBatch);
          if (_jumpAnim2.IsActive) _jumpAnim2.Draw(spriteBatch);

           

           
          if(_PickupOn)
          {
            switch(_CurrentPickup.GetType())
            {
                case 0:
                    spriteBatch.Draw(_HealthPickupImage, new Rectangle((((int)ScreenManager.Instance.Dimensions.X) / 2) - 25, (int)ScreenManager.Instance.Dimensions.Y - 50, 50, 50), Color.White);
                    break;
                case 1:
                    spriteBatch.Draw(_DamagePickupImage, new Rectangle((((int)ScreenManager.Instance.Dimensions.X) / 2) - 25, (int)ScreenManager.Instance.Dimensions.Y - 50, 50, 50), Color.White);
                    break;
                case 2:
                    spriteBatch.Draw(_SpeedPickupImage, new Rectangle((((int)ScreenManager.Instance.Dimensions.X) / 2) - 25, (int)ScreenManager.Instance.Dimensions.Y - 50, 50, 50), Color.White);
                    break;
                case 3:
                    spriteBatch.Draw(_JumpPickupImage, new Rectangle((((int)ScreenManager.Instance.Dimensions.X) / 2) - 25, (int)ScreenManager.Instance.Dimensions.Y - 50, 50, 50), Color.White);
                    break;

            }
}
 
        }
        



        private void RandPicup()
        {
            Random r = new Random((int)System.DateTime.Now.Ticks);
           
            int i = r.Next(0, 4);
            _CurrentPickup.SetType(i);
            
        }


    }

}
