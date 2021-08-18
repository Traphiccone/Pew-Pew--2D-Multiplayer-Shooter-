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
    public class TitleScreen : GameScreen
    {
        SpriteFont font;
        MenuManager menu;
        Texture2D bgImg;

        public override void LoadContent(ContentManager Content, InputManager inputManager)
        {
            base.LoadContent(Content, inputManager);
            if (font == null)
                font = this.content.Load<SpriteFont>("Fonts/Title");
            menu = new MenuManager();
            menu.LoadContent(content, "Menu", "Title");
            bgImg = this.content.Load<Texture2D>("Images/background");
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            menu.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            inputManager.Update();
            menu.Update(gameTime, inputManager);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 origin = new Vector2(ScreenManager.Instance.Dimensions.X / 2, ScreenManager.Instance.Dimensions.Y / 2);
            Rectangle sourceRect = new Rectangle(0, 0, bgImg.Width, bgImg.Height);
            spriteBatch.Draw(bgImg, origin, sourceRect, Color.White, 0.0f, origin, 1.0f, SpriteEffects.None, 0.0f);
            
            menu.Draw(spriteBatch);

        }
    }
}
