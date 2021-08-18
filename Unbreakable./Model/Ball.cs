using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Unbreakable
{
    /// <summary>
    /// Ball Model class maintains all the properties of Unbreakable ball
    /// </summary>
    public class Ball
    {
        #region Variables
        private ContentManager _content;
        private Texture2D _ballImage;
        private Vector2 _position;
        private int _hSpeed, _vSpeed;
        private FileManager _fileManager;
        private InputManager _inputManager;
        private List<List<string>> _attributes, _contents;
        private Animation _ballAnimation;
        private bool _stopUpdating;
        private SoundEffect _thud;
        #endregion

        #region Properties
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        public int HSpeed
        {
            get { return _hSpeed; }
            set { _hSpeed = value; }
        }
        public int VSpeed
        {
            get { return _vSpeed; }
            set { _vSpeed = value; }
        }
        public Texture2D Image
        {
            get { return _ballImage; }
            set { _ballImage = value; }
        }
        public bool StopUpdating
        {
            get { return _stopUpdating; }
            set { _stopUpdating = value; }
        }
        #endregion

        public void LoadContent(ContentManager content)
        {
            this._content = content;
            _fileManager = new FileManager();
            _inputManager = new InputManager();
            _thud = content.Load<SoundEffect>("Audio/thud");
            _attributes = new List<List<string>>();
            _contents = new List<List<string>>();
            _fileManager.LoadContent("Load/Ball.deb", _attributes, _contents);
            _position = new Vector2(ScreenManager.Instance.Dimensions.X / 2, 
                ScreenManager.Instance.Dimensions.Y / 2);
           
            // Load all ball properties from file
            for(int i = 0; i < _attributes.Count; i++)
            {
                for(int j = 0; j < _attributes[i].Count; j++)
                {
                    switch(_attributes[i][j])
                    {
                        case "Image":
                            _ballImage = content.Load<Texture2D>("Sprites/" + _contents[i][j]);
                            break;
                        case "HorizontalSpeed":
                            Random rand = new Random();
                            _hSpeed = int.Parse(_contents[i][j]);
                            if (rand.Next(0, 1) == 0) _hSpeed *= -1;
                            break;
                        case "VerticalSpeed":
                            rand = new Random();;
                            _vSpeed =  int.Parse(_contents[i][j]);
                            if (rand.Next(0, 1) == 0) _vSpeed *= -1;
                            break;
                    }
                }
            }
            _ballAnimation = new Animation();
            _ballAnimation.LoadContent(content,_ballImage,"",_position);
        }

        public void UnloadContent()
        {
            _content.Unload();
            _fileManager = null;
            _inputManager = null;
            _hSpeed = 0;
            _vSpeed = 0;
            _ballImage = null;
            _attributes.Clear();
            _contents.Clear();
            _position = Vector2.Zero;
            _ballAnimation.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            if (!StopUpdating)
            {
                _position.X += (float)(_hSpeed * gameTime.ElapsedGameTime.TotalSeconds);
                _position.Y += (float)(_vSpeed * gameTime.ElapsedGameTime.TotalSeconds);

                // Check for Window collision
                int MinX = 10;
                int MinY = 10;
                int MaxX = (int)ScreenManager.Instance.Dimensions.X - MinX - _ballImage.Width;
                int MaxY = (int)ScreenManager.Instance.Dimensions.Y - MinY - _ballImage.Height;
                // Bottom of window collision
                if (_position.Y > MaxY)
                {
                    _vSpeed *= -1;
                    _thud.Play();
                }
                // Top of window collision
                if (_position.Y < MinY)
                {
                    _vSpeed *= -1;
                    _thud.Play();
                }
                _ballAnimation.Position = new Vector2(_position.X, _position.Y);
                _ballAnimation.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _ballAnimation.Draw(spriteBatch);
        }
    }
}
