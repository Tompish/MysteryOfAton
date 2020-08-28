using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace MysteryOfAtonClient.Textboxes
{
    class LoginTextbox
    {
        private InputTextbox passwordBox;
        private InputTextbox userNameBox;
        private GameWindow _window;

        public StringBuilder userPassword { get { return passwordBox.displayText; } }
        public StringBuilder userName { get { return userNameBox.displayText; } }
        public LoginTextbox(ContentManager content, GameWindow window)
        {
            _window = window;
            passwordBox = new InputTextbox(content, window);
            userNameBox = new InputTextbox(content, window);
        }

        public void InitializeLoginbox()
        {
            float xCoord = _window.ClientBounds.Width * (float)0.33;
            float yCoord = _window.ClientBounds.Height * (float)0.33;

            userNameBox.SetDestinationRectangle(new Rectangle(Convert.ToInt32(xCoord)
                , Convert.ToInt32(yCoord)
                , Convert.ToInt32(xCoord * 0.9)
                , Convert.ToInt32(yCoord * 0.15)));

            passwordBox.SetDestinationRectangle(new Rectangle(Convert.ToInt32(xCoord * 2)
                , Convert.ToInt32(yCoord)
                , Convert.ToInt32(xCoord * 0.9)
                , Convert.ToInt32(yCoord * 0.15)));

            passwordBox.InitializeTextbox("Textbox", "MenuFont");
            userNameBox.InitializeTextbox("Textbox", "MenuFont");
            
        }

        public void Update(MouseState mouse)
        {
            userNameBox.CheckTextboxState(mouse);
            passwordBox.CheckTextboxState(mouse);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            userNameBox.Draw(spriteBatch);
            passwordBox.Draw(spriteBatch);
        }
    }
}
