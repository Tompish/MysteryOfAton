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
        private SpriteFont _spriteFont;

        public StringBuilder userPassword { get { return passwordBox.displayText; } }
        public StringBuilder userName { get { return userNameBox.displayText; } }
        public LoginTextbox(ContentManager content, GameWindow window)
        {
            var texture = content.Load<Texture2D>("Textbox");
            _spriteFont = content.Load<SpriteFont>("MenuFont");
            passwordBox = new InputTextbox(window, texture, _spriteFont);
            userNameBox = new InputTextbox(window, texture, _spriteFont);
        }

        public void InitializeLoginbox()
        {
            float xCoord = Client.width * (float)0.20;
            float yCoord = Client.height * (float)0.33;

            userNameBox.SetDestinationRectangle(new Rectangle(Convert.ToInt32(xCoord)
                , Convert.ToInt32(yCoord)
                , Convert.ToInt32(xCoord)
                , Convert.ToInt32(yCoord * 0.15)));

            passwordBox.SetDestinationRectangle(new Rectangle(Convert.ToInt32(xCoord * 3)
                , Convert.ToInt32(yCoord)
                , Convert.ToInt32(xCoord)
                , Convert.ToInt32(yCoord * 0.15)));
            
        }

        public void Update(TMouseState mouse)
        {
            userNameBox.CheckTextboxState(mouse);
            passwordBox.CheckTextboxState(mouse);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var user = "User Name";
            var pw = "Password";
            spriteBatch.DrawString(_spriteFont, user, new Vector2(Client.width *(float) 0.2-_spriteFont.MeasureString(user).X, Client.height*(float)0.33+ userNameBox.destinationRect.Height/3), Color.Yellow);
            userNameBox.Draw(spriteBatch);
            passwordBox.Draw(spriteBatch);
        }
    }
}
