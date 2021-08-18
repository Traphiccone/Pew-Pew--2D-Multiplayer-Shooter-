using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
/// <summary>
/// HOW TO USE SPRITESHEET ANIMATION: 
/// 1. Private variables
///     a. SpriteSheetAnimation moveAnimation = new SpriteSheetAnimation();
///     b. Vector2 tempFrames = Vector2.Zero;
/// 2. In LoadContent: 
///     a. Texture2D image = this.content.Load<Texture2D>("SPRITE_NAME");
///     b. Vector2 position = Vector2.Zero;
///     b. moveAnimation.LoadContent(content, image, "", position);
/// 3. In Update:
///     a.  moveAnimation.IsActive = true;
///     b.  Depending on state, choose sprite by the following format:
///         moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, COLUMN_NUMBER);
///         In the above, the x-axis contains different animations for the same state - like three different
///         sprites for walking and the Y-axis contains the state for instance, 2 would contain only walking
///         right.
///     c.  moveAnimation.IsActive = false; // in idle state
///     EXAMPLE:
///          moveAnimation.IsActive = true;
///            if (input.KeyDown(Keys.Right, Keys.D))
///            {
///                moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 2);
///            } (Please refer to image: Splash/player_sprite. 2 corresponds to row three containing 
///             player walking right)
///     d.  moveAnimation.Position = position; // You can get the position from player.Position
///     e.  moveAnimation.Update(gameTime); 
/// 4. In Draw:
///     a.  moveAnimation.Draw(spriteBatch);
/// </summary>
namespace Unbreakable
{
    public class SpriteAnimation : Animation
    {
        int frameCounter, switchFrame;

        Vector2 frames, currentFrame;

        public Vector2 Frames
        {
            set { frames = value; }
        }

        public Vector2 CurrentFrame
        {
            get { return currentFrame; }
            set { currentFrame = value; }
        }

        public int FrameWidth
        {
            get { return image.Width / (int)frames.X; }
        }

        public int FrameHeight
        {
            get { return image.Height / (int)frames.Y; }
        }

        public override void LoadContent(ContentManager Content, Texture2D image, string text, Vector2 position)
        {
            base.LoadContent(Content, image, text, position);
            frameCounter = 0;
            switchFrame = 126;
            frames = new Vector2(10, 1);
            currentFrame = new Vector2(0, 0);
            sourceRect = new Rectangle((int)currentFrame.X * FrameWidth, (int)currentFrame.Y * FrameHeight, FrameWidth, FrameHeight);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (isActive)
            {
                frameCounter += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (frameCounter >= switchFrame)
                {
                    frameCounter = 0;
                    currentFrame.X++;

                    if (currentFrame.X * FrameWidth >= image.Width)
                        currentFrame.X = 0;
                }
            }
            else
            {
                frameCounter = 0;
                currentFrame.X = 0;
            }
            sourceRect = new Rectangle((int)currentFrame.X * FrameWidth, (int)currentFrame.Y * FrameHeight, FrameWidth, FrameHeight);
        }
    }
}
