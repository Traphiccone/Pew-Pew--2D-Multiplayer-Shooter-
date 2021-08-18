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
    public class KeyboardManager
    {
        private Keys[] lastPressedKeys;
        private string text = "";


        public KeyboardManager()
        {
            lastPressedKeys = new Keys[0];
            Keys[] k = Keyboard.GetState().GetPressedKeys();
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public void Update()
        {
            KeyboardState kbState = Keyboard.GetState();
            Keys[] pressedKeys = kbState.GetPressedKeys();

            //check if any of the previous update's keys are no longer pressed
            foreach (Keys key in lastPressedKeys)
            {
                if (!pressedKeys.Contains(key))
                    OnKeyUp(key);
            }

            //check if the currently pressed keys were already pressed
            foreach (Keys key in pressedKeys)
            {
                if (!lastPressedKeys.Contains(key))
                    OnKeyDown(key);
            }

            //save the currently pressed keys so we can compare on the next update
            lastPressedKeys = pressedKeys;
        }

        private void OnKeyDown(Keys key)
        {
        }

        private void OnKeyUp(Keys key)
        {
            Keys last = lastPressedKeys[lastPressedKeys.Length - 1];
            // Sanitize input
            switch (last)
            {
                case Keys.Enter:
                    return;
                case Keys.Back:
                    if (text.Length != 0)
                    {
                        text = text.Remove(text.Length - 1);
                    } return;
                case Keys.RightShift:
                    return;
                case Keys.LeftShift:
                    return;
            }
            text += last;
        }
    }
}
