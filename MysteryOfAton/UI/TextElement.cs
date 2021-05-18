using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MysteryOfAtonClient.UI
{
    class TextElement : BaseElement
    {
        public StringBuilder sBuilder  = new StringBuilder("", 100);
        private SpriteFont _spriteFont;
        
        public int padding = 5;


        public TextElement(Texture2D? texture, string? text, SpriteFont spriteFont) : base(texture) {
            _spriteFont = spriteFont;
            if(text != null)
                sBuilder.Append(text);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_texture != null)
            {
                spriteBatch.Draw(_texture, position, Color.White);
                spriteBatch.DrawString(_spriteFont, sBuilder, internalRect.HasValue ? internalRect.Value.Location.ToVector2() : position, Color.White);
                
                foreach(BaseElement element in children)
                {
                    element.Draw(spriteBatch);
                }
                return;
            }

            spriteBatch.DrawString(_spriteFont, sBuilder, position + new Vector2(padding, padding), Color.White);

        }
    }
}
