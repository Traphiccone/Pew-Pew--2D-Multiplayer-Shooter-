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
    public class HumanGamePlayScreen : GameScreen
    {
        Bat _bat1;
        Bat _bat2;
        ControlBat _controller;
        private bool AI;
        private Animation _winAnimation;
        private Animation _name1Animation;
        private Animation _name2Animation;
        private FadeAnimation _restartAnimation;
        private bool gameOver = false;
        private string _restartText = "Press <Enter> or <Z> to play a new game";
        private string _name1Text = "Enter Player 1 Name: ";
        private string _name2Text = "Enter Player 2 Name: ";
        private bool _gotName1,_gotName2;
        private KeyboardManager _km;
        private KeyboardManager _km2;
        private string _name1 = "";
        private string _name2 = "";
        private SoundEffect _pongSound;
        private SoundEffect _gameoverSound;
        private Texture2D _bg;
        private Texture2D _g;
        private bool played;
        private int _level;

        private Texture2D _background;
        List<string> backgrounds = new List<string>();

        // Health Bar
        Texture2D _healthBarSprite1;
        Rectangle _healthRect1;
        Texture2D _healthBarSprite2;
        Rectangle _healthRect2;

        public override void LoadContent(ContentManager Content, InputManager inputManager)
        {
            base.LoadContent(Content, inputManager);
            _gotName1 = false;
            _gotName2 = false;
            played = false;
            _name1Animation = new Animation();
            _name2Animation = new Animation();
            _km = new KeyboardManager();
            _pongSound = content.Load<SoundEffect>("Audio/pong");
            _gameoverSound = content.Load<SoundEffect>("Audio/gameover");
            _bg = content.Load<Texture2D>("Images/enterName");

            _g = content.Load<Texture2D>("Images/game");
            _name1Animation.LoadContent(content, null, _name1Text, new Vector2(ScreenManager.Instance.Dimensions.X / 2,
               ScreenManager.Instance.Dimensions.Y / 2));

            backgrounds.Add("Splash/background");
            backgrounds.Add("Splash/windows_background");
            backgrounds.Add("Splash/ATAT_background");

            Random rand = new Random();
            _level = rand.Next(0,3);

            _background = content.Load<Texture2D>(backgrounds[_level]);

        }

        private void init()
        {
           // _ball = new Ball();
           // _ball.LoadContent(content);
            _bat1 = new Bat();
            _bat2 = new Bat();
            _bat1.LoadContent(content, new Vector2(0, ScreenManager.Instance.Dimensions.Y / 2), "1", _name1);
            _bat2.LoadContent(content, new Vector2(0, ScreenManager.Instance.Dimensions.Y / 2), "2", _name2);
            _controller = new ControlBat(_bat1, _bat2);
            _controller.LoadContent(content);
            _controller.CurrentStatePlayer1 = ControlBat._states.Idle;
            _controller.CurrentStatePlayer2 = ControlBat._states.Idle;
            _winAnimation = new Animation();
            _winAnimation.LoadContent(content, null, "Win", new Vector2(ScreenManager.Instance.Dimensions.X / 2,
                ScreenManager.Instance.Dimensions.Y / 2));
            _restartAnimation = new FadeAnimation();
            _restartAnimation.LoadContent(content, null, "Win", new Vector2(
                ScreenManager.Instance.Dimensions.X / 2 - content.Load<SpriteFont>("Fonts/Menu").MeasureString(_restartText.ToString()).X / 2,
                ScreenManager.Instance.Dimensions.Y / 2 + 30));
            AI = false;
            _healthBarSprite1 = content.Load<Texture2D>("Images/healthBar1");
            _healthBarSprite2 = content.Load<Texture2D>("Images/healthBar2");
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            _bat1.UnloadContent();
            _bat2.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (_gotName1 && _gotName2)
            {
                inputManager.Update();
                base.Update(gameTime);
                _controller.Update(content,gameTime, inputManager, AI);
                _bat1.Update(gameTime, inputManager);
                _bat2.Update(gameTime, inputManager);
                Collisions();
                _healthRect1 = new Rectangle(50, 20, _controller._health1, 20);
                _healthRect2 = new Rectangle((int)ScreenManager.Instance.Dimensions.X - 150, 20, _controller._health2, 20);
                if (gameOver)
                {
                    _winAnimation.Update(gameTime);
                    _restartAnimation.Update(gameTime);
                }
            }
            else
            {
                if(!_gotName1 && !_gotName2)
                {
                    _km.Update();
                    _name1 = _km.Text;
                    _name1Animation.Text = _name1Text + _name1;
                    _name1Animation.Position = new Vector2(ScreenManager.Instance.Dimensions.X / 2 -
                        content.Load<SpriteFont>("Fonts/Menu").MeasureString(_name1Animation.Text).X / 2,
                        ScreenManager.Instance.Dimensions.Y / 2);
                    _name1Animation.Update(gameTime);
                   
                   inputManager.Update();
                    if (inputManager.KeyPressed(Keys.Enter, Keys.Z))
                        {
                            _gotName1 = true;
                            _name1Animation.UnloadContent();
                            _km.Text = "";
                            _km2 = new KeyboardManager();
                            inputManager.StopUpdating = true;
                            _name2Animation.LoadContent(content, null, _name2Text, new Vector2(ScreenManager.Instance.Dimensions.X / 2,
                   ScreenManager.Instance.Dimensions.Y / 2));
                            
                        }
                  
                }
                else if(_gotName1 && !_gotName2)
                {
                    _km2.Update();
                    _name2 = _km2.Text;
                    _name2Animation.Text = _name2Text + _name2;
                    _name2Animation.Position = new Vector2(ScreenManager.Instance.Dimensions.X / 2 -
                        content.Load<SpriteFont>("Fonts/Menu").MeasureString(_name2Animation.Text).X / 2,
                        ScreenManager.Instance.Dimensions.Y / 2);
                    _name2Animation.Update(gameTime);
                    inputManager.StopUpdating = false;
                    inputManager.Update();
                    if (inputManager.KeyPressed(Keys.Enter, Keys.Z))
                    {
                        _gotName2 = true;
                        init();
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (_gotName1 && _gotName2)
            {
                Vector2 origin = new Vector2(ScreenManager.Instance.Dimensions.X / 2, ScreenManager.Instance.Dimensions.Y / 2);
                Rectangle sourceRect = new Rectangle(0, 0, _g.Width, _g.Height);
                //spriteBatch.Draw(_g, origin, sourceRect, Color.White, 0.0f, origin, 1.0f, SpriteEffects.None, 0.0f);
                spriteBatch.Draw(_background, new Rectangle(0, 0, (int)ScreenManager.Instance.Dimensions.X, (int)ScreenManager.Instance.Dimensions.Y), Color.White);

              
                _bat1.Draw(spriteBatch);
                _bat2.Draw(spriteBatch);
                _controller.Draw(spriteBatch);
                spriteBatch.Draw(_healthBarSprite1, _healthRect1, Color.Blue);
                spriteBatch.Draw(_healthBarSprite2, _healthRect2, Color.Red);
                if (gameOver)
                {
                    _winAnimation.Draw(spriteBatch);
                    _restartAnimation.Draw(spriteBatch);
                }
               
            }
             if(!_gotName1 && !_gotName2)
            {
                Vector2 origin = new Vector2(ScreenManager.Instance.Dimensions.X / 2, ScreenManager.Instance.Dimensions.Y / 2);
                Rectangle sourceRect = new Rectangle(0, 0, _bg.Width, _bg.Height);
                spriteBatch.Draw(_bg, origin, sourceRect, Color.White, 0.0f, origin, 1.0f, SpriteEffects.None, 0.0f);

                _name1Animation.Draw(spriteBatch);
            }
            if(_gotName1 && !_gotName2)
            {
                Vector2 origin = new Vector2(ScreenManager.Instance.Dimensions.X / 2, ScreenManager.Instance.Dimensions.Y / 2);
                Rectangle sourceRect = new Rectangle(0, 0, _bg.Width, _bg.Height);
                spriteBatch.Draw(_bg, origin, sourceRect, Color.White, 0.0f, origin, 1.0f, SpriteEffects.None, 0.0f);

                _name2Animation.Draw(spriteBatch);
            }

        }

        #region Private
        void Collisions()
        {

            if(_controller._health1 <= 0)
            {
                if(!played)
                {
                    _gameoverSound.Play();
                    played = true;
                }
                ResetGame();
            }
            else if(_controller._health2 <= 0)
            {
                if (!played)
                {
                    _gameoverSound.Play();
                    played = true;
                }
                ResetGame();
            }
        /*    Random rand = new Random();
            //Check Player1 bat if ball moving left, else check Player2 bat
            if (_ball.HSpeed < 0)
            {
                //check if ball has surpassed paddle
                if (_ball.Position.X < _bat1.Position.X + 20)
                {
                    //check if ball has hit paddle or went through
                    if ((_ball.Position.Y + _ball.Image.Height < _bat1.Position.Y) || (_ball.Position.Y > _bat1.Position.Y + _bat1.Image.Height))
                    {
                        //LOST!
                        if (!played)
                        {
                            _gameoverSound.Play();
                            played = true;
                        }
                        ResetGame();
                    }
                    else
                    {
                        //ball hit - calculate new speeds
                        //speed of ball changes randomly - 3 to 6
                        _bat1.increaseHit();
                        _pongSound.Play();
                        if (_ball.HSpeed < 0)
                            _ball.HSpeed = rand.Next(1, 3) * 100;
                        else _ball.HSpeed = rand.Next(-3, -1) * 100;

                        if (_ball.VSpeed < 0)
                            _ball.VSpeed = rand.Next(1, 3) * 100;
                        else _ball.VSpeed = rand.Next(-3, -1) * 100;
                    }
                }
            }
            else
            {
                //check if ball has surpassed paddle
                if (_ball.Position.X + _ball.Image.Width > _bat2.Position.X)
                {
                    //check if ball has hit paddle or went through
                    if ((_ball.Position.Y + _ball.Image.Height < _bat2.Position.Y) || (_ball.Position.Y > _bat2.Position.Y + _bat2.Image.Height))
                    {
                        //LOST!
                        if (!played)
                        {
                            _gameoverSound.Play();
                            played = true;
                        }
                        ResetGame();
                    }
                    else
                    {
                        //ball hit - calculate new speeds
                        //speed of ball changes randomly - 3 to 6
                        _bat2.increaseHit();
                        _pongSound.Play();
                        if (_ball.HSpeed < 0)
                            _ball.HSpeed = rand.Next(1, 3) * 100;
                        else _ball.HSpeed = rand.Next(-3, -1) * 100;

                        if (_ball.VSpeed < 0)
                            _ball.VSpeed = rand.Next(1, 3) * 100;
                        else _ball.VSpeed = rand.Next(-3, -1) * 100;
                    }
                }
            }*/
        }
        private void ResetGame()
        {
            gameOver = true;
            _bat1.StopUpdating = true;
            _bat2.StopUpdating = true;
            _controller.StopUpdating = true;
            string winner = "";
            if (_controller._health1 > _controller._health2)
            {
                winner = _bat1.Name;
                _winAnimation.Position = new Vector2(
                   ScreenManager.Instance.Dimensions.X / 2 -
                   content.Load<SpriteFont>("Fonts/Menu").MeasureString(winner + " won!").X / 2
                    , _winAnimation.Position.Y);
                _winAnimation.Text = winner + " won!";
            }
            else if (_controller._health1 < _controller._health2)
            {
                winner = _bat2.Name;
                _winAnimation.Position = new Vector2(
                   ScreenManager.Instance.Dimensions.X / 2 -
                   content.Load<SpriteFont>("Fonts/Menu").MeasureString(winner + " won!").X / 2
                    , _winAnimation.Position.Y);
                _winAnimation.Text = winner + " won!";
            }
            else
            {
                _winAnimation.Text = "Game is drawn!";
                _winAnimation.Position = new Vector2(
                   ScreenManager.Instance.Dimensions.X / 2 -
                   content.Load<SpriteFont>("Fonts/Menu").MeasureString(_winAnimation.Text).X / 2
                    , _winAnimation.Position.Y);
            }

            _restartAnimation.IsActive = true;
            _restartAnimation.Text = _restartText;
            if(inputManager.KeyPressed(Keys.Enter,Keys.Z))
            {
                string bat1Name = _bat1.Name;
                string bat2Name = _bat2.Name;
                _bat1 = new Bat();
                _bat2 = new Bat();
                _bat1.LoadContent(content, new Vector2(0, ScreenManager.Instance.Dimensions.Y / 2), "1", bat1Name);
                _bat2.LoadContent(content, new Vector2(0, ScreenManager.Instance.Dimensions.Y / 2), "2", bat2Name);
                _controller = new ControlBat(_bat1, _bat2);
                _controller.LoadContent(content);
                _controller.CurrentStatePlayer1 = ControlBat._states.Idle;
                _controller.CurrentStatePlayer2 = ControlBat._states.Idle;
                Random rand = new Random();
                _level = rand.Next(0, 3);

                _background = content.Load<Texture2D>(backgrounds[_level]);
                played = false;
                gameOver = false;
            }
           
        }
        #endregion

    }
}
