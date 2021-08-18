using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Unbreakable
{
    public class Bat
    {
        #region Variables
        private ContentManager _content;
        private Texture2D _batImage;
        private Vector2 _position;
        private int _speed;
        private FileManager _fileManager;
        private InputManager _inputManager;
        private List<List<string>> _attributes, _contents;
        private Animation _batAnimation;
        private Animation _countAnimation;
        private Animation _nameAnimation;
        private int _id = 1;
        private int _hitCount = 0;
        private string _name;
        private bool _stopUpdating;
        #endregion

        #region Properties
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        public int Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }
        public Texture2D Image
        {
            get { return _batImage; }
            set { _batImage = value; }
        }
        public bool StopUpdating
        {
            get { return _stopUpdating; }
            set { _stopUpdating = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public int HitCount
        {
            get { return _hitCount; }
            set { _hitCount = value; }
        }


        #endregion

        public void LoadContent(ContentManager content, Vector2 position, string BatID, string name)
        {
            this._content = content;
            _fileManager = new FileManager();
            _inputManager = new InputManager();
            _attributes = new List<List<string>>();
            _contents = new List<List<string>>();
            _fileManager.LoadContent("Load/Bat" + BatID + ".deb", _attributes, _contents);
            _position = position;
            this._id = int.Parse(BatID);
            this._name = name;

                // Load all bat properties from file
                for (int i = 0; i < _attributes.Count; i++)
                {
                    for (int j = 0; j < _attributes[i].Count; j++)
                    {
                        switch (_attributes[i][j])
                        {
                            case "Image":
                                _batImage = content.Load<Texture2D>("Sprites/" + _contents[i][j]);
                                break;
                            case "Speed":
                                Random rand = new Random();
                                _speed = int.Parse(_contents[i][j]);
                                break;
                        }
                    }
                }
            if(this._id == 1)
                _position = new Vector2(0, ScreenManager.Instance.Dimensions.Y);
            else
                _position = new Vector2(ScreenManager.Instance.Dimensions.X - _batImage.Width, ScreenManager.Instance.Dimensions.Y);
            _batAnimation = new Animation();
            _batAnimation.LoadContent(content, _batImage, "", _position);
            _countAnimation = new Animation();
            if (this._id == 1)
                _countAnimation.LoadContent(content, null, _hitCount.ToString(), new Vector2(_batImage.Width,
                    50));
            else
                _countAnimation.LoadContent(content, null, _hitCount.ToString(), new Vector2(
                    ScreenManager.Instance.Dimensions.X - _batImage.Width - content.Load<SpriteFont>("Fonts/Menu").MeasureString(_hitCount.ToString()).X,
                   50));
            _nameAnimation = new Animation();
            if (this._id == 1)
                _nameAnimation.LoadContent(content, null, _name.ToString(), new Vector2(_batImage.Width,
                    35));
            else
                _nameAnimation.LoadContent(content, null, _name.ToString(), new Vector2(
                    ScreenManager.Instance.Dimensions.X - _batImage.Width - content.Load<SpriteFont>("Fonts/Menu").MeasureString(_name.ToString()).X,
                   35));
            
        }

        public void UnloadContent()
        {
            _content.Unload();
            _fileManager = null;
            _inputManager = null;
            _speed = 0;
            _batImage = null;
            _attributes.Clear();
            _contents.Clear();
            _position = Vector2.Zero;
            _batAnimation.UnloadContent();
        }

        public void Update(GameTime gameTime, InputManager inputManager)
        {
            if(!_stopUpdating)
            { 
                _countAnimation.Text = _hitCount.ToString();
                _countAnimation.Update(gameTime);
                _nameAnimation.Update(gameTime);
                _batAnimation.Position = new Vector2(_position.X, _position.Y);
                _batAnimation.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _countAnimation.Draw(spriteBatch);
            _nameAnimation.Draw(spriteBatch);
            _batAnimation.Draw(spriteBatch);
        }

        public void increaseHit()
        {
            _hitCount++;
        }
    }
}
