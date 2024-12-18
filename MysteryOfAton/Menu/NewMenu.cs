using System;
using MysteryOfAtonClient.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace MysteryOfAtonClient.Menu
{
    public class NewMenu
    {
        private List<TextInputElement> _textInputElements;
        private ContentManager _content;

        public NewMenu(ContentManager content)
        {
            this._content = content;
        }

        public bool loadMenu(GameWindow window, ResolutionHandler rHandler) {
            bool sucess = true;
            try
            {
                this._textInputElements.Add(new TextInputElement(window, _content.Load<Texture2D>("Textbox.png"), "hejsan", _content.Load<SpriteFont>("MenuFont.spritefont"), rHandler));
                this._textInputElements[0].position = new Vector2(500, 500);
            }
            catch (Exception ex) {
                sucess = false;
                throw new NullReferenceException(ex.Message);
	        }
            return sucess;
	    }

        public void draw(SpriteBatch spriteBatch) {
            _textInputElements.ForEach((element) => element.Draw(spriteBatch));
	    }

        public void update() {
            this._textInputElements.ForEach(element => element.InputHandler());
	    }

        
    }
}
