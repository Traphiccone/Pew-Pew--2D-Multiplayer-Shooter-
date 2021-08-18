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
    public class ControlScreen : GameScreen
    {
        SpriteFont font;
        Texture2D controlImg;

        public override void LoadContent(ContentManager Content, InputManager inputManager)
        {
            base.LoadContent(Content, inputManager);
            if (font == null)
                font = this.content.Load<SpriteFont>("Fonts/Title");
            controlImg = this.content.Load<Texture2D>("Images/controls");
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            inputManager.Update();
            if(inputManager.KeyPressed(Keys.Enter,Keys.Z))
            {
                ScreenManager.Instance.AddScreen(new TitleScreen(), inputManager);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            Vector2 origin = new Vector2(ScreenManager.Instance.Dimensions.X / 2, ScreenManager.Instance.Dimensions.Y / 2);
            Rectangle sourceRect = new Rectangle(0, 0, controlImg.Width, controlImg.Height);
            spriteBatch.Draw(controlImg, origin, sourceRect, Color.White, 0.0f, origin, 1.0f, SpriteEffects.None, 0.0f);
            
        }
    }
}